using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace TwitterBot
{
    class Program
    {

        private static string customer_key = "QFipu8Kfv6qqMGR6KzKBGZUDs";
        private static string customer_key_secret = "at3D2BEyGyRikhF68qjKb5siA1sExVtLA6aGfNemDW6U0fjGZx";
        private static string access_token = "934241393802928128-lJqgULEfuiKjWNdXv1q2MaUh3Arr3Rq";
        private static string access_token_secret = "vznRoTljXM0lAGTfqbOTvZakC0oJc5sDMVbpPwHN7BqEF";
        private static TwitterService service = new TwitterService(customer_key, customer_key_secret, access_token, access_token_secret);

        private static int totalImageCount = 39;
        private static int currentImgId = 1;
        private static bool tweetSent;
        



        static void Main(string[] args)
        {

            List<string> imageList = new List<string>();
            createList(imageList);

            Console.WriteLine($"{DateTime.Now}> - Bot Started with " + imageList.Count.ToString() + " images loaded.");
            while (true)
            {
                checkIfTweet(imageList);
            }

        }
        
        private static void createList(List<String> imageList)
        {
            string imageName = "image ";
            string filePath;

            for (int i = 1; i <= totalImageCount; i++)
            {
                imageName = "image (" + i.ToString() + ").jpg";
                filePath = "C:/Users/Hal/Desktop/bot_images/" + imageName;
                imageList.Add(filePath);
                Console.WriteLine("Image " + i + "added to the list");
            }
        }

        private static void checkIfTweet(List<String> imageList)
        {
            if (
                DateTime.Now.TimeOfDay.Hours == 24 && 
                DateTime.Now.TimeOfDay.Minutes == 0 &&
                DateTime.Now.TimeOfDay.Seconds == 0 ||
                DateTime.Now.TimeOfDay.Hours == 0 &&
                DateTime.Now.TimeOfDay.Minutes == 0 &&
                DateTime.Now.TimeOfDay.Seconds == 0)
            {
                SendMediaTweet(($"{DateTime.Now} :: ImageNo. " + currentImgId.ToString()), currentImgId, imageList);
            }

        }

        private static void SendTweet(string _status)
        {

            service.SendTweet(new SendTweetOptions { Status = _status }, (tweet, responce) =>
             {
                 if (responce.StatusCode == HttpStatusCode.OK)
                 {
                     Console.ForegroundColor = ConsoleColor.Green;
                     Console.WriteLine($"<{DateTime.Now}> - Tweet Sent.");
                     Console.ResetColor();
                 }
                 else
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.WriteLine($"<ERROR> " + responce.Error.Message);
                     Console.ResetColor();
                 }
             });

        }

        private static void SendMediaTweet(string _status, int imageID, List<String> imageList)
        {
            using (var stream = new FileStream(imageList[imageID], FileMode.Open))
            {
                service.SendTweetWithMedia(new SendTweetWithMediaOptions
                {
                    Status = _status,
                    Images = new Dictionary<string, Stream> { { imageList[imageID], stream } }
                });

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"<{DateTime.Now}> - Tweet Sent.");
                Console.ResetColor();

                if ((currentImgId + 1) == imageList.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("<BOT> - End of Image Array");
                    Console.ResetColor();
                }
                else
                    currentImgId++;
            }
            
        }

    }
}
