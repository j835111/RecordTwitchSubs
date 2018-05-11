using System;
using System.IO;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace WebApplication.Models
{
    public class TwitchIRC
    {
        FileStream fs = new FileStream("SubsLog.txt", FileMode.OpenOrCreate);
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
            string text = "[" + DateTime.Now + "]" + " " + e.ReSubscriber.DisplayName + " " + e.ReSubscriber.SubscriptionPlanName + " " + e.ReSubscriber.Months + "\n";
            byte[] by = Encoding.UTF8.GetBytes(text);
            fs.Write(by, 0, by.Length);
            fs.Close();
        }
        
        private void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            string text = "[" + DateTime.Now.ToLongDateString() + "]" + " " + e.Subscriber.DisplayName + " " + e.Subscriber.SubscriptionPlanName + "\n";
            byte[] by = Encoding.UTF8.GetBytes(text);
            fs.Write(by, 0, by.Length);
            fs.Close();
        }
    }
}