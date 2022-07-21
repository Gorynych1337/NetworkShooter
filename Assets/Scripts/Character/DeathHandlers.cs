using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DeathHandlers : MonoBehaviourPunCallbacks, IPunObservable, IHittable
{
    public delegate void PlayerDeath([CanBeNull] Player killer, Player target);
    [CanBeNull] public static event PlayerDeath Death;
    
    private bool isDead;
    private PhotonView pv;
    private PlayerControl pc;

    public bool IsDead => isDead;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        isDead = false;
        
        if (!pv.IsMine)
        {
            return;
        }

        pc = GetComponent<PlayerControl>();

        Death += Die;
    }
    
    public void OnHit(Vector3 shotForceVector, Player shooter)
    {
        pv.RPC("RpcOnCharacterHit", RpcTarget.All, shotForceVector, shooter);
    }
    
    [PunRPC]
    void RpcOnCharacterHit(Vector3 shotForceVector, [CanBeNull] Player shooter)
    {
        if (!isDead)
            Death?.Invoke(shooter, pv.Controller);
        
        if (!pv.IsMine)
            return;
        
        pc.RB.freezeRotation = false;
        pc.RB.AddForce(shotForceVector);
    }

    private void Die(Player killer, Player target)
    {
        if (Equals(pv.Controller, target))
            isDead = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isDead);
        }
        else if (stream.IsReading)
        {
            isDead = (bool)stream.ReceiveNext();
        }
    }
}
