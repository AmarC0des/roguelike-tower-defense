/* Tile Type Class
 * ----------------------------
 * A Data contianer to store Tile type and 
 * runs code to randomly select Tile Stats for
 * the Tile this is attached based on the type. 
 * 
 * 
 * Modified 2/20/25:
 * -Basic setup of script
 * -Built SetTileStats function
 * 
 * 
 * Notes:
 * Stone type-Double Renvue
 * Forest type- increase chance of town spawn
 * Bog- Slows player but always has treasure chest
 * Town-BottleNeck and new tower
 * Boss-Boss is summoned
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileType", menuName = "Data/TileType")]
public class TileType :ScriptableObject
{
    public List<TileStats> allTileStats = new List<TileStats>(); //Holds all tiles of the given type


    //Method used to assign tile stats to TileManager that shares a gameobject with this
    public TileStats SetTileStats()
    {
      TileStats newStats = new TileStats();
      newStats = allTileStats[Random.Range(0, allTileStats.Count)]; //Randomly selects a tile stats in the list

      return newStats;
    } //End of SetTileStats
}
