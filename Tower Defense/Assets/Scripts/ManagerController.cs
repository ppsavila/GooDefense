using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using UnityEngine.UI;

public class ManagerController : MonoBehaviourPunCallbacks
{
    //TODO: Pegar qual player é se e o primeiro ou segundo e passar a position pra ele 
    [field:SerializeField] private Transform P1{get;set;}
    [field:SerializeField] private Transform P2{get;set;}

    [field:SerializeField] private Button SpawnButton{get;set;}
    private int actorId{get;set;}

    void Start()
    {
        actorId = PhotonNetwork.LocalPlayer.ActorNumber;
        SpawnButton.onClick.AddListener(SpawnPlayer);
       
    }

    void SpawnPlayer()
    {
        var Minion = PhotonNetwork.Instantiate("Prefabs/Minion", 
        actorId == 1 ? P1.position : P2.position,
        actorId == 1 ? P1.rotation : P2.rotation);
        int rnd = Random.Range(0,3);
        Minion.GetComponent<MinionController>().Init((MinionType) rnd,
        actorId == 1 ? P2 : P1);
    }
}
