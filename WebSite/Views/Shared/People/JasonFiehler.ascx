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
                <h1>Jason Fiehler – Founder &amp; CEO</h1>
            </header>
            <div class="content">
                <p>
                    Jason started Infuz in 2000. Since then, the agency has grown more than ten-fold in number of employees, clients and digital projects flowing through the halls. He’s no stranger to knowing what it takes to accomplishing big things, as he once climbed the Great Pyramid of Khufu on the Giza Plateau in Egypt. A graduate of the University of Richmond, he earned a degree in Business Administration. Jason’s career highlights include spearheading one of the largest consumer-based campaigns for SC Johnson, attracting millions of active members. When he’s not at the office, Jason is probably playing the latest video game, scuba diving, admiring his <a href="http://static.ddmcdn.com/gif/shark-teeth-4.jpg" target="_blank">megalodon shark</a> teeth or searching for geocache out in the wild. 
                </p>
                <ul>
                    <li><strong>Believe it or Not:</strong> He Has a Deep Appreciation and Respect for the Life and Work of <a href="http://www.imdb.com/name/nm0000200/" target="_blank">Bill Paxton</a></li>
                    <li><strong>Favorite Website:</strong> <a href="http://www.damninteresting.com/" target="_blank">Damn Interesting</a></li>
                    <li><strong>Worst Movie Ever:</strong> Anything by Michael Bay</li>
                    <li><strong>Alternate Career:</strong> International Underwater Treasure Hunter</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="https://www.facebook.com/jasonfiehler" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="http://twitter.com/jasonfiehler" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="http://www.linkedin.com/in/jasonfiehler" target="_blank" rel="nofollow" title="LinkedIn"><img alt="Linkedin" src="/Content/images/SMIcons/linkedin.png"/></a>
                    <a href="http://stlindex.com/User/jasonfiehler" target="_blank" rel="nofollow" title="STLIndex"><img alt="STLIndex" src="/Content/images/SMIcons/stlindex.png"/></a>
                    <a href="http://www.last.fm/user/jfiehler" target="_blank" rel="nofollow" title="Last.FM"><img alt="Last.FM" src="/Content/images/SMIcons/lastfm.png"/></a>
                    <a href="http://open.spotify.com/user/jasonfiehler" target="_blank" rel="nofollow" title="Spotify"><img alt="Spotify" src="/Content/images/SMIcons/spotify.png"/></a>
              </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>