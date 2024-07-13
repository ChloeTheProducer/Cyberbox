using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using Unity.Services.Lobbies;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Chat : MonoBehaviour
{
    public TMP_InputField MessageInputField;
    public TMP_Text ChatText;

    // Start is called before the first frame update
    void Start()
    {
        // SetupClient();
    }

    async void SetupClient()
    {
        await VivoxService.Instance.InitializeAsync();
        LoginOptions options = new LoginOptions();
        options.DisplayName = AuthenticationService.Instance.PlayerName;
        Debug.Log("Logging into Vivox as " + options.DisplayName);
        await VivoxService.Instance.LoginAsync(options);

        await VivoxService.Instance.JoinGroupChannelAsync(GetLobbyName(), ChatCapability.TextOnly);

        VivoxService.Instance.ChannelMessageReceived += OnChannelMessageReceived;
        
    }

    private string GetLobbyName()
    {
        List<string> lobbies = Lobbies.Instance.GetJoinedLobbiesAsync().Result;
        Debug.Log("Connecting to chat, lobby name " + lobbies[0]);
        return lobbies[0];
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTextbox()
    {
        if (!MessageInputField.gameObject.activeInHierarchy) // Show
        {
            MessageInputField.text = string.Empty;
            MessageInputField.gameObject.SetActive(true);
            MessageInputField.Select();
        }
        else // Hide
        {
            MessageInputField.gameObject.SetActive(false);
        }
    }

    public async void SendMessageAsync()
    {
        if (string.IsNullOrEmpty(MessageInputField.text))
        {
            return;
        }
        if (!MessageInputField.gameObject.activeInHierarchy)
        {
            return;
        }


        await VivoxService.Instance.SendChannelTextMessageAsync(GetLobbyName(), MessageInputField.text);
        MessageInputField.text = string.Empty;
        MessageInputField.gameObject.SetActive(false);
    }

    void OnChannelMessageReceived(VivoxMessage message)
    {
        var channelName = message.ChannelName;
        var senderName = message.SenderDisplayName;
        var senderId = message.SenderPlayerId;
        var messageText = message.MessageText;
        var timeReceived = message.ReceivedTime;
        var language = message.Language;
        var fromSelf = message.FromSelf;
        var messageId = message.MessageId;


    }


}
