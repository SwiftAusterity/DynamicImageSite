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
                <h1>Chris Sturgeon - VP, Account Services</h1>
            </header>
            <div class="content">
                <p>
                    A Tennessee native (<a href="http://www.govolsgo.com/" target="_blank">Go VOLS!</a>), Chris comes to us
                    by way of Los Angeles, California where he has spent the last 18 years
                    managing digital strategies for a number of well known brands and
                    agencies. With
                    degrees in English, Music and Screenwriting, Chris is a man of many
                    talents. His ability to insert &ldquo;Jaws&rdquo;  references into presentations is
                    unprecedented. Besides his new commute, cooking may be his favorite past
                    time, and when he’s not working he’s most likely found roaming the aisles
                    of his local grocery store. Ironically, we’ve found he has a hard time
                    spelling the word &ldquo;cuisine&rdquo;.
                </p>
                <ul>
                    <li><strong>First Job:</strong> Pill Counter, Stan’s Pharmacy</li>
                    <li><strong>Worst Movie:</strong> The Room</li>
                    <li><strong>Favorite Artist:</strong> Eric Clapton</li>
                    <li><strong>First Concert:</strong> Aerosmith</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.linkedin.com/pub/chris-sturgeon/1/b/904" target="_blank" rel="nofollow" title="LinkedIn"><img alt="LinkedIn" src="/Content/images/SMIcons/linkedin.png"/></a>
               </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>