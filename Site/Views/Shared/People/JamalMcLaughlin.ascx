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
                <h1>Jamal McLaughlin – Interactive Developer</h1>
            </header>
            <div class="content">
                <p>
                    Jamal is a front-end developer who can take a design and then transform it into a fully functional interactive digital experience. 
                    He’s an expert in Photoshop, Illustrator, Dreamweaver and Visual Studio, and teaches design classes at ITT Tech. Born and raised in Alton, Illinois, 
                    AKA the birthplace of <a href="http://en.wikipedia.org/wiki/Miles_Davis">Miles Davis</a>, Jamal earned his BFA in painting from nearby SIUE. The thing he loves most about his job is that each day he’s tackling 
                    something different. One day it’s putting a picture of Justin Bieber on an elf. The next it’s filming objects being flung into a box of cat litter. 
                    One thing’s for sure – Jamal is the highly skilled utility player any team and client would love to have in their lineup.
                </p>
                <ul>
                    <li><strong>First Concert:</strong> Tom Petty</li>
                    <li><strong>When Not At Work, He’s:</strong> Working in his Woodshop or Producing Music</li>
                    <li><strong>Alternate Career Choice:</strong> Stay at Home Dad</li>
                    <li><strong>Best Movie Ever:</strong> Spaceballs</li>
                </ul>
            </div>
            <footer>
 <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.facebook.com/jamalrsmclaughlin" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="http://twitter.com/jamalmclaughlin" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
               </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>