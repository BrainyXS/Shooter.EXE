using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    void Start()
    {
        PhotonNetwork.Instantiate(Player.name, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
    }
}
