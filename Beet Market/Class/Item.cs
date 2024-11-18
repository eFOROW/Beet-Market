using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Beet_Market
{
    public class Item
    {
        public string Title { get; set; }
        public DateTime Created_at { get; set; }
        public string ImageSource { get; set; }
        public int See { get; set; }
        public int Price { get; set; }

        /*
         * 제목 title
         * 게시일시 Created_at
         * 이미지url ImageSource
         * 거래상태 status int 0,1,2
         * 조회수 int see
         * 가격 int price
         * 작성자 uid
         * 설명 Description
         * 물건 id(PK) auto increase
         * 카테고리 category
         */

        public Item(string title, DateTime dateTime, string  imageSource, int see, int price)
        {
            Title = title;
            Created_at = dateTime;
            ImageSource = imageSource;
            See = see;
            Price = price;
        }
    }
}
