using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace SocialNetworkTesting
{
    public partial class FourSquareTesting : System.Web.UI.Page
    {
        private const string FourSquareClientID = "IH111LIKFAFNXTEUQGKQ3SSV3MXWXS3Q3FEMABROFXIX0F2H";
        private const string FourSquareClientSecret = "1BJSV1HA5ONR0RFVM4OV05HGSRUU24KQEKCJNSM0FPCEGF1I";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null && !IsPostBack)
            {
                Session["FourSquareCode"] = Request.QueryString["code"].ToString();
                ConnetToFourSquare();
            }
        }

        protected void btn_FourSquare_Click(object sender, EventArgs e)
        {
            if (Session["FourSquareToken"] != null)
                GetFourSquare();
            else
                ConnetToFourSquare();
        }

        private void ConnetToFourSquare()
        {
            string url = "";
            if (Request.QueryString["code"] == null && Session["FourSquareToken"] == null)
            {
                Response.Redirect("https://foursquare.com/oauth2/authenticate?client_id=" + FourSquareClientID + "&response_type=code&redirect_uri=http://localhost:5165/FourSquareTesting.aspx");
            }
            else if (Request.QueryString["code"] != null && Session["FourSquareToken"] == null)
            {
                url = "https://foursquare.com/oauth2/access_token?client_id=" + FourSquareClientID + "&client_secret=" + FourSquareClientSecret + "&code=" + Request.QueryString["code"].ToString() + "&grant_type=authorization_code&redirect_uri=http://localhost:51658/FourSquareTesting.aspx";
                string response = WebRequest(OAuth.WebMethod.GET, url, "");
                AcessToken acesstoken = new AcessToken();
                acesstoken = JsonConvert.DeserializeObject<AcessToken>(response);
                Session["FourSquareToken"] = acesstoken.access_token;
                GetFourSquare();
            }
        }

        private void GetFourSquare()
        {            
            DateTime todaydate = DateTime.Now;
            string strMonth = todaydate.Month < 10 ? "0" + todaydate.Month.ToString() : todaydate.Month.ToString();
            string strDay = todaydate.Day < 10 ? "0" + todaydate.Day.ToString() : todaydate.Day.ToString();
            string str_Date = todaydate.Year.ToString() + strMonth + strDay;

            string url = "https://api.foursquare.com/v2/users/self?oauth_token=" + Session["FourSquareToken"].ToString() + "&v=" + str_Date;
            JSONData.InnerHtml = WebRequest(OAuth.WebMethod.GET, url, "");
        }

        public string WebRequest(OAuth.WebMethod method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            //webRequest.UserAgent  = "Identify your application please.";
            //webRequest.Timeout = 20000;

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
    public class AcessToken
    {
        public string access_token { get; set; }
    }


}