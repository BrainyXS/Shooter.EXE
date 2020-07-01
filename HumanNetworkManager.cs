using Photon.Pun;
using UnityEngine;

public class HumanNetworkManager : MonoBehaviour
{
    [HideInInspector] public PhotonView myView;
    void Start()
    {
        myView = GetComponentInChildren<PhotonView>();
        if (!myView.IsMine)
        {
            GetComponent<FirstPersonController>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
