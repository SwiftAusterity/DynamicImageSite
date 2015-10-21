<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<MobileViewModel>" %>

<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>
<%@ Import Namespace="Site.Data.API" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.ContentPage.Title %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="pane <%= Model.MainClass %>">
        <div class="pane-inner">
            <div class="main">
                <% foreach (IContentPage page in Model.SectionPages)
                       if (!String.IsNullOrEmpty(page.PartialLocation)) 
                        {
                            Html.RenderPartial(page.PartialLocation, Model); 
                        } %>
            </div>
        </div>
    </section>
</asp:Content>
