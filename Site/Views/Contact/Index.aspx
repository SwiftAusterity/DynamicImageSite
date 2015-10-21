<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Site.ViewModels.ContactFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.ContentPage.Title %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="pane  <%= Model.MainClass %>">
        <div class="pane-inner">
            <div class="main"><% if (!String.IsNullOrEmpty(Model.ContentPage.PartialLocation)) { Html.RenderPartial(Model.ContentPage.PartialLocation, Model); } %></div>
        </div>
    </section>
</asp:Content>
