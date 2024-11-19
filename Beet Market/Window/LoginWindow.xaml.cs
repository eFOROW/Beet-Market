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
using System.Xml;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Beet_Market.ServiceReference1;

namespace Beet_Market
{
    /// <summary>
    /// LoginWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : Window
    {
        private KakaoManager kakaoManager = null;        
        private IkakaoLoginClient _proxy = new IkakaoLoginClient();

        public LoginWindow()
        {
            InitializeComponent();
            
            kakaoManager = new KakaoManager();
        }

        #region 로그인 Btn
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            KaKaoLogin_Dialog dialog = new KaKaoLogin_Dialog();
            dialog.ShowDialog();

            kakaoManager.KakaoUserData();

            if(KakaoData.UserId == null)
            {
                ToastMessage("로그인 실패");
                return;
            }

            ((IkakaoLogin)_proxy).Open();
            if(((IkakaoLogin)_proxy).User_Select(KakaoData.UserId))
            {
                ((IkakaoLogin)_proxy).User_Update(KakaoData.UserId, KakaoData.UserNickName, KakaoData.UserImg);
            }
            else
            {
                ((IkakaoLogin)_proxy).User_Insert(KakaoData.UserId, KakaoData.UserNickName, KakaoData.UserImg);
            }
            ((IkakaoLogin)_proxy).Close();

            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }

        private void ToastMessage(string message)
        {
            // 메시지 설정
            ToastMessageLabel.Content = message;

            // 첫 번째 애니메이션: 투명도 0에서 1로 변경
            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 0.7,
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };

            // 두 번째 애니메이션: 투명도 1에서 0으로 변경
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 0.7,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };

            // 첫 번째 애니메이션 완료 시 1.5초 대기 후 두 번째 애니메이션 실행
            fadeIn.Completed += (sender, e) =>
            {
                // 1초 대기 후 fadeOut 애니메이션 시작
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1.5); // 1.5초 후 실행
                timer.Tick += (s, args) =>
                {
                    // 두 번째 애니메이션 시작
                    lblToast.BeginAnimation(OpacityProperty, fadeOut);
                    timer.Stop(); // 타이머 종료
                };
                timer.Start(); // 타이머 시작
            };

            // `StackPanel`을 보이게 설정
            lblToast.Visibility = Visibility.Visible;  // 보이게 설정
            lblToast.BeginAnimation(OpacityProperty, fadeIn);  // 첫 번째 애니메이션 시작
        }
        #endregion
    }
}
