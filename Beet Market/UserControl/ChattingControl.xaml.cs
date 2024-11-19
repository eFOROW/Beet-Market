using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private ObservableCollection<ChatRoom> chatroom;
        CompositeCollection compositeCollection = new CompositeCollection();

        public ObservableCollection<ChatRoom> Chatroom
        {
            get { return chatroom; }
            set
            {
                chatroom = value;
                OnPropertyChanged(nameof(chatroom));
            }
        }

        public ChattingControl()
        {
            InitializeComponent();

            // 데이터 초기화
            Chatroom = new ObservableCollection<ChatRoom>
            {
                //new Person { Name = "Alice", Age = "../Image/icon.png", Job = "Engineer" },
                //new Person { Name = "Bob", Age = "../Image/icon.png", Job = "Designer" },
                //new Person { Name = "Charlie", Age = "../Image/icon.png", Job = "Manager" }

                new ChatRoom { A_Name="준서", A_imgUrl="http://k.kakaocdn.net/dn/IHEP4/btsJH09cxs3/cpcAB49sMrqqKwClkZmDM1/img_640x640.jpg", J_Name = "원형", J_imgUrl="http://k.kakaocdn.net/dn/csQZO5/btr8i7XVY8Y/T5xYeN0nmNAphNBDxK2DX1/img_640x640.jpg", P_Id = 1, Tag =1, C_Time = new DateTime(2023, 12, 10, 12, 54, 55) }
            };

            // 데이터 컨텍스트 설정
            DataContext = this;

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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            // 스크롤뷰어가 항상 마지막 메시지를 표시하도록 설정
            scrollViewer.ScrollToEnd();
        }
    }

    public class ChatRoom
    {
        /*
         * [인자 값] 클라이언트 로그인 UID
         * 
         * [채팅방 리스트 전체 출력시 반환클래스]
         * 게시자 닉네임, 게시자 imgurl, 참여자 닉네임, 참여자 imgurl, 채팅방Tag, 상품ID
         */

        public int P_Id { get; set; }
        public string A_Name { get; set; }
        public string A_imgUrl { get; set; }
        public string J_Name { get; set; }
        public string J_imgUrl { get; set; }
        public int Tag { get; set; }
        public DateTime C_Time { get; set; }
    }
}
