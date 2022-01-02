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
        private string appID = "rHZR-6M5K77TtoVPExKRPg";
        private string secret_key = "-NN2aCOYBsxsgOOf-UA5CbOWh_8Zpw";

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
            var token = AuthorizeUser(appID);
            var reddit = new RedditClient(appID, token);

            // Display the name and cake day of the authenticated user.
            Console.WriteLine("Username: " + reddit.Account.Me.Name);
            Console.WriteLine("Cake Day: " + reddit.Account.Me.Created.ToString("D"));

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

            // Replace this with whatever you want the app to do while it waits for the user to load the auth page and click Accept.  --Kris
            while (true) { }

            // Cleanup.  --Kris
            authTokenRetrieverLib.StopListening();

            return authTokenRetrieverLib.RefreshToken;
        }

        public static void OpenBrowser(string authUrl, string browserPath = @"C:\Program Files\Mozilla Firefox\firefox")
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
                ProcessStartInfo processStartInfo = new ProcessStartInfo(browserPath)
                {
                    Arguments = authUrl
                };
                Process.Start(processStartInfo);
            }
        }
    }
}
