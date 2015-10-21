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
                <h1>Charter<br>
                    Business<sup>®</sup></h1>
            </header>
            <div class="content">
                <p>
                    More than a redesign, we rebooted the brand’s <a href="http://www.charterbusiness.com/" target="_blank">digital presence</a> by focusing on what matters most... their customers’ needs.
                </p>
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
                        <img alt="" src="/Content/images/Work/Charter/Charter_Big.jpg">
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
            window.Infuz.Utility.loadVideoContent($videoSection.get(), '45267458', 448, 252, true);
            }
        );
    });
</script>
