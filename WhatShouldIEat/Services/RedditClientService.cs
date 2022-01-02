using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reddit;
using Reddit.AuthTokenRetriever;
using System.Diagnostics;

namespace WhatShouldIEat.Services
{
    class RedditClientService : IRedditClientService
    {
        private string APP_ID = "rswOv1IFQZ2hpKHOf5smog";
        private string SECRET_ID = "hqpiozsX9BdLT45Jd2l6S6t7cBiAoA";
        private static string FIREFOX_BROWSER_PATH = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        private static string CHROME_BROWSER_PATH = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        private static string ACCESS_TOKEN = "1421091109847-E8s9XAGrzJ2IQl43PBpt1D8tFL5xTQ";
        private static string REFRESH_TOKEN = "1421091109847-b2zQC4ZD2fa2ZMrw6ukEuaLrLQqRBQ";

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
            var refreshToken = AuthorizeUser(APP_ID, SECRET_ID);
            var reddit = new RedditClient(appId: APP_ID, refreshToken: refreshToken);
            //var reddit = new RedditClient(appId: "YourAppID", appSecret: "YourAppSecret");

            // Display the name and cake day of the authenticated user.
            Console.WriteLine("Username: " + reddit.Account.Me.Name);
            Console.WriteLine("Cake Day: " + reddit.Account.Me.Created.ToString("D"));
            while (true)
            { };
            var askReddit = reddit.Subreddit("AskReddit").About();
            Console.WriteLine(askReddit.Posts.Top[0]);

        }

        public void InitRedditClient()
        {

        }

        public static string AuthorizeUser(string appId, string appSecret = null, int port = 8080)
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
            return authTokenRetrieverLib.RefreshToken;
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
