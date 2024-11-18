using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beet_Market
{
    class KakaoApiEndPoint
    {
        // API 키
        public const string KakaoRestApiKey = "e749b456e13f095b0e82409f40cc523f";
        public const string KakaoSendMessageKey = "114311";

        // 리다이렉트 url
        public const string KakaoRedirectUrl = "https://www.google.com/oauth";

        // 로그인 url
        public const string KakaoLogInUrl = "https://kauth.kakao.com/oauth/authorize?client_id=" + KakaoRestApiKey + "&redirect_uri=" + KakaoRedirectUrl + "&response_type=code";

        // 루트 url
        public const string KakaoHostOAuthUrl = "https://kauth.kakao.com";
        public const string KakaoHostApiUrl = "https://kapi.kakao.com";

        // 이벤트 url
        public const string KakaoOAuthUrl = "/oauth/token"; // AccessToken
        public const string KakaoUnlinkUrl = "/v1/user/unlink"; // LogOut
        public const string KakaoTemplateMessageUrl = "/v2/api/talk/memo/send"; // Template 메시지
        public const string KakaoDefaultMessageUrl = "/v2/api/talk/memo/default/send"; // Default 메시지
        public const string KakaoUserDataUrl = "/v2/user/me"; // 사용자 데이터
    }
}
