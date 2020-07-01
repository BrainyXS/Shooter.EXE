using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class KillManager : MonoBehaviour
{
    public int Deaths;
    public int Kills;
    public TMP_Text Name;

    private void Start()
    {
        Deaths = 0;
        Kills = 0;
        Name.text = GetComponentInParent<PhotonView>().Owner.NickName;
    }
}
