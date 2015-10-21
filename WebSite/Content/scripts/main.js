; (function($, window, undefined) {

    var document = window.document;

    // DOM-ready
    $(function() {
        window.Infuz.Utility.tryAskAboutMobile();

        // Hide content initially to avoid content flash
        if (!window.Infuz.Utility.isMobile)
            $('#stage .pane').hide();

        //show the tutorial questionmark
        $('#link-help').show();

        // Add additional members to the application object
        // --------------------------------------------------
        (function() {

            // Set up mobile detect (tablets should use the full site)
            $('html').addClass((!window.Infuz.Utility.isMobile ? 'no-' : '') + 'mobile');

            window.Infuz.Utility.getSectionClass = function(path) {
                if (!path) return '';
                // If we are on a sub page then use the 'subsection' class
                var sectionClass = '', hashFragment = path.match(/[^\/]+/gi);
                if (hashFragment && hashFragment.length) {
                    sectionClass = (hashFragment[0] + ' ' + hashFragment.join('_')).toLowerCase();
                    if (hashFragment.length > 2) {
                        sectionClass += ' subsection';
                    }
                } else {
                    sectionClass = 'root';
                }
                return sectionClass;
            };

            // Manipulate the filter/sibling nav pop-up menu
            window.Infuz.Navigation.filterNav = {

                newSectionChange: true,

                verifyDisplay: function() {
                    var $this = $(this);
                    // Hide the nav if there are no valid links
                    $this[$this.children().length ? 'show' : 'hide']();
                },

                setActive: function(index) {
                    var $this = $(this), iconHtml = '<span class="icon icon-sm icon-arrow-up"></span>';
                    $this.find('li')
                    .removeClass('active')
                    .find('a .icon-arrow-up').remove().end()
                    .eq(index).addClass('active').find('a').prepend(iconHtml);
                },

                setAllInactive: function(index) {
                    var $this = $(this), iconHtml = '<span class="icon icon-sm icon-arrow-up"></span>';
                    $this.find('li')
                    .removeClass('active')
                    .find('a .icon-arrow-up').remove();
                },

                open: function(animate) {
                    if (animate == null) animate = true;
                    var $this = $(this), $items = $this.find('li:not(.active)');
                    $this.addClass('open');
                    if (!$items.queue() || ($items.queue() && $items.queue().length < 2)) {
                        var $icon = $this.find('li.active .icon');
                        $icon[animate ? 'fadeOut' : 'hide']();
                        $items[animate ? 'slideDown' : 'show']();
                    }
                },

                close: function(animate) {
                    if (animate == null) animate = true;
                    var $this = $(this);

                    if ($this != null) {
                        var $items = $this.find('li:not(.active)');
                        $this.removeClass('open');
                        if ($items != null && (!$items.queue() || ($items.queue() && $items.queue().length < 2))) {
                            var $icon = $this.find('li.active .icon');
                            $items[animate ? 'slideUp' : 'hide']();
                            $icon[animate ? 'fadeIn' : 'show']();
                        }
                    }
                }

            };

            // Manipulate the child nav horizontal menu
            window.Infuz.Navigation.childNav = {

                verifyDisplay: function() {
                    var $this = $(this);
                    // Hide the nav if there are no valid links
                    // Child nav always has a "base" page
                    $this[$this.children().length > 1 ? 'show' : 'hide']();
                },

                setActive: function(index) {
                    $(this).find('li').removeClass('active').eq(index).addClass('active');
                },

                open: function(animate) {
                    if (animate == null) animate = true;
                    var $this = $(this), $items = $this.find('li');
                    $items.each(
                        function() {
                            var cssProps = { left: 0, opacity: 1 };
                            if (animate) {
                                $(this).animate(cssProps, 500);
                            } else {
                                $(this).css(cssProps);
                            }
                        }
                    );
                },

                close: function(animate) {
                    if (animate == null) animate = true;
                    var $this = $(this), $items = $this.find('li');
                    $items.each(
                        function() {
                            var cssProps = { left: (-$(this).position().left), opacity: 0 };
                            if (animate) {
                                $(this).animate(cssProps, 500);
                            } else {
                                $(this).css(cssProps);
                            }
                        }
                    );
                }

            };

            // Manipulate the info box popup
            window.Infuz.Navigation.infoBox = {
                open: function(animate) {
                    if (animate == null) animate = true;

                    $('.info-button').addClass('active').html('- INFO');

                    $(this)[animate ? 'fadeIn' : 'show']();
                },

                close: function(animate) {
                    if (animate == null) animate = true;

                    $('.info-button').removeClass('active').html('+ INFO');

                    $(this)[animate ? 'fadeOut' : 'hide']();
                }
            };
            
            window.Infuz.Navigation.navigate = function(dir, axis) {
                // Check valid parameters
                if (dir == 'home' || dir == 'end') {
                    // Home is a special case
                    // End is a REALLY special case
                } else {
                    if ($.inArray(dir, ['prev', 'next']) == -1 ||
                        $.inArray(axis, ['main', 'section', 'child']) == -1) {
                        return;
                    }
                }

                // As long as the overlay isn't currently active, navigate
                if ($('#page-overlay').is(':not(:visible)')) {
                    switch (dir) {
                        case 'home':
                            if (!window.Infuz.Utility.hasVerticalScroll()) {
                                window.location.href = '/#!/';
                            }
                            break;

                        case 'end':
                            if (!window.Infuz.Utility.hasVerticalScroll()) {
                                if (confirm('Are you sure?')) {
                                    if (confirm('Really, really sure?')) {
                                        window.location.href = 'http://www.endoftheinternet.com/';
                                    }
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    var $item, $dest;
                    switch (axis) {
                        case 'main':
                            if (!window.Infuz.Utility.hasVerticalScroll()) {
                                $item = $('#primary-nav li.active');
                                $dest = $item[dir]();

                                // On the first area, so go to home page
                                if (dir == 'prev' && $item.length && !$item.prev().length) {
                                    navigate('home');
                                }

                                // On home page, so select first main nav item
                                if (dir == 'next' && !$item.length) {
                                    $dest = $('#primary-nav li:first');
                                }

                                if ($dest.length) {
                                    window.location.href = $dest.find('a').attr('href');
                                }
                            }
                            break;

                        case 'section':
                            if (!window.Infuz.Utility.hasVerticalScroll()) {
                                $dest = $('#section-nav:visible .filters li.active')[dir]();
                                if ($dest.length) {
                                    window.location.href = $dest.find('a').attr('href');
                                }
                            }
                            break;

                        case 'child':
                            if (!window.Infuz.Utility.hasHorizontalScroll()) {
                                $item = $('#paging-nav .' + dir), avail = $item.css('opacity') != '0' && $item.css('display') != 'none';
                                if (avail) {
                                    window.location.href = $item.attr('href');
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            };


            // Page transitions
            $.fn.cycle.transitions.crosshair = function($cont, $slides, opts) {
                var dir = window.Infuz.PageTransition.direction;
                var fx = dir.length ? 'scroll' + dir : 'fade';
                $.fn.cycle.transitions[fx].apply(this, arguments);
            };
            window.Infuz.PageTransition.init = function(opts) {
                $('.pane').data('transAfter', []);
                var options = $.extend({
                    fit: true,
                    fx: 'crosshair',
                    width: '100%',
                    height: '100%',
                    slideExpr: '.pane',
                    speed: 1500,
                    timeout: 0,
                    before: function(curr, next, opts, fwd) {
                        if (!window.Infuz.PageTransition.initial) {
                            $('#page-overlay').fadeIn();
                            clearTimeout(window.Infuz.PageTransition._rotateId);
                            delete window.Infuz.PageTransition._rotateId;
                        } else {
                            $('#page-overlay').fadeOut();
                            $(curr).hide();
                            window.Infuz.PageTransition.initial = false;
                        }
                    },
                    after: function(curr, next, opts, fwd) {
                        $('#page-overlay').fadeOut();

                        // Execute any extra pane effects
                        var callbacks = $(next).data('transAfter');
                        while (callbacks.length) {
                            (callbacks.shift()).call(next);
                        }
                    }
                }, opts);

                $('#stage').cycle(options);
            };
            // Add functionality to a page pane after the transition in effect (i.e. for videos, etc.)
            window.Infuz.PageTransition.addPaneCallback = function(target, callback) {
                if ($.isFunction(callback)) {
                    $(document.getElementById(target)).closest('.pane').data('transAfter').push(callback);
                }
            };

        })();

        // Main execution block
        // --------------------------------------------------

        // Added features for non-mobile experience
        if (!window.Infuz.Utility.isMobile) {

            // Reformat page hrefs for AJAX
            window.Infuz.Utility.sniffAndReplaceHashBangUrls();

            // Init page transition effects
            window.Infuz.PageTransition.init();

            // Init news rotator
            window.Infuz.Utility.startTicker($('#header .ticker'));

            $('#link-help').click(function(evt) {
                evt.preventDefault();
                $('#tutorial-overlay').fadeIn();
            });

            $('#tutorial-overlay').click(function(evt) {
                evt.preventDefault();
                $(this).fadeOut();
            });

            // Info Button
            $('.info-button').click(function(evt) {
                evt.preventDefault();
                var method = $(this).hasClass('active') ? 'close' : 'open';
                window.Infuz.Navigation.infoBox[method].call($('.infoBox'));
            });
            $(window)
            .on('click.info touchstart.info',
                function(evt) {
                    // Hide info box when clicking elsewhere
                    if ($('.info-button').hasClass('active')
                    && !$(evt.target).is('.info-button, .infoBox')
                    && !$(evt.target).parents().filter('.infoBox, .info-button').length) {
                        window.Infuz.Navigation.infoBox.close.call($('.infoBox'));
                    }
                }
            );

            // Set up filter/sibling navigation
            $('#section-nav .filters')
            .on('click', 'a',
                function(evt) {
                    var $this = $(this), $item = $this.closest('li'), $list = $this.closest('ul');

                    if ($list.hasClass('open')) {
                        if (!$item.hasClass('active')) {
                            window.Infuz.Navigation.filterNav.setActive.call($list.get(0), $item.index());

                            // Collapse child nav
                            window.Infuz.Navigation.childNav.close.call($list.siblings('.children').get(0));

                            // Change the nav here
                            window.Infuz.Navigation.filterNav.newSectionChange = true;
                        }
                        window.Infuz.Navigation.filterNav.close.call($list.get(0));
                    } else {
                        evt.preventDefault();
                        window.Infuz.Navigation.filterNav.open.call($list.get(0));
                    }
                }
            );
            $(window)
            .on('click.nav',
                function(evt) {
                    // Hide when clicking outside an open menu
                    var $list = $('#section-nav .filters');
                    if ($list.hasClass('open') && !$(evt.target).parents().filter($list).length) {
                        window.Infuz.Navigation.filterNav.close.call($list.get(0));
                    }
                }
            );
            
            // Only load hover state CSS for paging nav on non-touch devices
            if (!window.Infuz.Utility.isTouchDevice) {
                $('<link rel="stylesheet" href="/Content/css/hover.css" />').appendTo("head");
            }

            // Swipe!
            $("#stage").touchwipe({
                wipeRight: function() {
                    window.Infuz.Navigation.navigate('prev', 'child');
                },
                wipeLeft: function() {
                    window.Infuz.Navigation.navigate('next', 'child');
                },
                wipeUp: function() {
                    window.Infuz.Navigation.navigate('prev', 'section');
                },
                wipeDown: function() {
                    window.Infuz.Navigation.navigate('next', 'section');
                },
                min_move_x: 20,
                min_move_y: 20,
                preventDefaultEvents: true
            });
            
            // Set up key navigation
            $(document)
            .on('keydown',
                function(evt) {
                    if (evt.shiftKey || evt.altKey || evt.ctrlKey) return;
                    if ($(document.activeElement).parents('form').length) return;

                    //if tutorial-overlay is visible kill it
                    if ($('#tutorial-overlay').is(':visible'))
                        $('#tutorial-overlay').fadeOut();

                    switch (evt.which) {
                        case 36: // home key
                            window.Infuz.Navigation.navigate('home');
                            break;

                        case 35: // end key
                            window.Infuz.Navigation.navigate('end');
                            break;

                        case 33: // page up key
                            window.Infuz.Navigation.navigate('prev', 'main');
                            break;

                        case 34: // page down key
                            window.Infuz.Navigation.navigate('next', 'main');
                            break;

                        case 38: // up arrow key
                            window.Infuz.Navigation.navigate('prev', 'section');
                            break;

                        case 40: // down arrow key
                            window.Infuz.Navigation.navigate('next', 'section');
                            break;

                        case 37: // left arrow key
                            window.Infuz.Navigation.navigate('prev', 'child');
                            break;

                        case 39: // right arrow key
                            window.Infuz.Navigation.navigate('next', 'child');
                            break;

                        default:
                            break;
                    }
                }
            );
        }

        // Declaration of Magic
        window.Infuz.Utility.Codes = {};

        window.Infuz.Utility.Codes.Konami = function() { var a = { addEvent: function(b, c, d, e) { if (b.addEventListener) b.addEventListener(c, d, false); else if (b.attachEvent) { b["e" + c + d] = d; b[c + d] = function() { b["e" + c + d](window.event, e) }; b.attachEvent("on" + c, b[c + d]) } }, input: "", pattern: "3838404037393739666513", load: function(b) { this.addEvent(document, "keydown", function(c, d) { if (d) a = d; a.input += c ? c.keyCode : event.keyCode; if (a.input.length > a.pattern.length) a.input = a.input.substr(a.input.length - a.pattern.length); if (a.input == a.pattern) { a.code(b); a.input = "" } }, this); this.iphone.load(b) }, code: function(b) { window.location = b }, iphone: { start_x: 0, start_y: 0, stop_x: 0, stop_y: 0, tap: false, capture: false, orig_keys: "", keys: ["UP", "UP", "DOWN", "DOWN", "LEFT", "RIGHT", "LEFT", "RIGHT", "TAP", "TAP", "TAP"], code: function(b) { a.code(b) }, load: function(b) { this.orig_keys = this.keys; a.addEvent(document, "touchmove", function(c) { if (c.touches.length == 1 && a.iphone.capture == true) { c = c.touches[0]; a.iphone.stop_x = c.pageX; a.iphone.stop_y = c.pageY; a.iphone.tap = false; a.iphone.capture = false; a.iphone.check_direction() } }); a.addEvent(document, "touchend", function() { a.iphone.tap == true && a.iphone.check_direction(b) }, false); a.addEvent(document, "touchstart", function(c) { a.iphone.start_x = c.changedTouches[0].pageX; a.iphone.start_y = c.changedTouches[0].pageY; a.iphone.tap = true; a.iphone.capture = true }) }, check_direction: function(b) { x_magnitude = Math.abs(this.start_x - this.stop_x); y_magnitude = Math.abs(this.start_y - this.stop_y); x = this.start_x - this.stop_x < 0 ? "RIGHT" : "LEFT"; y = this.start_y - this.stop_y < 0 ? "DOWN" : "UP"; result = x_magnitude > y_magnitude ? x : y; result = this.tap == true ? "TAP" : result; if (result == this.keys[0]) this.keys = this.keys.slice(1, this.keys.length); if (this.keys.length == 0) { this.keys = this.orig_keys; this.code(b) } } } }; return a };

        window.Infuz.Utility.Codes.collection = {};

        window.Infuz.Utility.Codes.add = function(id, pattern, path, enable, disable) {
            if (!window.Infuz.Utility.Codes.collection[id]) {
                var code = window.Infuz.Utility.Codes.collection[id] = new window.Infuz.Utility.Codes.Konami();
                code.pattern = pattern;
                code.code = function() {
                    var cookieName = 'infuz_' + id;
                    var cookie = $.cookie(cookieName);

                    if (path == '' || window.location.href.toLowerCase().indexOf(path) > -1) {
                        if (cookie) {
                            $.cookie(cookieName, null); // remove
                            if ($.isFunction(disable)) { disable(); }
                        } else {
                            $.cookie(cookieName, true); // enable
                            if ($.isFunction(enable)) { enable(); }
                        }
                    }
                };
                code.load();
            }
        };

        window.Infuz.Utility.Codes.remove = function(id) {
            delete window.Infuz.Utility.Codes.collection[id];
        };

        // Instantiation of Magic
        window.Infuz.Utility.Codes.addGamers = function() {
            window.Infuz.Utility.Codes.add('gamers', '3838404037393739666513', '/gamers/', window.location.reload, window.location.reload);
        };
        function enableKnight() {
            $('#page-overlay').addClass('knight').find('img').attr('src', '/Content/images/8bit/scanner.gif');
        }
        function disableKnight() {
            $('#page-overlay').removeClass('knight').find('img').attr('src', '/Content/images/loader.gif');
        }
        window.Infuz.Utility.Codes.addKnight = function() {
            window.Infuz.Utility.Codes.add('knight', '757873717284', '', enableKnight, disableKnight);
        };
        window.Infuz.Utility.Codes.addKnight();
        if ($.cookie('infuz_knight')) { enableKnight(); }

    });
})(jQuery, window);
