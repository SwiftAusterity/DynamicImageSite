<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="group content-group" id="group-work-loufest">
    <div class="block left" style="display: none;">
        <div class="body">
            <section class="video" id="video-work-loufest">
                <header>
                </header>
                <div class="content">
                
                    <div class="video-container">
                        <div class="video-bigbutton">
                            <img alt="" src="/Content/images/Work/LouFest/LouFest_Small.jpg">
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
                    <h1>LouFest<br>
                        Music Festival</h1>
                </header>
                <div class="content">
                    <p>
                        We used Twitter to active concert-goers, making the annual music festival a more engaging and rewarding experience. 
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
        var $videoSection = $('#video-work-loufest');
        $videoSection.closest('.block').show(); // video for script-enabled only
        
        // Add a callback specific to this block of content.
        window.Infuz.Utility.loadVideoContent($videoSection.get(), '36361902', 200, 108, true);
    });
</script>
