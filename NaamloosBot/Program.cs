/*
	The code is messy, i know.
	I would love it if you gave me credit for my help :)
	http://www.twitter.com/naamloos_nl
	http://www.twitch.tv/hierisryan
	Comment should give more info
	
	Some stuff might be broken, but easy to fix.
*/

// Not sure if restsharp was actually used...
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using RestSharp;

namespace NaamloosBot
{
    class Program
    {
		//	ofc, setting vars.
        public static string newstring = "";
        string oldstring = "";
        public static System.IO.TextReader input;
        public static System.IO.TextWriter output;
        public static string buf, nick, owner, server, chan, oauth;
        static void Main(string[] args)
        {
            int port;
            System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
            int uptime;
            int msgcount = 0;
            int msglimit = 0;

            //	User input!
            Console.Write("Bot nickname = [BOT NAME]\r\n");
            nick = "[BOT NAME]";
            Console.Write("Server IP = IRC.Twitch.TV\r\n");
            server = "irc.twitch.tv";
            Console.Write("Server Port = 6667\r\n");
            port = 6667;
            Console.Write("Enter your twitch name: ");
            chan = "#" + Console.ReadLine();
            Console.Write("Enter oauth..");
			//	Create a twitch app or get oauth here:
			//	https://twitchapps.com/tmi/
			
            System.Diagnostics.Process.Start("[link to get oauth..]");
            oauth = Console.ReadLine();

            //	Server uses TCP, IRC is fairly ez
            sock.Connect(server, port);
            if (!sock.Connected)
            {
                Console.WriteLine("Failed to connect!");
                return;
            }
            input = new System.IO.StreamReader(sock.GetStream());
            output = new System.IO.StreamWriter(sock.GetStream());

            output.Write(
                "PASS " + "oauth:" + oauth + "\r\n" +
                "NICK " + nick + "\r\n"
            );
            output.Flush();
            Thread thread1 = new Thread(new ThreadStart(A));
            thread1.Start();
            output.Write("PRIVMSG " + chan + " :Hoi!\r\n"); output.Flush();
			
			/*
				This is hw we send msgs:
				"PRIVMSG [channelname (username streamer)] :Hey, this is a message!\r\n
			*/
			
            //	Messages should be received too!
            for (buf = input.ReadLine(); ; buf = input.ReadLine())
            {

                //	Every message will be logged in console.
                Console.WriteLine(buf);
				
				//	Send message every (user input) messages!
                if (msgcount == msglimit)
                {
                    output.Write("PRIVMSG " + chan + " :/me Thanks to http://www.twitter.com/Naamloos_nl or http://twitch.tv/hierisryan/ for the bot!\r\n"); output.Flush();
                    msgcount = 0;
                }
                msgcount = msgcount + 1;
                //	A simple PING command :)
				//	We are using ToUpper for capital insensitivity
                if (buf.ToUpper().Contains("::ping".ToUpper())) {
					output.Write("PRIVMSG " + chan + " :Pong!\r\n");
					output.Flush();
					}
					
				//	Not sure what this did.
                if (buf[0] != ':') continue;

                //	001 command? tell twitch we are a bot and join a channel!
                if (buf.Split(' ')[1] == "001")
                {
                    output.Write(
                        "MODE " + nick + " +B\r\n" +
                        "JOIN " + chan + "\r\n"
                    );
                    output.Flush();
                }
            }
        }
		
		//	Typing in console sends a message!
        static void setstr(string stringeske)
        {
            output.Write("PRIVMSG " + chan + " :"+stringeske+"\r\n"); output.Flush();
        }
        static void A()
        {
            while (true)
            {
                string lestring = Console.ReadLine();
                setstr(lestring);
            }
        }
    }
        }
