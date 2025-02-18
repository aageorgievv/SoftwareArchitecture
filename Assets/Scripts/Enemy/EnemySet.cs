using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySet
{
    public GameObject enemyPrefab;
    public int enemyAmount;
    public float spawnDelay = 1f;
}
