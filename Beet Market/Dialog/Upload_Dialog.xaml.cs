using Beet_Market.ServiceReference2;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Beet_Market
{
    /// <summary>
    /// Upload_Dialog.xaml에 대한 상호 작용 논리
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class Upload_Dialog : Window, IUploadDialog
    {
        private ProductInsertClient _proxy = new ProductInsertClient();
        private string imgUrl = string.Empty;

        // 지도용
        private (double Lat, double Lng)? marker = null; // 하나의 마커 좌표 저장
        private ProductInsertClient Pc = new ProductInsertClient();

        public Upload_Dialog()
        {
            _proxy.On();
            InitializeComponent();

            WebBrowserVersionSetting();
            InitializeWebBrowser();
            LoadMap();
        }

        #region 이미지 선택
        private async void OnUploadImageButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            // 파일 선택 대화상자 열기
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var upload = new FirebaseStorageUpload();

                imgUrl = await upload.UploadImageAsync(filePath);

                if (imgUrl != null)
                {
                    btn.Visibility = Visibility.Collapsed;
                    upload_end_btn.Visibility = Visibility.Visible;
                    MessageBox.Show("이미지 업로드 완료", "알림");
                }
                else
                {
                    MessageBox.Show("이미지 업로드에 실패\n(다시 시도해주세요)", "오류");
                }
            }
        }

        #endregion

        #region 거래방식
        private List<string> GetSelectedItem()
        {
            var selectedItems = typeListBox.SelectedItems;

            if (selectedItems.Count > 0)
            {
                // 선택된 항목들의 Content 값을 리스트에 저장
                var selectedContents = new List<string>();

                foreach (var item in selectedItems)
                {
                    // ListBoxItem으로 캐스팅하여 Content 값 추출
                    if (item is ListBoxItem listBoxItem)
                    {
                        selectedContents.Add(listBoxItem.Content.ToString());                        
                    }
                }

                return selectedContents;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 작성완료
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 제목
            string title = upload_title.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("제목을 입력해주세요", "알림");
                return;
            }

            // 가격
            if (string.IsNullOrEmpty(upload_price.Text.Trim()))
            {
                MessageBox.Show("가격을 입력해주세요", "알림");
                return;
            }
            int price = int.Parse(upload_price.Text.Trim());

            // 사진
            if (imgUrl == string.Empty)
            {
                MessageBox.Show("사진은 필수입니다", "알림");
                return;
            }

            // 거래방식
            var selectedContents = GetSelectedItem();

            if (selectedContents == null)
            {
                MessageBox.Show("거래방식을 1개 이상은 선택해주세요", "알림");
                return;
            }

            // 카테고리
            string category = (string)CategorySplitButton.Content;
            if (category == "카테고리")
            {
                MessageBox.Show("카테고리를 선택해주세요", "알림");
                return;
            }

            // 내용
            string description = upload_descript.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("내용을 입력해주세요", "알림");
                return;
            }

            // 장소
            if(marker == null)
            {
                MessageBox.Show("장소를 선택해주세요", "알림");
                return;
            }
            var jsonObject = new
            {
                Lat = marker.Value.Lat,
                Lng = marker.Value.Lng
            };

            // JSON 객체를 문자열로 직렬화
            string json = JsonConvert.SerializeObject(jsonObject);


            // 전부 이상없음
            //MessageBox.Show($"제목 : {title}\n가격 : {price}\n거래방식 : {string.Join(", ", selectedContents)}\n" +
            //    $"이미지url : {imgUrl}\n카테고리 : {category}\n내용 : {description}");


            if (_proxy.InsertProduct(title, imgUrl, description, price, category, string.Join(", ", selectedContents), KakaoData.UserId, json))
            {
                _proxy.Off();
                this.Close();
            }
            else
            {
                MessageBox.Show("게시글 업로드 실패", "오류");
            }
        }
        #endregion

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Content is TextBlock textBlock)
                {
                    CategorySplitButton.Content = textBlock.Text;
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // 이벤트 취소 (숫자 외 문자는 입력 안됨)
            }
        }


        #region 지도
        private void LoadMap()
        {
            string html = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <title>Kakao Map</title>
                    <script type=""text/javascript"" src=""https://dapi.kakao.com/v2/maps/sdk.js?appkey=49ddc3469ac5358a3ed1a148416dd8c1""></script>
                    <style>
                        html, body {
                            width: 100%;
                            height: 100%;
                            margin: 0;
                            padding: 0;
                        }
                        #map {
                            width: 100%;
                            height: 100%;
                        }
                    </style>
                </head>
                <body>
                    <div id=""map""></div>
                    <script>
                        let map, marker = null;
                        const mapContainer = document.getElementById('map');
                        const mapOptions = {
                            center: new kakao.maps.LatLng(36.336375081991022, 127.44627110365974),
                            level: 3
                        };

                        map = new kakao.maps.Map(mapContainer, mapOptions);

                        // 지도 클릭 이벤트
                        kakao.maps.event.addListener(map, 'click', function(mouseEvent) {
                            const latlng = mouseEvent.latLng;
                            addMarker(latlng.getLat(), latlng.getLng());
                            // 마커 좌표를 C#으로 전송
                            window.external.AddMarker(latlng.getLat(), latlng.getLng());
                        });

                        // 하나의 마커만 추가
                        function addMarker(lat, lng) {
                            // 기존 마커 제거
                            if (marker) {
                                marker.setMap(null);
                            }
                            // 새 마커 추가
                            marker = new kakao.maps.Marker({
                                position: new kakao.maps.LatLng(lat, lng),
                                map: map
                            });
                        }

                        // 저장된 마커를 지도에 추가
                        function loadMarker(lat, lng) {
                            addMarker(lat, lng);
                        }
                    </script>
                </body>
                </html>";

            // HTML 파일을 임시 디렉토리에 저장
            string desktopPath = @"C:\Temp";
            string tempPath = System.IO.Path.Combine(desktopPath, "map.html");
            File.WriteAllText(tempPath, html);

            // WebBrowser에서 HTML 파일 로드
            webBrowser1.Navigate(tempPath);
        }

        private void InitializeWebBrowser()
        {
            webBrowser1.ObjectForScripting = this; // JavaScript에서 C# 메서드 호출 가능
        }

        private void WebBrowserVersionSetting()
        {
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
            {
                key.SetValue(appName, 11001, RegistryValueKind.DWord); // IE11 엔진 사용
            }
        }
        public void AddMarker(double lat, double lng)
        {
            marker = (lat, lng); // 하나의 마커 좌표 저장
        }
        #endregion
    }

    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IUploadDialog
    {
        void AddMarker(double lat, double lng); // JavaScript에서 호출하는 메서드
    }

}
