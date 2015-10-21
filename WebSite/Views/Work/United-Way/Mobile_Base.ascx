<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="group content-group" id="group-work-unitedway">
    <div class="block left" style="display: none;">
        <div class="body">
            <section class="video" id="video-work-unitedway">
                <header>
                </header>
                <div class="content">
                
                    <div class="video-container">
                        <div class="video-bigbutton">
                            <img alt="" src="/Content/images/Work/United-Way/United_Way_Small.jpg">
                        </div>
                    </div>
                    
                </div>
                <footer>
                </footer>
            </section>
        </div>
    </div>

    <div class="block right">
        <div class="intro">
            <section>
                <header>
                    <h1>United Way of Greater St. Louis</h1>
                </header>
                <div class="content">
                    <p>
                        We designed and built <a href="http://helpingpeople.infuzyourbrand.com/" target="_blank">helpingpeople.org</a> for United Way of Greater St. Louis to
                        share their stories of success, showcase how donations affect lives and demonstrate
                        the impact United Way of Greater St. Louis has on our community.
                    </p>
                </div>
                <footer>
                </footer>
            </section>
        </div>
    </div>

    <span class="vshim"></span>
</div>

<script>
    $(function() {
        var $videoSection = $('#video-work-unitedway');
        $videoSection.closest('.block').show(); // video for script-enabled only
        
        // Add a callback specific to this block of content.
        window.Infuz.Utility.loadVideoContent($videoSection.get(), '36355526', 200, 108, true);
    });
</script>
