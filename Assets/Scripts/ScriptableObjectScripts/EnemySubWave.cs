using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "Spawner/SubWave")]
public class EnemySubWave : ScriptableObject
{
    public GameObject EnemyPrefab;
    private float enemyMaxHealth => EnemyPrefab.GetComponent<EnemyController>().ScriptableObject.maxHealth;
    public int NumberToSpawn;
    public int TotalWaveHealth => (int)enemyMaxHealth * NumberToSpawn;
}
