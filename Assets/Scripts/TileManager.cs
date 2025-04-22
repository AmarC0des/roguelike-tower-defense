/* Tile Manager Class
 * ----------------------------
 * Runs the tile generation works as 
 * a proxy between the game and the
 * tile data. 
 * 
 * 
 * Modified 2/20/25:
 * -Basic setup of script
 * 
 * 
 * 
 * 
 * NOTES:
 * When game manager is made, we might need to update how the tile spawns enemies.
 * 
 * Its possible we might run a spawns per tile in which case the current method is fine
 * and we can just update the spawn location to the furthest tile that way enemy spawns
 * will seem less predictable. This is the idea I am leaning toward.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileType type;
    private float timeSinceLastSpawn = -5; //5 second runoff before spawntimer begins

    [SerializeField]
    private TileStats tileStats;

    [SerializeField]
    private CinemachinePath path;
    
    public int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        tileStats = type.SetTileStats(); //sets TileStats via method in TileType 
        path = gameObject.GetComponent<CinemachinePath>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= tileStats.spawnRate) //checks if it is time to spawn
        {
            SpawnEnemy();

            timeSinceLastSpawn = 0f; //timer resets
        }
    }//End of Update


    //Method used to spawn enemies on the tile
    public void SpawnEnemy()
    {
        if( enemyCount <= tileStats.maxEnemyCount) //checks to make sure max enemy count per tile is set.
        {
            GameObject newEnemy;

            Debug.Log("EnemySpawned");
            newEnemy = Instantiate(tileStats.GetEnemy(), path.m_Waypoints[0].position, Quaternion.identity);
            newEnemy.GetComponent<CinemachineDollyCart>().m_Path = path;
            enemyCount++;
            return;
        }
        Debug.Log("Enemy Maximum Count Reached");

    }//End of SpawnEnemy
}


