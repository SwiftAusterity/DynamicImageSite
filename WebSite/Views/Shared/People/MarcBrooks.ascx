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
                <h1>Marc Brooks – Hack Prime</h1>
            </header>
            <div class="content">
                <p>
                    Known around these parts as “Mr. STL Tweets” Marc is the guy who is obsessed with digging into, analyzing and curating data—usually having to do with 
                    Twitter. Originally from Albuquerque, New Mexico, famous for its <a href="http://www.youtube.com/watch?v=e8TUwHTfOOU" target="_blank" rel="nofollow">wrong turns</a>, Marc studied Computer Science and Electronics Engineering at Washington 
                    University. He’s been honing his computer genius skills since 1980, over the years contributing code to several Microsoft and open-source projects. When 
                    he’s not facing a monitor, Marc is can be found in the stands at a <a href="http://blues.nhl.com/" target="_blank" rel="nofollow">Blues</a> game, at home building furniture or in his garage tinkering with his car. 
                </p>
                <ul>
                    <li><strong>Best Band Ever:</strong><a href="http://www.77s.com" target="_blank" rel="nofollow"> The 77’s</a></li>
                    <li><strong>Best Website Ever:</strong> <a href="http://zombo.com/" target="_blank" rel="nofollow">zombo.com</a></li>
                    <li><strong>Best Known Around The Office For:</strong><a href="http://www.forumsforums.com/3_9/showthread.php?t=53759" target="_blank" rel="nofollow"> Awful Puns</a></li>
                    <li><strong>Strangest Possession:</strong><a href="http://en.wikipedia.org/wiki/Blowgun" target="_blank" rel="nofollow"> Five-Foot Blowgun</a></li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="https://www.facebook.com/idisposable" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="http://www.twitter.com/IDisposable" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="http://www.linkedin.com/in/marcbrooks" target="_blank" rel="nofollow" title="LinkedIn"><img alt="Linkedin" src="/Content/images/SMIcons/linkedin.png"/></a>
                    <a href="https://foursquare.com/idisposable" target="_blank" rel="nofollow" title="Foursquare"><img alt="Foursquare" src="/Content/images/SMIcons/foursquare.png"/></a>
                    <a href="http://about.me/IDisposable" target="_blank" rel="nofollow" title="About.Me"><img alt="About.Me" src="/Content/images/SMIcons/aboutme.png"/></a>
                    <a href="http://stackoverflow.com/users/2076/idisposable" target="_blank" rel="nofollow" title="StackOverflow"><img alt="StackOverflow" src="/Content/images/SMIcons/stackoverflow.png"/></a>
                    <a href="http://www.last.fm/user/IDisposable" target="_blank" rel="nofollow" title="Last.FM"><img alt="Last.FM" src="/Content/images/SMIcons/lastfm.png"/></a>
              </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>