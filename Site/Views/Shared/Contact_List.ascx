<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Site.Data.API.IContact>>" %>

    <table>
        <tr>
            <th></th>
            <th>
                Email
            </th>
            <th>
                Subject
            </th>
            <th>
                Body
            </th>
            <th>
                Phone
            </th>
            <th>
                FirstName
            </th>
            <th>
                LastName
            </th>
            <th>
                JobTitle
            </th>
            <th>
                CompanyName
            </th>
            <th>
                Address
            </th>
            <th>
                Address2
            </th>
            <th>
                City
            </th>
            <th>
                State
            </th>
            <th>
                Zipcode
            </th>
            <th>
                Created
            </th>
        </tr>

    <% foreach (var item in Model)
           Html.RenderPartial("Contact_ListRow", item);
        %>

    </table>

