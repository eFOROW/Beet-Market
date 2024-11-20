using Beet_Market.ServiceReference1;
using Beet_Market.ServiceReference2;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Beet_Market
{
    /// <summary>
    /// 채팅방 부분
    /// </summary>
    public partial class ChattingControl : UserControl
    {
        private KakaoManager km = KakaoManager.Instance;
        private ObservableCollection<ChatRoom> Chatroom = new ObservableCollection<ChatRoom>();
        CompositeCollection compositeCollection = new CompositeCollection();

        private static readonly string firebaseUrl = "https://practice-web-db.firebaseio.com/";
        static FirebaseClient firebase = new FirebaseClient(firebaseUrl);

        public ObservableCollection<Message> Messages { get; set; } = null;
        private string currentUser; // 현재 사용자의 이름
        private string tag = string.Empty; // 현재 채팅방 Tag
        private IDisposable firebaseSubscription; // 기존 구독 관리

        ObservableCollection<InboundMessage> inboundMessages = new ObservableCollection<InboundMessage>();    // 상대가 하는 말
        ObservableCollection<OutboundMessage> outboundMessages = new ObservableCollection<OutboundMessage>(); // 내가하는 말


        public ChattingControl()
        {
            InitializeComponent();

            km.Update();
            Chatroom = km.Chatroom;

            // 데이터 초기화
            //Chatroom = new ObservableCollection<ChatRoom>
            //{
            //    //new Person { Name = "Alice", Age = "../Image/icon.png", Job = "Engineer" },
            //    //new Person { Name = "Bob", Age = "../Image/icon.png", Job = "Designer" },
            //    //new Person { Name = "Charlie", Age = "../Image/icon.png", Job = "Manager" }

            //    new ChatRoom { A_Name="준서", A_imgUrl="http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg", J_Name = "원형", J_imgUrl="http://k.kakaocdn.net/dn/csQZO5/btr8i7XVY8Y/T5xYeN0nmNAphNBDxK2DX1/img_640x640.jpg", P_Id = 1, Tag =1, C_Time = new DateTime(2023, 12, 10, 12, 54, 55) }
            //};

            // 데이터 컨텍스트 설정
            DataContext = km;

            #region 채팅 초기화
            inboundMessages.Add(new InboundMessage { Name = "준서", Message = "오늘 약속 장소는", ReceivedTime = new DateTime(2023, 12, 10, 12, 50, 12).ToString("tt HH:mm"), imgUrl= "http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg" });
            inboundMessages.Add(new InboundMessage { Name = "준서", Message = "강남역 10번 출구야", ReceivedTime = new DateTime(2023, 12, 10, 12, 50, 15).ToString("tt HH:mm"), imgUrl = "http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg" });
            this.compositeCollection.Add(new CollectionContainer() { Collection = inboundMessages });

            outboundMessages.Add(new OutboundMessage { MessageId = 1, Message = "그래 알았어", SentTime = new DateTime(2023, 12, 10, 12, 51, 30).ToString("tt HH:mm") });
            outboundMessages.Add(new OutboundMessage { MessageId = 2, Message = "몇 시쯤 볼까?", SentTime = new DateTime(2023, 12, 10, 12, 51, 45).ToString("tt HH:mm") });

            this.compositeCollection.Add(new CollectionContainer() { Collection = inboundMessages });
            this.compositeCollection.Add(new CollectionContainer() { Collection = outboundMessages });

            this.compositeCollection.Add(new InboundMessage { Name = "준서", Message = "7시", ReceivedTime = new DateTime(2023, 12, 10, 12, 53, 02).ToString("tt HH:mm"), imgUrl = "http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg" });
            this.compositeCollection.Add(new OutboundMessage { MessageId = 1, Message = "ㅇㅇ", SentTime = new DateTime(2023, 12, 10, 12, 54, 55).ToString("tt HH:mm") });

            conversationList.ItemsSource = compositeCollection;
            #endregion

            // ObservableCollection 초기화
            Messages = new ObservableCollection<Message>();

            // 초기 사용자 설정
            currentUser = "User1"; // 기본값: User1

            // Firebase에서 초기 데이터 가져오기
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            // 스크롤뷰어가 항상 마지막 메시지를 표시하도록 설정
            scrollViewer.ScrollToEnd();
        }

        private void UpdateMessageBox()
        {
            textBox2.Text = string.Join(Environment.NewLine,
                    Messages
                    .Where(m => m != null && m.Sender != null) // null 값 필터링
                    .Select(m => $"{m.Sender}: {m.Content}"));
        }

        private void switchUserButton_Click(object sender, EventArgs e)
        {
            // 유저 이름 전환 (User1 ↔ User2)
            currentUser = (currentUser == "User1") ? "User2" : "User1";
            MessageBox.Show($"현재 사용자: {currentUser}");
        }

        // 기존 Firebase 리스너 중지 메서드
        private void StopFirebaseListener()
        {
            if (firebaseSubscription != null)
            {
                firebaseSubscription.Dispose();
                firebaseSubscription = null; // 리스너를 완전히 제거
            }
        }

            // LoadInitialMessages에서 리스너를 저장
        private void LoadInitialMessages(string tag)
        {
            try
            {
                // 기존 메시지 초기화
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Clear(); // 기존 메시지를 완전히 제거
                    UpdateMessageBox(); // UI 초기화
                });

                // Firebase 리스너 등록 (기존 리스너가 중지되었는지 확인)
                StopFirebaseListener(); // 중복 리스너 방지

                firebaseSubscription = firebase
                    .Child("ChatRoom/" + tag + "/messages")
                    .AsObservable<Message>()
                    .ObserveOnDispatcher()
                    .Subscribe(change =>
                    {
                        if (change.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Messages.Add(change.Object);
                                UpdateMessageBox();
                                scrollViewer.ScrollToEnd();
                            });
                        }
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading messages: {ex.Message}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var newMessage = new Message
                {
                    Sender = KakaoData.UserNickName, // 현재 사용자의 이름
                    Content = textBox1.Text,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                textBox1.Text = ""; // 입력창 초기화

                // Firebase에 메시지 추가
                await firebase.Child("ChatRoom/" + tag + "/messages").PostAsync(newMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }

        #region 채팅방 리스트 선택 리스너
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChatRoom selectedItem = (ChatRoom)chatroomlist.SelectedItem;

            if (selectedItem == null)
            {
                return; // 선택된 항목이 없으면 종료
            }

            // 현재 태그 업데이트
            string newTag = selectedItem.Tag.ToString();

            if (tag == newTag)
            {
                return; // 태그가 동일하면 변경하지 않음
            }

            tag = newTag; // 태그 업데이트

            // 기존 메시지 초기화
            Messages.Clear();

            // 기존 Firebase 리스너 중지
            StopFirebaseListener();

            // 새로운 메시지 로드 및 리스너 등록
            LoadInitialMessages(tag);
        }
        #endregion
    }

    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public long Timestamp { get; set; }
    }

}
