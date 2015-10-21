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
                <h1>Jill Schanzle – Chief Operating Officer</h1>
            </header>
            <div class="content">
                <p>
                    The cliché saying that someone wears a variety of hats at the office actually fits perfectly to describe Jill’s wide-ranging role at Infuz. 
                    She’s the go-to for human resource information, operations, when the heat/AC won’t come on, and anything to do with <a href="http://www.youtube.com/watch?v=wM4SegTDuoc" target="_blank">money</a> coming in or going out of the 
                    agency. Which explains her instant messenger handle of “monemvr.” Why do we feel comfortable placing so much responsibility on and trust within Jill? She 
                    used to pack heat as an owner of an armored truck company. Don’t let that scare you though, as Jill’s one of the friendliest, funniest and wittiest of us 
                    all. 
                </p>
                <ul>
                    <li><strong>Best Band Ever:</strong> Journey</li>
                    <li><strong>Highest Achievement:</strong> 15 Million Points on the Monopoly Pinball Machine</li>
                    <li><strong>Animal Version of Herself:</strong> Koala Bear</li>
                    <li><strong>Favorite Food:</strong> Beef Carpaccio</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.facebook.com/jill.schanzle" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="http://www.twitter.com/monemvr" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="http://www.linkedin.com/pub/jill-schanzle/8/266/1b9" target="_blank" rel="nofollow" title="LinkedIn"><img alt="Linkedin" src="/Content/images/SMIcons/linkedin.png"/></a>
                </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>