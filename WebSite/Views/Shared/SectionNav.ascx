<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewModel>" %>
<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>
<%@ Import Namespace="System.Collections.Generic" %>
<nav id="section-nav">
    <ul class="filters">
        <% if (Model.ContentPage.SectionNav.Count() > 1)
               foreach (Site.Data.API.INavElement sectionNavElement in Model.ContentPage.SectionNav)
               { %>
                <li><a href="<%= sectionNavElement.Url %>"><%= sectionNavElement.Name %></a></li>       
            <% }  %>
    </ul>
    <ul class="children">
         <%  if (Model.ContentPage.SubNav.Count() > 1) 
                 foreach (Site.Data.API.INavElement sectionNavElement in Model.ContentPage.SubNav)
            {
                    var activeAttr = sectionNavElement.Name == Model.ContentPage.SectionName ? " class=\"active\"" : "";
                    %><li<%= activeAttr %>><a href="<%= sectionNavElement.Url %>" style="background: url(<%= sectionNavElement.Thumbnail %>) no-repeat;"><span class="alt-content"><%= sectionNavElement.Name %></span><b></b></a></li>       
        <% }  %>
   </ul>
</nav>
