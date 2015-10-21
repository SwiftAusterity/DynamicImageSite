<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewModel>" %>

<%@ Import Namespace="Site.Models" %>
<%@ Import Namespace="Site.ViewModels" %>

<%
    if (Context.User.Identity != null && Context.User.Identity.IsAuthenticated) {
%>
        Welcome <b><%= Html.Encode(Model.UserScreenName) %></b>!
        [ <%= Html.ActionLink("Logout", "LogOff", "Login") %> ]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Login", "Index", "Login") %> ]
<%
    }
%>
