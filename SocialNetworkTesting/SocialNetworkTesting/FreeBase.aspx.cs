using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Newtonsoft.Json;

namespace SocialNetworkTesting
{
    public partial class FreeBase : System.Web.UI.Page
    {
        private const string GoogleClientID = "225938896921.apps.googleusercontent.com";
        private const string GoogleClientSecret = "tixf3q_yuBZde3fIs2_wOckv";
        private const string APIKey = "AIzaSyALU-Rfg_4kOF44WvXoI9LXw2A_ItKtF_o";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null && !IsPostBack)
            {
                ConnectToGooelePlus();
            }
        }

        protected void btn_FreeBase_Click(object sender, EventArgs e)
        {
            Session["FreeBaseText"] = txt_search.Text;
            if (Session["FreeBaseToken"] != null)
                GetGooglePlusData();
            else
                ConnectToGooelePlus();
        }

        private void ConnectToGooelePlus()
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = GoogleClientID;
            provider.ClientSecret = GoogleClientSecret;

            if (Request.QueryString["code"] == null && Session["FreeBaseToken"] == null)
            {
                string callbackURL = HttpUtility.UrlEncode("http://localhost:51658/FreeBase.aspx");
                Response.Redirect("https://accounts.google.com/o/oauth2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile&state=%2Fprofile&redirect_uri=http://localhost:51658/FreeBase.aspx&response_type=code&client_id=" + GoogleClientID + "&approval_prompt=force&access_type=offline");
            }
            else if (Request.QueryString["code"] != null && Session["FreeBaseToken"] == null)
            {
                string url = "";
                string Googlecode = Request.QueryString["code"].ToString();

                url = "https://accounts.google.com/o/oauth2/token";

                string data = "code=" + Googlecode
                    + "&client_id=" + GoogleClientID
                    + "&client_secret=" + GoogleClientSecret
                    + "&redirect_uri=http://localhost:51658/FreeBase.aspx"
                    + "&grant_type=authorization_code";

                string TempData = WebRequest(OAuth.WebMethod.POST, url, "");
                //IAuthorizationState istae = GetAuthorization(Googlecode);
                //if (istae.AccessToken != null)
                //{
                //    Session["FreeBaseToken"] = istae.AccessToken;
                //    GetGooglePlusData();
                //}

                //url = "https://accounts.google.com/o/oauth2/token";
                //data =  "client_id=" + GoogleClientID
                //    + "&client_secret=" + GoogleClientSecret
                //    + "&refresh_token=" + istae.RefreshToken
                //    + "&grant_type=refresh_token";
                //string TempData = WebRequest(OAuth.WebMethod.POST, url, data);
                //refreshtoken(istae.RefreshToken, istae.AccessToken);

            }
        }


        private string refreshtoken(string refreshkey, string accesstoken)
        {
            string reply = "";
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = GoogleClientID;
            provider.ClientSecret = GoogleClientSecret;
            NativeApplicationClient arg = provider;
            IAuthorizationState state = new AuthorizationState();
            string callbackurl = "http://localhost:51658/FreeBase.aspx";
            state.Callback = new Uri(callbackurl);
            state.RefreshToken = refreshkey;
            
            //IAuthorizationState istae = arg.ProcessUserAuthorization("", state);
            bool status = arg.RefreshToken(state);

            return reply;
        }

        private IAuthorizationState GetAuthorization(string Code)
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = GoogleClientID;
            provider.ClientSecret = GoogleClientSecret;
            NativeApplicationClient arg = provider;
            IAuthorizationState state = new AuthorizationState();
            string uristring = "http://localhost:51658/FreeBase.aspx";
            state.Callback = new Uri(uristring);
            string authCode = Code;
            return arg.ProcessUserAuthorization(authCode, state);
        }

        private void GetGooglePlusData()
        {
            string freebase = "", freebaseData = "", TempData = "";
            int Start = 1, End = 100, Check = 100;
            List<FreeBaseData> list = new List<FreeBaseData>();
            if (Session["FreeBaseText"] != null)
            {
                freebase = Session["FreeBaseText"].ToString();
                Session["FreeBaseText"] = null;
            }
            else
                freebase = txt_search.Text;

            while (Check == 100 && list.Count <= 15)
            {
                FreeBaseData obj = new FreeBaseData();
                string url = "https://www.googleapis.com/freebase/v1/search?query=" + freebase + "&access_token=" + Session["FreeBaseToken"].ToString() + "&start=" + Start + "&limit=" + End;
                TempData = WebRequest(OAuth.WebMethod.GET, url, "");
                obj = JsonConvert.DeserializeObject<FreeBaseData>(TempData);
                list.Add(obj);
                freebaseData = freebaseData + TempData;
                if (obj.cursor > 0)
                {
                    Check = obj.cursor;
                    Start = End + 1;
                }
                else
                    Check = 0;
            }
            JSONData.InnerHtml = freebaseData;
        }

        public string WebRequest(OAuth.WebMethod method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;

            if (method == OAuth.WebMethod.POST)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = WebResponseGet(webRequest);

            webRequest = null;

            return responseData;

        }

        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";
            

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch(WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
                
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }


    public class FreeBaseData
    {
        public string status { get; set; }
        public List<FreeBaseResult> result { get; set; }
        public int cursor { get; set; }
        public int cost { get; set; }
        public int hits { get; set; }
    }

    public class FreeBaseResult
    {
        public string mid { get; set; }
        public string name { get; set; }
        public FreeBaseNotable notable { get; set; }
        public string lang { get; set; }
        public double score { get; set; }
    }

    public class FreeBaseNotable
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}