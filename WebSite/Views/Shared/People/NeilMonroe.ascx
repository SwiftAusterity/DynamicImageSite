<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewModel>" %>

<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>
<%@ Import Namespace="Site.Data.API" %>

<%
    var viewId = Model.ContentPage.PartialLocation.Replace("~/", "").Replace("/", "-").Replace(".ascx", "").ToLower();
%>
<div id="<%= viewId %>" style="display: none;"></div>

<div class="block left">
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

<div class="block right">
    <div class="intro">
        <section>
            <header>
                <h1>Neil Monroe – Senior Interactive Developer</h1>
            </header>
            <div class="content">
                <p>
                    As a front-end developer, Neil is a master of HTML, CSS, and his specialty: <a href="http://www.amazon.com/JavaScript-Dummies-Emily-Vander-Veer/dp/0764576593" target="_blank">JavaScript</a>. A St. Louis native, Neil attended Purdue University to pursue a degree in Computer Science. Since then, he’s always stayed ahead of the game by teaching himself the latest technical languages and code as they hit the marketplace. When he’s not deciphering code, Neil enjoys long-distance running as he once finished the St. Louis marathon in less than five hours.</p>
                <ul>
                    <li><strong>Believe it or Not:</strong> Neil Has Never Been to Delaware</li>
                    <li><strong>Favorite Website:</strong> <a href="http://Zombo.com/" target="_blank">Zombo.com</a></li>
                    <li><strong>Worst Movie Ever:</strong> Transmorphers 2: The Fall of Man</li>
                    <li><strong>Alternate Career:</strong> Sports Physical Therapist</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.facebook.com/neil.monroe.me" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="https://twitter.com/#!/arcadian" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="https://github.com/neilmonroe" target="_blank" rel="nofollow" title="GitHub"><img alt="GitHub" src="/Content/images/SMIcons/github.png"/></a>
                    <a href="https://plus.google.com/107896613925140911119/about" target="_blank" rel="nofollow" title="Google&#43;"><img alt="Google+" src="/Content/images/SMIcons/googleplus.png"/></a>
                    <a href="http://www.linkedin.com/pub/neil-monroe/1/a48/809" target="_blank" rel="nofollow" title="LinkedIn"><img alt="Linkedin" src="/Content/images/SMIcons/linkedin.png"/></a>
                    <a href="http://neilmonroe.yelp.com/" target="_blank" rel="nofollow" title="Yelp"><img alt="Yelp" src="/Content/images/SMIcons/yelp.png"/></a>
                </div>
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
