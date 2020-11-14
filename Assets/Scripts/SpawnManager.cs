using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public static List<GameObject> SpawnedEnemies = new List<GameObject>();
    public static bool AllEnemiesDead => SpawnedEnemies.Count == 0;
    
    public float timeBeforeFirstWave = 3f;
    public float timeBetweenEnemySpawns = 1f;
    public float timeBetweenWaves = 10f;
    public SpawnPoint[] spawnPoints;
    public EnemyWave[] waves;

    private float timeOfLastSpawn;
    private float secondsBetweenSpawns = 1;
    private int _waveIndex = 0;
    private float _waveCountDown;
    private bool WaveStarted => _waveCountDown <= 0;
    
    private void Start()
    {
        _waveCountDown = timeBeforeFirstWave;
    }
    
    private void Update()
    {
        if (Time.time - timeOfLastSpawn > secondsBetweenSpawns)
        {
            timeOfLastSpawn = Time.time;
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                SpawnEnemy(waves[0].subWave.Skip(1).Take(1).FirstOrDefault().enemies[0], spawnPoint.transform, spawnPoint.GetPath());
            }
        }
        //if (WaveStarted && _waveIndex < waves.Length)
        //{
        //    SpawnWave();
        //    _waveCountDown = timeBetweenWaves;
        //    _waveIndex++;
        //}
        
        //if(!WaveStarted && AllEnemiesDead)
        //    _waveCountDown -= Time.deltaTime;
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
   
    IEnumerator SpawnSubWave(EnemySubWave subWave, SpawnPoint spawnPoint)
    {
        foreach (var enemy in subWave.enemies)
        {
            SpawnEnemy(enemy, spawnPoint.GetSpawnLocation(), spawnPoint.GetPath());
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
        }
    }

    void SpawnEnemy(GameObject prefabToSpawn, Transform spawnPoint, Transform[] waypoints)
    {
        var enemy = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDeath += OnSpawnedEnemyKilled;
        enemyController.SetWaypoints(waypoints);
        SpawnedEnemies.Add(enemy);
    }
    
    void OnSpawnedEnemyKilled(object sender, GameObject gameObjectKilled)
    {
        SpawnedEnemies.Remove(gameObjectKilled);
    }

    private void OnDestroy()
    {
        if (SpawnedEnemies != null && SpawnedEnemies.Any())
        {
            // This is unlikely to matter, but if this object gets destroyed we must deregister all callbacks.
            // If we do not the callbacks will still happen, but will throw a NullReferenceException
            
            //Reply : Ah i remember seeing the error and making a mental note that i have to deregister,
            //but this was interesting to see how you went about it. p.s didnt know List.Any() existed either tbh ^^
            
            foreach (GameObject enemyGameObject in SpawnedEnemies)
            {
                if (enemyGameObject != null)
                {
                    enemyGameObject.GetComponent<EnemyController>().OnDeath -= OnSpawnedEnemyKilled; 
                }
            }
        }
    }
}
