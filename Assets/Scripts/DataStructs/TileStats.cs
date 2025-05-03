/* Tile Stats Class
 * -----------------------------
 * This class is used to define tile stats in game as 
 * well as hold info on the types of enemies that will spawn
 * on the tile. 
 * 
 * Modified 2/6/25:
 * -Basic set up for the class.
 * 
 * Modified 2/20/25:
 * -Changed Name for clarification 
 * -Updated info held in this SO
 * 
 */


using UnityEngine;

//More stronger enemies show up as the wave number increases.
//Ryan Trozzolo
[CreateAssetMenu(fileName = "TileStats", menuName = "ScriptableObjects/TileStatsScriptableObject")]
public class TileStats : ScriptableObject
{
    public const int TOTAL_WAVES = 10;
    public int maxEnemyCount;
    public float spawnRate;
    public EnemyGroup[] enemyGroups;
    public GameObject boss;
    public GameObject finalBoss;
    
    
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject[] enemies;
        
    }
    public GameObject GetEnemy(int wave)
    {
        if(wave == 10)
        {
            return finalBoss;
        }
        EnemyGroup enemyGroup = enemyGroups[wave/(TOTAL_WAVES/enemyGroups.Length)]; //scale up to total waves by the number of enemy packs.
        //ie if the number of enemy groups equals 5, and the total waves equals 10, then each enemy group will show up 2 times.
        //0011223344
        //enemy groups will then be customly sorted to be from weak to strong.
        return enemyGroup.enemies[Random.Range(0, enemyGroup.enemies.Length)];
    }
}

