using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{

    public List<GameObject> tilesPrefabs = new List<GameObject>(); //tiles are made of terrain/buildings and paths
    public List<GameObject> placedTiles = new List<GameObject>();

    public GameObject furthestTile; //set in inspector to be the first tile
  
    void Start()
    {

        placedTiles.Add(furthestTile); //Adds initial tile to the list
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //just a test, will be removed when game is more defined
        {
            PlaceTile();
        }
    }

    //Method for dropping a new terrain in. 
    public void PlaceTile()  //I need to check for which side the tile will be placed and use that value tiles are being place based on wayy point and not tile terrain
    {
        GameObject newTile;
        newTile =   Instantiate(furthestTile, furthestTile.transform.position, Quaternion.identity); //Instatiantes new tile and saves to temp
        furthestTile = newTile; 

    }//End of PlaceTile
    

    private void ConnectPaths()
    {
        //connects the enemy paths from each tile
    }
}
