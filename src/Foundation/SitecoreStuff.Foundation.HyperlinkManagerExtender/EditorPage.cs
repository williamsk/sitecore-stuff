using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreStuff.Foundation.HyperlinkManagerExtender
{
    public class EditorPage : Sitecore.Shell.Controls.RichTextEditor.EditorPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Editor.ExternalDialogsPath = "~/sitecore modules/Sitecore Stuff/HyperlinkManagerExtender";
        }
    }
}
