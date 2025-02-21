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

using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTile", menuName = "Data/TileStats")]
public class TileStats : ScriptableObject {

    public List<GameObject> enemies = new List<GameObject>();//List of enemies that spawn here
    public float spawnRate; //how fast enemies spawn
    public int maxEnemyCount; //maxium enemies allowed. 



    public GameObject GetEnemy()
    {
        GameObject newEnemy;
        newEnemy = enemies[Random.Range(0, enemies.Count)];
        return newEnemy;
    }
}
