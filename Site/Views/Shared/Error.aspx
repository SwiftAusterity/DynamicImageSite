<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Static.Master" Inherits="System.Web.Mvc.ViewPage<Site.ViewModels.ErrorViewModel>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>
<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="pane error">
        <div class="pane-inner">
            <div class="main"><h2>Sorry, an error occurred while processing your request.</h2></div>
        </div>
    </section>
</asp:Content>
