using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Util;
using DotNetOpenAuth.OAuth2;
using System.IO;
using System.Net;
using System.Text;

namespace SocialNetworkTesting
{
    public partial class GooglePlusTesting : System.Web.UI.Page
    {
        private const string GoogleClientID = "96598752803.apps.googleusercontent.com";
        private const string GoogleClientSecret = "kItiURH_zJ4C2KLicRn8oGt7";
        private const string APIKey = "AIzaSyALU-Rfg_4kOF44WvXoI9LXw2A_ItKtF_o";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null && !IsPostBack)
            {                
                ConnectToGooelePlus();
            }
        }

        protected void btn_GooglePlus_Click(object sender, EventArgs e)
        {
            if (Session["GooglePlusToken"] != null)
                GetGooglePlusData();
            else
                ConnectToGooelePlus();
        }

        private void ConnectToGooelePlus()
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = GoogleClientID;
            provider.ClientSecret = GoogleClientSecret;

            if (Request.QueryString["code"] == null && Session["GooglePlusToken"] == null)
                Response.Redirect("https://accounts.google.com/o/oauth2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile&state=%2Fprofile&redirect_uri=http://localhost:51658/GooglePlusTesting.aspx&response_type=code&client_id=" + GoogleClientID + "&approval_prompt=force");
            else if (Request.QueryString["code"] != null && Session["GooglePlusToken"] == null)
            {
                string Googlecode = Request.QueryString["code"].ToString();
                IAuthorizationState istae = GetAuthorization(Googlecode);
                if (istae.AccessToken != null)
                {
                    Session["GooglePlusToken"] = istae.AccessToken;
                    GetGooglePlusData();
                }
            }
        }

        private IAuthorizationState GetAuthorization(string Code)
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = GoogleClientID;
            provider.ClientSecret = GoogleClientSecret;
            NativeApplicationClient arg = provider;
            IAuthorizationState state = new AuthorizationState();
            string uristring = "http://localhost:51658/GooglePlusTesting.aspx";
            state.Callback = new Uri(uristring);
            string authCode = Code;
            return arg.ProcessUserAuthorization(authCode, state);
        }

        private void GetGooglePlusData()
        {
            string url = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + Session["GooglePlusToken"].ToString();
            JSONData.InnerHtml = WebRequest(OAuth.WebMethod.GET, url, "");
            string freebase = "{\"type\": \"/music/artist\",\"name\":\"The Police\",\"album\":[]}";
            //freebase = UrlEncode(freebase);
            url = "https://www.googleapis.com/freebase/v1/search?query=" + freebase + "&access_token=" + Session["GooglePlusToken"].ToString();
            JSONData.InnerHtml = WebRequest(OAuth.WebMethod.GET, url, "");            
        }

        protected string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
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
            catch
            {
                throw;
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
}