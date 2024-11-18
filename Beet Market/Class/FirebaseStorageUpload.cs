using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.IO;

public class FirebaseStorageUpload
{
    private static readonly string FirebaseStorageUrl = "https://firebasestorage.googleapis.com/v0/b/practice-web-db.appspot.com/o";
    private static readonly string FirebaseBucketName = "practice-web-db.appspot.com";

    public async Task<string> UploadImageAsync(string filePath)
    {
        try
        {
            var byteArray = File.ReadAllBytes(filePath);

            var client = new HttpClient();
            var fileContent = new ByteArrayContent(byteArray);
            fileContent.Headers.Add("Content-Type", "image/png");

            // Firebase Storage로 POST 요청
            var response = await client.PostAsync($"{FirebaseStorageUrl}?uploadType=multipart&name=Beet_Market/{Path.GetFileName(filePath)}", fileContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonResponse);

                // 다운로드 토큰을 추출
                string token = jsonObject["downloadTokens"]?.ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    // token을 포함한 다운로드 URL 생성
                    string downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{FirebaseBucketName}/o/Beet_Market%2F{Uri.EscapeDataString(Path.GetFileName(filePath))}?alt=media&token={token}";
                    return downloadUrl; // 업로드가 성공하면 url를 반환
                }
                else
                {
                    MessageBox.Show("Token not found in the response.");
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Upload Failed: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
            return null;
        }
    }
}
