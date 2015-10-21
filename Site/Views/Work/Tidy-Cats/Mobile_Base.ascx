<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="group content-group" id="group-work-tidycats">
    <div class="block left" style="display: none;">
        <div class="body">
            <section class="video" id="video-work-tidycats">
                <header>
                </header>
                <div class="content">
                
                    <div class="video-container">
                        <div class="video-bigbutton">
                            <img alt="" src="/Content/images/Work/Tidy-Cats/Tidy_Cats_Small.jpg">
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
                    <h1>Purina<sup>®</sup><br>
                        Tidy Cats</h1>
                </header>
                <div class="content">
                    <p>
                        We helped position Tidy Cats as the brand that brings cat owners together
                        through the sharing of stories, photos and celebrating life with their cats.
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
        var $videoSection = $('#video-work-tidycats');
        $videoSection.closest('.block').show(); // video for script-enabled only
        
        // Add a callback specific to this block of content.
        window.Infuz.Utility.loadVideoContent($videoSection.get(), '36991156', 200, 108, true);
    });
</script>
