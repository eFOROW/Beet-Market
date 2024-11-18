using Beet_Market.ServiceReference2;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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

namespace Beet_Market
{
    /// <summary>
    /// UsedItemsControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UsedItemsControl : UserControl
    {
        private ProductInsertClient _proxy = new ProductInsertClient();        

        public ObservableCollection<Product> Items { get; set; } = new ObservableCollection<Product>();
        public Product selectedCard = null;

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
        }

        private void Update()
        {
            Items.Clear();
            Product[] temp = _proxy.GetProductList();
            foreach (Product product in temp)
            {                
                Items.Add(product);
            }
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
        }
        #endregion

        #region 툴바

        // 새로고침
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _proxy.On();
            _proxy.See_insert(selectedCard.P_Id);
            Update();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            category_listbox.Width = 900;
        }
        #endregion

        #region 거리상태표시
        //private void TextBlock_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    switch(p_status.Text)
        //    {
        //        case "판매중":
        //            p_status.Foreground = new SolidColorBrush(Colors.DimGray);
        //            break;
        //        case "예약중":
        //            p_status.Foreground = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "거래완료":
        //            p_status.Foreground = new SolidColorBrush(Colors.LightGray);
        //            break;
        //    }
        //}
        #endregion
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
