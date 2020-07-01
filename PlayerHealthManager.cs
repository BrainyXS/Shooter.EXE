using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviourPun
{
    public float maxHealth = 100;
    [HideInInspector] public float health = 100;
    private Image m_Amo;
    private Image m_Healt;
    private bool isMine;
    public Animator animator;
    private KillManager _KillManager;
    public AudioSource HitAs;

    private void Start()
    {
        _KillManager = GetComponent<KillManager>();
        health = maxHealth;
        isMine = GetComponentInParent<PhotonView>().IsMine;
        if (isMine)
        {
            m_Amo = GameObject.FindWithTag("TargettableAMO").GetComponent<Image>();
            m_Healt = GameObject.FindWithTag("TargettableHealth").GetComponent<Image>();
        }
    }

    [PunRPC]
    public void AdaptShooterKills(int id, bool toall, int failId)
    {
        if (photonView.ViewID == id)
        {
            _KillManager.Kills++;
        }

        if (photonView.ViewID == failId && toall)
        {
            GetComponent<KillManager>().Deaths++;
        }

        if (toall)
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<PlayerHealthManager>().AdaptShooterKills(id, false, failId);
            }
        }
    }

    public void TakeDamage(int damage, int attackerViewId)
    {
        
        if (!animator.GetBool("Respawn"))
        {
            photonView.RPC("Hitaplayer", RpcTarget.All, attackerViewId, photonView.ViewID, damage);

        }
        Debug.Log(health.ToString() + "" + attackerViewId + isMine);
    }

    [PunRPC]
    public void Hitaplayer(int viewId, int opferid, int damage)
    {
        if (viewId == opferid)
        {
            return;
        }
        var componentInParent = GetComponentInParent<PhotonView>();
        if (componentInParent.ViewID != viewId && !componentInParent.IsMine)
        {
            HitAs.Play();
        }

        if (componentInParent.ViewID == opferid)
        {
            health -= damage;
            m_Healt.fillAmount -= (damage / 100f);
            
            if (health <= 0f)
            {
                photonView.RPC("AdaptShooterKills", RpcTarget.All, viewId, true, photonView.ViewID);
                StartCoroutine("Respawn");
            }
        }
    }

    private IEnumerator Respawn()
    {
        var cont = GetComponent<FirstPersonController>();
        var charcont = GetComponent<CharacterController>();
        charcont.enabled = false;
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        cont.enabled = false;
        transform.position = new Vector3(0f, -100f, 0f);
        while (health < maxHealth)
        {
            health++;
            m_Healt.fillAmount += 1 / maxHealth;
            yield return new WaitForSeconds(0.2f);
        }
        transform.position = new Vector3(12, 60.75f, 0f);
        rb.useGravity = true;
        cont.enabled = true;
        charcont.enabled = true;
    }
}
