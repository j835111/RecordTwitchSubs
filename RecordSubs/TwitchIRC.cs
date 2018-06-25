using System;
using System.IO;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using ServiceStack.Text;

namespace RecordSubs
{
    public class TwitchIRC
    {
        StreamWriter writer = new StreamWriter("SubsLog.csv", true, new UTF8Encoding(true));     
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
            string[] text = { DateTime.Now.ToString(), e.ReSubscriber.DisplayName, e.ReSubscriber.Login, e.ReSubscriber.SubscriptionPlanName, e.ReSubscriber.Months.ToString() };
            writer.Write(CsvSerializer.SerializeToCsv<string>(text));
            writer.Flush();
            writer.Close();
        }
        
        private void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            string[] text = { DateTime.Now.ToString(), e.Subscriber.DisplayName, e.Subscriber.Login, e.Subscriber.SubscriptionPlanName.ToString() };
            writer.Write(CsvSerializer.SerializeToCsv<string>(text));
            writer.Flush();
            writer.Close();
        }
    }
}