using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Ninject;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Models;
using Site.ViewModels;

using Infuz.Utilities;

namespace Site.Controllers
{
    [HandleError]
    public class TextAdventureController : Controller
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public UserDataProvider UserProvider { get; set; }

        [Inject]
        public IContentPageRepository ContentPageRepository { get; set; }

        [Inject]
        public ITextCommandRepository TextCommandRepository { get; set; }

        public ActionResult Index()
        {
            var viewModel = new TextAdventureViewModel();
            return View("Index", viewModel);
        }

        private System.Random rand;

        #region synonyms and command trees
        private List<String> ConJunkTions = new List<String>
                                            { 
                                                "the"
                                                , "of"
                                            };

        private List<KeyValuePair<String, String>> SynonymTargets = new List<KeyValuePair<String, String>>
                        {
                           new KeyValuePair<String, String>("north", "n")
                           , new KeyValuePair<String, String>("west", "w")
                           , new KeyValuePair<String, String>("south", "s")
                           , new KeyValuePair<String, String>("east", "e")
                           , new KeyValuePair<String, String>("up", "u")
                           , new KeyValuePair<String, String>("down", "d")
                           , new KeyValuePair<String, String>("help", "?")
                           , new KeyValuePair<String, String>("help", "comm")
                           , new KeyValuePair<String, String>("help", "man")
                            , new KeyValuePair<String, String>("help", "commands")
                           , new KeyValuePair<String, String>("help", "command")
                           , new KeyValuePair<String, String>("look", "l")
                           , new KeyValuePair<String, String>("disconnect", "exit")
                           , new KeyValuePair<String, String>("disconnect", "quit")
                      };

        private List<KeyValuePair<String, String>> SynonymPhrases = new List<KeyValuePair<String, String>>
                        {
                            new KeyValuePair<String, String>("look ", "look at ")
                           , new KeyValuePair<String, String>("look ", "read ")
                           , new KeyValuePair<String, String>("north", "walk north")
                           , new KeyValuePair<String, String>("north", "go north")
                           , new KeyValuePair<String, String>("north", "head north")
                           , new KeyValuePair<String, String>("west", "walk west")
                           , new KeyValuePair<String, String>("west", "go west")
                           , new KeyValuePair<String, String>("west", "head west")
                           , new KeyValuePair<String, String>("south", "walk south")
                           , new KeyValuePair<String, String>("south", "go south")
                           , new KeyValuePair<String, String>("south", "head south")
                           , new KeyValuePair<String, String>("east", "walk east")
                           , new KeyValuePair<String, String>("east", "go east")
                           , new KeyValuePair<String, String>("east", "head east")
                           , new KeyValuePair<String, String>("up", "walk up")
                           , new KeyValuePair<String, String>("up", "go up")
                           , new KeyValuePair<String, String>("up", "head up")
                           , new KeyValuePair<String, String>("down", "walk down")
                           , new KeyValuePair<String, String>("down", "go down")
                           , new KeyValuePair<String, String>("down", "head down")
                      };


        private List<KeyValuePair<String, String>> JunkCommands = new List<KeyValuePair<String, String>>
                        {
                            new KeyValuePair<String, String>("Masturbate", "This isn't a Sierra game, you know.")
                           , new KeyValuePair<String, String>("xyzzy", "Nothing Happens")
                           , new KeyValuePair<String, String>("fuck", "PARITY ERROR.")
                           , new KeyValuePair<String, String>("shit", "PARITY ERROR.")
                           , new KeyValuePair<String, String>("Rent", "Try Save, there's no rent here.")
                           , new KeyValuePair<String, String>("Save", "Did you really think that would work?")
                           , new KeyValuePair<String, String>("Who", "How am I supposed to know?")
                           , new KeyValuePair<String, String>("Score", "Marlins 2, Cardinals 4, bot 6th")
                           , new KeyValuePair<String, String>("I want to kill myself", "And how are you going to get this 'to kill myself'?")
                           , new KeyValuePair<String, String>("i", "Check your pockets.")
                           , new KeyValuePair<String, String>("inv", "Check your pockets.")
                           , new KeyValuePair<String, String>("inventory", "Check your pockets.")
                           , new KeyValuePair<String, String>("shutdown", "Nice try.")
                           , new KeyValuePair<String, String>("bill paxton", "Administrator mode unlocked.")
                           , new KeyValuePair<String, String>("hearthstone", "That's so 2004. (try <em>recall</em>)")
                      };
        #endregion

        //This whole thing is AJAX, and we only really need one method for getting anything.
        public ActionResult MakeCommand(String path, String command, String callback)
        {
            rand = new System.Random();

            var viewModel = new BaseViewModel(Kernel, HttpContext);
            path = path.Replace("_", "/");

            var contentPage = ContentPageRepository.Get(path, false);
            var contentMap = ContentPageRepository.Get(false).ToList();

            if (contentPage != null)
            {
                viewModel.ContentPage = contentPage;

                var helped = false;
                var renderPage = false;
                var newWindowPath = String.Empty;
                var toDescriptor = new StringBuilder();

                //We're going to need to interpret the command we got sent.
                command = CrunchGrammer(command);

                //Movement and base "look" will be handled by paths with empty commands
                if (String.IsNullOrEmpty(command))
                    renderPage = true;
                else
                {
                    //We need to accept "say" commands to talk to people on people pages
                    if (command.StartsWith("disconnect", StringComparison.InvariantCultureIgnoreCase))
                    {
                        helped = true;
                        toDescriptor.Append("<br/>");
                        toDescriptor.Append(GetExitStatement());
                        toDescriptor.Append("<br/>");
                    }

                    //We need to accept "say" commands to talk to people on people pages
                    if (command.StartsWith("say", StringComparison.InvariantCultureIgnoreCase))
                    {
                        helped = true;
                        toDescriptor = Converse(command, contentPage);
                    }

                    if (command.Equals("map", StringComparison.InvariantCultureIgnoreCase))
                    {
                        helped = true;
                        toDescriptor.Append(GenerateMapString(contentPage, contentMap));
                    }

                    //back to start!
                    if (command.Equals("recall", StringComparison.InvariantCultureIgnoreCase))
                    {
                        helped = true;
                        renderPage = true;
                        contentPage = contentMap.ElementAt(0);
                        toDescriptor.Append("You use a scroll of town portal.");
                        toDescriptor.Append("<br/>");
                        toDescriptor.Append("<br/>");
                    }

                    //We'll want to accept "look" directed commands and add an openNewWindow so they can see images and videos
                    if (command.Equals("look", StringComparison.InvariantCultureIgnoreCase))
                    {
                        helped = true;
                        renderPage = true;
                    }
                    else if (command.StartsWith("look", StringComparison.InvariantCultureIgnoreCase) && command.Length >= 6)
                    {
                        helped = true;

                        //Does the object exist?
                        var renderedHtml = RenderHtml(contentPage.PartialLocation, viewModel);
                        var objects = GetPageObjects(renderedHtml, contentPage);
                        var objectName = command.Substring(5);

                        if (objects.Any(kvp => kvp.Key.Replace("link_", String.Empty).Equals(objectName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var thing = objects.FirstOrDefault(kvp => kvp.Key.Replace("link_", String.Empty).Equals(objectName, StringComparison.InvariantCultureIgnoreCase));

                            //is a link?
                            if (thing.Value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newWindowPath = thing.Value;
                                toDescriptor.AppendLine("You look at the " + thing.Key.Replace("link_", String.Empty) + ".");
                            }
                            else //isnt a link
                            {
                                toDescriptor.AppendLine(thing.Value);
                            }
                        }
                        else
                            toDescriptor.AppendLine("That does not exist.");
                    }

                    //We need to accept "help" commands, which output the help text.
                    if (command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
                    {
                        helped = true;
                        toDescriptor = AppendHelp();
                    }

                    //Are we moving somewhere?
                    if (!helped)
                    {
                        var directions = GetDirections(contentPage, contentMap);

                        if (directions.Any(dir => dir.Key.Equals(command, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var newDirection = directions.FirstOrDefault(dir => dir.Key.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                            renderPage = true;
                            helped = true;

                            var newPage = ContentPageRepository.Get(newDirection.Value.Url, false);

                            if (newPage == null)
                            {
                                helped = false;
                                renderPage = false;
                            }
                            else
                            {
                                contentPage = newPage;
                                toDescriptor = new StringBuilder();
                                toDescriptor.AppendFormat("You walk {0}.", newDirection.Key);
                                toDescriptor.AppendLine("<br/>");
                                toDescriptor.AppendLine("<br/>");
                            }
                        }
                    }

                    //We can accept junk commands for amusement
                    if (JunkCommands.Any(kp => kp.Key.Equals(command, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        helped = true;
                        toDescriptor = AppendJunkCommand(command);
                    }

                    //Finally, we need to throw up errors on unknown commands, this is a takeover
                    if (!helped)
                    {
                        TextCommandRepository.Save(command, path, false);
                        helped = true;
                        toDescriptor = AppendUnknownCommandError(command);
                    }
                    else
                        TextCommandRepository.Save(command, path, true);
                }

                if (renderPage)
                {
                    viewModel.ContentPage = contentPage;

                    var renderedHtml = RenderHtml(contentPage.PartialLocation, viewModel);
                    var objects = GetPageObjects(renderedHtml, contentPage);
                    var directions = GetDirections(contentPage, contentMap);
                    toDescriptor.Append(ProcessHtml(renderedHtml, directions, contentPage, objects).ToString());
                }

                return new JsonPResult(callback,
                       Json(new
                       {
                           toDescriptor = toDescriptor.ToString(),
                           url = contentPage.Url,
                           area = contentPage.SectionName,
                           section = contentPage.SubSectionName,
                           commandEcho = command,
                           pathEcho = path,
                           openNewWindow = newWindowPath,
                           title = contentPage.Title
                       }));
            }

            return new JsonPResult(callback, Json(new { success = false, error = "Invalid content identifier." }));
        }

        private IDictionary<String, INavElement> GetDirections(IContentPage page, IList<IContentPage> contentMap)
        {
            var returnValue = new Dictionary<String, INavElement>();

            //these are easy
            if (page.ForwardNav != null)
                returnValue.Add("east", page.ForwardNav);

            if (page.BackwardNav != null)
                returnValue.Add("west", page.BackwardNav);

            //We might have an north/south because we're somewhere in a section that has more than one sub
            if (page.SectionNav.Count() > 1)
            {
                var section = page.SectionNav.ToList();
                var currentSectionIndex = section.IndexOf(section.FirstOrDefault(s => s.Url.Equals(page.SubSectionUrl)));

                if (currentSectionIndex > 0)
                    returnValue.Add("north", section.ElementAt(currentSectionIndex - 1));

                if (currentSectionIndex < section.Count() - 1)
                    returnValue.Add("south", section.ElementAt(currentSectionIndex + 1));
            }

            //We'll always have either an up, down or both
            var mainNav = page.PrimaryNav.ToList();
            var currentMainIndex = mainNav.IndexOf(mainNav.FirstOrDefault(m => m.Url.Equals(page.SectionUrl)));

            if (currentMainIndex > 0)
                returnValue.Add("up", mainNav.ElementAt(currentMainIndex - 1));
            else if (currentMainIndex == 0)
                returnValue.Add("up", contentMap.ElementAt(0).SubNav.ElementAt(0));

            if (currentMainIndex < mainNav.Count() - 1)
                returnValue.Add("down", mainNav.ElementAt(currentMainIndex + 1));

            return returnValue;
        }

        private IDictionary<String, String> GetPageObjects(String html, IContentPage page)
        {
            var returnValue = new Dictionary<String, String>();

            //add the mural
            var muralUri = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + page.Backgrounds.ElementAt(0));
            returnValue.Add("mural", muralUri.AbsoluteUri);

            //add the nameplate
            returnValue.Add("nameplate", "The nameplate reads, '" + page.Title + "'.");

            //banner
            var roomDesc = GetPageInnerContent(html, page);
            if (!String.IsNullOrEmpty(roomDesc))
                returnValue.Add("banner", "The banner reads: <p>" + roomDesc + "<p>");

            //Append the info box text
            if (page.InfoText.Length > 0)
            {
                var infoBoxText = new StringBuilder();
                AppendInfoBoxText(infoBoxText, page.InfoText);

                returnValue.Add("sign", infoBoxText.ToString());
            }

            if (!String.IsNullOrEmpty(html))
            {
                var contentLength = html.Length;
                //find videos
                //http://player.vimeo.com/video/36723314?title=0&byline=0&portrait=0&autoplay=1
                //$videoSection.get(), '
                int iterator = 0;
                while (iterator < contentLength)
                {
                    var newIndex = html.IndexOf("$videoSection.get(), '", iterator);

                    if (newIndex > -1)
                    {
                        newIndex += 22;
                        var endIndex = html.IndexOf("'", newIndex);
                        var newHref = String.Format("http://player.vimeo.com/video/{0}?title=0&byline=0&portrait=0&autoplay=1"
                                                    , html.Substring(newIndex, endIndex - newIndex));
                        newIndex = endIndex + 1;

                        var newTitle = "video";

                        Uri linkUri = new Uri(newHref);

                        returnValue.Add(newTitle, linkUri.AbsoluteUri);
                    }
                    else break;

                    iterator = newIndex;
                }

                //Find any anchor
                iterator = 0;
                while (iterator < contentLength)
                {
                    var newIndex = html.IndexOf("href=\"", iterator);

                    if (newIndex > -1)
                    {
                        newIndex += 6;
                        var endIndex = html.IndexOf("\"", newIndex);
                        var newHref = html.Substring(newIndex, endIndex - newIndex);
                        newIndex = endIndex + 1;

                        newIndex = html.IndexOf(">", newIndex);
                        newIndex++;
                        endIndex = html.IndexOf("</a>", newIndex);
                        var newTitle = "link_" + html.Substring(newIndex, endIndex - newIndex);
                        newIndex = endIndex + 1;

                        Uri linkUri;
                        if (Uri.IsWellFormedUriString(newHref, UriKind.Absolute))
                            linkUri = new Uri(newHref);
                        else
                            linkUri = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + newHref);

                        if (returnValue.Any(kvp => kvp.Key.Equals(newTitle)))
                        {
                            var i = 2;
                            var newNewTitle = String.Empty;
                            while (returnValue.Any(kvp => kvp.Key.Equals(newNewTitle)))
                                newNewTitle = String.Format("{0}_{1}", newTitle, i);

                            newTitle = newNewTitle;
                        }

                        returnValue.Add(newTitle, linkUri.AbsoluteUri);
                    }
                    else break;

                    iterator = newIndex;
                }
            }

            return returnValue;
        }

        private String GetPageHeaderTitle(String html, IContentPage page)
        {
            var returnValue = String.Format("<h1>page.Title</h1>");

            if (!String.IsNullOrEmpty(html))
            {
                //Find the right header
                var contentLength = html.Length;
                var iterator = 0;
                while (iterator < contentLength)
                {
                    //find anchors
                    var newIndex = html.IndexOf("<header>", iterator);

                    if (newIndex > -1)
                    {
                        newIndex += 8;
                        var endIndex = html.IndexOf("</header>", newIndex);
                        var newHeader = html.Substring(newIndex, endIndex - newIndex);
                        newIndex = endIndex + 1;

                        if (!String.IsNullOrEmpty(newHeader.Trim()))
                        {
                            //I dont like html in my headers, just use the title please.
                            if (newHeader.IndexOf("<span") > -1)
                                break;

                            returnValue = newHeader.Replace("<br>", " ").Trim();
                            break;
                        }
                    }
                    else
                        break;

                    iterator = newIndex;
                }
            }
            return returnValue;
        }

        private String GetPageInnerContent(String html, IContentPage page)
        {
            var returnValue = page.Title;

            if (!String.IsNullOrEmpty(html))
            {
                //Find the right header
                var contentLength = html.Length;
                var iterator = 0;
                while (iterator < contentLength)
                {
                    //find anchors
                    var newIndex = html.IndexOf("<div class=\"content\">", iterator);
                    if (newIndex > -1)
                    {
                        newIndex += 21;
                        newIndex = html.IndexOf("<p>", newIndex);
                        newIndex += 3;
                        var endIndex = html.IndexOf("</p>", newIndex);
                        var newBody = html.Substring(newIndex, endIndex - newIndex);
                        newIndex = endIndex + 1;

                        if (!String.IsNullOrEmpty(newBody.Trim()))
                        {
                            returnValue = newBody.Replace("<br>", " ").Trim();
                            break;
                        }
                    }
                    else
                        break;

                    iterator = newIndex;
                }
            }
            return returnValue;
        }

        private StringBuilder ProcessHtml(string html, IDictionary<String, INavElement> directions, IContentPage page, IDictionary<String, String> objects)
        {
            var returnValue = new StringBuilder();

            //find the room header/title thing
            var roomHeader = GetPageHeaderTitle(html, page);
            returnValue.AppendLine(roomHeader);
            returnValue.AppendLine("<br/>");

            //Append the initial state
            returnValue.AppendFormat("You {0} in a {1} room. ", GetRandomStance(), GetRandomRoomAdjective());

            //append the room desc
            var roomDesc = GetPageInnerContent(html, page);

            if (!String.IsNullOrEmpty(roomDesc))
                returnValue.AppendFormat("A <strong>banner</strong> hangs across the room. ");

            //Append the background mural
            var muralHref = page.Backgrounds.ElementAt(0);
            returnValue.AppendFormat("A <a href=\"{1}\" target=\"_blank\">mural</a> {0} the celing. "
                , GetRandomControlVerb(), muralHref);

            //Append the nameplate
            returnValue.AppendFormat("A {0} {1} <strong>nameplate</strong> is affixed to the center of the floor. "
                , GetRandomRoomAdjective(), GetRandomNameplateAdjective());

            //Append a video if we need to
            foreach (KeyValuePair<String, String> obj in objects
                .Where(vid => vid.Key.Equals("video", StringComparison.InvariantCultureIgnoreCase)))
                returnValue.AppendFormat("A large plasma television hangs on a wall, playing a <a href=\"{0}\" target=\"_blank\">video</a>. ", obj.Value);

            //Append any links in the body
            foreach (KeyValuePair<String, String> obj in objects
                .Where(lk => lk.Key.StartsWith("link_", StringComparison.InvariantCultureIgnoreCase)))
            {
                var objName = obj.Key.Replace("link_", String.Empty);
                var linkDecoration = GetRandomLinkDecoration();
                returnValue.AppendFormat("{2}<a href=\"{1}\" target=\"_blank\">{0}</a>{3}", objName, obj.Value, linkDecoration.First, linkDecoration.Second);
            }

            //Append the info box text
            if (page.InfoText.Length > 0)
                AppendInfoBoxText(returnValue, page.InfoText);

            var personName = GetPerson(page);
            if (!String.IsNullOrEmpty(personName))
            {
                //Append the person if we're on that page
                returnValue.AppendLine("<br/>");
                returnValue.AppendLine("<br/>");
                returnValue.AppendFormat("<em>{0}</em> {1}. ", personName, GetNPCStance());
            }

            //Append the directions
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("<br/>");
            returnValue.Append("Exits: ");
            foreach (KeyValuePair<String, INavElement> dir in directions)
                returnValue.AppendFormat("<em>{1}</em>, ", dir.Value.Url, dir.Key);

            if (directions.Count() > 0)
                returnValue.Length -= 2;

            return returnValue;
        }

        private void AppendInfoBoxText(StringBuilder sb, String infoText)
        {
            sb.AppendFormat("A {0} {1} <strong>sign</strong> is attached to the floor. ", GetRandomRoomAdjective(), GetRandomNameplateAdjective());
        }

        private StringBuilder AppendUnknownCommandError(String command)
        {
            var returnValue = new StringBuilder();

            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue.Append("You shouldn't type drunk.");
                    break;
                case 2:
                    returnValue.Append("Why not try the help command before you break something.");
                    break;
                case 3:
                    returnValue.Append("Please press alt-F4 to continue.");
                    break;
                case 4:
                    returnValue.AppendFormat("Yeah, I've heard of {0} but I didn't think you'd be into that kind of thing.", command);
                    break;
                case 5:
                    returnValue.Append("Abort, Retry, Fail?");
                    break;
                case 6:
                    returnValue.Append("What are you doing, Dave?");
                    break;
                default:
                    returnValue.Append("That's ERMAZING!");
                    break;
            }

            return returnValue;
        }

        private StringBuilder AppendHelp()
        {
            var returnValue = new StringBuilder();

            returnValue.AppendLine("Welcome to the realm of infuz.com.");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("You can move around by using one of the provided cardinal directions (north, south, etc).");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("You can access this help text at any time by using <em>help</em> or <em>?</em> for short.");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("If there is someone else in the room you can speak to them with: say <text>");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("Anything depicted in yellow or red can be looked at. You can accomplish that using: <em>look [object]</em>.");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("If you ever lose track of your cursor, just hit the <strong>TAB</strong> button.");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("Finally, if you're ever confused as to where you are, simply use the <em>look</em> or <em>map</em> commands without anything after it.");
            returnValue.AppendLine("<br/>");
            returnValue.AppendLine("Feel free to explore the vast landscape of modern digital advertising.");


            return returnValue;
        }

        private StringBuilder AppendJunkCommand(String command)
        {
            var returnValue = new StringBuilder();
            var junkCom = JunkCommands.FirstOrDefault(jc => jc.Key.Equals(command, StringComparison.InvariantCultureIgnoreCase));

            returnValue.AppendLine(junkCom.Value);

            return returnValue;
        }

        private StringBuilder Converse(String command, IContentPage page)
        {
            var returnValue = new StringBuilder();
            var person = GetPerson(page);

            if (String.IsNullOrEmpty(person))
                returnValue.AppendLine(GetRandomCrazy());
            else
                returnValue.AppendLine(GetConversation(command, person, page));

            return returnValue;
        }

        private String GetPerson(IContentPage page)
        {
            var returnValue = String.Empty;
            var pageName = page.Url;

            if (pageName.EndsWith("/") && pageName.Length > 1)
            {
                pageName = pageName.Substring(0, pageName.Length - 1);
                pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            }

            switch (pageName)
            {
                case "Jonathan-Sackett":
                    returnValue = "Jonathan Sackett";
                    break;
                case "Kelli-Schwahn":
                    returnValue = "Kelli Schwahn";
                    break;
                case "Heath-Harris":
                    returnValue = "Heath Harris";
                    break;
                case "Erin-Fiehler":
                    returnValue = "Erin Fiehler";
                    break;
                case "Neil-Monroe":
                    returnValue = "Neil Monroe";
                    break;
                case "Marc-Brooks":
                    returnValue = "Hack Prime";
                    break;
                case "Michael Bischoff":
                    returnValue = "Michael Bischoff";
                    break;
                case "Erica-Smith":
                    returnValue = "Erica Smith";
                    break;
                case "Jason-Fiehler":
                    returnValue = "Jason Fiehler";
                    break;
                case "Jill-Schanzle":
                    returnValue = "Jill Schanzle";
                    break;
                case "Chris-Sturgeon":
                    returnValue = "Chris Sturgeon";
                    break;
                case "Jamal-McLaughlin":
                    returnValue = "Jamal McLaughlin";
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        String GetConversation(String command, String person, IContentPage page)
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = "Have you seen my pet Grue around here?";
                    break;
                case 2:
                    returnValue = "I wonder what the Chat Gem does.";
                    break;
                case 3:
                    returnValue = "Blimpin' aint easy.";
                    break;
                case 4:
                    returnValue = "How bout dem Cards, eh?";
                    break;
                case 5:
                    returnValue = "I like turtles.";
                    break;
                case 6:
                    returnValue = "Somebody set us up the bomb.";
                    break;
                case 7:
                    returnValue = "Check out this <a href=\"http://www.youtube.com/watch?v=oHg5SJYRHA0\" target=\"_blank\">great video</a> I found!";
                    break;
                case 8:
                    break;
                default:
                    //use the social feed
                    if (page.SocialMediaFeedItems.Count() > 0)
                    {
                        var socialItem = page.SocialMediaFeedItems.ElementAt(rand.Next(0, page.SocialMediaFeedItems.Count() - 1));

                        switch (socialItem.TypeClass)
                        {
                            case SocialMediaTypeClass.facebook:
                            case SocialMediaTypeClass.twitter:
                                returnValue = socialItem.Body;
                                break;
                            case SocialMediaTypeClass.lastfmtrack:
                                returnValue = String.Format("Hey have you heard, {0}?", socialItem.Body);
                                break;
                            default:
                                returnValue = "...";
                                break;
                        }
                    }
                    else
                        returnValue = "...";
                    break;
            }

            return String.Format("{0} says, '{1}'. ", person, returnValue);
        }

        private String GetRandomCrazy()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                default:
                    returnValue = "You talk to yourself, but you're not sure if anyone is listening. ";
                    break;
            }

            return returnValue;
        }

        private String GetRandomStance()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = "mill about";
                    break;
                case 2:
                    returnValue = "pace around";
                    break;
                default:
                    returnValue = "stand";
                    break;
            }

            return returnValue;
        }

        private Tuple<String, String> GetRandomLinkDecoration()
        {
            var returnValue = new Tuple<String, String>(String.Empty, String.Empty);
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = new Tuple<String, String>("A leaflet describing ", " lies on a coffee table. ");
                    break;
                case 2:
                    returnValue = new Tuple<String, String>("A bird flies in and sings a song accurately describing, '", "'. ");
                    break;
                case 3:
                    returnValue = new Tuple<String, String>("A piece of Modern Art vaugely references ", " on the wall. ");
                    break;
                case 4:
                    returnValue = new Tuple<String, String>("", " pops into your head as a mysterious force passes through the room. ");
                    break;
                case 5:
                    returnValue = new Tuple<String, String>("Wall speakers belt out an accapella song about ", ". ");
                    break;
                case 6:
                    returnValue = new Tuple<String, String>("The floor erupts in a brief pyrotechnics display which takes the shape of ", ". ");
                    break;
                case 7:
                    returnValue = new Tuple<String, String>("Three mice scurry in, do a short interpretive dance of ", " and leave. ");
                    break;
                case 8:
                    returnValue = new Tuple<String, String>("A loudspeaker announces, '", "'. ");
                    break;
                case 9:
                    returnValue = new Tuple<String, String>("A little blue bird sings, '", "'. ");
                    break;
                default:
                    returnValue = new Tuple<String, String>("A painting of ", " hangs on the wall. ");
                    break;
            }

            return returnValue;
        }

        private String GetNPCStance()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = String.Format("mills about the room {0}", GetNPCVerb());
                    break;
                case 2:
                    returnValue = "stands in the corner, staring at something";
                    break;
                default:
                    returnValue = "sits on a small stool, smiling";
                    break;
            }

            return returnValue;
        }

        private String GetNPCVerb()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = "muttering";
                    break;
                case 2:
                    returnValue = "looking for something";
                    break;
                default:
                    returnValue = "quietly";
                    break;
            }

            return returnValue;
        }

        private String GetRandomControlVerb()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = "dominates";
                    break;
                case 2:
                    returnValue = "stretches across";
                    break;
                default:
                    returnValue = "occupies the entirety of";
                    break;
            }

            return returnValue;
        }
        private String GetRandomRoomAdjective()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = "small";
                    break;
                case 2:
                    returnValue = "large";
                    break;
                case 3:
                    returnValue = "wee little";
                    break;
                case 4:
                    returnValue = "not so wee";
                    break;
                case 5:
                    returnValue = "friggin huge";
                    break;
                case 6:
                    returnValue = "decidedly orange";
                    break;
                default:
                    returnValue = "medium sized";
                    break;
            }

            return returnValue;
        }

        private String GetRandomNameplateAdjective()
        {
            var returnValue = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    returnValue = "brass";
                    break;
                case 2:
                    returnValue = "bronze";
                    break;
                case 3:
                    returnValue = "lime jello";
                    break;
                case 4:
                    returnValue = "breadboard";
                    break;
                case 5:
                    returnValue = "duct tape";
                    break;
                case 6:
                    returnValue = "steel";
                    break;
                default:
                    returnValue = "glass";
                    break;
            }

            return returnValue;
        }

        private String GetExitStatement()
        {
            var returnValue = String.Empty;
            var randomized = String.Empty;
            switch (rand.Next(1, 10))
            {
                case 1:
                    randomized = "Quitters never win.";
                    break;
                case 2:
                    randomized = "Go ahead and leave me; I think I prefer to stay inside.";
                    break;
                case 3:
                    randomized = "Well… you fight like a cow!";
                    break;
                case 4:
                    randomized = "It is difficult to go alone. Take <a href=\"#\" class=\"screenshot\" rel=\"http://f.cl.ly/items/0S0w0j0A053N2J0p153d/It%27s%20Dangerous,%20Take%20This%20Kitten.jpg\">this</a>.";
                    break;
                case 5:
                    randomized = "Fine, I didn't need you here anyways.";
                    break;
                case 6:
                    randomized = "Winners never quit!";
                    break;
                default:
                    randomized = "Are you sure you want to exit?";
                    break;
            }

            returnValue = String.Format("{0} <a href=\"/\">QUIT</a>&nbsp;<a href=\"#\">CANCEL</a>", randomized);

            return returnValue;
        }
        private string RenderHtml(String pathName, BaseViewModel model)
        {
            return this.RenderPartialToString(pathName, model);
        }

        private string CrunchGrammer(String command)
        {
            var newCommand = command;

            //whole words we can munch up
            foreach (KeyValuePair<String, String> synonym in SynonymPhrases)
                newCommand = newCommand.Replace(synonym.Value, synonym.Key);

            //pieces we can kill or change
            var newCommandSplit = newCommand.Split(' ');
            newCommand = String.Empty;
            foreach (String splitItem in newCommandSplit)
            {
                if (ConJunkTions.Any(key => key.Equals(splitItem, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                if (SynonymTargets.Any(kvp => kvp.Value.Equals(splitItem, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var splitTarget = SynonymTargets.FirstOrDefault(kvp => kvp.Value.Equals(splitItem, StringComparison.InvariantCultureIgnoreCase));
                    newCommand += splitTarget.Key;
                }
                else
                    newCommand += splitItem + ' ';
            }

            newCommand = newCommand.Trim();

            if (newCommand.Equals(" "))
                newCommand = String.Empty;

            return newCommand;
        }

        private String GenerateMapString(IContentPage page, IList<IContentPage> contentMap)
        {
            var sb = new StringBuilder();
            var spacingMax = 1;
            var longestSection = contentMap.Where(pg => pg.SectionUrl.Equals(page.SectionUrl, StringComparison.InvariantCultureIgnoreCase))
                                           .OrderByDescending(p => p.SubNav.Count()).FirstOrDefault();
            if (longestSection != null)
                spacingMax = longestSection.SubNav.Count();

            //We'll always have either an up, down or both
            var mainNav = page.PrimaryNav.ToList();
            var currentMainIndex = mainNav.IndexOf(mainNav.FirstOrDefault(m => m.Url.Equals(page.SectionUrl)));
            INavElement upNav = null;
            INavElement downNav = null;

            if (currentMainIndex > 0)
                upNav = mainNav.ElementAt(currentMainIndex - 1);
            else if (currentMainIndex == 0)
                upNav = contentMap.ElementAt(0).SubNav.ElementAt(0);

            if (currentMainIndex < mainNav.Count() - 1)
                downNav = mainNav.ElementAt(currentMainIndex + 1);

            sb.AppendFormat("<h1>{0} Area Map</h1>", page.SectionName);
            sb.AppendFormat("<h3>Legend: You are <em>here</em>. <strong>Mouseover</strong> for room previews.</h3>");
            sb.AppendLine("<br/>");
            if (upNav != null)
            {
                var spaceCount = upNav.Name.Length / 2 + spacingMax;
                var space = String.Empty;
                for (int i = 0; i < spaceCount; i++)
                    space += "&nbsp;";

                sb.AppendFormat("{0}^<br/>", space);
                sb.AppendFormat("{0}|<br/>", space);

                var iter = 0;
                while (iter < spacingMax)
                {
                    sb.Append("&nbsp;");
                    iter++;
                }

                sb.AppendFormat("<strong>{0}</strong><br/>", upNav.Name);
                sb.AppendFormat("{0}|<br/>", space);
            }

            foreach (INavElement secNav in page.SectionNav)
            {
                var sectionRoot = contentMap.FirstOrDefault(pg => pg.SubSectionUrl.Equals(secNav.Url));
                sb.Append("<p>");

                var currentSubCount = sectionRoot.SubNav.Count();
                while (currentSubCount < spacingMax)
                {
                    sb.Append("&nbsp;");
                    currentSubCount++;
                }

                foreach (INavElement subNav in sectionRoot.SubNav)
                {
                    if (subNav.Url.Equals(page.Url, StringComparison.InvariantCultureIgnoreCase))
                        sb.AppendFormat("<em><a href=\"#\" title=\"{0}\" class=\"screenshot\" rel=\"{1}\">0</a></em>-", subNav.Name, subNav.Thumbnail);
                    else
                        sb.AppendFormat("<a href=\"#\" title=\"{0}\" class=\"screenshot\" rel=\"{1}\">0</a>-", subNav.Name, subNav.Thumbnail);
                }

                if (sb.ToString().EndsWith("-"))
                    sb.Length--;

                currentSubCount = sectionRoot.SubNav.Count();
                while (currentSubCount < spacingMax)
                {
                    sb.Append("&nbsp;");
                    currentSubCount++;
                }

                sb.AppendFormat("&nbsp;&nbsp;::&nbsp;{0}", secNav.Name);

                sb.AppendLine("</p></br>");
            }

            if (downNav != null)
            {
                var spaceCount = downNav.Name.Length / 2 + spacingMax;
                var space = String.Empty;
                for (int i = 0; i < spaceCount; i++)
                    space += "&nbsp;";

                sb.AppendFormat("{0}|<br/>", space);

                var iter = 0;
                while (iter < spacingMax)
                {
                    sb.Append("&nbsp;");
                    iter++;
                }

                sb.AppendFormat("<strong>{0}</strong><br/>", downNav.Name);
                sb.AppendFormat("{0}|<br/>", space);
                sb.AppendFormat("{0}v<br/>", space);
            }

            return sb.ToString();
        }
    }
}
