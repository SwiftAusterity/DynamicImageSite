<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Static.Master" Inherits="System.Web.Mvc.ViewPage<Site.ViewModels.ErrorViewModel>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Missing
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="pane missing" style="z-index:10;">
        <div class="pane-inner">
            <div class="main">

                <div class="block left">
                    <div class="intro">
                        <section>
                            <header>
                                <h1 class="alt-content">404 MISSING</h1>
                            </header>
                            <div class="content">
                                <p>
                                </p>
                            </div>
                            <footer>
                            </footer>
                        </section>
                    </div>
                </div>

                <div class="block right">
                    <div class="body">
                        <section>
                            <header>
                            </header>
                            <div class="content">
                            </div>
                            <footer>
                            </footer>
                        </section>
                    </div>
                </div>

                <span class="vshim"></span>

                <% Html.RenderPartial("~/Views/Shared/Spotlight.ascx"); %>

            </div>
        </div>
    </section>
</asp:Content>
