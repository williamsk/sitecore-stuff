using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Sitecore.StringExtensions;
using Sitecore.Xml;

namespace SitecoreStuff.Foundation.HyperlinkManagerExtender
{
    public class HyperlinkManagerExtender : IHyperlinkManagerExtender
    {
        public string Markup
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(GetMarkup());
                sb.Append(GetCleanScript());
                sb.Append(GetLoadLinkScript());
                sb.Append(GetModifyLinkScript());
                return sb.ToString();
            }
        }

        public List<Extension> Extensions { get; private set; }

        public HyperlinkManagerExtender()
        {
            Extensions = new List<Extension>();
        }

        public virtual void AddExtension(System.Xml.XmlNode node)
        {
            Extensions.Add(new Extension()
            {
                Name = XmlUtil.GetAttribute("name", node),
                Markup = XmlUtil.GetAttribute("markup", node),
                CleanFunc = XmlUtil.GetAttribute("clean", node),
                LoadLinkFunc = XmlUtil.GetAttribute("loadLink", node),
                ModifyLinkFunc = XmlUtil.GetAttribute("modifyLink", node)
            });
        }

        public class Extension
        {
            public string Name { get; set; }
            public string Markup { get; set; }
            public string CleanFunc { get; set; }
            public string LoadLinkFunc { get; set; }
            public string ModifyLinkFunc { get; set; }
        }

        protected virtual string GetMarkup()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            foreach (var extension in Extensions)
                sb.Append(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(extension.Markup)));
            sb.AppendLine();
            return sb.ToString();
        }

        protected virtual string GetCleanScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("<script>");
            sb.AppendLine("oldXSC = Telerik.Web.UI.Widgets.LinkManager.prototype._hyperlinkManagerExtenderSetupChildren;");
            sb.AppendLine("Telerik.Web.UI.Widgets.LinkManager.prototype._hyperlinkManagerExtenderSetupChildren = function () {");
            foreach (var extension in Extensions)
                sb.AppendLine("{0}();".FormatWith(extension.CleanFunc));
            sb.AppendLine("oldXSC.apply(this, null);");
            sb.AppendLine("}</script>");
            return sb.ToString();
        }

        protected virtual string GetLoadLinkScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("<script>");
            sb.AppendLine("oldXLLP = Telerik.Web.UI.Widgets.LinkManager.prototype._hyperlinkManagerExtenderLoadLinkProperties;");
            sb.AppendLine("Telerik.Web.UI.Widgets.LinkManager.prototype._hyperlinkManagerExtenderLoadLinkProperties = function (currentLink) {");
            foreach (var extension in Extensions)
                sb.AppendLine("{0}(currentLink);".FormatWith(extension.LoadLinkFunc));
            sb.AppendLine("oldXLLP.apply(this, currentLink);");
            sb.AppendLine("}</script>");
            return sb.ToString();
        }

        protected virtual string GetModifyLinkScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("<script>");
            sb.AppendLine("oldXGML = Telerik.Web.UI.Widgets.LinkManager.prototype._hyperlinkManagerExtenderGetModifiedLink;");
            sb.AppendLine("Telerik.Web.UI.Widgets.LinkManager.prototype._hyperlinkManagerExtenderGetModifiedLink = function (resultLink) {");
            foreach (var extension in Extensions)
                sb.AppendLine("{0}(resultLink);".FormatWith(extension.ModifyLinkFunc));
            sb.AppendLine("oldXGML.apply(this, resultLink);");
            sb.AppendLine("}</script>");
            return sb.ToString();
        }
    }
}