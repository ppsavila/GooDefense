using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class MinionController : MonoBehaviourPunCallbacks, IPunObservable
{
    [field: SerializeField] private Transform Target{get;set;}
    private NavMeshAgent Agent{get;set;}
    [field: SerializeField] private PhotonView PView{get;set;}
    [field: SerializeField] private SpriteRenderer SR{get;set;}
    [field: SerializeField] private Minion Minion{get;set;}
    private MinionData MinionData{get;set;}
    [field: SerializeField] private MinionType MinionType{get;set;}
    [field: SerializeField] private MinionVfx MinionVfx{get;set;}

    [field: SerializeField] private SpriteRenderer SpriteBar{get;set;}

    private float ActualLife{get;set;}
    private float StartLife{get;set;}
    private float Speed{get;set;}

    public bool inCD = false;
    private bool inSlow = false;

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        //Init(MinionType,);
    }

    public void Init(MinionType type, Transform target)
    {
        if(MinionType == MinionType.Unknow)
            MinionData = Minion.GetMinion(type);
        else
            MinionData = Minion.GetMinion(MinionType);
        Target = target;
        ActualLife = MinionData.Life;
        StartLife = MinionData.Life;
        Speed = MinionData.Speed;
        SR.sprite = MinionData.Gfx;
        Agent.speed = Speed;
        inCD = false;
        inSlow = false;
    }

    void Update()
    {
        if(photonView.IsMine)
        {
           SetAgentPos();
        }
        else
        {
          SetAgentPos();
        }

        if(!inSlow)
            Agent.speed = MinionData.Speed;


        var percent = (ActualLife/StartLife);
        SpriteBar.transform.localScale = new Vector2(percent, 1);
    }
    
    private void SetAgentPos()
    {
        Agent.SetDestination(Target.position);
    }   

    private void SmoothAgentPos()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(ActualLife);
        }
        else if( stream.IsReading)
        {
           var smooth =  stream.ReceiveNext();
        }
    }

    public void TakeDamage(float damage, float CD)
    {
        ActualLife -= damage;
        if(ActualLife <= 0)
        {
            Destroy(this.gameObject);
        }
        StartCoroutine(DamageCD(CD));
    }

    public void TakeSlow(float damage, float CD)
    {
        if(Speed <= 1)
        {
            inSlow = false;
            return;
        }

        inSlow = true;
        Speed -= damage;
        StartCoroutine(SlowCD(CD));
    }

    IEnumerator SlowCD(float CD)
    {  
        MinionVfx.TakeDamage(Color.green, CD);
        inCD = true;
        yield return new WaitForSeconds(CD);
        inCD = false;
    }

    IEnumerator DamageCD(float CD)
    {
        MinionVfx.TakeDamage(Color.red, CD);
        inCD = true;
        yield return new WaitForSeconds(CD);
        inCD = false;
    }
}
