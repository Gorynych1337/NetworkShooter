using Photon.Pun;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public void BackToMenuButton()
    {
        PhotonNetwork.LeaveRoom(true);
    }
}
