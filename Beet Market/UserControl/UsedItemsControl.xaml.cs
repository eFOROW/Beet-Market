using Beet_Market.ServiceReference2;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using Beet_Market.ServiceReference1;
using System.ComponentModel;

namespace Beet_Market
{
    /// <summary>
    /// UsedItemsControl.xaml에 대한 상호 작용 논리
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class UsedItemsControl : UserControl, IUploadDialog
    {
        private ProductInsertClient _proxy = new ProductInsertClient();
        private ChatroomClient cr_Client = new ChatroomClient();

        private KakaoManager km = KakaoManager.Instance;

        public ObservableCollection<Product> Items { get; set; } = new ObservableCollection<Product>();
        public Product selectedCard = null;

        // 지도용
        private (double Lat, double Lng)? marker = null; // 하나의 마커 좌표 저장
        private ProductInsertClient Pc = new ProductInsertClient();

        public UsedItemsControl()
        {
            _proxy.On();
            InitializeComponent();

            // 샘플 데이터
            /*Items = new ObservableCollection<Item>
            {
                new Item("아이폰 15 팔아요", DateTime.Parse("2024-11-12 14:30:45"), "https://media.bunjang.co.kr/product/250577323_1_1705929449_w1200.jpg?crop=0", 120, 350000),
                new Item("냉장고 안써서 싸게 드려요", DateTime.Parse("2024-11-14 10:05:10"), "https://m.happydreammarket.co.kr/web/product/big/20200624/a16ec862290774ee3002b714c1c09b9d.jpg", 85, 150000),
                new Item("레이싱 헬멧 실착 1회,새거 쿨거시 네고 가능!", DateTime.Parse("2024-11-15 09:00:55"), "https://firebasestorage.googleapis.com/v0/b/practice-web-db.appspot.com/o/Beet_Market%2Fhelmet.jpg?alt=media&token=f9b06ab0-3b42-4586-80f6-2410c43f5318", 150, 150000),
                new Item("LG 43인치 TV 팝니다", DateTime.Parse("2024-11-16 16:45:30"), "https://media.bunjang.co.kr/product/296219301_1_1730447137_w360.jpg", 200, 400000),
                new Item("중고 책상 팔아요", DateTime.Parse("2024-11-15 11:20:00"), "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGfNXy7Cd5B4-MX1kibrVuE8dYy1NFY3f_8Q&s", 50, 50000),
                new Item("어린이 자전거 팝니다", DateTime.Parse("2024-11-13 10:15:30"), "https://firebasestorage.googleapis.com/v0/b/practice-web-db.appspot.com/o/Beet_Market%2Fbike.jpeg?alt=media&token=c70ff031-8d0f-4be7-bfe3-7a4e933b0cbd", 60, 80000),
                new Item("에어컨 팔아요", DateTime.Parse("2024-11-12 17:50:45"), "https://m.happydreammarket.co.kr/web/product/big/202009/7d5278b9b8968bb9ac1bf94605cb9002.jpg", 75, 200000),
                new Item("전동 킥보드 팝니다", DateTime.Parse("2024-11-11 13:05:05"), "https://youthpress.net/xe/files/attach/images/9794/399/603/5e86d39d79d948815c11203d29254fb9.jpeg", 125, 250000),
                new Item("디지털 카메라 팔아요", DateTime.Parse("2024-11-09 15:40:10"), "../Image.png", 90, 450000),
                new Item("아이패드 미니 5세대 팔아요", DateTime.Parse("2024-11-10 08:00:00"), "https://media.bunjang.co.kr/product/299971986_1_1731418865_w360.jpg", 180, 280000),
                new Item("블루투스 스피커 팝니다", DateTime.Parse("2024-11-08 20:30:15"), "https://dimg.donga.com/wps/NEWS/IMAGE/2023/08/30/120941840.1.jpg", 110, 70000),
                new Item("컴퓨터 의자 팔아요", DateTime.Parse("2024-11-07 18:25:20"), "https://media.bunjang.co.kr/product/206706735_1_1702720006_w360.jpg", 65, 120000),
                new Item("전자레인지 팝니다", DateTime.Parse("2024-11-06 21:40:50"), "https://i0.wp.com/naayo.co.kr/wp-content/uploads/2015/04/microwave-oven.jpg?resize=640%2C480", 95, 60000),
                new Item("노트북 판매합니다", DateTime.Parse("2024-11-05 07:50:05"), "https://media.bunjang.co.kr/product/290015962_1_1726712362_w360.jpg", 135, 550000),
                new Item("테이블 팝니다", DateTime.Parse("2024-11-04 14:10:10"), "../Image.png", 110, 130000)
            };*/

            Update();

            this.DataContext = this;

            WebBrowserVersionSetting();
            InitializeWebBrowser();
            LoadMap();
        }

        private void Update()
        {
            Items.Clear();
            Product[] temp = _proxy.GetProductList();
            foreach (Product product in temp)
            {                
                Items.Add(product);
            }

            category_listbox.SelectedIndex = 0;
        }

        #region 판매글 작성 이벤트
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Upload_Dialog dialog = new Upload_Dialog();
            if (dialog.ShowDialog() == true)
            {
                _proxy.On();
                _proxy.See_insert(selectedCard.P_Id);
                Update();
            }
        }
        #endregion

        #region 카드 클릭
        private void Card_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (DrawerHost.IsRightDrawerOpen == false)
                DrawerHost.IsRightDrawerOpen = true;

            selectedCard = (sender as FrameworkElement).DataContext as Product;
            info_Grid.DataContext = selectedCard;

            if (DrawerHost.IsRightDrawerOpen == false)
                category_listbox.Width = 900;
            else category_listbox.Width = 800;

            _proxy.On();
            _proxy.See_insert(int.Parse(pid.Text));
        }
        #endregion

        #region 툴바

        // 새로고침
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _proxy.On();
            Update();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            category_listbox.Width = 900;
        }
        #endregion

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
            webBrowser2.Navigate(tempPath);
        }

        private void InitializeWebBrowser()
        {
            webBrowser2.ObjectForScripting = this; // JavaScript에서 C# 메서드 호출 가능
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
                                 // 이후 WebBrowser에 저장된 좌표를 보내는 로직
                                 // JavaScript에서 마커 추가
            webBrowser2.InvokeScript("loadMarker", lat, lng); // JavaScript 함수 호출
        }
        #endregion

        private void webBrowser2_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // P_Js가 DataContext로 바인딩된 JSON 문자열이라고 가정
            if (e.NewValue is string jsonString)
            {
                try
                {
                    // JSON 문자열을 객체로 변환 (예: {"lat": 37.7749, "lng": -122.4194})
                    var coordinates = JsonConvert.DeserializeObject<Coordinates>(jsonString);

                    if (coordinates != null)
                    {
                        // 기존 marker 상태 확인
                        if (marker.HasValue && (marker.Value.Lat == coordinates.Lat && marker.Value.Lng == coordinates.Lng))
                        {
                            // 동일한 좌표일 경우, 호출하지 않음
                            return;
                        }

                        // AddMarker 메서드를 호출하여 마커를 지도에 추가
                        AddMarker(coordinates.Lat, coordinates.Lng);
                    }
                }
                catch (Exception ex)
                {
                    // 예외 처리 (잘못된 JSON 형식이나 다른 오류 발생 시)
                    Debug.WriteLine("JSON 파싱 오류: " + ex.Message);
                }
            }
        }

        #region 채팅방 생성
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            cr_Client.On();
            int p_id = int.Parse(pid.Text.ToString());
            string a_id = uid.Text.ToString();

            if (a_id == KakaoData.UserId)
            {
                MessageBox.Show("스스로 채팅방 생성불가", "알림");
                return;
            }

            if (cr_Client.InsertChatRoom(p_id, a_id, KakaoData.UserId) == 0)
                MessageBox.Show("채팅방 생성에 실패했습니다.", "알림");
            else
                MessageBox.Show("채팅방을 생성하였습니다.", "알림");

            
        }
        #endregion

        private void category_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _proxy.On();

            var selectedItem = category_listbox.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                Items.Clear();
                string selectedCategory = selectedItem.Content.ToString();
                Product[] temp = _proxy.GetProductCate(selectedCategory);
                foreach (Product product in temp)
                {
                    Items.Add(product);
                }
            }            
        }
    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 숫자를 상태 문자열로 변환
            switch (value)
            {
                case 0: return "판매중";
                case 1: return "예약중";
                case 2: return "거래완료";
                default: return "알 수 없음"; // 예외 처리
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 상태 문자열을 숫자로 변환
            switch (value?.ToString())
            {
                case "판매중": return 0;
                case "예약중": return 1;
                case "거래완료": return 2;
                default: return Binding.DoNothing; // 변환 실패 시
            }
        }
    }
}
