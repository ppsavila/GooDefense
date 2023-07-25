using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [field: SerializeField] private Tower Tower{get;set;}
    [field: SerializeField] private TowerType TowerType{get;set;}
    [field: SerializeField] private SpriteRenderer SR{get;set;}
    private TowerData TowerData{get;set;}

    private float AtkSpeed{get;set;}
    private float Atk{get;set;}
    private float Range{get;set;}
    

    void Awake()
    {
        Init(TowerType);
    }

    public void Init(TowerType type)
    {
        
        if(TowerType == TowerType.Unknow)
            TowerData = Tower.GetTower(type);
        else
            TowerData = Tower.GetTower(TowerType);

        AtkSpeed = TowerData.AtkSpeed;
        Atk = TowerData.Atk;
        Range = TowerData.Range;
        SR.sprite = TowerData.Gfx;
    }

    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,Range, 2);
        if(colliders.Length == 0 )
            return;

        if(TowerData.TowerType == TowerType.Dot) // DotAtaca todos na area
        {
            foreach(Collider2D col in colliders)
            {
               var minion = col.GetComponent<MinionController>();
               if(!minion.inCD)
                minion.TakeDamage(Atk, AtkSpeed);
            }
        }
        else if(TowerData.TowerType == TowerType.Dps) 
        {
            var minion = colliders.First().GetComponent<MinionController>();
            if(!minion.inCD)
                minion.TakeDamage(Atk, AtkSpeed);
        }else if(TowerData.TowerType == TowerType.Slow) 
        {
          foreach(Collider2D col in colliders)
            {
               var minion = col.GetComponent<MinionController>();
               if(!minion.inCD)
                minion.TakeSlow(Atk, AtkSpeed);
            }
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
