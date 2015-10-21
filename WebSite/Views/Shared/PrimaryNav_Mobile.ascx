<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewModel>" %>
<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>
<%@ Import Namespace="System.Collections.Generic" %>
<nav id="primary-nav">
    <div class="logo">
        <a href="/m/">
            <span class="icon icon-sm icon-cornerfade-bl"></span>
            <span class="alt-content">Infuz</span>
        </a>
    </div>
    <%-- There should be no spaces between the li elements. They are inline-block. --%>
    <ul><% if (Model.ContentPage.PrimaryNav.Count() > 1)
           foreach (Site.Data.API.INavElement sectionNavElement in Model.ContentPage.PrimaryNav)
           {
               var activeAttr = sectionNavElement.Name == Model.ContentPage.SectionName ? " class=\"active\"" : "";
               %><li<%=activeAttr %>><a href="<%= sectionNavElement.Url %>"><%= sectionNavElement.Name %></a></li><% } %><li><a href="http://blog.infuz.com/" rel="nofollow">Blog</a></li>
    </ul>
</nav>
