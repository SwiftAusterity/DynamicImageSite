; (function($, window, undefined) {

    var document = window.document;

    // Add additional members to the application object
    // --------------------------------------------------
    (function() {
        window.Infuz.Ajax.formatRoute = function(route) {
            return route
                    .replace(/([^\/])$/, '$1/') // add a slash to the end
                    .replace('#!', '')          // remove hash bang
                    .replace('!', '')           // or just bang
                    .replace(/\//gi, '_');      // replace slash with underscore
        };

        window.Infuz.Ajax.getRoutesFromService = function() {
            var ajaxUrl = '/Ajax/GetRoutes/' + window.Infuz.Utility.isMobile;
            $.getJSON(ajaxUrl, function(data) {
                for (x = 0; x < data.routes.length; x += 1) {
                    $.routes.add('!' + data.routes[x].Url, data.routes[x].Url, data.routes[x].SectionName, data.routes[x].SubSectionName, null, function(data) {
                        var route = $.routes.current.route;
                        var prevRoute = '';

                        if (window.Infuz.Navigation.previousRoute != null)
                            prevRoute = window.Infuz.Navigation.previousRoute.route;

                        window.Infuz.Ajax.loadContent(window.Infuz.Ajax.formatRoute(route), window.Infuz.Ajax.formatRoute(prevRoute));
                    });
                }
            })
            .complete(function(data) {
                if (location.hash.length > 0) {
                    $.routes.load(location.hash);
                }
            });
        };

        window.Infuz.Ajax.loadContent = function(route, prevRoute) {
            var ajaxUrl = '/Ajax/GetContent/' + route + '/' + prevRoute;
            $.getJSON(ajaxUrl, function(data) {
                // Success
                if (!data) return;
                if (data.success === false) return;

                document.title = data.title;

                // Ensure that videos are stopped
                window.Infuz.Utility.resetVideoContainers();

                // Assign primary nav items
                // ----------------------------------------------------------
                var $primaryNav = $('#primary-nav ul');
                $primaryNav.empty();

                var newPrimaryNav = '';
                for (x = 0; data.primaryNav && x < data.primaryNav.length; x += 1) {
                    var activeClass = data.primaryNav[x].Url.split('/')[1] == data.area ? ' class="active"' : '';
                    newPrimaryNav += '<li' + activeClass + '><a href="/#!' + data.primaryNav[x].Url + '">' + data.primaryNav[x].Name + '</a></li>';
                }
                newPrimaryNav += '<li><a href="http://blog.infuz.com/">Blog</a></li>';

                $primaryNav.html(newPrimaryNav);

                // Assign section nav items
                // ----------------------------------------------------------
                var $sectionNav = $('#section-nav .filters');
                $sectionNav.empty();

                // Only show section nav if more than one item
                if (data.sectionNav.length > 1) {
                    var newSectionNav = '';
                    for (x = 0; x < data.sectionNav.length; x += 1) {
                        newSectionNav += '<li><a href="/#!' + data.sectionNav[x].Url + '">' + data.sectionNav[x].Name + '</a></li>';
                    }

                    $sectionNav.html(newSectionNav);

                    // Scan the items to see which (if any) need to be marked active
                    $('li', $sectionNav).each(
                        function() {
                            var $item = $(this);
                            if (location.href.toLowerCase().indexOf($('a', $item).prop('href').toLowerCase()) > -1
                                || (location.href + '/').toLowerCase().indexOf($item.find('a').prop('href').toLowerCase()) > -1) {
                                window.Infuz.Navigation.filterNav.setActive.call($sectionNav.get(), $item.index());
                            }
                        }
                    );

                    window.Infuz.Navigation.filterNav.close.call($sectionNav.get(), false);

                    // Auto-show the section nav for first visit
                    if ($.cookie('navTip') != 'true') {
                        window.Infuz.Navigation.filterNav.open.call($sectionNav.get());
                        $.cookie('navTip', 'true', { expires: 60, path: '/' });
                    }
                }
                else {
                    window.Infuz.Navigation.filterNav.setAllInactive.call($sectionNav.get(), false);
                    window.Infuz.Navigation.filterNav.close.call($sectionNav.get(), false);
                }

                // Hide the infobox
                var $infoBox = $('.infoBox');
                var $infoButton = $('.info-button');

                $infoButton.fadeOut();
                window.Infuz.Navigation.infoBox.close.call($infoBox.get());

                // Ensure we need to show the nav
                window.Infuz.Navigation.filterNav.verifyDisplay.call($sectionNav.get());

                // Perform next page animation
                // ----------------------------------------------------------
                var bgImageUrl = data.backgrounds[0];
                if ($.cookie('infuz_gamers') && data.section == 'Gamers' && bgImageUrl.indexOf('/8bit/') == -1) {
                    bgImageUrl = bgImageUrl.replace('/Images/', '/Images/8bit/');
                }
                var sectionClass = window.Infuz.Utility.getSectionClass(data.url);
                var nextPane = $('#stage .pane:hidden:last')
                    .attr('class', 'pane')
                    .addClass(sectionClass);

                $('.main', nextPane).html(data.html);

                // Use a plugin for browsers that don't support the CSS3 property "background-size"
                if (window.Infuz.Support.css('backgroundSize', nextPane.get(0))) {
                    nextPane.css('background-image', 'url("' + bgImageUrl + '")');
                } else {
                    // Delay helps make sure the container element's size is fully established
                    setTimeout(function() {
                        nextPane.css('background', 'none');
                        $('.pane-inner', nextPane).anystretch(bgImageUrl);

                        // Trigger resize to ensure image is re-calculated to proper size
                        $(window).resize();
                    }, 50);
                }

                window.Infuz.Utility.sniffAndReplaceHashBangUrls(nextPane);

                window.Infuz.PageTransition.direction = (data.direction && data.direction.length ? data.direction : '');
                window.Infuz.PageTransition.bgImage = bgImageUrl;

                $('#stage').cycle('next');

                // Save the last page loaded
                window.Infuz.PageTransition.last = data;

                // Infobox
                // ----------------------------------------------------------
                $infoBox.fadeOut();
                $infoButton.fadeOut();
                if (data.infoText.length > 0) {
                    $infoBox.html(data.infoText);
                    $infoButton.fadeIn();

                    // Initial open
                    window.Infuz.Navigation.infoBox.open.call($infoBox.get());
                }

                // Update prev/next nav
                // ----------------------------------------------------------
                var $pageNav = $('#paging-nav'),
                $first = $pageNav.find('a:first'),
                $next = $pageNav.find('a.next'),
                $prev = $pageNav.find('a.prev'),

                pageNavUpdate = function(node, $item) {
                    if (node != null && node.Url != null) {
                        $item
                            .add('.icon', $item)
                            .fadeIn()
                            .css({ 'cursor': 'pointer' })
                            .off('click.pagenav')
                            .attr('href', '/#!' + node.Url);
                    } else {
                        $item
                            .add('.icon', $item)
                            .fadeOut()
                            .css({ 'cursor': 'default' })
                            .on('click.pagenav', function(evt) { evt.preventDefault(); })
                            .attr('href', '');
                    }
                };

                pageNavUpdate(data.forwardNav, $next);
                pageNavUpdate(data.backwardNav, $prev);

                // Assign child nav items and click actions
                // ----------------------------------------------------------
                var $childNav = $('#section-nav .children');
                var newSubNav = '';
                for (x = 0; x < data.subNav.length; x += 1) {
                    newSubNav += '<li style="opacity: 0"><a href="/#!' + data.subNav[x].Url + '" style="background: url(' + data.subNav[x].Thumbnail + ') no-repeat;"><span class="alt-content">' + data.subNav[x].Name + '</span><b></b></a></li>';
                }

                $childNav.each(
                    function() {
                        var $this = $(this);
                        var currentRoute = $.routes.current;
                        var previousRoute = window.Infuz.Navigation.previousRoute;
                        var newNavNeeded = !previousRoute
                                            || (window.Infuz.Navigation.filterNav.newSectionChange)
                                            || previousRoute.level != currentRoute.level
                                            || previousRoute.area != currentRoute.area
                                            || previousRoute.section != currentRoute.section
                                            ;

                        if (newNavNeeded) {
                            window.Infuz.Navigation.childNav.close.call(this, false);
                            $this.html(newSubNav);
                            window.Infuz.Navigation.childNav.open.call(this);
                        }

                        $this
                        .off('click.childNav')
                        .on('click.childNav', 'a',
                            function() {
                                var $item = $(this).closest('li');
                                window.Infuz.Navigation.childNav.setActive.call(this, $item.index());
                                $item.siblings().removeClass('active').end().addClass('active');
                            }
                        );

                        // Scan the items to see which (if any) need to be marked active
                        $this.find('li').each(
                            function() {
                                var $item = $(this);
                                if ($item.find('a').prop('href').toLowerCase() == location.href.toLowerCase()) {
                                    window.Infuz.Navigation.childNav.setActive.call($this, $item.index());
                                }
                            }
                        );

                        // Ensure we need to show the nav
                        window.Infuz.Navigation.childNav.verifyDisplay.call(this);
                    }
                );

                // Might need to start the ticker
                window.Infuz.Utility.startTicker($('.ticker', nextPane));

                window.Infuz.Navigation.filterNav.newSectionChange = false;

                // Set up page rotate, if necessary
                clearTimeout(window.Infuz.PageTransition._rotateId);
                delete window.Infuz.PageTransition._rotateId;

                if (data.slideshow && window.Infuz.Utility.isSlideshowRunning) { 
                    var rotatePause = 10000; // ms pause between slides
                    var nextUrl = $next.attr('href');
                    var rotateTo = nextUrl.length ? nextUrl : '';
                    if (rotateTo.length) {
                        window.Infuz.PageTransition._rotateId = setTimeout(function() {
                            window.location.href = rotateTo;
                        }, rotatePause);
                    }
                }

                // Track the page at the bottom
                // don't make it tell google if its being rotated into
                if (!window.Infuz.PageTransition._rotateId || data.url == '/' || !window.Infuz.Utility.isSlideshowRunning)
                    _gaq.push(['_trackPageview', route.toLowerCase().replace(/_/gi, '/')]);
            })
            .complete(function(data) {
                // Request complete
            })
            .error(function() {
                // Request error
            });
        };

    })();

})(jQuery, window);