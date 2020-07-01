using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Unity.RemoteConfig;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string version;
    private const int maxPlayersPerRoom = 20;
    private bool _isConnecting;
    public WaitingPanelManager waitingPanel;
    public GameObject waitingPanelObject;
    public GameObject UnsupportedVersion;

    public string OnlineVersion { get; set; }
    public string OnlineLink { get; set; }
    
    public struct userAttributes {}
    public struct appAttributes { }

    private void Awake()
    {
        ConfigManager.FetchCompleted += getConfigs;
        PhotonNetwork.AutomaticallySyncScene = true;
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    public void getConfigs(ConfigResponse response)
    {
        OnlineLink = ConfigManager.appConfig.GetString("download");
        OnlineVersion = ConfigManager.appConfig.GetString("version");
    }

    public void Search()
    {
        if (version == OnlineVersion)
        {
            waitingPanelObject.SetActive(true); 
            _isConnecting = true;

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = version;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            UnsupportedVersion.SetActive(true);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void DownlaodGame()
    {
        Application.OpenURL(OnlineLink);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        if (_isConnecting)
        {
            Debug.Log("Searching Room");
            waitingPanel.SetText("Searching for Room");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    private void Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room found, create one");
        waitingPanel.SetText("Creating new Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers = 20});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to Room!");
        waitingPanel.SetText("Connected to Room");
        StartCoroutine("LoadGame");
    }

    private IEnumerator LoadGame()
    {
        Debug.Log("Loading Scene");
        waitingPanel.SetText("Load Game");
        yield return new WaitForEndOfFrame();
        PhotonNetwork.LoadLevel("Game");
    }

}
