<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.ContactFormViewModel>" %>

<div class="block left">
    <div class="intro">
        <section>
            <header>
                <h1>We Would Love<br>
                    To Meet You</h1>
            </header>
            <div class="content">
            
                <!-- Location Info -->
                <div>
                    <p class="welcome group">
                       For all business inquiries, please contact Chris Sturgeon, VP Account Services at 314.584.8033 or use the form.
                    </p>
                </div>
                <!-- /Location Info -->
                
            </div>
            <footer>
            </footer>
        </section>
    </div>
</div>

<div class="block right">
    <div class="body">
        <section class="contact-form">
            <div class="content">
                <!-- Contact Form -->
                <% Html.BeginForm("SubmitContact", "Contact", FormMethod.Post, new { id = "contact-form" }); %>
                    <span class="status error">
                        <% foreach (String error in Model.Errors)
                       { %>
                            <%= error %><br />
                    <% } %> 
                    </span>
                    <span class="status success">
                        <% foreach (String message in Model.Messages)
                       { %>
                            <%= message %><br />
                    <% } %> 
                    </span>
                    <fieldset>
                    <%
                        var checkboxCssAttributes = new Dictionary<String, object>();
                            checkboxCssAttributes.Add("class", "checkbox");
                        %>
                        <div class="group">
                            <p id="fullname-group">
                                <label>Name*<br>
                                    <input id="fullname" class="text" name="fullname" value="<%= Model.FullName %>" type="text" />
                                </label>
                            </p>
                            <p id="email-group">
                                <label>Email*<br>
                                    <input id="email" class="text" name="email" value="<%= Model.Email %>" type="text" />
                                </label>
                           </p>
                        </div>
                        <p id="details-group">
                            <label>Details*<br>
                                <textarea id="body" cols="40" rows="5" name="body" value="<%= Model.Body %>"></textarea>
                            </label>
                        </p>
                        <p id="enewsletter-group" class="group">
                            <%= Html.CheckBox("enewsletter", Model.eNewsletter, checkboxCssAttributes)%>
                            <label for="enewsletter">Stay in touch, see our latest work and uncover trends and insights with our quarterly enewsletter.</label>
                            <input type="submit" class="button" value="Submit" id="submitContact" />
                        </p>
                    </fieldset>
                    <p><small>* Required information</small></p>
                <% Html.EndForm(); %>
                <!-- /Contact Form -->
            </div>
        </section>
    </div>
</div>

<span class="vshim"></span>

<script>
    $(function() {
        $('#contact-form')
        .submit(function(evt) {
            var form = this;

            // Grab and store input elements
            var inputs = $(':input', form);

            // Common ajax error handler
            function errorHandler(jqXHR, message) {
                // Set the error and show/hide it
                $('.status.error', form).html(message || '').toggle(!!message);
            }

            // Common ajax success handler
            function successHandler(jqXHR, message) {
                // Set the status and show/hide it
                $('.status.success', form).html(message || '').toggle(!!message);
            }

            // Flag for successful submission
            var successfulSubmit = false;
            
            $.ajax({
                url: '/Ajax/Contact/Submit',
                data: {
                    'fullname': $('#fullname').val(),
                    'email': $('#email').val(),
                    'body': $('#body').val(),
                    'enewsletter': $('#enewsletter:checked').val() != undefined
                },
                type: 'post',
                dataType: 'json',
                success: function(data, textStatus, jqXHR) {
                    successfulSubmit = (data.success === 'true');
                    
                    // Clear status messages
                    $('.status', form).empty();
                    
                    if (successfulSubmit) {
                        // Successful submission
                        successHandler(jqXHR, 'Your request was successfully submitted.');

                        // Clear the form
                        $('#email, #fullname, #body').val('');
                        $('#enewsletter').removeProp('checked');
                    } else {
                        // Error with user data
                        errorHandler(jqXHR, data.errors.join('<br>'));
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    // Error with request
                    errorHandler(jqXHR, 'There was a problem when trying to submit your request. Please try again in a moment.');
                },

                // Disable/Enable input elements
                beforeSend: function(jqXHR, settings) {
                    inputs.attr('disabled', 'disabled');
                },
                complete: function(jqXHR, textStatus) {
                    if (!successfulSubmit) {
                        inputs.removeAttr('disabled');
                        inputs[0].focus();
                    }
                }
            });

            // Prevent normal form submission
            evt.preventDefault();
        });
    });
</script>
