using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Threading.Tasks;
using Photon.Realtime;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] private int RespawnTime;
    
    private PhotonView pv;
    private GameObject controller;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        if (controller)
        {
            PhotonNetwork.Destroy(controller);
        }
        
		Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
		controller = PhotonNetwork.Instantiate(Path.Combine("Player"),
            spawnpoint.position, spawnpoint.rotation, 0, new object[] { pv.ViewID });
        
        DeathHandlers.Death += OnDie;
    }

    public void SetWinner(RoomScore.ScoreBoardRecord record)
    {
        if (controller == null || controller.GetComponentInChildren<GUIManager>() == null)
            return;
        
        controller.GetComponentInChildren<GUIManager>().ShowUI(UIs.WinnerScreen);
        controller.GetComponentInChildren<WinnerScreen>().SetWinner(record.Player.NickName, record.Kills);
        controller.GetComponent<PlayerControl>().SwitchInputToMenu();
    }
    
    public void SetTime(float timeRemaining)
    {
        if (controller != null && controller.GetComponentInChildren<Timer>() != null)
            controller.GetComponentInChildren<Timer>().SetTime(timeRemaining);
    }

    private async void OnDie(Player killer, Player target)
    {
        if (!Equals(controller.GetPhotonView().Controller, target))
            return;
        
        await Task.Delay(3000);
        
        PhotonNetwork.Destroy(controller);
        
        await Task.Delay(RespawnTime * 1000);
        
        CreateController();
    }
}
