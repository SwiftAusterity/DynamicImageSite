<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewModel>" %>

<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>

<nav id="paging-nav">
    <% if (Model.ContentPage.InfoText.Length > 0)
       { %>
        <a href="#" class="info-button" rel="nofollow">+ INFO</a>
        <div class="infoBox">
            <%= Model.ContentPage.InfoText %>
        </div>
    <% }
       else
       { %>
            <a href="#" class="info-button" style="display: none;" rel="nofollow">+ INFO</a>
            <div class="infoBox" style="display: none;">
            </div>
    <% } %>
    <% if (Model.ContentPage.BackwardNav != null)
       { %>
        <a href="<%= Model.ContentPage.BackwardNav.Url %>" class="prev">
            <span class="icon icon-sm icon-arrow-left"></span>
            <span class="alt-content">Prev</span>
        </a>
    <% }
       else
       { %>
        <a href="" style="display: none;" class="prev">
            <span class="icon icon-sm icon-arrow-left"></span>
            <span class="alt-content">Prev</span>
        </a>
       <% }

       if (Model.ContentPage.ForwardNav != null)
       { %>
        <a href="<%= Model.ContentPage.ForwardNav.Url %>" class="next">
            <span class="icon icon-sm icon-arrow-right"></span>
            <span class="alt-content">Next</span>
        </a>
    <% } 
          else
       { %>
        <a href="" style="display: none;" class="next">
            <span class="icon icon-sm icon-arrow-right"></span>
            <span class="alt-content">Next</span>
        </a>
       <% } %>
 
</nav>
