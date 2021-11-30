using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using UnityEngine;


public class TwitchChat : MonoBehaviour
{
    public static TwitchChat Instance {
        get {
            if(_instance == null) {
                _instance = new TwitchChat();
            }
            return _instance;
        }
    }

    private static TwitchChat _instance;

    private CommandCollection _commands;
    private TcpClient _twitchClient;
    private StreamReader _reader;
    private StreamWriter _writer;
    

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }


    void Update()
    {
        if(_twitchClient != null && _twitchClient.Connected) {
            ReadChat();
        }
    }

    public void SetNewCommandCollection(CommandCollection commands) {
        _commands = commands;
    }

    public void Connect(TwitchCredentials credentials, CommandCollection commands) {
        _commands = commands;
        //Cannot connect to twitch without. 6667 is Twitch's port for non SSL IRC Clients
        _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);

        _reader = new StreamReader(_twitchClient.GetStream());
        _writer = new StreamWriter(_twitchClient.GetStream());

        //Based upon Twitch API information https://dev.twitch.tv/docs/irc/guide
        _writer.WriteLine("PASS " + credentials.password);
        _writer.WriteLine("NICK " + credentials.username);
        _writer.WriteLine("USER " + credentials.username + " 8 * :" + credentials.username);
        _writer.WriteLine("JOIN #" + credentials.channelName);
        _writer.Flush();
    }

    private void ReadChat() {
        if(_twitchClient.Available > 0) {
            string message = _reader.ReadLine();
            Debug.Log(message);

            //Responding to Twitch's automatic Ping response. Must reply with Pong or else we get disconnected
            if (message.Contains("PING")) {
                _writer.WriteLine("PONG");
                _writer.Flush();
                return;
            }

            if (message.Contains("PRIVMSG")){
                var splitPoint = message.IndexOf("!", 1);
                var author = message.Substring(0, splitPoint);
                author = author.Substring(1);

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);

                if(message.StartsWith(TwitchCommands.CmdPrefix)){
                    int index = message.IndexOf(" ");
                    string command = index > -1 ? message.Substring(0, index) : message;
                    _commands.ExecuteCommand(command, new TwitchCommandData{author = author, message = message});
                }
            }
        }
    }
}
