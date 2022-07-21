using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomTimer : MonoBehaviourPunCallbacks, IPunObservable
{
    public static RoomTimer Instanse;
    
    [SerializeField] private float MatchTime;
    private float timeRemaining;
    
    private bool isPlay = true;
    public bool IsTimeRunning { get; set; }

    private void Awake()
    {
        if (Instanse)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instanse = this;

        RestartTimer();
    }

    private void FixedUpdate()
    {
        if (!IsTimeRunning)
            return;

        if (isPlay)
            SceneLoader.Instanse.NowController.SetTime(timeRemaining);
        
        timeRemaining -= Time.fixedDeltaTime;

        if (timeRemaining >= 0) 
            return;
        
        isPlay = false;
        SceneLoader.Instanse.NowController.SetWinner(RoomScore.Instanse.Scoreboard.First());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timeRemaining);
        }
        else if (stream.IsReading)
        {
            timeRemaining = (float)stream.ReceiveNext();
        }
    }

    public override void OnLeftRoom()
    {
        RestartTimer();
    }

    private void RestartTimer()
    {
        timeRemaining = MatchTime;

        IsTimeRunning = true;
    }
}
