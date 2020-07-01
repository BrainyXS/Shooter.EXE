using System;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

public class ShootedBullet : MonoBehaviourPun
{
    [HideInInspector] public int Damage = 8; 
    [HideInInspector] public int ShooterID;
    [SerializeField] private Rigidbody rb;

    private async void OnTriggerEnter(Collider other)
    {
        
        if ((other.GetComponent<PhotonView>() != null && other.GetComponent<PhotonView>().ViewID == ShooterID )|| ShooterID == 0)
        {
            return;
        }
        
        var player = other.gameObject.GetComponent<PlayerHealthManager>();
        if (player != null && !other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            player.TakeDamage(8, ShooterID);
        }

        await Task.Delay(100);
        PhotonNetwork.Destroy(gameObject);


    
    }

    public void Setup(Transform t, int id)
    {
        Damage = 8;
        transform.rotation = t.rotation;
        transform.position = t.position;
        ShooterID = id;

        rb.AddForce(transform.up * 300, ForceMode.Impulse);
    }
}