using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace SocialNetworkTesting
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string UserName = "S2yALN9tJo9ny2mG:0b98ddedb8f15c44c69b60fbaa3daf4ec351c34307b5caef80ecb3dc0f81576c";
            sb.AppendLine(UserName);
            UTF8Encoding enc = new UTF8Encoding();
            byte[] b = enc.GetBytes(UserName);
            string cvtd = Convert.ToBase64String(b);
            sb.AppendLine(cvtd);
            byte[] c = Convert.FromBase64String(cvtd);
            string backAgain = enc.GetString(c);
            sb.AppendLine(backAgain);

        }
    }
}
