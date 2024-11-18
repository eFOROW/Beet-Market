using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Beet_Market
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TabItemData 객체 리스트 설정
            List<TabItemData> tabItems = new List<TabItemData>
            {
                new TabItemData
                {
                    Title = "중고거래",
                    Icon = "../Image/icon_unselected.png",
                    IconSelected = "../Image/icon.png",
                    Content = new UsedItemsControl()
                },
                new TabItemData
                {
                    Title = "채팅",
                    Icon = "../Image/chat_icon.png",
                    IconSelected = "../Image/chat_icon_selected.png",
                    Content = new ChattingControl()
                },
                new TabItemData
                {
                    Title = "나의 비트",
                    Icon = "../Image/profile_icon.png",
                    IconSelected = "../Image/profile_icon_selected.png",
                    Content = new MyBeetControl()
                }
            };

            // TabControl의 ItemsSource 설정
            tabcontrol.ItemsSource = tabItems;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl == null) return;

            foreach (TabItemData tab in tabControl.ItemsSource)
            {
                tab.IsSelected = false;
            }

            if (tabControl.SelectedItem is TabItemData selectedTab)
            {
                selectedTab.IsSelected = true;
            }
        }
    }

    public class TabItemData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; set; }
        public string Icon { get; set; }
        public string IconSelected { get; set; }
        public object Content { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
