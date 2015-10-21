<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="block left">
    <div class="body">
        <section>
            <header>
            </header>
            <div class="content">
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
                <h1 class="alt-content">404 MISSING</h1>
            </header>
            <div class="content">
                <p>
                </p>
            </div>
            <footer>
            </footer>
        </section>
    </div>
</div>

<span class="vshim"></span>

<% Html.RenderPartial("~/Views/Shared/Spotlight.ascx"); %>
