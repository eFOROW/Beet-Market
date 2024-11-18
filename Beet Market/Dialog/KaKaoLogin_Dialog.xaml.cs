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
using System.Windows.Shapes;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using WebBrowser = System.Windows.Forms.WebBrowser;
using MessageBox = System.Windows.MessageBox;

namespace Beet_Market
{
    /// <summary>
    /// KaKaoLogin_Dialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KaKaoLogin_Dialog : Window
    {
        KakaoManager kakaoManager;

        public KaKaoLogin_Dialog()
        {
            WebBrowserVersionSetting();

            InitializeComponent();

            kakaoManager = new KakaoManager();
           
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted; // 웹 브라우저 창이 로드 될 때 마다 발생 이벤트

            webBrowser.Navigate(KakaoApiEndPoint.KakaoLogInUrl); // 웹 브라우저 처음 열릴 URL 설정
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (kakaoManager.GetUserToKen(webBrowser))
            {
                kakaoManager.GetAccessToKen();
                this.Close();
            }
        }

        private void WebBrowserVersionSetting()
        {
            RegistryKey registryKey = null; // 레지스트리 변경에 사용 될 변수

            int browserver = 0;
            int ie_emulation = 0;
            var targetApplication = Process.GetCurrentProcess().ProcessName + ".exe"; // 현재 프로그램 이름

            // 사용자 IE 버전 확인
            using (WebBrowser wb = new WebBrowser())
            {
                browserver = wb.Version.Major;
                if (browserver >= 11)
                    ie_emulation = 11001;
                else if (browserver == 10)
                    ie_emulation = 10001;
                else if (browserver == 9)
                    ie_emulation = 9999;
                else if (browserver == 8)
                    ie_emulation = 8888;
                else
                    ie_emulation = 7000;
            }

            try
            {
                registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

                // IE가 없으면 실행 불가능
                if (registryKey == null)
                {
                    //MessageBox.Show("웹 브라우저 버전 초기화에 실패했습니다..!");
                    //Application.Exit();
                    return;
                }

                string FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));

                // 이미 키가 있다면 종료
                if (FindAppkey == ie_emulation.ToString())
                {
                    registryKey.Close();
                    return;
                }

                // 키가 없으므로 키 셋팅
                registryKey.SetValue(targetApplication, unchecked((int)ie_emulation), RegistryValueKind.DWord);

                // 다시 키를 받아와서
                FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));

                // 현재 브라우저 버전이랑 동일 한지 판단
                if (FindAppkey == ie_emulation.ToString())
                {
                    return;
                }
                else
                {
                    //MessageBox.Show("웹 브라우저 버전 초기화에 실패했습니다..!");
                    //Application.Exit();
                    return;
                }
            }
            catch
            {
                //MessageBox.Show("웹 브라우저 버전 초기화에 실패했습니다..!");
                //Application.Exit();
                return;
            }
            finally
            {
                // 키 메모리 해제
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
        }
    }
}
