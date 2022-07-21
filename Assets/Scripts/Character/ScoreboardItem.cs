using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text username;
    [SerializeField] private TMP_Text deaths;
    [SerializeField] private TMP_Text kills;

    public void Initialize(Player _player, int _deaths, int _kills)
    {
        username.text = _player.NickName;
        deaths.text = _deaths.ToString();
        kills.text = _kills.ToString();
    }
    
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
