;(function ($, window, undefined) {
    
    var document = window.document;
    
    // DOM-ready
    $(function () {
    
        // Set up mobile detect (tablets should use the full site)
        var isMobile = DetectTierIphone()
            || DetectTierRichCss()
            || DetectTierOtherPhones();
        $('html').addClass((!isMobile ? 'no-' : '') + 'mobile');

        // Helper functions
        function randomRange(minVal, maxVal) {
            return Math.floor(minVal + (Math.random() * (maxVal - minVal)));
        }
    
        // Added features for non-mobile experience
        if (!isMobile) {
        
            $('#stage').cycle({
                fit: true,
                fx: 'scrollLeft',
                width: '100%',
                height: '100%',
                slideExpr: '.pane',
                timeout: 0
            });

            $('#ticker-slides').cycle({
                fit: true,
                fx: 'fade',
                pause: true
            });
            
            var filterNav = {
            
                setActive: function (index) {
                    var iconHtml = '<span class="icon icon-sm icon-arrow-up"></span>';
                    var $items = $('#section-nav .filters li');
                    $items.removeClass('active open');
                    $items.find('a .icon-arrow-up').remove();
                    $items.eq(index).addClass('active').find('a').prepend(iconHtml);
                },
                
                open: function () {
                    var $active = $(this).find('li.active');
                    var $items = $(this).find('li:not(.active)');
                    if ($items.queue().length < 2) {
                        $active.addClass('open').find('.icon').fadeOut();
                        $items.slideDown();
                    }
                },
                
                close: function () {
                    var $active = $(this).find('li.active');
                    var $items = $(this).find('li:not(.active)');
                    if ($items.queue().length < 2) {
                        $items.slideUp();
                        $active.removeClass('open').find('.icon').fadeIn();
                    }
                }
                
            };
            
            $('#section-nav .filters')
            .hover(filterNav.open, filterNav.close)
            .click(
                function (evt) {
                    evt.preventDefault();

                    filterNav.setActive.call(this, $(evt.target).parent().index());
                    filterNav.close.call(this);
                    
                    // Collapse child nav
                    $('#section-nav .children li').each(
                        function () {
                            $(this).animate({ left: (-$(this).position().left), opacity: 0 }, 500);
                        }
                    );

                    // TODO: AJAX get content
                    
                    // On complete, show new content and new child nav
                    setTimeout(
                        function () {
                            $('#section-nav .children li').each(
                                function () {
                                    $(this).animate({ left: 0, opacity: 1 }, 500);
                                }
                            );
                        }, 1000
                    );
                }
            );
                        
            var pageInit = function () {
                filterNav.setActive(0);
            };
            
            pageInit();
        
        }
            
    });
})(jQuery, window);
