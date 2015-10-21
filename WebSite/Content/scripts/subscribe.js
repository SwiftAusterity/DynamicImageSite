; (function($, window, undefined) {

    var document = window.document;

    // DOM-ready
    $(function() {

        if (!window.Infuz.Utility.isMobile) {
            // E-newsletter popup
            function clearSubscribeForm(api) {
                // Clear error and form values
                $('.error', api.elements.tooltip).empty();
                $('form :input', api.elements.tooltip).val('');
            }
            
            // Cancel the default click action
            var $subscribeLink = $('#link-enewsletter');
            $subscribeLink.click(function(evt) {
                evt.preventDefault();
                bindSubscribeTip();
                $(this).qtip('show');
            });
            
            function $getSubscribeForm() {
                return $('#form-enewsletter').clone().removeAttr('id');
            }

            function $getSubscribeThanks() {
                return $('#confirm-enewsletter').clone().removeAttr('id');
            }

            // Options for the Enewsletter popup form
            function getSubscribeOptions() {
                return {
                    id: 'subscribe',
                    content: {
                        text: $getSubscribeForm()
                    },
                    position: {
                        my: 'bottom right',
                        at: 'top left'
                    },
                    show: {
                        event: 'click',
                        modal: {
                            on: true
                        }
                    },
                    hide: {
                        event: 'unfocus'
                    },
                    style: {
                        classes: 'qtip-enewsletter',
                        tip: false,
                        widget: true
                    },
                    events: {
                        render: function(evt, api) {
                            // Capture the form submission
                            $('form', this).submit(function(evt) {
                                suppressOverflow();
                                
                                // Grab and store input elements
                                var inputs = $(':input', this);

                                // Common ajax error handler
                                function errorHandler(jqXHR, message) {
                                    // Set the error and show/hide it
                                    $('.status', api.elements.tooltip).addClass('error').html(message || '').toggle(!!message);
                                    api.reposition();
                                }
                                
                                // Flag for successful submission
                                var successfulSubmit = false;
                                
                                // Setup AJAX request
                                $.ajax({
                                    url: '/Ajax/Subscribe/Submit',
                                    data: {
                                        'fullname': $('#subscribe-fullname', api.elements.tooltip).val(),
                                        'email': $('#subscribe-email', api.elements.tooltip).val(),
                                        'enewsletter': true
                                    },
                                    type: 'post',
                                    dataType: 'json',
                                    success: function(data, status, jqXHR) {
                                        successfulSubmit = (data.success === 'true');
                                        
                                        // On success, show message
                                        if (successfulSubmit) {
                                            // Replace the contents of the form tip with thank you message.
                                            // Doing it this way so we don't get a fade flicker in between.
                                            $('.enewsletter-content', api.elements.content).html($getSubscribeThanks().html());
                                            $('button', api.elements.content).click(function(evt) {
                                                api.hide();
                                                setTimeout(removeSubscribeTip, 90);
                                            });
                                            api.reposition();
                                        }

                                        // Call error handler on error status too.
                                        else {
                                            errorHandler(jqXHR, data.errors.join('<br>'));
                                        }
                                    },
                                    error: errorHandler,

                                    // Disable/Enable input elements
                                    beforeSend: function(jqXHR, settings) {
                                        inputs.attr('disabled', 'disabled');
                                    },
                                    complete: function(jqXHR, textStatus) {
                                        if (!successfulSubmit) {
                                            inputs.removeAttr('disabled');
                                            inputs[0].focus();
                                        }
                                        
                                        setTimeout(restoreOverflow, 90);
                                    }
                                });

                                // Prevent normal form submission
                                evt.preventDefault();
                            });
                        },
                        hide: function(evt, api) {
                            setTimeout(removeSubscribeTip, 90);
                        }
                    }
                };
            }

            function removeSubscribeTip() {
                $subscribeLink.qtip('destroy').removeData('qtip');
                restoreOverflow();
            }
            
            // Bind the subscribe form tip
            function bindSubscribeTip() {
                removeSubscribeTip();
                $subscribeLink.qtip(getSubscribeOptions());
            }
            
            function suppressOverflow() {
                // Prevent scroll bars from appearing if the box changes size or
                // needs to be repositioned. This will get reset upon closing.
                $('html, body').css('overflow', 'hidden');
            }
            
            function restoreOverflow() {
                // Restore scrollbar functionality
                $('html, body').css('overflow', '');
                
                // If the window is small, make sure we go back to the corner
                // of the window where this form is located. Produces some flicker,
                // but with the current implementation of the popup, another
                // method is not available.
                $(window).scrollTop(99999).scrollLeft(99999);
            }
        }

    });
})(jQuery, window);
