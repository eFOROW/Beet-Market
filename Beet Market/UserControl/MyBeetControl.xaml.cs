using Beet_Market.ServiceReference2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;

namespace Beet_Market
{
    /// <summary>
    /// MyBeetControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyBeetControl : UserControl
    {
        //public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Product> Items { get; set; } = new ObservableCollection<Product>();
        private ProductInsertClient _client = new ProductInsertClient();
        public UserData userdata = new UserData();

        public MyBeetControl()
        {
            InitializeComponent();

            //Items = new ObservableCollection<Item>
            //{
            //    new Item("아이폰 15 팔아요", DateTime.Parse("2024-11-12 14:30:45"), "https://media.bunjang.co.kr/product/250577323_1_1705929449_w1200.jpg?crop=0", 120, 350000),
            //    new Item("냉장고 안써서 싸게 드려요", DateTime.Parse("2024-11-14 10:05:10"), "https://m.happydreammarket.co.kr/web/product/big/20200624/a16ec862290774ee3002b714c1c09b9d.jpg", 85, 150000),                
            //    new Item("LG 43인치 TV 팝니다", DateTime.Parse("2024-11-16 16:45:30"), "https://media.bunjang.co.kr/product/296219301_1_1730447137_w360.jpg", 200, 400000),

            //};

            this.DataContext = this;
            profile_grid.DataContext = userdata;
            Update();            
        }

        private void Update()
        {
            _client.On();
            Items.Clear();
            Product[] temp = _client.GetUserProducts(KakaoData.UserId);
            _client.Off();
            foreach (Product product in temp)
            {
                Items.Add(product);
            }            
        }

        #region 삭제하기
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Product product = button?.DataContext as Product;
            if (product != null)
            {
                var result = MessageBox.Show($"정말로 삭제하시겠습니까?\n({product.Title})", "알림", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _client.On();
                    if (_client.DeleteProduct(product.P_Id))
                        MessageBox.Show("삭제되었습니다", "알림");
                    else
                        MessageBox.Show("삭제에 실패하였습니다\n다시 시도해주세요!", "오류");
                    _client.Off();                   
                }
            }
        }
        #endregion
    }
}
