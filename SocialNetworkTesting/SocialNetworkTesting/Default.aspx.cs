using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Specialized;

using Newtonsoft.Json;


namespace SocialNetworkTesting
{
    public partial class _Default : System.Web.UI.Page
    {
        OAuth oauth_obj = new OAuth();

        //My app....
        //private string Li_consumerKey = "p6z9mnkxm4nk";
        //private string Li_consumerSecret = "AS1rR6NsCcnPU9wf";

        private string Li_consumerKey = "pv9wxlw9jg84";
        private string Li_consumerSecret = "c6iEhLH3lcWriE7u";

        //PP app....
        //private string Li_consumerKey = "g4dzghuwx9ff";
        //private string Li_consumerSecret = "Zxit5T8vHyWJiScK";

        private string Twi_consumerKey = "vyg3KP8HJVL5Wr4WnnRU5A";
        private string Twi_consumerSecret = "S5ByXVrBl9DkQxtuWWFEANi0TzBWAfZWHx7ikKCLIY";

        public const string Li_REQUEST_TOKEN = "https://api.linkedin.com/uas/oauth/requestToken";
        public const string Li_AUTHORIZE = "https://api.linkedin.com/uas/oauth/authorize";
        public const string Li_ACCESS_TOKEN = "https://api.linkedin.com/uas/oauth/accessToken";

        public const string Twi_REQUEST_TOKEN = "https://api.twitter.com/oauth/request_token";
        public const string Twi_AUTHORIZE = "https://api.twitter.com/oauth/authorize";
        public const string Twi_ACCESS_TOKEN = "https://api.twitter.com/oauth/access_token";

        public string REQUEST_TOKEN = "";
        public string AUTHORIZE = "";
        public string ACCESS_TOKEN = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["reuqestToken"] = "365f2063-18fb-4ea8-8aff-4bbb93c8a8e5";
            //Session["reuqestTokenSecret"] = "5ce36d96-842b-4da0-b5ef-b7ef2195fb74";

            string oauth_token = "", oauth_verifier = "";
            if (Request.QueryString["oauth_token"] != null && Request.QueryString["oauth_verifier"] != null && IsPostBack == false)
            {
                if (Request.QueryString["SocialNetwork"].ToString() == "3")
                {
                    oauth_token = Request.QueryString["oauth_token"].ToString();
                    oauth_verifier = Request.QueryString["oauth_verifier"].ToString();
                    ConnectToLinkedin(oauth_token, oauth_verifier);
                    GetCurrentUserData();
                }
                else if (Request.QueryString["SocialNetwork"].ToString() == "2")
                {
                    oauth_token = Request.QueryString["oauth_token"].ToString();
                    oauth_verifier = Request.QueryString["oauth_verifier"].ToString();
                    ConnectToTwitter(oauth_token, oauth_verifier);
                    GetTwitterData();                
                }
            }
        }

        protected void btn_Linkedin_Click(object sender, EventArgs e)
        {
            if (Session["reuqestTokenSecret"] == null && Session["reuqestToken"] == null)            
                ConnectToLinkedin("", "");            
            else
                GetCurrentUserData();            
        }

        protected void btn_Twitter_Click(object sender, EventArgs e)
        {
            if (Session["Twi_reuqestTokenSecret"] == null && Session["Twi_reuqestToken"] == null)
                ConnectToTwitter("", "");
            else
                GetTwitterData();
        }

        private void ConnectToLinkedin(string oauth_token, string oauth_verifier)
        {
            if (Request.QueryString["oauth_token"] == null && Request.QueryString["oauth_verifier"] == null)
            {
                #region RequestToken

                string request_token = Li_REQUEST_TOKEN;
                string response = "";
                string callbackurl = "";
                string strURLChecking = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Host);

                if (Request.Url.Host == "localhost")
                    callbackurl = "http://localhost:51658/Default.aspx?SocialNetwork=3";
                else if (strURLChecking.Contains("dev") == true)
                    callbackurl = strURLChecking + "/Default.aspx?SocialNetwork=3";
                else
                    callbackurl = strURLChecking + "/Default.aspx?SocialNetwork=3";

                string encodevalue = HttpUtility.UrlEncode("r_emailaddress+r_network");

                //request_token += "?scope=" + encodevalue;
                response = oauth_obj.GetPost(request_token, Li_consumerKey, Li_consumerSecret, "", "", callbackurl, "", OAuth.WebMethod.POST);

                if (response.Length > 0)
                {
                    NameValueCollection qs = HttpUtility.ParseQueryString(response);
                    if (qs["oauth_token"] != null)
                    {
                        Session["reuqestTokenSecret"] = qs["oauth_token_secret"];
                        response = Li_AUTHORIZE + "?oauth_token=" + qs["oauth_token"];
                        Response.Redirect(response);
                    }
                }

                #endregion
            }
            else
            {
                #region AccessToken/Data Display

                oauth_token = Request.QueryString["oauth_token"].ToString();
                oauth_verifier = Request.QueryString["oauth_verifier"].ToString();
                string response = "";
                if (Session["reuqestTokenSecret"] != null)
                {
                    response = oauth_obj.GetPost(Li_ACCESS_TOKEN, Li_consumerKey, Li_consumerSecret, oauth_token, Session["reuqestTokenSecret"].ToString(), "", oauth_verifier, OAuth.WebMethod.POST);
                    if (response.Length > 0)
                    {
                        NameValueCollection qs = HttpUtility.ParseQueryString(response);
                        if (qs["oauth_token"] != null)
                        {
                            Session["reuqestTokenSecret"] = qs["oauth_token_secret"];
                            Session["reuqestToken"] = qs["oauth_token"];
                        }
                    }
                }
                #endregion
            }
        }

        private void ConnectToTwitter(string oauth_token, string oauth_verifier)
        {
            if (Request.QueryString["oauth_token"] == null && Request.QueryString["oauth_verifier"] == null)
            {
                #region RequestToken

                string response = "";
                string callbackurl = "";
                string strURLChecking = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Host);

                if (Request.Url.Host == "localhost")
                    callbackurl = "http://localhost:51658/Default.aspx?SocialNetwork=3";
                else if (strURLChecking.Contains("dev") == true)
                    callbackurl = strURLChecking + "/Default.aspx?SocialNetwork=3";
                else
                    callbackurl = strURLChecking + "/Default.aspx?SocialNetwork=3";

                response = oauth_obj.GetPost(Twi_REQUEST_TOKEN, Twi_consumerKey, Twi_consumerSecret, "", "", callbackurl, "", OAuth.WebMethod.GET);

                if (response.Length > 0)
                {
                    NameValueCollection qs = HttpUtility.ParseQueryString(response);
                    if (qs["oauth_token"] != null)
                    {
                        Session["Twi_reuqestTokenSecret"] = qs["oauth_token_secret"];
                        response = Twi_AUTHORIZE + "?oauth_token=" + qs["oauth_token"];
                        Response.Redirect(response);
                    }
                }

                #endregion
            }
            else
            {
                #region AccessToken/Data Display

                oauth_token = Request.QueryString["oauth_token"].ToString();
                oauth_verifier = Request.QueryString["oauth_verifier"].ToString();
                string response = "";
                if (Session["Twi_reuqestTokenSecret"] != null)
                {
                    response = oauth_obj.GetPost(Twi_ACCESS_TOKEN, Twi_consumerKey, Twi_consumerSecret, oauth_token, Session["Twi_reuqestTokenSecret"].ToString(), "", oauth_verifier, OAuth.WebMethod.GET);
                    if (response.Length > 0)
                    {
                        NameValueCollection qs = HttpUtility.ParseQueryString(response);
                        if (qs["oauth_token"] != null)
                        {
                            Session["Twi_reuqestTokenSecret"] = qs["oauth_token_secret"];
                            Session["Twi_reuqestToken"] = qs["oauth_token"];
                        }
                    }
                }
                #endregion
            }
        }

        private void GetCurrentUserData()
        {
            string data = "";
            string url = "http://api.linkedin.com/v1/people/~:(id,first-name,last-name,email-address,industry,headline,current-status,specialties,interests,positions,languages,skills,educations,phone-numbers,date-of-birth,picture-url,public-profile-url,location,certifications,recommendations-received,publications)?format=json";
            //data = oauth_obj.Get(url, Li_consumerKey, Li_consumerSecret, Session["reuqestToken"].ToString(), Session["reuqestTokenSecret"].ToString(), "", "", OAuth.WebMethod.GET);
            
            url = "http://api.linkedin.com/v1/people/~/email-address?format=json";
            //data = data + "<br/><br/><br/><br/>" + oauth_obj.Get(url, Li_consumerKey, Li_consumerSecret, Session["reuqestToken"].ToString(), Session["reuqestTokenSecret"].ToString(), "", "", OAuth.WebMethod.GET);

            url = "http://api.linkedin.com/v1/people/~/connections:(id,first-name,last-name,email-address,industry,headline,current-status,specialties,interests,positions,languages,skills,educations,phone-numbers,date-of-birth,picture-url,public-profile-url,location,certifications,recommendations-received,publications)?format=json";
            data = data + "<br/><br/><br/><br/>" + oauth_obj.Get(url, Li_consumerKey, Li_consumerSecret, Session["reuqestToken"].ToString(), Session["reuqestTokenSecret"].ToString(), "", "", OAuth.WebMethod.GET);
            JSONData.InnerHtml = data;

        }

        private void GetTwitterData()
        {
            string url = "http://search.twitter.com/search.json?q=pepsi&include_entities=true&result_type=mixed";
            string data = oauth_obj.Get(url, Twi_consumerKey, Twi_consumerSecret, Session["Twi_reuqestToken"].ToString(), Session["Twi_reuqestTokenSecret"].ToString(), "", "", OAuth.WebMethod.GET);
            JSONData.InnerHtml = data;
            //Testing test = JsonConvert.DeserializeObject<Testing>(data);
            //url = "http://search.twitter.com/search.json" + test.refresh_url;
            //data = oauth_obj.Get(url, Twi_consumerKey, Twi_consumerSecret, Session["Twi_reuqestToken"].ToString(), Session["Twi_reuqestTokenSecret"].ToString(), "", "", OAuth.WebMethod.GET);
            //JSONData.InnerHtml = data;
        }

        public class Testing
        {
            public string refresh_url { get; set; }
        }
    }
}
