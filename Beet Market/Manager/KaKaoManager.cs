using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RestSharp;


namespace Beet_Market
{
    public class KakaoManager
    {
        public KakaoManager()
        {
        }

        public bool GetUserToKen(WebBrowser webBrowser)
        {
            string wUrl = webBrowser.Url.ToString();
            string userToken = wUrl.Substring(wUrl.IndexOf("=") + 1);

            if (wUrl.CompareTo(KakaoApiEndPoint.KakaoRedirectUrl + "?code=" + userToken) == 0)
            {
                Console.WriteLine("유저 토큰 얻기 성공");
                KakaoData.userToken = userToken;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetAccessToKen()
        {
            var client = new RestClient(KakaoApiEndPoint.KakaoHostOAuthUrl);

            var request = new RestRequest(KakaoApiEndPoint.KakaoOAuthUrl, Method.Post);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_id", KakaoApiEndPoint.KakaoRestApiKey);
            request.AddParameter("redirect_uri", KakaoApiEndPoint.KakaoRedirectUrl);
            request.AddParameter("code", KakaoData.userToken);

            var restResponse = client.Execute(request);
            var json = JObject.Parse(restResponse.Content);

            KakaoData.accessToken = json["access_token"].ToString();

            return true;
        }

        public void KakaoTalkLogOut()
        {
            var client = new RestClient(KakaoApiEndPoint.KakaoHostApiUrl);

            var request = new RestRequest(KakaoApiEndPoint.KakaoUnlinkUrl, Method.Post);
            request.AddHeader("Authorization", "bearer " + KakaoData.accessToken);

            if (client.Execute(request).IsSuccessful)
            {
                Console.WriteLine("로그아웃 성공");
            }
            else
            {
                Console.WriteLine("로그아웃 실패");
            }
        }

        public void KakaoUserData()
        {
            var client = new RestClient(KakaoApiEndPoint.KakaoHostApiUrl);

            var request = new RestRequest(KakaoApiEndPoint.KakaoUserDataUrl, Method.Get);
            request.AddHeader("Authorization", "bearer " + KakaoData.accessToken);

            var restResponse = client.Execute(request);

            if (!restResponse.IsSuccessful)
            {
                Console.WriteLine($"Error: {restResponse.StatusDescription}");
                // MessageBox.Show("사용자 데이터를 가져오는데 실패했습니다.");
                return;
            }

            var json = JObject.Parse(restResponse.Content);

            try
            {
                // 사용자 ID, 닉네임, 이미지 가져오기
                KakaoData.UserId = json["id"].ToString();
                KakaoData.UserNickName = json["properties"]["nickname"]?.ToString();
                KakaoData.UserImg = json["properties"]["profile_image"]?.ToString();
                KakaoData.Connected_at = json["connected_at"].ToString();

                //Console.WriteLine($"ID: {KakaoData.UserId}, 닉네임: {KakaoData.UserNickName}, 이미지: {KakaoData.UserImg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing user data: {ex.Message}");
                MessageBox.Show("사용자 데이터를 처리하는 중 오류가 발생했습니다.");
            }
        }
    }
}
