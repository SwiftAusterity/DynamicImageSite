// JavaScript for client-side app setup. Load this file first.

; (function(window, undefined) {

    var document = window.document;

    // Application object
    // --------------------------------------------------
    window.Infuz = {};

    // Infuz.Ajax namespace
    // --------------------------------------------------
    window.Infuz.Ajax = {};

    window.Infuz.Ajax.formatRoute = function(route) {
        return route
                    .replace(/([^\/])$/, '$1/') // add a slash to the end
                    .replace('#!', '')          // remove hash bang
                    .replace('!', '')           // or just bang
                    .replace(/\//gi, '_');      // replace slash with underscore
    };

    // Infuz.Utility namespace
    // --------------------------------------------------
    window.Infuz.Utility = {};

    // Return a number within the given min and max values.
    window.Infuz.Utility.randomRange = function(minVal, maxVal) {
        minVal = minVal || 0;
        maxVal = maxVal || 1;
        return Math.floor(minVal + (Math.random() * (maxVal - minVal)));
    };

    // HTML-encode a string
    window.Infuz.Utility.htmlEncode = function(value) {
        return $('<div/>').text(value).html();
    };

    // HTML-decode an encoded string
    window.Infuz.Utility.htmlDecode = function(value) {
        return $('<div/>').html(value).text();
    };

    // Infuz.Navigation namespace
    // --------------------------------------------------
    window.Infuz.Navigation = {
        currentRoom: null
    };

    window.Infuz.Ajax.loadContent = function(pageName, command) {
        var ajaxUrl = '/Ajax/TextADV/' + pageName + "/" + command;
        $.getJSON(ajaxUrl, function(data) {
            // Success
            if (!data) return;
            if (data.success === false) return;

            /* Data fields
            toDescriptor     New "html" coming in
            url              Url for the page we're going to pass in
            area             "Area" for the page, mostly irrelevant
            section          "Section" for the page, also mostly irrelevant
            commandEcho      What we asked of the system
            pathEcho         What url we asked for
            openNewWindow    Should we pop up a new window? If this has text in it, we need to open a new window with this path
            title            New page title
            */

            //Set the current room
            window.Infuz.Navigation.currentRoom = data;

            document.title = data.title;

            //objects
            var input = $('#commandLine');
            var submitButton = $('#submit');
            var viewPort = $('#viewPort');

            //update the prompt

            //Scroll the viewport
            if (data.toDescriptor.length > 0) {
                viewPort.append('<br/><p>' + data.toDescriptor + '</p><br />');
            }

            //junk up the A.Screenies if we need to
            screenshotPreview();

            setTimeout(function() { viewPort.animate({ scrollTop: viewPort.prop("scrollHeight") - viewPort.height() }, 50); }, 10);

            //Open a new window? (usually because someone looked at something)
            if (data.openNewWindow && data.openNewWindow.length > 0) {
                window.open(data.openNewWindow, data.commandEcho);
            }
        })
        .complete(function(data) {
            // Request complete
        })
        .error(function() {
            // Request error
        });
    };

    function bindSubmit() {
        $('#submit').click(function(e) {
            e.preventDefault();
            var commandInput = $('#commandLine');
            var command = '';
            var currentRoom = '';
            var input = $('#commandLine');

            if (commandInput.val())
                command = commandInput.val();

            if (window.Infuz.Navigation.currentRoom)
                currentRoom = window.Infuz.Navigation.currentRoom.url;

            if (currentRoom.length > 0 || command.length > 0) {
                //Clear the input
                input.val('');

                window.Infuz.Ajax.loadContent(window.Infuz.Ajax.formatRoute(currentRoom), command);
            }
            //else NOTHING! We aren't going to request the system for nothing, that's dumb. });
        });

        $('#commandForm').submit(function(e) {
            e.preventDefault();
            var commandInput = $('#commandLine');
            var command = '';
            var currentRoom = '';
            var input = $('#commandLine');

            if (commandInput.val())
                command = commandInput.val();

            if (window.Infuz.Navigation.currentRoom)
                currentRoom = window.Infuz.Navigation.currentRoom.url;

            if (currentRoom.length > 0 || command.length > 0) {
                //Clear the input
                input.val('');

                window.Infuz.Ajax.loadContent(window.Infuz.Ajax.formatRoute(currentRoom), command);
            }
            //else NOTHING! We aren't going to request the system for nothing, that's dumb.
        });

    }

    function SubmitCommand(e) {

    }

    // Initialization for the dom
    $(function() {
        bindSubmit();

        //"connect" to the adv
        var viewPort = $('#viewPort');
        viewPort.append('<p>Connected to www.infuz.com via port 80.</p>');
        viewPort.append('<p>Contacting server...</p>');

        //Make the initial room call
        window.Infuz.Ajax.loadContent('_', '');

        $('#commandLine').focus();

        // Set up key navigation
        $(document)
        .on('keydown',
            function(evt) {
                if (evt.shiftKey || evt.altKey || evt.ctrlKey) return;

                var navigate = function(dir) {
                    // Check valid parameters
                    if (dir != 'recallCommand') {
                        return;
                    }
                    else if (window.Infuz.Navigation.currentRoom != null && window.Infuz.Navigation.currentRoom.commandEcho != null) {
                        var input = $('#commandLine');
                        input.val(window.Infuz.Navigation.currentRoom.commandEcho);
                        $('#commandLine').focus();
                    }
                };

                switch (evt.which) {
                    case 38: // up key
                        navigate('recallCommand');
                        break;
                    default:
                        break;
                }
            }
        );
    });
})(window);