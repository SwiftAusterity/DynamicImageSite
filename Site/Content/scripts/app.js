// JavaScript for client-side app setup. Load this file first.

; (function(window, undefined) {

    var document = window.document;

    // Application object
    // --------------------------------------------------
    window.Infuz = {};

    // Infuz.Ajax namespace
    // --------------------------------------------------
    window.Infuz.Ajax = {};

    // Infuz.Support namespace
    // --------------------------------------------------
    window.Infuz.Support = {};

    window.Infuz.Support.check = function(prop, obj) {
        return prop.toString() in (obj || document.documentElement);
    };

    window.Infuz.Support.event = function(prop, obj) {
        return window.Infuz.Support.check('on' + prop, obj);
    };

    window.Infuz.Support.css = function(prop, obj) {
        if (!obj) return;
        return window.Infuz.Support.check(prop, obj.style);
    };

    // Infuz.Utility namespace
    // --------------------------------------------------
    window.Infuz.Utility = {};

    window.Infuz.Utility.isSlideshowRunning = true;

    window.Infuz.Utility.isMobile = false; //DetectTierIphone() || DetectTierRichCss() || DetectTierOtherPhones());

    window.Infuz.Utility.isTouchDevice = window.Infuz.Support.event('touchstart');

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

    // Check for horizontal scrolling
    window.Infuz.Utility.hasHorizontalScroll = function() {
        var adjust = 1, win = $(window), orig = win.scrollLeft(),
        hasScroll = win.scrollLeft(adjust).scrollLeft() != orig || win.scrollLeft(-adjust).scrollLeft() != orig;
        win.scrollLeft(orig);
        return hasScroll;
    };

    // Check for vertical scrolling
    window.Infuz.Utility.hasVerticalScroll = function() {
        var adjust = 1, win = $(window), orig = win.scrollTop(),
        hasScroll = win.scrollTop(adjust).scrollTop() != orig || win.scrollTop(-adjust).scrollTop() != orig;
        win.scrollTop(orig);
        return hasScroll;
    };

    // Attempt to redirect to the hash-bang URL for this request
    window.Infuz.Utility.tryHandleRedirect = function() {
        var host = window.location.host;
        var path = window.location.pathname;
        var hash = window.location.hash;
        var protocol = window.location.protocol;
        var search = window.location.search;

        if (search.length > 0)
            search = '/' + search;

        if (path.indexOf("/m/") != -1 || path == "/m")
            window.Infuz.Utility.isMobile = true;

        if (!window.Infuz.Utility.isMobile && path.indexOf("#!") == -1 && hash.length == 0) {
            var newUrl = protocol + '//' + host + search + '/#!' + path;
            window.location = newUrl;
        }
    };

    window.Infuz.Utility.checkForMobileCookie = function() {
        return $.cookie("infuz-mobile-detection") != null;
    };

    window.Infuz.Utility.checkForMobileCookieValue = function() {
        return $.cookie("infuz-mobile-detection") == 'true';
    };

    window.Infuz.Utility.checkForAntiMobileKiller = function() {
        var name = "antiMobile".replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);

        if (results == null)
            return false;
        else
            return true;
    };


    window.Infuz.Utility.tryAskAboutMobile = function() {
        var mobileCookieExists = window.Infuz.Utility.checkForMobileCookie();

        if (mobileCookieExists) {
            if (window.Infuz.Utility.checkForAntiMobileKiller()) {
                $.cookie("infuz-mobile-detection", null);
                window.Infuz.Utility.isMobile = false;
            }
            else
                window.Infuz.Utility.isMobile = window.Infuz.Utility.checkForMobileCookieValue();

            if (window.Infuz.Utility.isMobile) {
                var host = window.location.host;
                var protocol = window.location.protocol;

                var newUrl = protocol + '//' + host + '/m/';
                window.location = newUrl;
            }
        }
        else {
            var mobileDevice = DetectTierIphone() || DetectTierRichCss() || DetectTierOtherPhones();

            if (mobileDevice) {
                var overlay = $('#mobile-choice-overlay');
                //ask about making a cookie
                overlay.show();

                $('#acceptMobile').click(function(evt) {
                    evt.preventDefault();
                    $.cookie("infuz-mobile-detection", "true", { expires: 7, path: '/' });
                    overlay.hide();

                    var host = window.location.host;
                    var protocol = window.location.protocol;

                    var newUrl = protocol + '//' + host + '/m/';
                    window.location = newUrl;
                });

                $('#denyMobile').click(function(evt) {
                    evt.preventDefault();
                    $.cookie("infuz-mobile-detection", "false", { expires: 356, path: '/' });
                    overlay.hide();
                });
            }
        }
    };

    // Reformat URLs with hash bang format
    window.Infuz.Utility.sniffAndReplaceHashBangUrls = function(context) {
        $('a[href][href!="#"][rel!="nofollow"]', context || document).attr('href', function() {
            var host = window.location.host;

            if (this.href.indexOf(host) > -1) {
                var slashIndex = this.href.indexOf('/', this.href.indexOf('/', this.href.indexOf('/', 0) + 1) + 1);
                return this.href.substring(0, slashIndex) + '#!/' + this.href.substring(slashIndex + 1);
            }
            else
                return this.href;
        });
    };

    // Set up and start the news ticker
    window.Infuz.Utility.startTicker = function($cont) {
        var $slides = $('.ticker-slides', $cont);
        $('a', $slides).attr('target', '_blank');
        if ($slides.children().length > 1) {
            $slides.cycle({ fit: true, width: '100%', pause: true });
        }
    };

    // Load video content into a container
    window.Infuz.Utility.loadVideoContent = function(context, videoId, width, height, autoplay) {
        if (!(videoId && videoId.length)) return;
        if (!(width && width > 0)) return;
        if (!(height && height > 0)) return;

        // Set up video playback
        var $cont = $('.video-container', context);
        var autoplayVal = (autoplay === true ? '1' : '0');
        var videoTemplate =
            '<div class="video-wrapper">' +
            '<!--[if (gte IE 8)|!(IE)]><!-->' +
            '<iframe src="http://player.vimeo.com/video/' + videoId + '?title=0&amp;byline=0&amp;portrait=0&amp;autoplay=' + autoplayVal + '"' +
            '    width="' + width + '" height="' + height + '" frameborder="0" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>' +
            '<!--<![endif]-->' +
            '<!--[if lt IE 8 ]>' +
            '<object width="' + width + '" height="' + height + '">' +
            '    <param name="allowfullscreen" value="true" />' +
            '    <param name="allowscriptaccess" value="always" />' +
            '    <param name="movie" value="http://vimeo.com/moogaloop.swf?clip_id=' + videoId + '&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=&amp;fullscreen=1&amp;autoplay=' + autoplayVal + '&amp;loop=0" />' +
            '    <embed src="http://vimeo.com/moogaloop.swf?clip_id=' + videoId + '&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=&amp;fullscreen=1&amp;autoplay=' + autoplayVal + '&amp;loop=0" type="application/x-shockwave-flash" allowfullscreen="true" allowscriptaccess="always" width="' + width + '" height="' + height + '"></embed>' +
            '</object>' +
            '<![endif]-->' +
            '</div>';

        $('.video-bigbutton', $cont).click(function(evt) {
            evt.preventDefault();

            // Ensure that videos are stopped
            window.Infuz.Utility.resetVideoContainers();

            $('img', this).fadeOut('slow',
                function() {
                    // Render the video player content
                    $cont.append(videoTemplate);
                    if ($.fn.fitVids) {
                        $cont.fitVids();
                    }
                    $('iframe', $cont).on('load', function() {
                        $('.video-wrapper', $cont).css('opacity', 0).animate({ 'opacity': 1 }, 2000);
                    });
                }
            );
        });
    };

    window.Infuz.Utility.resetVideoContainers = function() {
        var panes = $('#stage .pane');

        // Make sure videos are not still playing
        $('.video-wrapper', panes).remove();
        $('.video-bigbutton img', panes).show();
    };

    // Infuz.Navigation namespace
    // --------------------------------------------------
    window.Infuz.Navigation = {
        previousRoute: null
    };

    // Infuz.PageTransition namespace
    // --------------------------------------------------
    window.Infuz.PageTransition = {};

    // Keep track of the prev page
    window.Infuz.PageTransition.last = null;

    // Denotes first load state
    window.Infuz.PageTransition.initial = true;

    // Direction of transition
    window.Infuz.PageTransition.direction = 'Left'; // Left | Right | Up | Down

    window.Infuz.PageTransition.bgImage = '';

    // Initialization for the application
    (function() {
        $(window).mousedown(function() {
            window.Infuz.Utility.isSlideshowRunning = false;
            clearTimeout(window.Infuz.PageTransition._rotateId);
            delete window.Infuz.PageTransition._rotateId;

            $(window).unbind('mousedown');
        });

        if (!window.Infuz.Utility.isMobile)
            window.Infuz.Utility.tryHandleRedirect();
    })();

})(window);