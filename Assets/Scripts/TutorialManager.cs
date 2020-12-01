using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int numberToSpawn;
    [SerializeField]
    private SpawnPoint[] spawnPoints;

    private NewPlayerController player;
    private BigCanon bigCanon;

    private bool turretReloaded;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteTutorial());
        player = FindObjectOfType<NewPlayerController>();
        bigCanon = FindObjectOfType<BigCanon>();
    }

    IEnumerator ExecuteTutorial()
    {
        UIManager.Message("Press [F8] to see controls and objectives...");
        SpawnEnemies();
        yield return new WaitForSeconds(7);
        yield return AmmoFactory();
        yield return null;
    }

    #region Tutorial Steps

    void SpawnEnemies()
    {
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            StartCoroutine(SpawnWave(spawnPoint));
        }
    }

    IEnumerator AmmoFactory()
    {
        UIManager.Message("Turn to the right and drive towards the yellow building.");
        yield return new WaitForSeconds(7);
        UIManager.Message("Drive into the flashing blue square and then stop.");
        UIManager.Message("Wait there until the ammo bar (blue bar at the bottom left) is full.");
        yield return WaitForAmmoLoaded();
        UIManager.Message("Good job!");
        UIManager.Message("Find a turret in need of ammo.");
        yield return new WaitForSeconds(4);
        UIManager.Message("You can tell a turret is in need of ammo if it's orange bar is low or empty.");
        yield return WaitForTurretReloaded();
        UIManager.Message("Good job!");
        UIManager.Message("Drive past the enemies and collect some of the scrap they dropped.\r\n" +
            "Be quick, they will try to kill you!");
        yield return WaitForScrapCollected();
        UIManager.Message("Good job!");
        UIManager.Message("Scrap is used to repair turrets and your main base.\r\n" +
            "It is also used to build the Grand Cannon.");
        yield return new WaitForSeconds(10);
        UIManager.Message("Return to the main base and drive around to the back.\r\nGo up to the wireframe structure.");
        yield return new WaitForSeconds(10);
        UIManager.Message("When you near the wireframe structure press 'R' to use your scrap to build the cannon.\r\n" +
            "You will need a lot of scrap to complete it.");
        yield return WaitForScrapGivenToGrandCannon();
        UIManager.Message("Good job!");
        UIManager.Message("We have given you enough scrap to finish the cannon. Go ahead and hit 'R' again.");
        player.Scrap.Collect(100);



        IEnumerator WaitForTurretReloaded()
        {
            List<TurretShooting> shooters = FindObjectsOfType<TurretController>().ToList().Select(x => x.Shooting).ToList();
            shooters.ForEach(x => x.Reloaded += onTurretReloaded);

            while (!turretReloaded) yield return new WaitForSeconds(1);
        }

        IEnumerator WaitForAmmoLoaded()
        {
            while (player.AmmoBoxes < player.MAXAmmoBoxes) yield return new WaitForSeconds(1);
        }

        IEnumerator WaitForScrapCollected()
        {
            while (player.Scrap.Amount < 1) yield return new WaitForSeconds(1);
        }

        IEnumerator WaitForScrapGivenToGrandCannon()
        {
            while (bigCanon.health.CurrentHealth == 0) yield return new WaitForSeconds(1);
        }
    }
















    #endregion Tutorial Steps


    private void onTurretReloaded()
    {
        turretReloaded = true;
    }

    IEnumerator SpawnWave(SpawnPoint spawnPoint)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            SpawnEnemy(enemyPrefab, spawnPoint.GetSpawnLocation(), spawnPoint.GetPath());
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy(GameObject prefabToSpawn, Transform spawnPoint, Vector3[] waypoints)
    {
        FindObjectOfType<AudioManager>().PlayOneShot("enemySpawn");
        var enemy = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.SetWaypoints(waypoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
