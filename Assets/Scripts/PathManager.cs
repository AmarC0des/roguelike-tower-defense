using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class PathManager : MonoBehaviour
{

    public List<GameObject> tilesPrefabs = new List<GameObject>(); //tiles are made of terrain/buildings and paths
    public List<Button> tileOptionButtons = new List<Button>(); // UI tile option buttons (RawImages)
    public List<GameObject> placedTiles = new List<GameObject>();
    public List<RawImage> tileOptionImages = new List<RawImage>();        // RawImages assigned in Inspector

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
        if (Input.GetKeyDown(KeyCode.Space)) //just a test, will be removed when game is more defined
        {
            PlaceTile();
        }
    }

    //Method for dropping a new terrain in. 
    public void PlaceTile()
    {
        if (selectedTileIndex < 0 || selectedTileIndex >= tilesPrefabs.Count)
        {
            Debug.LogWarning("No valid tile selected!");
            return;
        }

        GameObject selectedPrefab = tilesPrefabs[selectedTileIndex];
        GameObject newTile = Instantiate(selectedPrefab, furthestTile.transform.position + new Vector3(0, 0, 50), Quaternion.identity);
        furthestTile = newTile;
        placedTiles.Add(newTile);

        selectionPanel.SetActive(false);

        // Reset selection
        if (currentlySelectedButton != null)
        {
            ColorBlock colors = currentlySelectedButton.colors;
            currentlySelectedButton.targetGraphic.color = colors.normalColor;
            currentlySelectedButton = null;
        }

        selectedTileIndex = -1;
    }

    

    private void ConnectPaths()
    {
        //connects the enemy paths from each tile
    }
}
