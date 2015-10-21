<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.ContactFormMobileViewModel>" %>

<div class="group" id="group-contact">
    <div class="block left">
        <div class="intro">
            <section>
                <header>
                    <h1>Introduce<br>
                        Yourself</h1>
                </header>
                <div class="content">
                
                    <!-- Location Info -->
                    <div>
                        <p>
                            611 N. Tenth Street<br>
                            Fourth Floor<br>
                            Saint Louis, MO 63101<br>
                            Phone: (314) 584-8000<br>
                            Fax: (314) 584-8080<br>
                            <a href="http://maps.google.com/maps?q=611+North+10th+Street,+Saint+Louis,+MO&amp;z=16" class="alt">Map Us</a>
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
                <header>
                    <h2>We Would Love To Meet You</h2>
                </header>
                <div class="content">

                    <!-- Contact Form -->
                    <p class="welcome group">
                        For all business inquiries, please contact Chris Sturgeon, VP Account Services at 314.584.8033 or use the form below.
                    </p>
                    <% Html.BeginForm("MobileSubmitContact", "Contact", new { mobile = true }, FormMethod.Post, new { id = "contact-form" }); %>
                        <span class="status"></span>
                        <fieldset>
                            <div class="group">
                                <p id="fullname-group">
                                    <label>Name*<br>
                                        <%
                                            var textCssAttributes = new Dictionary<String, object>();
                                                textCssAttributes.Add("class", "text");
                                            var checkboxCssAttributes = new Dictionary<String, object>();
                                                checkboxCssAttributes.Add("class", "checkbox");
                                            %>
                                        <%= Html.TextBox("fullname", String.Empty, textCssAttributes) %></label>
                                </p>
                                <p id="email-group">
                                    <label>Email*<br>
                                        <%= Html.TextBox("email", String.Empty, textCssAttributes) %></label>
                                </p>
                            </div>
                            <p id="details-group">
                                <label>Details*<br>
                                    <%= Html.TextArea("body", new { cols = 40, rows = 5 }) %></label>
                            </p>
                            <p id="enewsletter-group" class="group">
                                <%= Html.CheckBox("enewsletter", false, checkboxCssAttributes)%>
                                <label for="enewsletter">Stay in touch, see our latest work and uncover trends and insights with our quarterly enewsletter.</label>
                            </p>
                            <input type="submit" class="button" value="Submit" id="submitContact" />
                        </fieldset>
                        <p><small>* Required information</small></p>
                    <% Html.EndForm(); %>
                    <!-- /Contact Form -->
                </div>
            </section>
        </div>
    </div>

    <span class="vshim"></span>
</div>

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
                $('.status', form).addClass('error').html(message || '').toggle(!!message);
            }

            // Common ajax success handler
            function successHandler(jqXHR, message) {
                // Set the status and show/hide it
                $('.status', form).addClass('success').html(message || '').toggle(!!message);
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

