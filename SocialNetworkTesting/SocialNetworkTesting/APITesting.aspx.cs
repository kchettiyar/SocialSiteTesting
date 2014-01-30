using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ppAPI.Model.SocialNetwork;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;
using ppAPI.Model.AccountHolderManager;


namespace SocialNetworkTesting
{
    public partial class APITesting : System.Web.UI.Page
    {
        public enum Method { GET, POST };
        public static string APIKey = "man60MoL";
        //public static string APPID = "NYHgkzBEwSluYXhRLYjUAhK1HzNtf1SDrg3wlJFi", RestAPIKey = "bMvgoPtVBW2K3qROkwY9HyFuj5Cp4rj0sz8ppl0I";//Andriod
        public static string APPID = "DfFBK958nmlit3MMzNpXdVF0qpKywNLsp7udlOfu", RestAPIKey = "5WPGsrM9sKQ46oZi5WvjVq0VkHThIKKF9WVxriR3";//ios

        protected void Page_Load(object sender, EventArgs e)
        {
            AccountLogin();
        }



        #region Testing

        public void AccountLogin()
        {
            string URL = "http://localhost:53958/Settings/UpdatePicture?APIKey=pure0012";

            UpdateImage64 OBJData = new UpdateImage64() { MemberID = 100084, Image = "https://fbcdn-sphotos-a-a.akamaihd.net/hphotos-ak-ash3/481220_471755899546420_802842820_n.jpg" };
            
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(OBJData.Image);
            OBJData.Image = System.Convert.ToBase64String(bytes);

            string data = JsonConvert.SerializeObject(OBJData);

            string response = "";// HttpRequestMethod(Method.POST, URL, data);
            string response1 = response;
        }

        public class UpdateImage64
        {
            public int MemberID { get; set; }
            public string Image { get; set; }
        }

        #endregion


        #region Registration

        public int SaveAccountHolderRegistrationDetails()
        {
            int reply = 0;
            //string URL = "http://localhost:62679/v1/FaceBook/FacebookDetails/Save?APIKey=man60MoL";
            string URL = "http://dev.ahrest.pureprofile.com/v1/FaceBook/FacebookDetails/Save?APIKey=man60MoL";

            string data = JsonConvert.SerializeObject(SetFacebookValue());
            string response = HttpRequestMethod(Method.POST, URL, data);
            return reply;
        }

        public ListAccountHolder SetAHValue()
        {
            ListAccountHolder obj = new ListAccountHolder();
            obj.AccountID = 0;
            obj.EmailAddress = "chettiyar.kamlesh@gmail.com";
            obj.FirstName = "Kamlesh";
            obj.LastName = "Chettiyar";
            DateTime DOB = new DateTime(1984, 07, 16);
            obj.DateOfBirth = DOB.ToString();
            obj.Gender = "Female";
            obj.FacebookID = "";
            obj.APIKey = "tAWn6694";
            obj.Password = "123456";
            //obj.CountryID = "36745144553756424D3168424B3765444A7A484E44673D3D";
            obj.CountryID = "80";
            obj.RegistrationType = "1";
            obj.ValidationURL = "https://accounts.pureprofile.com/register/validate?key=";
            obj.PushedToProfile = false;
            obj.PushedDate = DateTime.Now.ToString();
            obj.CreatedDate = DateTime.Now.ToString();
            return obj;
        }

        public ListFacebook SetFacebookValue()
        {
            ListFacebook obj = new ListFacebook();
            obj.AHFacebookID = 0;
            obj.AHID = 5362811;
            obj.FacebookID = "100003920233737";
            obj.AccessToken = "AAAFbWJ7vZB0sBAGKNWZBtNZCdrJBV9Da5TZC8vQM80O7jp3RwnAPgpHgvj5CWuJ3xUk3XAZCUpU6vZA35ZAl5hw64ycY6oZApQyREJIJW935igZDZD";
            obj.EmailAddress = "chettiyar.kamlesh@gmail.com";
            obj.FirstName = "Kamlesh";
            obj.LastName = "Chettiyar";
            obj.DateOfBirth = new DateTime(1984, 07, 05).ToString();
            obj.Gender = "Male";
            obj.Location = "Mumbai,India";
            obj.Country = "India";
            obj.Picture = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-ash4/370963_100001360429158_1363937619_q.jpg";
            obj.DatePermitted = DateTime.Now.ToString();
            obj.TokenExpiryDate = 5161508;
            obj.Source = 1;

            return obj;
        }

        #endregion


        #region Notification

        public string PushNotificationCall()
        {
            string url = "https://api.parse.com/1/push", data = PushNotificationData();
            string data1 = SiteHttpRequestMethod(Method.POST, url, data);
            return data1;
        }

        public string PushNotificationData()
        {
            string value = "{}";
            PushNotification obj = new PushNotification();
            PushNotificationInfo info = new PushNotificationInfo();
            string[] channels = { "" };
            obj.channels = channels;
            
            info.alert = "https://www.Google.co.in#VKulkarni";
            info.sound = "I am ready";
            info.title = "I am not ready";
            //info.action = "android.intent.action.FIRE_LINK";
            obj.data = info;
            value = JsonConvert.SerializeObject(obj);
            return value;
        }

        public class PushNotification
        {
            public string[] channels { get; set; }
            public string type { get; set; }
            
            public PushNotificationInfo data { get; set; }
        }

        public class PushNotificationInfo
        {
            public string alert { get; set; }
            public string action { get; set; }
            public string sound { get; set; }
            public string title { get; set; }
        }

        #endregion

        public string FBProfile()
        {
            string url = "", data = "";
            url = "https://graph.facebook.com/me?oauth_token=AAAFbWJ7vZB0sBAGbTejvboSZAhrKTgHoPvK7QJqXBi3JX1H0GxZApiX6ZCQxcMRkrsofvPkyLh6MhIaFUa4VJMPyGzrJAuZC1mZCuQXUkSpAZDZD&fields=id,email,first_name,last_name,birthday,gender,location,picture,hometown&format=json";
            SettingFBProfile FBProfile = new SettingFBProfile();
            data = SiteHttpRequestMethod(Method.GET, url, "");
            FBProfile = JsonConvert.DeserializeObject<SettingFBProfile>(data);
            return data;
        }

        public string BusinessTesting()
        {
            string reply = "";
            string url = "http://localhost:51008/API/BusinessContact/UpdateBusinessContact";
            BusinessContacts obj = SetBusinessContacts();
            string data = JsonConvert.SerializeObject(obj);
            reply = SiteHttpRequestMethod(Method.POST, url, data);
            return reply;
        }

        public string Testing()
        {
            string reply = "";
            SaveSocialNetworkConnectionDetails obj = SetValues();
            string url = "http://localhost:56986/SocialNetwork/GetSocialNetworkStatus/5366088?APIKey=man60MoL";
            //string url = "http://localhost:56986/SocialNetwork/SaveSocialNetwordProfile?APIKey=man60MoL";
            //string url = "http://dev.socialnetworkapi.pureprofile.com/SocialNetwork/SaveSocialNetwordProfile?APIKey=man60MoL";
            //string url = "http://localhost:56986/SocialNetwork/DeactivateSocialNetworkConnection?APIKey=man60MoL";
            //string url = "http://dev.soc/SocialNetwork/GetSocialNetworkStatus/5362811?APIKey=man60MoL";
            string data = JsonConvert.SerializeObject(obj);
            reply = SiteHttpRequestMethod(Method.POST, url, data);
            //reply = SiteHttpRequestMethod(Method.GET, url, "");
            return reply;
        }


        public SocialNetworkConnectionsStatus SetValues1()
        {
            ppAPI.Model.SocialNetwork.SocialNetworkConnectionsStatus obj = new ppAPI.Model.SocialNetwork.SocialNetworkConnectionsStatus();
            obj.AccountHolderID = 5362811;
            obj.ConnectionStatus = 0;
            obj.ConnectionStatusMsg = "";
            obj.SocialNetworkID = 1;
            obj.Source = 2;
            return obj;
        }

        public SaveSocialNetworkConnectionDetails SetValues()
        {
            ppAPI.Model.SocialNetwork.SaveSocialNetworkConnectionDetails obj = new ppAPI.Model.SocialNetwork.SaveSocialNetworkConnectionDetails();

            obj.AccountHolderID = 5351571;
            obj.SocialNetworkID = 3;
            obj.SocialNetworkUserID = "110619069713160329550";
            obj.EmailOrScreenName = "";
            obj.AccessToken = "bacba157-07de-40d7-adbc-80a22f1f0909";
            obj.AccessTokenSecret = "";
            obj.Source = 3;
            obj.ExpiryDate = 0;
            obj.ClientID = "";
            obj.ClientSecret = "LbE7lmNgrAJUgE4A";

            return obj;
        }

        public BusinessContacts SetBusinessContacts()
        {
            BusinessContacts obj = new BusinessContacts();
            obj.BusinessContactId = "6637552B657465554C41422B514B74336C43556233773D3D";
            obj.RegisteredByTeamMemberID = "6E4D6E4774476A653753645549632B447572797136513D3D";
            obj.EmailAddress = "kamlesh.chettiyar@gmail.com";
            obj.Password = "PASSWORD01";
            obj.TitleId = "672B4C6A68662F682F35584149797770383667582B673D3D";
            obj.FirstName = "Kamlesh";
            obj.LastName = "Chettiyar";
            obj.Pictures = "";
            obj.Position = "Software Programmer";
            obj.BusinessId = "49414B43314B4755394F3474762B787831466A6638773D3D";
            obj.CountryId = "672B4C6A68662F682F35584149797770383667582B673D3D";
            obj.RegionId = "672B4C6A68662F682F35584149797770383667582B673D3D";
            obj.Address1 = "lnfgln";
            obj.Address2 = "gfknfdgk";
            obj.Postcode = "50040";
            obj.MobilePhone = "9988776655";
            obj.Telephone = "2233445566";
            obj.LinkedInURL = "https://linkedin.com";
            obj.TwitterURL = "https://twitter.com";
            obj.FacebookURL = "https://facebook.com";
            obj.Notes = "sdlnflnsflsdnfls";
            obj.IpAddress = "192.168.95.34";


            return obj;
        }

        public static string HttpRequestMethod(Method method, string url, string postData)
        {

            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            //webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
            webRequest.Timeout = 120000;

            if (method == Method.POST)
            {
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(postData);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteData.Length;
                //webRequest.Headers.Add("APIKEY", APIKey);

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }

                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }
            else
            {

            }

            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }

        public static string WebResponseGet(HttpWebRequest webRequest)
        {
            string responseData = "";
            try
            {
                var response = (HttpWebResponse)webRequest.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseData = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
            }

            return responseData;
        }


        public string SiteHttpRequestMethod(Method method, string url, string postData)
        {

            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
            webRequest.Timeout = 2000000;

            if (method == Method.POST)
            {
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(postData);
                webRequest.ContentType = "application/json";
                //webRequest.Headers.Add("APIKEY", APIKey);
                //webRequest.Headers.Add("Content-Type", "application/json");
                webRequest.Headers.Add("X-Parse-Application-Id", APPID);
                webRequest.Headers.Add("X-Parse-REST-API-Key", RestAPIKey);

                webRequest.ContentLength = byteData.Length;

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch (Exception ex)
                {
                    throw;
                }

                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = SiteWebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }

        public string SiteWebResponseGet(HttpWebRequest webRequest)
        {
            string responseData = "";
            try
            {
                var response = (HttpWebResponse)webRequest.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseData = sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        string text1 = text;
                    }
                }

                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
            }

            return responseData;
        }

        //protected void fb_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("/Testing.aspx");
        //}




        public class ListAccountHolder
        {            
            public int AccountID { get; set; }
            public string EmailAddress { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string FacebookID { get; set; }
            public string APIKey { get; set; }
            public string Password { get; set; }
            public string CountryID { get; set; }
            public string RegistrationType { get; set; }
            public string ValidationURL { get; set; }
            public bool PushedToProfile { get; set; }
            public string PushedDate { get; set; }
            public string CreatedDate { get; set; }
        }
    }


    public class SettingFBProfile
    {
        public string id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime birthday { get; set; }
        public string gender { get; set; }
        public string picture { get; set; }
    }



    public class BusinessContacts
    {
        public string BusinessContactId { get; set; }// encrypted 

        public string RegisteredByTeamMemberID { get; set; } // encrypted 

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string TitleId { get; set; }// encrypted 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Pictures { get; set; }

        public string Position { get; set; }

        public string BusinessId { get; set; }// encrypted 

        public string CountryId { get; set; }// encrypted 

        public string RegionId { get; set; }// encrypted 

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Postcode { get; set; }

        public string Telephone { get; set; }

        public string MobilePhone { get; set; }

        public string LinkedInURL { get; set; }

        public string FacebookURL { get; set; }

        public string TwitterURL { get; set; }

        public string Notes { get; set; }

        public string IpAddress { get; set; }
    }

}