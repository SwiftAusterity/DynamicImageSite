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
                <h1>Erica Smith - Digital Engagement Specialist</h1>
            </header>
            <div class="content">
                <p>
                    Breaking news alerts, military history and tacos might be a few of her favorite things, but an
                    infectious laugh is what this <a href="http://www.southforkregulators.org/" target="_blank">Blackwater, Missouri</a> native is best known for around the office.
                    Our clients count on Erica to deliver smart digital engagement strategies, but we love the fact
                    that she always knows where the food trucks will be lining up at lunchtime.  When she's not on
                    Twitter or curating content, Erica might be busy sorting M&amp;M's into a perfect Roy G Biv ensemble.
                </p>
                <ul>
                    <li><strong>Hardest Word to Spell:</strong> Separate</li>
                    <li><strong>Best Band Ever:</strong> The Beautiful South</li>
                    <li><strong>Best Movie Ever:</strong> Casablanca</li>
                    <li><strong>First Concert:</strong> Garth Brooks</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.twitter.com/ericasmith" title="Twitter" target="_blank" rel="nofollow"><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="http://www.facebook.com/liveandkern" target="_blank" title="Facebook" rel="nofollow"><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="http://storify.com/ericasmith" target="_blank" rel="nofollow" title="Storify"><img alt="Storify" src="/Content/images/SMIcons/storify.png"/></a>
                    <a href="http://kernmore.tumblr.com/" target="_blank" rel="nofollow" title="Tumblr"><img alt="Tumblr" src="/Content/images/SMIcons/tumblr.png"/></a>
                    <a href="http://www.pinterest.com/liveandkern" target="_blank" rel="nofollow" title="Pinterest"><img alt="Pinterest" src="/Content/images/SMIcons/pinterest.png"/></a>
                    <a href="http://www.foursquare.com/ericasmith" target="_blank" rel="nofollow" title="foursquare"><img alt="foursquare" src="/Content/images/SMIcons/foursquare.png"/></a>
                    <a href="http://gplus.to/ericasmith" target="_blank" rel="nofollow" title="Google&#43;"><img alt="Google+" src="/Content/images/SMIcons/googleplus.png"/></a>
                    <a href="http://www.linkedin.com/in/esmith13" target="_blank" rel="nofollow" title="LinkedIn"><img alt="LinkedIn" src="/Content/images/SMIcons/linkedin.png"/></a>
                    <a href="http://instagrid.me/liveandkern/" onclick="return false" target="_blank" rel="nofollow" title="Instagram"><img alt="Instagram" src="/Content/images/SMIcons/instagram.png"/></a>
                    <a href="http://liveandkern.etsy.com" target="_blank" rel="nofollow" title="Etsy"><img alt="Etsy" src="/Content/images/SMIcons/etsy.png"/></a>
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
