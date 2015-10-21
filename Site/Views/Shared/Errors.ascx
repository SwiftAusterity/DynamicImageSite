<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<String>>" %>

<%
    if (Model != null && Model.Count() > 0)
    {
%>
<ul>
    <%
        foreach (String error in Model)
        {
            %>
            <li class="message error"><%= error %></li>
            <%
        }
    %>
</ul>
<%
    }
%>
