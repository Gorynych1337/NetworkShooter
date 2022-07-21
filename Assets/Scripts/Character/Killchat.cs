using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Photon.Realtime;
using UnityEngine;

public class Killchat : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject killChatItemPrefab;
    
    [SerializeField] private int killchatSize;
    List<KillchatItem> killChatList;

    private void Awake()
    {
        killChatList = new List<KillchatItem>();
        
        DeathHandlers.Death += AddKillRecord;
    }
    
    private void AddKillRecord(Player killer, Player target)
    {
        KillchatItem item = Instantiate(killChatItemPrefab, container).GetComponent<KillchatItem>();

        if (Equals(killer, target))
        {
            item.Initialize(target.NickName);
        }
        else
        {
            item.Initialize(killer.NickName, target.NickName);
        }
        
        killChatList.Add(item);

        if (killChatList.Count < killchatSize)
            return;
        
        Destroy(killChatList[0].gameObject);
        killChatList.RemoveAt(0);
    }
}
