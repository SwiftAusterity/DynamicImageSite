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
                <h1>Kelli Schwahn – Executive Assistant</h1>
            </header>
            <div class="content">
                <p>
                    She’s the one who greets all of us in the morning as we step off the elevator. With the best seat in the house (front desk), of course she’s 
                    extremely friendly, personable and more than likely good with direction. Her hospitable nature comes from being born and raised in the south 
                    (It’s not just a stereotype). Before coming to Infuz, Kelli spent 14 years in media buying and also worked as a portrait photographer. 
                    Outside of the office she enjoys collecting Stephen King books and watching both the good and bad King movies—whether it’s IT, The Tommyknockers, or 
                    <a href="http://www.youtube.com/watch?v=tMja9C6Htts" target="_blank">Pet Sematary</a>, she loves them all equally. 
                </p>
                <ul>
                    <li><strong>Best Known at the Office For:</strong> Stocking the Liquor and Soda</li>
                    <li><strong>First Concert:</strong> Journey</li>
                    <li><strong>Favorite Website:</strong> <a href="http://Etsy.com/" target="_blank">Etsy.com</a></li>
                    <li><strong>Best Movie Ever:</strong> Shawshank Redemption</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                    <a href="http://www.facebook.com/profile.php?id=1597351703&ref=tn_tnmn" target="_blank" rel="nofollow" title="Facebook" ><img alt="Facebook" src="/Content/images/SMIcons/facebook.png"/></a>
                    <a href="https://twitter.com/#!/KelliSchwahn" target="_blank" rel="nofollow" title="Twitter" ><img alt="Twitter" src="/Content/images/SMIcons/twitter.png"/></a>
                    <a href="http://www.linkedin.com/in/kellischwahn" target="_blank" rel="nofollow" title="LinkedIn"><img alt="Linkedin" src="/Content/images/SMIcons/linkedin.png"/></a>
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
