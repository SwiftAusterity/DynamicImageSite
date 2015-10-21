<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<ISocialMediaItem>>" %>

<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>
<%@ Import Namespace="Site.Data.API" %>

<% bool notFirst = false;
   foreach (var SMItem in Model.Where(item => item != null)) 
    {%>
       <%= SMItem.AsHtml(notFirst)%>
    <% notFirst = true;
} %>
