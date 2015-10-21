<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="group content-group" id="group-work-nccs">
    <div class="block left" style="display: none;">
        <div class="body">
            <section class="video" id="video-work-nccs">
                <header>
                </header>
                <div class="content">
                
                    <div class="video-container">
                        <div class="video-bigbutton">
                            <img alt="" src="/Content/images/Work/NCCS/NCCS_Small.jpg">
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
                    <h1>National Children’s Cancer Society</h1>
                </header>
                <div class="content">
                    <p>
                        The Infuz 2010 Summer Intern team created a Facebook fundraising campaign to
                        drive awareness and donations for the National Children's Cancer Society.
                        Face-Off Against Cancer used the psychology of gaming to bring out competitive
                        instincts or new donors across the country.
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
        var $videoSection = $('#video-work-nccs');
        $videoSection.closest('.block').show(); // video for script-enabled only
        
        // Add a callback specific to this block of content.
        window.Infuz.Utility.loadVideoContent($videoSection.get(), '34678407', 200, 108, true);
    });
</script>
