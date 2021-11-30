using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITwitchCommandHandler {
    void HandleCommand(TwitchCommandData data);
}

public struct TwitchCommandData {
    public string author;
    public string message;
}

//Used to pass information from UI to manager allowing for a connection
public struct TwitchCredentials {
    public string channelName;
    public string username;
    public string password;
}

public static class TwitchCommands {
    //List all commands in this class and change prefix
    public static readonly string CmdPrefix = "!";
    public static readonly string CmdMessage = "message";
    public static readonly string CmdSpawn = "spawn";
}

//message command
public class TwitchDisplayMessageCommand : ITwitchCommandHandler {
    public void HandleCommand(TwitchCommandData data) {
        Debug.Log($"Raw Message:{data.message}");
        string actualMessage = data.message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdMessage).Length).TrimStart(' ');
        Debug.Log($"{data.author} says {actualMessage}");
    }
}

public class TwitchSpawnCommand : ITwitchCommandHandler {
    public void HandleCommand(TwitchCommandData data) {
        Debug.Log($"{data.author} Spawned an entity");
        Spawner.Instance.SpawnObject();
    }
}

public class CommandCollection {
    private Dictionary<string, ITwitchCommandHandler> _commands;

    public CommandCollection(){
        _commands = new Dictionary<string, ITwitchCommandHandler>();
        _commands.Add(TwitchCommands.CmdMessage, new TwitchDisplayMessageCommand());
        _commands.Add(TwitchCommands.CmdSpawn, new TwitchSpawnCommand());
    }

    public bool HasCommand(string command) {
        return _commands.ContainsKey(command) ? true:false;
    }

    public void ExecuteCommand(string command, TwitchCommandData data) {
        command = command.Substring(1);
        if(HasCommand(command)){
            _commands[command].HandleCommand(data);
        }
    }
}