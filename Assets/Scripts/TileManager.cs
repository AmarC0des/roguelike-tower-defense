/* Tile Manager Class
 * ----------------------------
 * Runs the tile generation works as 
 * a proxy between the game and the
 * tile data. 
 * 
 * 
 * Modified by Camron Carr 2/20/25:
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
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public TileType type;
    private float timeSinceLastSpawn = -5; //5 second runoff before spawntimer begins

    [SerializeField]
    private TileStats tileStats;

    [SerializeField]
    private CinemachinePath path;
    
    public int enemyCount;
    private int totalSpawnedEnemies = 0;

    [SerializeField]
    private Text enemyCountText;

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Stores spawned enemies


    // Start is called before the first frame update
    void Awake()
    {
        tileStats = type.SetTileStats(); //sets TileStats via method in TileType 
        path = gameObject.GetComponent<CinemachinePath>();

        UpdateEnemyCountUI(); // Update UI at start
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
        if( totalSpawnedEnemies <= tileStats.maxEnemyCount) //checks to make sure max enemy count per tile is set.
        {
            GameObject newEnemy;

            Debug.Log("EnemySpawned");
            newEnemy = Instantiate(tileStats.GetEnemy(), path.m_Waypoints[0].position, Quaternion.identity);
            newEnemy.GetComponent<CinemachineDollyCart>().m_Path = path;
            enemyCount++;
            spawnedEnemies.Add(newEnemy); // Keep track of spawned enemies
            totalSpawnedEnemies++;

            UpdateEnemyCountUI(); // Update the UI when enemy is spawned
            return;
        }
        Debug.Log("Enemy Maximum Count Reached");

    }//End of SpawnEnemy


    //UI method to message player the number of enemies left
    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies Left: " + enemyCount.ToString();
        }
    }//End of UpdateEnemyCountUI

}


