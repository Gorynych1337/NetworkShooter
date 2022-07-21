using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;


    [SerializeField] private TMP_InputField NickName;
    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private Text _errorText;
    [SerializeField] private Text _roomNameText;
    [SerializeField] private Transform _roomList;
    [SerializeField] private GameObject _roomBtnPrefab;

    [SerializeField] private Transform _playerList;
    [SerializeField] private GameObject _playerNamePrefab;

    [SerializeField] private GameObject _startGameBtn;



    private string RoomID;
    private void Start()
    {
        MenuManager.instance.OpenMenu("loading");
        instance = this;
        PhotonNetwork.ConnectUsingSettings();
        //Debug.Log(message: "Присоединяемся к Мастер серверу");
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log(message: "Присоединились к Мастер серверу");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetNick()
    {
        if (string.IsNullOrEmpty(NickName.text))
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 100).ToString("000");
        }
        else
        {
            PhotonNetwork.NickName = new string(NickName.text.Take(15).ToArray());
        }
    }


    public override void OnJoinedLobby()
    {
        //Debug.Log(message: "Присоединились к Лобии");
        MenuManager.instance.OpenMenu("title");
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void CreateRoom()
    {
        //Debug.Log(InputField.text);
        SetNick();


        if (!string.IsNullOrEmpty(InputField.text))
        {
            PhotonNetwork.CreateRoom(InputField.text);
        }
        else
        {
            PhotonNetwork.CreateRoom("Room  " + Random.Range(0, 100).ToString("000"));
        }

        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {


        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("roomMenu");


        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < _playerList.childCount; i++)
        {
            Destroy(_playerList.GetChild(i).gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        _startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorText.text = "Error: " + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");

    }
    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        for (int i = 0; i < _roomList.childCount; i++)
        {
            Destroy(_roomList.GetChild(i).gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(_roomBtnPrefab, _roomList).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(player);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        //Debug.Log("Игрок" + player.NickName + " вышел!!!");

        for (int i = 0; i < _playerList.childCount; i++)
        {

            if (_playerList.GetChild(i).GetComponent<TMP_Text>().text == player.NickName)
                Destroy(_playerList.GetChild(i).gameObject);
        }
    }

}
