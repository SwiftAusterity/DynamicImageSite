<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Data.API.IContact>" %>

        <tr>
            <td>
                <%
                    if (Model.IsARealBoy)
                    {%>
                        <a href="<%= String.Format("/Admin/Account/Edit/{0}", Model.Email) %>">Go To</a>
                 <% }
                    else
                    {
                        var url = String.Format("/Admin/Account/CreatePrepop?email={0}&phone={1}&firstName={2}&lastName={3}&jobTitle={4}&companyName={5}&address={6}&address2={7}&city={8}&state={9}&zipcode={10}",
                            Model.Email, Model.Phone, Model.FirstName, Model.LastName, Model.JobTitle, Model.CompanyName,
                            Model.Address, Model.Address2, Model.City, Model.State, Model.Zipcode); %>
                            
                        <a href="<%= url %>">New</a>
                  <% } %>
            </td>
            <td>
                <%= Html.Encode(Model.Email) %>
            </td>
            <td>
                <%= Html.Encode(Model.Subject)%>
            </td>
            <td>
                <%= Html.Encode(Model.Body)%>
            </td>
            <td>
                <%= Html.Encode(Model.Phone)%>
            </td>
            <td>
                <%= Html.Encode(Model.FirstName)%>
            </td>
            <td>
                <%= Html.Encode(Model.LastName)%>
            </td>
            <td>
                <%= Html.Encode(Model.JobTitle)%>
            </td>
            <td>
                <%= Html.Encode(Model.CompanyName)%>
            </td>
            <td>
                <%= Html.Encode(Model.Address)%>
            </td>
            <td>
                <%= Html.Encode(Model.Address2)%>
            </td>
            <td>
                <%= Html.Encode(Model.City)%>
            </td>
            <td>
                <%= Html.Encode(Model.State)%>
            </td>
            <td>
                <%= Html.Encode(Model.Zipcode)%>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", Model.Created))%>
            </td>
        </tr>

