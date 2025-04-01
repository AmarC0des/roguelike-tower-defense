/* Tower Placement Manager Class
 * ----------------------------
 * Class used for tower placement.
 * This is attached to the character prefab and
 * will be able to identify whether or not a tower
 * is permited to be placed. If it is, it will
 * show a preview of it, then place the tower
 * once the player confirms the location. 
 * 
 * 
 * Created by Eric Nunez 3/20/25:
 * -Basic setup of script
 * -Added Functions for tower preview and placement
 * 
 * Modified by Camron Carr 3/23/23
 * -Added functions for terrin analysis and placement
 * constraints. 
 * 
 * 
 * 
 * NOTES:
 * Things still needed:
 * -Better materials
 * -More robust tower selection via UI. 
 * -Fix Layer detection
 * 
 */



using System;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab; // The actual tower prefab to place
    private GameObject previewTower; // The transparent preview version of the tower
    public Material canPlaceMat; // Transparent material for preview
    public Material cannotPlaceMat; //transparent mat turns red when not able to be placed.
    public Material solidMaterial; // Solid material for the final tower

    public float curTerrainHeight;
    public Terrain terrain;
    TerrainData terrainData;
    float[,,] splatMapData;

    private bool isPlacing = false; // Tracks whether the player is currently previewing a tower

    public GameObject player;

    void Update()
    {
        // Detects when the player presses "T" to start or confirm tower placement
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isPlacing)
            {
                StartPreview(); // Start preview mode
            }
            else
            {
                ConfirmPlacement(); // Confirm and finalize tower placement
            }
        }

        // While in preview mode, keep updating the preview tower's position
        if (isPlacing && previewTower != null)
        {
            UpdatePreview(); // Update the preview based on terrain check
        }
    }

    // Starts the tower placement preview
    public void StartPreview()
    {
        GetTerrain();
        isPlacing = true; // Enable placement mode
        previewTower = Instantiate(towerPrefab, player.transform, worldPositionStays: false); // Create the preview tower
        player.GetComponent<PlayerController>().DetachCharacter();
        // Apply the transparent material to make it look like a preview
        SetTowerMaterial(previewTower, canPlaceMat);
    }

    // Confirms the placement, making the tower solid and stopping preview mode
    void ConfirmPlacement()
    {
        if (previewTower != null && terrain != null)
        {
            if (CanPlaceOnTerrain(splatMapData, terrainData, 1))//Checks to see if tower can be placed on speific terrain layer
            {
                isPlacing = false; // Disable placement mode
                SetTowerMaterial(previewTower, solidMaterial); // Apply the solid material to finalize the tower
                previewTower.transform.parent = null;
                previewTower = null; // Clear reference to preview tower
                player.GetComponent<PlayerController>().AttachCharacter();
            }
            else
            {
                Debug.Log("Tower cannot be placed here! ");
            }
        }
    }

    // Calculates the position where the tower should be placed
    Vector3 GetPlacementPosition()
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z); // Place tower near the player at ground level
    }

    // Applies a given material to all parts of the tower (useful for changing transparency)
    void SetTowerMaterial(GameObject tower, Material material)
    {
        Renderer[] renderers = tower.GetComponentsInChildren<Renderer>(); // Get all renderers in the tower object
        foreach (Renderer rend in renderers)
        {
            rend.material = material; // Apply the new material
        }
    }

    void GetTerrain() //function used to get the terrain data via raycast
    {
        Ray ray = new Ray(transform.position, Vector3.down); 
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            Debug.Log(hit);
            //check to see if a terrain was hit
            if (hit.collider.GetType() == typeof(TerrainCollider))
            {
                terrain = hit.collider.gameObject.GetComponent<Terrain>(); //get terrain from collider
                Debug.Log(terrain.gameObject.name);
            }

        }
    }//End of GetTerrain() 

    void GenerateSplatMapData()
    {
        Collider towerCollider = GetComponentInChildren<BoxCollider>();
        if (!towerCollider) return; //Ensure collider exists

        Bounds bounds = towerCollider.bounds; //get collider bounds
        Vector3 boundsMin = bounds.min;
        Vector3 boundsMax = bounds.max;

        terrainData = terrain.terrainData; //grab terrain data
        Vector3 terrainPos = terrain.transform.position;

        Vector3 localMin = boundsMin - terrainPos; //converts to local space
        Vector3 localMax = boundsMax - terrainPos;

        int splatX = Mathf.FloorToInt(localMin.x / terrainData.size.x * terrainData.alphamapWidth); //finds starting x-coor
        int splatZ = Mathf.FloorToInt(localMin.z / terrainData.size.z * terrainData.alphamapHeight); //finds starting z-coor
        int splatWidth = Mathf.CeilToInt((localMax.x - localMin.x) / terrainData.size.x * terrainData.alphamapWidth); //finds splat width
        int splatHeight = Mathf.CeilToInt((localMax.z - localMin.z) / terrainData.size.z * terrainData.alphamapHeight); //finds splat hieght

        splatX = Mathf.Clamp(splatX, 0, terrainData.alphamapWidth - splatWidth); //clamps from 0-to found values above, prevents out-of-bounds errors
        splatZ = Mathf.Clamp(splatZ, 0, terrainData.alphamapHeight - splatHeight);
        splatWidth = Mathf.Min(splatWidth, terrainData.alphamapWidth - splatX); //finds range for the width and height of splatmap
        splatHeight = Mathf.Min(splatHeight, terrainData.alphamapHeight - splatZ);

        splatMapData = terrainData.GetAlphamaps(splatX, splatZ, splatWidth, splatHeight);

        splatMapData = terrainData.GetAlphamaps(splatX, splatZ, splatWidth, splatHeight);
    }

    bool CanPlaceOnTerrain(float[,,] splatMapData, TerrainData terrainData, int forbiddenLayerIndex = 1)
    {
        //Checks for layer that is not allowed for placement
        for (int z = 0; z < splatMapData.GetLength(0); z++) //height
        {
            for (int x = 0; x < splatMapData.GetLength(1); x++) //width
            {
                float weight = splatMapData[z, x, forbiddenLayerIndex];
                if (weight > 0.5f) //Checks weight of splatmap data
                {
                    return false; //Layer is present, so terrain cannot be placed
                }
            }
        }
        return true; //Safe to place tower
    }

    void UpdatePreview()
    {
        GetTerrain();

        if (previewTower != null)
        {
            previewTower.transform.position = GetPlacementPosition();
        }

        if (terrain != null)                 
            GenerateSplatMapData();
            if (CanPlaceOnTerrain(splatMapData, terrainData, 1))//Check to see if it can be placed
            {
                SetTowerMaterial(previewTower, canPlaceMat);
                Debug.Log("Placement allowed!");
            }
            else
            {
                SetTowerMaterial(previewTower, cannotPlaceMat);
                Debug.Log("Placement blocked due to forbidden texture!");
            }
    }
}

