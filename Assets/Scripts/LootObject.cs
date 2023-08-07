using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Dungeon/Loot", order = 1)]
public class LootObject : ScriptableObject
{
    public GameObject prefab;
    public float mass = 3.0f;
    public float value = 80.0f;
    //public float 
}
