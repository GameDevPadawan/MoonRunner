using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "Spawner/Wave")]
public class EnemyWave : ScriptableObject
{
    public EnemySubWave[] subWaves;

}
