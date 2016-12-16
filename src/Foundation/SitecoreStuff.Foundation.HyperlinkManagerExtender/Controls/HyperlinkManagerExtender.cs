using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Sitecore.Configuration;

namespace SitecoreStuff.Foundation.HyperlinkManagerExtender.Controls
{
    public class HyperlinkManagerExtender : WebControl
    {
        private static IHyperlinkManagerExtender Extender = null;

        static HyperlinkManagerExtender()
        {
            Extender = Factory.CreateObject("SitecoreStuff/HyperlinkManagerExtender", true) as IHyperlinkManagerExtender;
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(Extender.Markup);
        }
    }
}
