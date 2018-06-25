using System;
using System.IO;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
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
            string[] text = { DateTime.Now.ToString(), e.ReSubscriber.DisplayName, e.ReSubscriber.Login, SubscriptionEnumConvert(e.ReSubscriber.SubscriptionPlan), e.ReSubscriber.Months.ToString() };
            writer.Write(CsvSerializer.SerializeToCsv<string>(text));
            writer.Flush();
        }
        
        private void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            string[] text = { DateTime.Now.ToString(), e.Subscriber.DisplayName, e.Subscriber.Login, SubscriptionEnumConvert(e.Subscriber.SubscriptionPlan) };
            writer.Write(CsvSerializer.SerializeToCsv<string>(text));
            writer.Flush();
        }

        private string SubscriptionEnumConvert(SubscriptionPlan plan)
        {
            switch (plan)
            {
                case SubscriptionPlan.Prime:
                    return "Prime";
                case SubscriptionPlan.Tier1:
                    return "4.99";
                case SubscriptionPlan.Tier2:
                    return "9.99";
                case SubscriptionPlan.Tier3:
                    return "24.99";
            }
            return "error";
        }
    }
}