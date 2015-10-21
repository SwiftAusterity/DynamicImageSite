<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="group content-group" id="group-work">
    <div class="block left" style="display: none;">
        <div class="body">
            <section class="video" id="video-work">
                <header>
                </header>
                <div class="content">
                
                    <div class="video-container">
                        <div class="video-bigbutton">
                            <img alt="" src="/Content/images/Work/Reel_Small.jpg">
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
                    <h1>Interaction<br>
                        By Design</h1>
                </header>
                <div class="content">
                    <p>
                        We engineer targeted ideas that beg to be
                        implemented. Our solutions are carefully planned
                        and executed using our unique methodology to
                        relationship building.
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
        var $videoSection = $('#video-work');
        $videoSection.closest('.block').show(); // video for script-enabled only
        
        // Add a callback specific to this block of content.
        window.Infuz.Utility.loadVideoContent($videoSection.get(), '27809709', 200, 108, true);
    });
</script>
