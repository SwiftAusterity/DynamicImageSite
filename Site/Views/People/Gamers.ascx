<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>
<%
    var viewId = Model.ContentPage.PartialLocation.Replace("~/", "").Replace("/", "-").Replace(".ascx", "").ToLower();
%>
<div id="<%= viewId %>" style="display: none;"></div>

<div class="block left">
    <div class="intro">
        <section>
            <header>
                <h1>Gamers</h1>
            </header>
            <div class="content">
                <p>
                    When not conquering the world of digital marketing, many of us enjoy conquering
                    imaginary worlds on our favorite gaming consoles. All of those in this group also
                    know the cheat-code for unlimited lives in Contra by heart. So in case you forgot
                    it, hit one of us up.
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

<script>
    $(function() {
        // Add a callback specific to this pane.
        // 'this' will be the pane that this partial is loaded into.
        window.Infuz.PageTransition.addPaneCallback(
            '<%= viewId %>',
            window.Infuz.Utility.Codes.addGamers
        );
    });
</script>
