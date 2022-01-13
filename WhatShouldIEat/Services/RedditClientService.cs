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

        public List<Post> RequestRecipePosts(Recipe recipe)
        {
            string subredditName = "recipes";
            SearchGetSearchInput searchInput;

            if (recipe.Ingredients != null && recipe.Ingredients.Count > 0)
            {
                string ingredientsString = "";
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

            return reddit.Subreddit(subredditName).Search(searchInput);
            }

        public Comment FindInstructionInPost(string originalPoster, List<Comment> postComments)
        {
            List<Comment> commentsFormOp = FindCommentsFromOP(originalPoster, postComments);
            Comment commentWithInstruction = null;

            if (commentsFormOp.Count > 0)
            {
                foreach (Comment comment in commentsFormOp)
                {
                    if(comment.Body.ToLower().Contains("ingredients") &&
                        comment.Body.ToLower().Contains("instructions"))
                    {
                        commentWithInstruction = comment;
                    }
                }
            }
            return commentWithInstruction;
        }

        /*
         * Sucht der Position eines bestimmten Wortes in einem String.
         */
        public int GetPosOfWord(string strSource, string strStart)
        {
            if (strSource.Contains(strStart))
            {
                return strSource.IndexOf(strStart, 0) + strStart.Length;
            }

            return -1;
        }

        public List<Comment> FindCommentsFromOP(string originalPoster, List<Comment> comments)
        {
            List<Comment> commentsFromOp = new List<Comment>();
            foreach (Comment comment in comments)
            {
                if (String.Compare(originalPoster, comment.Author) == 0)
                {
                    commentsFromOp.Add(comment);
                }
            }
            return commentsFromOp;
        }

        public string GetIngredientsInComment(string strSource)
        {
            string strRes = "";
            string[] ingredientsStringVariations = { "**ingredients**", "**ingredients:**", "ingredients:", "*ingredients*", "*ingredients:*",
                                                     "shoppinglist", " **shoppinglist**", "**shoppinglist:**", "shoppinglist:", "*shoppinglist:*" };

            string[] instructionStringVariations = { "**instructions**", "**instructions:**", "*instructions:*", "instructions:", "*instructions*",
                                                     "**directions**", "**directions:**", "*directions:*", "directions:", "*directions*",
                                                     "**method**", "**method:**", "*method:*", "method:", "*method*",
                                                     "**preperation**", "**preperation:**", "*preperation:*", "preperation:", "*preperation*"};

            foreach (string strStart in ingredientsStringVariations)
            {
                foreach(string strEnd in instructionStringVariations)
                {
                    if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                    {
                        int Pos1 = strSource.IndexOf(strStart) + strStart.Length;
                        int Pos2 = strSource.IndexOf(strEnd);
                        strRes = strSource.Substring(Pos1, Pos2 - Pos1);
                    }
                }
            }
            return strRes;
        }

        public string GetInstructionFromComment(Comment comment)
        {
            int index = GetPosOfWord(comment.Body.ToLower(), "instructions");
            string instruction = "";

            if (index >= 0)
            {
                instruction = comment.Body.Substring(index);
                while (instruction[0] != '\n' && instruction[1] != '\n')
                {
                    instruction = instruction.Substring(1);
                    // entferne überschüssigen Code.
                }
            }
            return instruction;
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
