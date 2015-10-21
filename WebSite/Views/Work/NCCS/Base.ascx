<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>
<%
    var viewId = Model.ContentPage.PartialLocation.Replace("~/", "").Replace("/", "-").Replace(".ascx", "").ToLower();
    var videoSectionId = viewId.Replace("view", "video");
%>
<div id="<%= viewId %>" style="display: none;"></div>

<div class="block left">
    <div class="intro">
        <section>
            <header>
                <h1>The National Children’s Cancer Society</h1>
            </header>
            <div class="content">
                <p>
                    We created a Facebook fundraising campaign to drive awareness and donations for
                    the National Children’s Cancer Society, a non-profit organization dedicated to
                    improving the quality of life for children with cancer and their families worldwide.
                    <a href="http://www.facebook.com/thenccs?sk=app_209341512454884" title="Face-Off Against Cancer" target=_"blank">Face-Off Against Cancer</a> used the psychology of gaming to bring out competitive instincts of new donors across the country.</p>
            </div>
            <footer>
            </footer>
        </section>
    </div>
</div>

<div class="block right" style="display: none;">
    <div class="body">
        <section class="video" id="<%= videoSectionId %>">
            <header>
            </header>
            <div class="content">
            
                <div class="video-container">
                    <div class="video-bigbutton">
                        <img alt="" src="/Content/images/Work/NCCS/NCCS_Big.jpg">
                    </div>
                </div>
                
            </div>
            <footer>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>

<script>
    $(function() {
        var $videoSection = $('#<%= videoSectionId %>');
        $videoSection.closest('.block').show(); // video for script-enabled only
        
        // Add a callback specific to this pane.
        // 'this' will be the pane that this partial is loaded into.
        window.Infuz.PageTransition.addPaneCallback(
            '<%= viewId %>',
            function() {
                window.Infuz.Utility.loadVideoContent($videoSection.get(), '34678407', 448, 252, true);
            }
        );
    });
</script>
