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
                <h1>Heath Harris – Senior Art Director</h1>
            </header>
            <div class="content">
                <p>
                    A self-taught creative, Heath claims he earned his BA in Persuasion from the Streets of St. Louis. His approach is based on creating connections through 
                    art, be it a user interface, <a href="http://www.flickr.com/photos/djdenim/545132482/">geodesic dome</a> or photograph. You may have seen Heath’s work through <a href="http://www.artcrank.com/stlouis">ArtCrank</a>, as his bicycle poster contributions have been 
                    on display and hung around town. As an avid <a href="http://www.flickr.com/photos/djdenim/5614431754/in/set-72157626360870739">explorer</a> of abandoned buildings who was once Midwest-famous for throwing <a href="http://www.mindplasma.net/Root_Folder/Html_Files/Events/1990s/0_1990s.html">raves</a>, Heath is truly a unique 
                    character and one-of-a-kind talent we’re delighted to have on board, even though he occasionally rocks the boat.
                </p>
                <ul>
                    <li><strong>First Concert:</strong> Peaches &amp; Herb at Six Flags</li>
                    <li><strong>Best Known at the Office For:</strong> Elevator Art Installations</li>
                    <li><strong>Greatest Accomplishment:</strong> The So-far Successful Raising of Three Children</li>
                    <li><strong>Favorite Website:</strong> <a href="http://Superbad.com/" target="_blank">superbad.com</a></li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://twitter.com/dj_denim" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="http://www.flickr.com/photos/djdenim/" target="_blank" rel="nofollow" title="Flickr"><img alt="Flickr" src="/Content/images/SMIcons/flickr.png"/></a>
                    <a href="http://heathropolis.tumblr.com/" target="_blank" rel="nofollow" title="Tumblr"><img alt="Tumblr" src="/Content/images/SMIcons/tumblr.png"/></a>
                    <a href="http://logosfeaturingthesaintlouisarch.tumblr.com/" target="_blank" rel="nofollow" title="Logos Featuring The Saint Louis Arch"><img alt="Tumblr" src="/Content/images/SMIcons/tumblr.png"/></a>
                    <a href="http://www.linkedin.com/in/heathropolis" target="_blank" rel="nofollow" title="LinkedIn"><img alt="Linkedin" src="/Content/images/SMIcons/linkedin.png"/></a>
                    <a href="http://pinterest.com/heaps/" target="_blank" rel="nofollow" title="Pinterest"><img alt="Pinterest" src="/Content/images/SMIcons/pinterest.png"/></a>
                    <a href="http://www.behance.net/heathropolis" target="_blank" rel="nofollow" title="Behance"><img alt="Behance" src="/Content/images/SMIcons/behance.png"/></a>
                    <a href="http://gplus.to/heathharris" target="_blank" rel="nofollow" title="Google&#43;"><img alt="Google+" src="/Content/images/SMIcons/googleplus.png"/></a>
                    <a href="http://stlindex.com/User/dj_denim" target="_blank" rel="nofollow" title="STLIndex"><img alt="STLIndex" src="/Content/images/SMIcons/stlindex.png"/></a>
                    <a href="http://vimeo.com/15527961" target="_blank" rel="nofollow" title="Vimeo"><img alt="Vimeo" src="/Content/images/SMIcons/vimeo.png"/></a>
              </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>