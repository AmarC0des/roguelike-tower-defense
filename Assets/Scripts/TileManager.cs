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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public TileType type;
    private float timeSinceLastSpawn = -5; //5 second runoff before spawntimer begins

    public TileStats tileStats;

    bool canSpawn;
    private int totalSpawnedEnemies = 0;

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Stores spawned enemies


    // Start is called before the first frame update
    void Awake()
    {
        tileStats = type.SetTileStats(); //sets TileStats via method in TileType
        canSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= tileStats.spawnRate) //checks if it is time to spawn
            {
                SpawnEnemy();

                timeSinceLastSpawn = 0f; //timer resets
            }
        }

        if(GameManager.Instance.currentState == GameManager.GameState.Wave) { 
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }
    

    }//End of Update


    //Method used to spawn enemies on the tile
    public void SpawnEnemy()
    {
        if( totalSpawnedEnemies < tileStats.maxEnemyCount) //checks to make sure max enemy count per tile is set.
        {
            GameObject newEnemy;
            newEnemy = Instantiate(tileStats.GetEnemy(GameManager.Instance.waveCount), GameManager.Instance.pathManager.path.m_Waypoints[0].position, Quaternion.identity);
            newEnemy.GetComponent<CinemachineDollyCart>().m_Path = GameManager.Instance.pathManager.path;
            spawnedEnemies.Add(newEnemy); //keep track of spawned enemies
            totalSpawnedEnemies++;
            GameManager.Instance.UpdateUI();
            
 
            return;
        }

    }//End of SpawnEnemy

}


