using System;
using System.Collections.Generic;
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
    /// OutboundMessageBubble.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OutboundMessageBubble : UserControl
    {
        public OutboundMessageBubble()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextMessageProperty = DependencyProperty.Register("TextMessage", typeof(string), typeof(OutboundMessageBubble), new UIPropertyMetadata(string.Empty));
        public string TextMessage
        {
            get { return (string)GetValue(TextMessageProperty); }
            set { SetValue(TextMessageProperty, value); }
        }

        public static readonly DependencyProperty TimeStampProperty = DependencyProperty.Register("TimeStamp", typeof(string), typeof(OutboundMessageBubble), new UIPropertyMetadata(string.Empty));
        public string TimeStamp
        {
            get { return (string)GetValue(TimeStampProperty); }
            set { SetValue(TimeStampProperty, value); }
        }

        public static readonly DependencyProperty OutboundMessageBubbleFontSizeProperty = DependencyProperty.Register("OutboundMessageBubbleFontSize", typeof(string), typeof(OutboundMessageBubble), new PropertyMetadata("", OutboundMessageBubbleFontSizeChanged));

        public string OutboundMessageBubbleFontSize
        {
            get { return (string)GetValue(OutboundMessageBubbleFontSizeProperty); }
            set
            {
                SetValue(OutboundMessageBubbleFontSizeProperty, value);
            }
        }

        private static void OutboundMessageBubbleFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is OutboundMessageBubble instance)
            {
                instance.lblTimeStamp.Height = (Convert.ToDouble(instance.OutboundMessageBubbleFontSize) / 3) + 2;
                instance.lblTimeStamp.FontSize = (Convert.ToDouble(instance.OutboundMessageBubbleFontSize) / 3);
            }
        }
    }

    public class OutboundMessage
    {
        public int MessageId { set; get; }
        public string Message { set; get; }
        public string SentTime { set; get; }

        public OutboundMessage() { }

        public OutboundMessage(int messageId, string message, string sentTime)
        {
            MessageId = messageId;
            Message = message;
            SentTime = sentTime;
        }
    }
}
