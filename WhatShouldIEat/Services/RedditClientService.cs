using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reddit;
using Reddit.AuthTokenRetriever;
using System.Diagnostics;
using Reddit.Controllers;
using Reddit.Inputs.Search;
using System.IO;
using WhatShouldIEat.Model;


namespace WhatShouldIEat.Services
{
    class RedditClientService : IRedditClientService
    {
        private string APP_ID = "rswOv1IFQZ2hpKHOf5smog";
        private string SECRET_ID = "hqpiozsX9BdLT45Jd2l6S6t7cBiAoA";
        private static string FIREFOX_BROWSER_PATH = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        private static string CHROME_BROWSER_PATH = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        private RedditClient reddit;

        public RedditClientService()
        {
            RetrievAccessToken();
        }

        public void RequestRecepies(Recipe recipe)
        {
            string ingredientsString = "";
            string subredditName = "recipes";
            SearchGetSearchInput searchInput;

            if (recipe.Ingredients != null)
            {
                foreach (string ingredient in recipe.Ingredients)
                {
                    ingredientsString += ingredient + " ";
                }
                searchInput = new SearchGetSearchInput(ingredientsString);
            }
            else
            {
                searchInput = new SearchGetSearchInput(recipe.Dish);
            }

            List<Post> posts = reddit.Subreddit(subredditName).Search(searchInput);  // Search r/MySub

            // Backup search, if no recipe could be found in the selected subreddit
            if (posts.Count == 0)
            {
                posts = reddit.Subreddit("all").Search(searchInput);  // Search r/all
            }

            string title = posts[0].Title;
            string instruction = FindInstructionForRecipe(posts[0]);
            //string instruction = getPosOfWord(temp, "instructions");
        }

        private string FindInstructionForRecipe(Post post)
        {
            string originalPoster = post.Author;
            List<Comment> postComments = post.Comments.GetComments();
            List<Comment> commentsFormOp = FindCommentsFromOP(originalPoster, postComments);
            string instruction = "";

            if(commentsFormOp.Count > 0)
            {
                foreach(Comment comment in commentsFormOp)
                {
                    int index = getPosOfWord(comment.Body.ToLower(), "ingredients");

                    if (index >= 0)
                    {
                        instruction = comment.Body.Substring(index);
                        while (instruction[0] != '1')
                        {
                            instruction = instruction.Substring(1);
                            // entferne überschüssigen Code.
                        }
                    }
                    Console.WriteLine(instruction);
                }
            }

            return instruction;
        }

        /*
         * Sucht der Position eines bestimmten Wortes in einem String.
         */
        private int getPosOfWord(string strSource, string strStart)
        {
            if (strSource.Contains(strStart))
            {
                return strSource.IndexOf(strStart, 0) + strStart.Length;
            }

            return -1;
        }

        private List<Comment> FindCommentsFromOP(string originalPoster, List<Comment> comments)
        {
            List<Comment> commentsFromOp = new List<Comment>();
            foreach(Comment comment in comments)
            {
                if(String.Compare(originalPoster, comment.Author) == 0)
                {
                    commentsFromOp.Add(comment);
                }
            }
            return commentsFromOp;
        }

        private void RetrievAccessToken()
        {
            var authTokenRetrieverLib = AuthorizeUser(APP_ID, SECRET_ID);
            reddit = new RedditClient(APP_ID, authTokenRetrieverLib.RefreshToken, SECRET_ID, authTokenRetrieverLib.AccessToken);
        }

        public AuthTokenRetrieverLib AuthorizeUser(string appId, string appSecret = null, int port = 8080)
        {
            // Create a new instance of the auth token retrieval library.  --Kris
            AuthTokenRetrieverLib authTokenRetrieverLib = new AuthTokenRetrieverLib(appId, appSecret, port);

            // Start the callback listener.  --Kris
            // Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
            authTokenRetrieverLib.AwaitCallback(true);

            // Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
            OpenBrowser(authTokenRetrieverLib.AuthURL());

            Console.WriteLine("Please accept the authentication to continue.");
            Console.WriteLine();
            while (authTokenRetrieverLib.AccessToken == null) { };

            // Cleanup.  --Kris
            authTokenRetrieverLib.StopListening();
            return authTokenRetrieverLib;
        }

        private void OpenBrowser(string authUrl = "about:blank")
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(FIREFOX_BROWSER_PATH)
                {
                    Arguments = authUrl
                };
                Process.Start(processStartInfo);
            }
        }
    }
}
