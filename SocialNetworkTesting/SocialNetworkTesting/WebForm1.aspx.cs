using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace SocialNetworkTesting
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string RequestToken = "http://restapi.surveygizmo.com/head/oauth/request_token";
        string Authencatetion = "http://restapi.surveygizmo.com/head/oauth/authenticate?oauth_token=";
        string URLAccessToken = "http://restapi.surveygizmo.com/head/oauth/access_token";
        string ConsumerKey = "2af16cf8dd391a1764da1e432aa84dd50521c6d44", ConsumerSecret = "eccf19bf273aaada18001632ca422495";
        //string ConsumerKey = "551cd8b73cfd260a4662f7ed05ed3ad40521c659e", ConsumerSecret = "ac1f2ff35e2295b393e4c07741d70b90";

        OAuth oauth_obj = new OAuth();
        string output = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string response = "";

            if (Session["Token"] != null && Session["TokenSecret"] != null)
            {
                string Token = Session["Token"].ToString(), TokenSecret = Session["TokenSecret"].ToString();
                string URLAccountUser = "https://restapi.surveygizmo.com/v2/survey/1279808/surveyresponse/1";
                response = oauth_obj.Get(URLAccountUser, ConsumerKey, ConsumerSecret, Token, TokenSecret, "", "", OAuth.WebMethod.GET);
            }
            else
            {
                if (Session["oauth_token"] != null && Session["oauth_token_secret"] != null)
                {
                    string oauth_token = "", oauth_verifier = "";
                    oauth_token = Request.QueryString["oauth_token"].ToString();
                    oauth_verifier = Request.QueryString["oauth_verifier"].ToString();
                    response = oauth_obj.Get(URLAccessToken, ConsumerKey, ConsumerSecret, oauth_token, "", "", oauth_verifier, OAuth.WebMethod.GET);
                    NameValueCollection qs = HttpUtility.ParseQueryString(response);
                    Session["Token"] = qs["oauth_token"];
                    Session["TokenSecret"] = qs["oauth_token_secret"];

                    string URLAccountUser = "http://restapi.surveygizmo.com/head/AccountUser.debug";
                    response = oauth_obj.Get(URLAccountUser, ConsumerKey, ConsumerSecret, qs["oauth_token"], qs["oauth_token_secret"], "", "", OAuth.WebMethod.GET);
                }
                else
                {
                    string callbackurl = "http://localhost:51658/WebForm1.aspx";
                    response = oauth_obj.Get(RequestToken, ConsumerKey, ConsumerSecret, "", "", callbackurl, "", OAuth.WebMethod.GET);
                    NameValueCollection qs = HttpUtility.ParseQueryString(response);
                    Session["oauth_token"] = qs["oauth_token"];
                    Session["oauth_token_secret"] = qs["oauth_token_secret"];
                    string URL = Authencatetion + qs["oauth_token"] + "&oauth_callback=" + HttpUtility.UrlEncode(callbackurl);
                    Response.Redirect(URL);
                }
            }
            output = response;
        }
    }

    public class ClsRequestToken
    {
        public string oauth_callback_confirmed { get; set; }
        public string oauth_token { get; set; }
        public string oauth_token_secret { get; set; }
        public string xoauth_token_ttl { get; set; }
    }
}