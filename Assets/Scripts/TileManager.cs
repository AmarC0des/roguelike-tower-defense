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
    private int totalSpawnedEnemies = 0; // Keeps track of total enemies ever spawned

    [SerializeField]
    private Text enemyCountText; // Reference to UI text component

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

        // Press X key to delete an enemy
        if (Input.GetKeyDown(KeyCode.X))
        {
            DeleteEnemy();
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
    }

    //Method testing to delete enemies to keep track of enemies but will be modified later to use it with enemy health
    private void DeleteEnemy()
    {
        if (spawnedEnemies.Count > 0) // Check if there are enemies to delete
        {
            GameObject enemyToDelete = spawnedEnemies[0]; // Get the first enemy
            spawnedEnemies.RemoveAt(0); // Remove from list
            Destroy(enemyToDelete); // Delete enemy
            enemyCount--;
            UpdateEnemyCountUI(); // Update UI
            Debug.Log("Enemy Deleted");
        }
        else
        {
            Debug.Log("No enemies to delete");
        }
    }
}


