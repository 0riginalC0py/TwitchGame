using UnityEngine;
using UnityEngine.UI;

public class TwitchChatSettingsUI : MonoBehaviour
{
    public InputField passwordInput;
    public InputField usernameInput;
    public InputField channelNameInput;
    public TwitchChat TwitchChat;


    public void Connect()
    {
        TwitchCredentials credentials = new TwitchCredentials{ channelName = channelNameInput.text.ToLower(), username = usernameInput.text.ToLower(), password = passwordInput.text
        };
        TwitchChat.Connect(credentials, new CommandCollection());
    }
}
