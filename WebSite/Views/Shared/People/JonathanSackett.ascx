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
                <h1>Jonathan Sackett – Senior Account Executive</h1>
            </header>
            <div class="content">
                <p>
                    Our clients love Jonathan. He’s passionate about his work, his teammates and his brands. Jonathan’s the guy who knows the marketing calendar by heart, 
                    can recite the brand guide without notes and he’s probably visited every website mentioning his clients’ brand that exists on the Internet. With an 
                    English degree from Wittenberg University, Jonathan truly is a man of his word when it comes to building trusting relationships between his team and his 
                    clients. Apart from talking shop, be sure to ask Jonathan about his four-letter passion: <a href="http://www.amazingribs.com/images/pix/pork_cuts_large.jpg">MEAT</a>. If you’re lucky, you’ll earn an invitation to his annual 
                    “Meat Fest” event where he’ll cook you an unforgettable smorgasbord of grilled and smoked treats. 
                </p>
                <ul>
                    <li><strong>First Concert:</strong> Neil Diamond</li>
                    <li><strong>Personal Best:</strong> 2 Billion Points on the Twilight Zone Pinball Game</li>
                    <li><strong>Worst Movie Ever:</strong> The Adventures of Buckaroo Banzai Across the 8th Dimension.</li>
                    <li><strong>Alternate Career:</strong> Fishing Guide</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.facebook.com/jmsackett5" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="https://twitter.com/#!/jmsackett5" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="https://plus.google.com/u/0/117936596875781722053/posts" target="_blank" rel="nofollow" title="Google&#43;"><img alt="Google+" src="/Content/images/SMIcons/googleplus.png"/></a>
                </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>