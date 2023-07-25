using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Minion")]
public class Minion : ScriptableObject
{
    [field:SerializeField] public List<MinionData> Minions{get;set;}

    public MinionData GetMinion(MinionType target)
    {
        return Minions.Find(x => x.MinionType == target);
    }

}   

[Serializable]
public class MinionData
{
    [field: SerializeField] public MinionType MinionType{get;set;}
    [field: SerializeField] public Sprite Gfx{get;set;}
    [field: SerializeField] public float Life{get;set;}
    [field: SerializeField] public float Speed{get;set;}
}

[Serializable]
public enum MinionType
{
    Unknow = 0, 
    Fast = 1,
    Strong = 2,
    Fat =3
}