using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace SocialNetworkTesting
{
    public partial class Testing : System.Web.UI.Page
    {
        private const string FacebookClientID = "405277099497702";
        private const string FacebookClientSecret = "9937c4e010b0b1875b37a28784287bc5";

        //private const string FacebookClientID = "230415300341432";
        //private const string FacebookClientSecret = "7d39975c33450fe549436a1f06450fb8";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null && !IsPostBack)
            {
                Session["FacebookCode"] = Request.QueryString["code"].ToString();
                ConnectToFacebook();
            }
            else
            {
                ConnectToFacebook();
            }
        }

        protected void btn_Facebook_Click(object sender, EventArgs e)
        {
            if (Session["FacebookToken"] != null)
                GetFacebookData();
            else
                ConnectToFacebook();
        }

        private void ConnectToFacebook()
        {
            string url = "";
            if (Request.QueryString["code"] == null && Session["FacebookToken"] == null)
            {
                Response.Redirect("https://www.facebook.com/dialog/oauth?client_id=" + FacebookClientID + "&scope=user_birthday,email&redirect_uri=http://localhost:51658/Testing.aspx");
            }
            else if (Request.QueryString["code"] != null && Session["FacebookToken"] == null)
            {
                url = "https://graph.facebook.com/oauth/access_token?client_id=" + FacebookClientID + "&client_secret=" + FacebookClientSecret + "&display=popup&code=" + Request.QueryString["code"].ToString() + "&redirect_uri=http://localhost:51658/Testing.aspx";
                string response = WebRequest(OAuth.WebMethod.GET, url, "");
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                Session["FacebookToken"] = qs["access_token"].ToString();
                GetFacebookData();
            }
        }

        private void GetFacebookData()
        {
            string url = "https://graph.facebook.com/me?fields=id,name&sdk=ios&sdk_version=2&access_token=" + Session["FacebookToken"].ToString() + "&format=json";
            string urlFeed = "https://graph.facebook.com/me/feed?access_token=" + Session["FacebookToken"].ToString() + "&format=json";
            JSONData.InnerHtml = WebRequest(OAuth.WebMethod.GET, url, "");
            string data = WebRequest(OAuth.WebMethod.GET, urlFeed, "");
            try
            {
                FacebookFeed _data = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookFeed>(data);
                for (int i = 0; i < _data.data.Count; i++)
                {
                    string content = _data.data[i].id;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
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

    public class FacebookCommon
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class FacebookFeed
    {
        public List<FacebookFeedData> data { get; set; }
    }

    public class FacebookFeedData
    {
        public string id { get; set; }
        public FacebookCommon from { get; set; }
        public string story { get; set; }
        public FacebookFeedStoryTags story_tags { get; set; }
    }

    public class FacebookFeedStoryTags
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        public List<FacebookFeedStoryTagsData> this[string name]
        {
            get
            {
                if (properties.ContainsKey(name))
                {
                    //return properties[name];
                }
                return null;
            }
            set
            {

            }
        }
    }

    public class FacebookFeedStoryTagsData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string offset { get; set; }
        public string length { get; set; }
        public string type { get; set; }
    }


}