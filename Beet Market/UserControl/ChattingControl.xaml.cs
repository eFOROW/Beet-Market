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
        private string currentUser; // 현재 사용자의 이름 (User1 또는 User2)


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
            ObservableCollection<InboundMessage> inboundMessages = new ObservableCollection<InboundMessage>();

            inboundMessages.Add(new InboundMessage { Name = "준서", Message = "오늘 약속 장소는", ReceivedTime = new DateTime(2023, 12, 10, 12, 50, 12).ToString("tt HH:mm"), imgUrl= "http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg" });
            inboundMessages.Add(new InboundMessage { Name = "준서", Message = "강남역 10번 출구야", ReceivedTime = new DateTime(2023, 12, 10, 12, 50, 15).ToString("tt HH:mm"), imgUrl = "http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg" });
            this.compositeCollection.Add(new CollectionContainer() { Collection = inboundMessages });

            ObservableCollection<OutboundMessage> outboundMessages = new ObservableCollection<OutboundMessage>();

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
            LoadInitialMessages();

            // Firebase에서 실시간 메시지 수신
            firebase
                .Child("ChatRoom/room1/messages")
                .AsObservable<Message>()
                .ObserveOnDispatcher() // UI 스레드에서 처리
                .Subscribe(change =>
                {
                    if (change.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                    {
                        var newMessage = change.Object;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Messages.Add(newMessage);
                            UpdateMessageBox();
                        });
                    }
                });

        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            // 스크롤뷰어가 항상 마지막 메시지를 표시하도록 설정
            scrollViewer.ScrollToEnd();
        }

        private void UpdateMessageBox()
        {
            // Messages 리스트 데이터를 TextBox2에 출력
            textBox2.Text = string.Join(Environment.NewLine, Messages.Select(m => $"{m.Sender}: {m.Content}"));
        }

        private void switchUserButton_Click(object sender, EventArgs e)
        {
            // 유저 이름 전환 (User1 ↔ User2)
            currentUser = (currentUser == "User1") ? "User2" : "User1";
            MessageBox.Show($"현재 사용자: {currentUser}");
        }

        private async void LoadInitialMessages()
        {
            try
            {
                var messages = await firebase.Child("ChatRoom/room1/messages").OnceAsync<Message>();

                foreach (var message in messages)
                {
                    Messages.Add(message.Object);
                }

                // 메시지를 텍스트박스에 표시
                UpdateMessageBox();
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
                    Sender = currentUser, // 현재 사용자의 이름
                    Content = textBox1.Text,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                textBox1.Text = ""; // 입력창 초기화

                // Firebase에 메시지 추가
                await firebase.Child("ChatRoom/room1/messages").PostAsync(newMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }
    }

    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public long Timestamp { get; set; }
    }

}
