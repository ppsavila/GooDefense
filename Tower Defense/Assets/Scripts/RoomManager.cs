using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [field:SerializeField] private Button JoinButton{get;set;}
    [field:SerializeField] private Button CreateButton{get;set;}

    [field:SerializeField] private TMP_InputField JoinRoomText{get;set;}
    [field:SerializeField] private TMP_InputField CreateRoomText{get;set;}

    void Start()
    {
        JoinButton.onClick.AddListener(Join);
        CreateButton.onClick.AddListener(Create);
    }

    void Join()
    {
       PhotonNetwork.JoinRoom(JoinRoomText.text, null);
    }

    void Create()
    {
        PhotonNetwork.CreateRoom(CreateRoomText.text, new Photon.Realtime.RoomOptions{MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom()
    {
        print("Sucesso ao conectar");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Erro ao conectar" + returnCode + " Message : " + message);
    }
}
