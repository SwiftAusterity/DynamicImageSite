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
                <h1>Michael Bischoff - Interactive Designer</h1>
            </header>
            <div class="content">
                <p>
                    He may be a man of few words, but this young designer has many talents. Michael lives, breathes and eats
                    interactive and motion graphics and when he’s not in front of a monitor, you can find him behind the lens
                    documenting life and landscapes. A graduate from Maryville University’s Interactive Design program, Michael
                    loves that his job entails creating awesome things for clients.
                </p>
                <ul>
                    <li><strong>Best Band Ever:</strong> Rush</li>
                    <li><strong>If he were an animal, he’d be:</strong> A Wolf</li>
                    <li><strong>Best Movie Ever:</strong> Fight Club</li>
                    <li><strong>First Concert:</strong> Boston and Styx (or what was left of them)</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://pinterest.com/m1keyb" target="_blank" rel="nofollow" title="Pinterest"><img alt="Pinterest" src="/Content/images/SMIcons/pinterest.png"/></a>
                    <a href="https://vimeo.com/user3122363" target="_blank" rel="nofollow" title="Vimeo"><img alt="Vimeo" src="/Content/images/SMIcons/vimeo.png"/></a>
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
