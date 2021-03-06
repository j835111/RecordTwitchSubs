﻿using ServiceStack.Text;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace RecordSubs
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool ButtonSate { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink source = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(source.NavigateUri.AbsoluteUri));
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ButtonSate)
            {
                if (!File.Exists("SubsLog.csv"))
                {
                    string[] item = { "時間", "名稱", "ID", "訂閱方案", "月數" };
                    StreamWriter writer = new StreamWriter("SubsLog.csv", true, new UTF8Encoding(true));
                    writer.Write(CsvSerializer.SerializeToCsv<string>(item));
                    writer.Flush();
                    writer.Close();
                }                    
                TwitchIRC irc = new TwitchIRC();
                irc.Start(username.Text, twitchoauth.Text, channelname.Text);
                state.Text = "Twitch聊天室連接中...\n";
                state.Text += "訂閱紀錄中...";
                ButtonSate = true;
            }
            
        }
    }
}
