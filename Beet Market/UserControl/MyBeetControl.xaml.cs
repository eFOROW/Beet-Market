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
        public ObservableCollection<Product> Items { get; set; } = new ObservableCollection<Product>();
        private ProductInsertClient _client = new ProductInsertClient();
        public UserData userdata = new UserData();

        public MyBeetControl()
        {
            InitializeComponent();

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


        #region 거래상태수정
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Product selectedCard = (sender as FrameworkElement).DataContext as Product;
            // sender는 클릭된 MenuItem
            MenuItem clickedItem = sender as MenuItem;

            // 클릭된 MenuItem의 Header 값을 확인하여 조건문으로 분기
            if (clickedItem != null)
            {
                string header = clickedItem.Header.ToString();

                _client.On();
                switch (header)
                {
                    case "판매중":
                        // "판매중" 클릭 처리
                        if (_client.UpdateProduct(selectedCard.P_Id, 0))
                            MessageBox.Show("'판매중'으로 변경하였습니다", "알림");
                        else
                            MessageBox.Show("상태 변경에 실패하였습니다", "알림");
                        break;

                    case "예약중":
                        // "예약중" 클릭 처리
                        if (_client.UpdateProduct(selectedCard.P_Id, 1))
                            MessageBox.Show("'예약중'으로 변경하였습니다", "알림");
                        else
                            MessageBox.Show("상태 변경에 실패하였습니다", "알림");
                        break;

                    case "거래완료":
                        // "거래완료" 클릭 처리
                        if (_client.UpdateProduct(selectedCard.P_Id, 2))
                            MessageBox.Show("'거래완료'로 변경하였습니다", "알림");
                        else
                            MessageBox.Show("상태 변경에 실패하였습니다", "알림");
                        break;
                }
                _client.Off();
                Update();
            }
        }
        #endregion
    }
}
