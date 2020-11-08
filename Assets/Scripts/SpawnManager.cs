using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public static List<GameObject> SpawnedEnemies = new List<GameObject>();
    public static bool AllEnemiesDead => SpawnedEnemies.Count == 0;
    
    public float timeBeforeFirstWave = 3f;
    public float timeBetweenEnemySpawns = 1f;
    public float timeBetweenWaves = 10f;
    public Transform[] spawnPoints;
    public EnemyWave[] waves;
    
    private int _waveIndex = 0;
    private float _waveCountDown;
    private bool WaveStarted => _waveCountDown <= 0;
    
    private void Start()
    {
        _waveCountDown = timeBeforeFirstWave;
    }
    
    private void Update()
    {
        if (WaveStarted && _waveIndex < waves.Length)
        {
            SpawnWave();
            _waveCountDown = timeBetweenWaves;
            _waveIndex++;
        }
        
        if(!WaveStarted && AllEnemiesDead)
            _waveCountDown -= Time.deltaTime;
    }

    public void SpawnWave()
    {
        int spawnpointIndex = 0;
        foreach (var subWave in waves[_waveIndex].subWave)
        {
            StartCoroutine(SpawnSubWave(subWave, spawnPoints[spawnpointIndex]));
            spawnpointIndex++;
        }
    }
   
    IEnumerator SpawnSubWave(EnemySubWave subWave, Transform spawnPoint)
    {
        foreach (var enemy in subWave.enemies)
        {
            SpawnEnemy(enemy, spawnPoint);
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
        }
    }

    void SpawnEnemy(GameObject prefabToSpawn, Transform spawnPoint)
    {
        var enemy = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        SpawnedEnemies.Add(enemy);
    }
    
    //todo listen for enemyDeath event and remove from SpawnedEnemyList
}
