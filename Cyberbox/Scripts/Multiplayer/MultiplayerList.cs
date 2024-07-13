using UnityEngine;
using Unity.Services.Lobbies;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Services.Relay;
using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MultiplayerList : MonoBehaviour
{
    public TitleManager titleManager;
    public Button buttonTemplate;
    public GameObject playerPrefab;

    public TMP_InputField serverNameField;
    public TMP_InputField playerCountField;
    public TMP_InputField mapField;
    void Start()
    {
        DontDestroyOnLoad(NetworkManager.Singleton.gameObject); // Don't destroy the Network Manager when switching scenes
    }

    /// <summary>
    /// Loads the Server List
    /// </summary>
    public async void LoadList()
    {
        buttonTemplate.gameObject.SetActive(false);

        try
        {
            await UnityServices.InitializeAsync();// Init Unity Services

            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25; // Amount of Lobbies to Load

            // Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                    asc: false, // decending
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            foreach (Lobby lobby in lobbies.Results)
            {
                Button newLobby = Instantiate(buttonTemplate, buttonTemplate.transform.parent); // Clone template Lobby Button
                newLobby.gameObject.SetActive(true); // Show
                newLobby.transform.Find("MapText").GetComponent<TMP_Text>().text = lobby.Name; // Set Lobby Name
                newLobby.transform.Find("PlayerText").GetComponent<TMP_Text>().text = lobby.Players.Count + "/" + lobby.MaxPlayers; // Set Lobby Count
                newLobby.onClick.AddListener(delegate { JoinServerAsync(lobby.Id); }); // Set onClick Behavior
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            titleManager.OpenMessageBox("Error", e.Message, 0);
        }
    }

    /// <summary>
    /// Joins the Server with the Join Code specified
    /// </summary>
    public async void JoinServerAsync(string lobbyId)
    {
        Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
        await StartClientWithRelay(lobby.Data["relayJoinCode"].Value); // Starts Client with Relay Join Code from Lobby
        await SceneManager.LoadSceneAsync(lobby.Data["Map"].Value); // Load Scene
        PrepareScene(lobby.Data["Map"].Value);
        await SceneManager.UnloadSceneAsync("Title Screen");
    }

    /// <summary>
    /// Starts Relay Host Creation and Unity Lobbies Creation
    /// </summary>
    public async void CreateServerAsync()
    {
        await SceneManager.LoadSceneAsync(mapField.text); // Load Scene
        PrepareScene(mapField.text);

        string joinCode = await StartHostWithRelay(int.Parse(playerCountField.text)); // Host server with specified playercount

        CreateLobbyOptions options = new CreateLobbyOptions(); // Creates Lobby option variable
        options.Data = new Dictionary<string, DataObject>()
        {
            { // Sets the Join Code property
                "relayJoinCode", new DataObject(
                    visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                    value: joinCode.ToString())
            },
            { // Sets the Map property
                "Map", new DataObject(
                    visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                    value: mapField.text)
            },
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(serverNameField.text, int.Parse(playerCountField.text), options);
        await SceneManager.UnloadSceneAsync("Title Screen");
    }

    /// <summary>
    /// Hosts the server with Unity Relay
    /// </summary>
    /// <param name="maxConnections"></param>
    /// <returns></returns>
    public async Task<string> StartHostWithRelay(int maxConnections)
    {
        await UnityServices.InitializeAsync();// Init Unity Services

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Sign in as guest
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections); // Allocate Relay Service
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls")); // Set Relay Data
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); // Get Join Code
        return NetworkManager.Singleton.StartHost() ? joinCode : null; // Start server
    }

    /// <summary>
    /// Joins the server as a client using Unity Relay
    /// </summary>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();// Init Unity Services

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Sign in as guest
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode); // Join Allocation
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls")); // Set Relay Data

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient(); // Join server
    }

    /// <summary>
    /// Prepares scene, makes it active and removes player
    /// </summary>
    /// <param name="sceneName"></param>
    public void PrepareScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        Destroy(GameObject.Find("Player"));
    }
}
