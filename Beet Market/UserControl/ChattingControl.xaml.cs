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

namespace Beet_Market
{
    /// <summary>
    /// 채팅방 부분
    /// </summary>
    public partial class ChattingControl : UserControl
    {
        private ObservableCollection<Person> _people;
        CompositeCollection compositeCollection = new CompositeCollection();

        public ObservableCollection<Person> People
        {
            get { return _people; }
            set
            {
                _people = value;
                OnPropertyChanged(nameof(People));
            }
        }

        public ChattingControl()
        {
            InitializeComponent();

            // 데이터 초기화
            People = new ObservableCollection<Person>
            {
                new Person { Name = "Alice", Age = "../Image/icon.png", Job = "Engineer" },
                new Person { Name = "Bob", Age = "../Image/icon.png", Job = "Designer" },
                new Person { Name = "Charlie", Age = "../Image/icon.png", Job = "Manager" }
            };

            // 데이터 컨텍스트 설정
            DataContext = this;

            ObservableCollection<InboundMessage> inboundMessages = new ObservableCollection<InboundMessage>();

            inboundMessages.Add(new InboundMessage { MessageId = 1, Message = "오늘 약속 장소는", ReceivedTime = new DateTime(2023, 12, 10, 12, 50, 12).ToString("tt HH:mm") });
            inboundMessages.Add(new InboundMessage { MessageId = 2, Message = "강남역 10번 출구야", ReceivedTime = new DateTime(2023, 12, 10, 12, 50, 15).ToString("tt HH:mm") });
            this.compositeCollection.Add(new CollectionContainer() { Collection = inboundMessages });

            ObservableCollection<OutboundMessage> outboundMessages = new ObservableCollection<OutboundMessage>();

            outboundMessages.Add(new OutboundMessage { MessageId = 1, Message = "그래 알았어", SentTime = new DateTime(2023, 12, 10, 12, 51, 30).ToString("tt HH:mm") });
            outboundMessages.Add(new OutboundMessage { MessageId = 2, Message = "몇 시쯤 볼까?", SentTime = new DateTime(2023, 12, 10, 12, 51, 45).ToString("tt HH:mm") });

            this.compositeCollection.Add(new CollectionContainer() { Collection = inboundMessages });
            this.compositeCollection.Add(new CollectionContainer() { Collection = outboundMessages });

            this.compositeCollection.Add(new InboundMessage { MessageId = 1, Message = "7시", ReceivedTime = new DateTime(2023, 12, 10, 12, 53, 02).ToString("tt HH:mm") });
            this.compositeCollection.Add(new OutboundMessage { MessageId = 1, Message = "ㅇㅇ", SentTime = new DateTime(2023, 12, 10, 12, 54, 55).ToString("tt HH:mm") });

            conversationList.ItemsSource = compositeCollection;
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

    public class Person
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Job { get; set; }
    }
}
