using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PathManager : MonoBehaviour
{

    public List<GameObject> tilesPrefabs = new List<GameObject>(); //tiles are made of terrain/buildings and paths
    public List<Button> tileOptionButtons = new List<Button>(); // UI tile option buttons (RawImages)
    public List<GameObject> placedTiles = new List<GameObject>();
    public List<Image> tileOptionImages = new List<Image>();        // RawImages assigned in Inspector Ryan: changed to Images from Raw Images because Images support individual sprites instead of a full texture.

    public GameObject furthestTile; //set in inspector to be the first tile

    public GameObject selectionPanel;     // Assign the UI panel with the 3 options
    private int selectedTileIndex = -1;
    public Button currentlySelectedButton = null;

    public CinemachinePath path;
    private void Start()
    {
        selectionPanel.SetActive(false);
    }

    public void ShowTileSelection()
    {
        // Activate the selection panel when it's time to show it
        selectionPanel.SetActive(true);

        for (int i = 0; i < tileOptionImages.Count && i < tilesPrefabs.Count; i++)
        {
            // Setup button listeners
            int index = i;
            tileOptionButtons[i].onClick.RemoveAllListeners();
            tileOptionButtons[i].onClick.AddListener(() => SelectTile(index));
        }
    }

    public void SelectTile(int index)
    {
        Debug.Log("Selected tile index: " + index);

        // Reset previously selected button's color
        if (currentlySelectedButton != null)
        {
            ColorBlock colors = currentlySelectedButton.colors;
            currentlySelectedButton.targetGraphic.color = colors.normalColor;
        }

        // Update selected button and color
        currentlySelectedButton = tileOptionButtons[index];
        ColorBlock selectedColors = currentlySelectedButton.colors;
        currentlySelectedButton.targetGraphic.color = selectedColors.selectedColor;

        // Save selected tile index
        selectedTileIndex = index;
    }


    void Update()
    {
       
    }

    //Method for dropping a new terrain in. 
    public void PlaceTile()
    {

        if (selectedTileIndex < 0 || selectedTileIndex >= tilesPrefabs.Count)
        {
            Debug.LogWarning("No valid tile selected!");
            return;
        }

        GameObject selectedPrefab = tilesPrefabs[selectedTileIndex * 3 + Random.Range(0, 3)]; //x3 is the starting index (what type is it), random value is the offset (what random tile is it thats in the group)
        // GameObject selectedPrefab = tilesPrefabs[Random.Range(0, tilesPrefabs.Count)];

        GameObject newTile = Instantiate(selectedPrefab, furthestTile.transform.position + new Vector3(0, 0, 50), Quaternion.identity);
        furthestTile = newTile;
        placedTiles.Insert(0, newTile);

        selectionPanel.SetActive(false);

        // Reset selection
        if (currentlySelectedButton != null)
        {
            ColorBlock colors = currentlySelectedButton.colors;
            currentlySelectedButton.targetGraphic.color = colors.normalColor;
            currentlySelectedButton = null;
        }

        selectedTileIndex = -1;
        ConnectPaths();
        GameManager.Instance.ChangeState();
    }



    public void ConnectPaths()
    {
        float zOffset = 0;
        List<CinemachinePath.Waypoint> allWaypoints = new List<CinemachinePath.Waypoint>(); //creates a list of way points due to not knowing size of path at runtime

        foreach (GameObject tile in placedTiles)
        {
            zOffset = tile.transform.position.z;
            CinemachinePath tilePath = tile.GetComponent<CinemachinePath>(); //grabs current tiles path

            for (int i = 0; i < tilePath.m_Waypoints.Length; i++)  //for each Waypoint in the tile path:
            {
                tilePath.m_Waypoints[i].position.z += zOffset;
                allWaypoints.Add(tilePath.m_Waypoints[i]); //add to waypoint to all waypoint list.
                tilePath.m_Waypoints[i].position.z -= zOffset;
               
            }

        }

        path.m_Waypoints = allWaypoints.ToArray();

        path.InvalidateDistanceCache();

        Debug.Log($"Merged {allWaypoints.Count} waypoints into the combined path.");
    }

    public int GetMaxEnemyCount()
    {
        int count = 0;

        foreach (GameObject tile in placedTiles)
        {
            if (tile.GetComponent<TileManager>() != null)
            {
                count += tile.GetComponent<TileManager>().tileStats.maxEnemyCount;
            }
        }

        return count;
    }
}
