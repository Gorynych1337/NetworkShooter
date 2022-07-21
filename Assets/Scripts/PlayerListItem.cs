using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{

    [SerializeField] private TMP_Text playerName;
    private Player _player;
    

    public void SetUp(Player player)
    {
        _player = player;
        playerName.text = player.NickName;
    }


    public override void OnPlayerLeftRoom(Player oherPlayer)
    {
        if(_player == oherPlayer) Destroy(gameObject);
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
   
}
