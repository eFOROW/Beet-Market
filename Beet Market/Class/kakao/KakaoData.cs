using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beet_Market
{
    public class KakaoData
    {
        // 유저 데이터
        public static string userToken; // 유저 토큰
        public static string accessToken; // 에셋 토큰
        public static string UserNickName; // 사용자 이름
        public static string Connected_at; // 사용자 가입일
        public static string UserId; // 사용자 아이디
        public static string UserImg; // 사용자 이미지
    }

    public class UserData
    {
        private string userid;
        public string UserId
        {
            get { return userid; }
            set { userid = KakaoData.UserId; }
        }

        private string username;
        public string UserNickName
        {
            get { return username; }
            set { username = KakaoData.UserNickName; }
        }

        private string userimg;
        public string UserImg
        {
            get { return userimg; }
            set { userimg = KakaoData.UserImg; }
        }

        private string connected_at;
        public string Connected_at
        {
            get { return connected_at; }
            set { connected_at = ConvertToKoreanTime(value); }
        }

        public UserData()
        {
            UserId = KakaoData.UserId;
            UserNickName = KakaoData.UserNickName;
            UserImg = KakaoData.UserImg;
            Connected_at = KakaoData.Connected_at;
        }

        // UTC를 한국 시간으로 변환하는 메서드
        private string ConvertToKoreanTime(string utcDateTime)
        {
            DateTime utcTime = DateTime.Parse(utcDateTime);

            TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
            DateTime koreaTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, koreaTimeZone);

            return koreaTime.ToString("yyyy년MM월dd일 HH:mm:ss");
        }
    }
}
