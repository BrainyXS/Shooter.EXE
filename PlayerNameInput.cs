using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button StartGameButton;
    [SerializeField] private NetworkManager manager;

    private const string PrefsNameKey = "PlayerName";

    private void Start()
    {
        StartGameButton.interactable = false;
        SetupInputFiled();
    }

    private void SetupInputFiled()
    {
        if (!PlayerPrefs.HasKey(PrefsNameKey))
        {
            return;
        }

        GetComponent<TMP_InputField>().text = PlayerPrefs.GetString(PrefsNameKey);
        SetName(PlayerPrefs.GetString(PrefsNameKey));
    }

    public void SetName(string name)
    {
        StartGameButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        PhotonNetwork.NickName = GetComponent<TMP_InputField>().text;
        PlayerPrefs.SetString(PrefsNameKey, GetComponent<TMP_InputField>().text);
        SetName(GetComponent<TMP_InputField>().text);
    }

    public void Connect()
    {
        manager.Search();
    }
}
