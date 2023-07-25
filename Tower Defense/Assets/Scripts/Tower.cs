using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Tower")]
public class Tower : ScriptableObject
{
    [field:SerializeField] public List<TowerData> Towers{get;set;}

    public TowerData GetTower(TowerType target)
    {
        return Towers.Find(x => x.TowerType == target);
    }
}   

[Serializable]
public class TowerData
{
    [field: SerializeField] public TowerType TowerType{get;set;}
    [field: SerializeField] public Sprite Gfx{get;set;}
    [field: SerializeField] public float AtkSpeed{get;set;}
    [field: SerializeField] public float Atk{get;set;}
    [field: SerializeField] public float Range{get;set;}
}

[Serializable]
public enum TowerType
{
    Unknow = 0, 
    Dps = 1,
    Dot = 2,
    Slow =3
}
