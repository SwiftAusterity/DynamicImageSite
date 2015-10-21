<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>
<%
    var viewId = Model.ContentPage.PartialLocation.Replace("~/", "").Replace("/", "-").Replace(".ascx", "").ToLower();
    var videoSectionId = viewId.Replace("view", "video");
%>
<div id="<%= viewId %>" style="display: none;"></div>

<div class="block left">
    <div class="intro dark">
        <section>
            <header>
                <h1>Ability Meets Opportunity</h1>
            </header>
            <div class="content">
                <p>Part research, part development and all passion. Infuz labs is the mashup of our intellectual curiosity, social wiring and technical chops. This is the workshop where our creative, strategy and development teams create unique products to solve common problems. From content mining and aggregation tools to our <a href="http://www.buzzradius.com/" target="_blank">Buzz Radius</a> framework that magnifies real-time interactions, Infuz Labs brings interaction and analysis to life in productive ways.
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
            
                <!--
                <div class="video-container">
                    <div class="video-bigbutton">
                        <img alt="" src="/Content/images/Labs/Labs_Big.jpg">
                    </div>
                </div>
                -->
                
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
        /*
        window.Infuz.PageTransition.addPaneCallback(
            '<%= viewId %>',
            function() {
                window.Infuz.Utility.loadVideoContent($videoSection.get(), '00000000', 448, 252, true);
            }
        );
        */
    });
</script>

