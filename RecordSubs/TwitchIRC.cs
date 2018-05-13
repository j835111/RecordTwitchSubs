using System;
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace WebApplication.Models
{
    public class TwitchIRC
    {
        StreamWriter writer = new StreamWriter("SubsLog.txt", true);
        TwitchClient client;
        public void Start(string username, string oauth, string channelname)
        {
            ConnectionCredentials credentials = new ConnectionCredentials(username, oauth);

            client = new TwitchClient();
            client.Initialize(credentials, channelname);

            client.OnReSubscriber += onReSubscriber;
            client.OnNewSubscriber += onNewSubscriber;

            client.Connect();
        }

        private void onReSubscriber(object sender, OnReSubscriberArgs e)
        {
            string text = "[" + DateTime.Now + "]" + " " + e.ReSubscriber.DisplayName + "(" + e.ReSubscriber.UserId + ")" + " " + e.ReSubscriber.SubscriptionPlanName + " " + e.ReSubscriber.Months + "\n";
            writer.WriteLine(text);
            writer.Flush();
            writer.Close();
        }
        
        private void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            string text = "[" + DateTime.Now + "]" + " " + e.Subscriber.DisplayName + "(" + e.Subscriber.UserId + ")" + " " + e.Subscriber.SubscriptionPlanName + "\n";
            writer.WriteLine(text);
            writer.Flush();
            writer.Close();
        }
    }
}