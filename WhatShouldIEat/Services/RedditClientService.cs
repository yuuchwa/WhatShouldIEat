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
            var input = new SearchGetSearchInput("Tomato");
            List<Post> posts = reddit.Subreddit("AskReddit").Search(input);  // Search r/MySub
            if (posts.Count == 0)
            {
                posts = reddit.Subreddit("all").Search(new SearchGetSearchInput("Bernie Sanders"));  // Search r/all
            }

            Console.WriteLine("Post: " + posts[0]);
        }

        public void RequestRecepiesByCountry(List<string> ingredieces, string nationality)
        {
            
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
