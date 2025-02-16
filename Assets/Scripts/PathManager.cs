using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{

    public List<TileSO> tiles = new List<TileSO>(); //tiles are made of terrain/buildings and paths
    public List<TileSO> placedTiles = new List<TileSO>();

    public TileSO furthestTile; //set in inspector to be the first tile

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceTile();
        }
    }

    //Method for dropping a new terrain in. 
    public void PlaceTile()  // I need to check for which side the tile will be placed and use that value tiles are being place based on wayy point and not tile terrain
    {
        Instantiate(RandomizeTile().tilePrefab, furthestTile.endPos, Quaternion.identity);


    }

    //Method for Randomly Selecting a tile to place
    public TileSO RandomizeTile()
    {
        TileSO newTile; 

        newTile = tiles[Random.Range(0, tiles.Count-1)]; //randomly selects a tile from the list

        return newTile;
    }// End of RandomizeTile
}
