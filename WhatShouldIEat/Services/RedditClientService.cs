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

namespace WhatShouldIEat.Services
{
    class RedditClientService : IRedditClientService
    {
        private string APP_ID = "rswOv1IFQZ2hpKHOf5smog";
        private string SECRET_ID = "hqpiozsX9BdLT45Jd2l6S6t7cBiAoA";
        private static string FIREFOX_BROWSER_PATH = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        private static string CHROME_BROWSER_PATH = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        // 

        public RedditClientService()
        {
            RetrievAccessToken();
            InitRedditClient();
        }

        public void RequestRecepies()
        {
            
        }

        public void RequestRecepiesByCountry()
        {
            
        }

        private void RetrievAccessToken()
        {
            var authTokenRetrieverLib = AuthorizeUser(APP_ID, SECRET_ID);
            Console.ReadLine();
            var reddit = new RedditClient(APP_ID, authTokenRetrieverLib.RefreshToken, SECRET_ID, authTokenRetrieverLib.AccessToken);

            // Display the name and cake day of the authenticated user.
            var input = new SearchGetSearchInput("Foot");
            List<Post> posts = reddit.Subreddit("AskReddit").Search(input);  // Search r/MySub
            if (posts.Count == 0)
            {
                posts = reddit.Subreddit("all").Search(new SearchGetSearchInput("Bernie Sanders"));  // Search r/all
            }

            Console.WriteLine("Post: " + posts[0]);
        }

        public void InitRedditClient()
        {

        }

        public AuthTokenRetrieverLib AuthorizeUser(string appId, string appSecret = null, int port = 8080)
        {
            // Create a new instance of the auth token retrieval library.  --Kris
            AuthTokenRetrieverLib authTokenRetrieverLib = new AuthTokenRetrieverLib(appId, appSecret, port);

            // Start the callback listener.  --Kris
            // Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
            authTokenRetrieverLib.AwaitCallback();

            // Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
            OpenBrowser(authTokenRetrieverLib.AuthURL());

            // Cleanup.  --Kris
            authTokenRetrieverLib.StopListening();
            return authTokenRetrieverLib;
        }

        public static void OpenBrowser(string authUrl = "about:blank")
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
                ProcessStartInfo processStartInfo = new ProcessStartInfo(FIREFOX_BROWSER_PATH)
                {
                    Arguments = authUrl
                };
                Process.Start(processStartInfo);
            }
        }
    }
}
