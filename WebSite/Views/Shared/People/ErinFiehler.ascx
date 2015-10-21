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
                <h1>Erin Fiehler – Research Specialist</h1>
            </header>
            <div class="content">
                <p>
                    She spends her days following the pulse and trends on news sites, new
                    studies and any other nugget of information that’s useful for our
                    clients and industry. Data analysis and Internet sleuthing must come
                    natural to her as she earned a Mathematics degree from Webster University.
                    There’s no question that Erin is a <a href="http://dogswearinghats.tumblr.com/" target="_blank">dog lover</a>, as she proudly displays a
                    gallery of dog photos and trinkets at her desk – some featuring her beloved
                    Chihuahua Trina, some of simply cute dogs she’s never met. A St. Louis
                    native, Erin also spent time living in Athens Greece with her family. And
                    to answer your question, yes she’s related to our CEO (Jason Fiehler).
                    They’re brother and sister.
                </p>
                <ul>
                    <li><strong>Best Known For:</strong> Being Dog Crazy</li>
                    <li><strong>Favorite Website:</strong> <a href="http://Amazon.com/" target="_blank">Amazon.com</a></li>
                    <li><strong>Best Movie of All-Time:</strong> Legally Blonde</li>
                    <li><strong>Alternate Career:</strong> Dog Walker</li>
                </ul>
            </div>
            <footer>
                <div class="ticker" style="position: relative">
                    <div class="ticker-slides">
                        <% Html.RenderPartial("SocialMediaTickerItems", Model.ContentPage.SocialMediaFeedItems); %>
                    </div>
                </div>
                <div class="social">
                </div>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>