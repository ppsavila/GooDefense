using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [field:SerializeField] private Button ConnectButton{get;set;}
    [field:SerializeField] private GameObject ConnectScreen{get;set;}
    [field:SerializeField] private GameObject DisconnectScreen{get;set;}

    void Start()
    {
        ConnectButton.onClick.AddListener(Connect);
    }

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
      
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
       DisconnectScreen.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        if(DisconnectScreen.activeSelf)
           DisconnectScreen.SetActive(false);  
        ConnectScreen.SetActive(true);
    }
}
