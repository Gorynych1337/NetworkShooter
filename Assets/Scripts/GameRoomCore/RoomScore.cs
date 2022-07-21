using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomScore : MonoBehaviourPunCallbacks
{
    public struct ScoreBoardRecord
    {
        public int Kills => kills;
        public int Deaths => deaths;
        public Player Player { get; }
        
        private int kills;
        private int deaths;
        
        public ScoreBoardRecord(Player _player)
        {
            Player = _player;
            kills = 0;
            deaths = 0;
        }

        public void AddDeath() => deaths++;

        public void AddKill() => kills++;

    }
    
    public static RoomScore Instanse;
    public List<ScoreBoardRecord> Scoreboard;

    private void Awake()
    {
        if (Instanse)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instanse = this;
        
        DeathHandlers.Death += DeathHandler;

        Scoreboard = new List<ScoreBoardRecord>();
    }
    
    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            var record = new ScoreBoardRecord(player);
            Scoreboard.Add(record);
        } 
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        var leftPlayerRecord = Scoreboard.Find(x => Equals(x.Player, otherPlayer));
        Scoreboard.Remove(leftPlayerRecord);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var record = new ScoreBoardRecord(newPlayer);
        Scoreboard.Add(record);
    }

    private void DeathHandler([CanBeNull] Player killer, Player target)
    {
        var targetRecord = Scoreboard.Find(x => Equals(x.Player, target));
        int targetIndex = Scoreboard.IndexOf(targetRecord);
        targetRecord.AddDeath();
        Scoreboard[targetIndex] = targetRecord;
        
        if (Equals(killer, target))
            return;

        var killerRecord = Scoreboard.Find(x => Equals(x.Player, killer));
        int killerIndex = Scoreboard.IndexOf(killerRecord);
        killerRecord.AddKill();
        Scoreboard[killerIndex] = killerRecord;
        
        SortScoreboard();
    }

    private void SortScoreboard()
    {
        if (Scoreboard.Count <= 1)
            return;
        
        for (int i = 0; i <= Scoreboard.Count; i++)
        {
            for (int j = i + 1; j < Scoreboard.Count; j++)
            {
                if (Scoreboard[i].Kills < Scoreboard[j].Kills)
                {
                    (Scoreboard[j], Scoreboard[i]) = (Scoreboard[i], Scoreboard[j]);
                }
            }
        }
    }
}
