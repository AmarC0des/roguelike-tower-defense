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
 * 
 * Modified by Camron Carr 3/23/25
 * -Added functions for terrin analysis and placement
 * constraints. 
 * 
 * 
 * Modified by Camron Carr 4/1/25
 * -Finished tower placement. Now there is a check to see what
 * terrain the tower is on to look for certain layers that are
 * forbidden from placement. 
 * 
 * 
 * NOTES:
 * Things still needed:
 * -More UI elements, to show buttons and relay info to the user
 * -Need to make forbiddenLayers an array that holds all layers
 * in the game that are not allowed. 
 */



using System;
using System.Collections.Generic;
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
    List<Material> towerOGMaterials = new();

    void Update()
    {
        // Ryan Commented
        // // Detects when the player presses "T" to start or confirm tower placement
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     if (!isPlacing)
        //     {
        //         StartPreview(null); // Start preview mode
        //     }
        //     else
        //     {
        //         ConfirmPlacement(); // Confirm and finalize tower placement
        //     }
        // }


        // While in preview mode, keep updating the preview tower's position
        if (isPlacing && previewTower != null)
        {
            //Ryan Added
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConfirmPlacement(); // Confirm and finalize tower placement
            }

            UpdatePreview(); // Update the preview based on terrain check
        }
    }

    // Starts the tower placement preview
    public void StartPreview(TowerSO towerSO)
    {
        isPlacing = true; // Enable placement mode
        previewTower = Instantiate(towerSO.tower, transform, worldPositionStays: false); // Create the preview tower
        previewTower.GetComponent<Collider>().isTrigger = true;
        player.GetComponent<PlayerController>().DetachCharacter();
        previewTower.GetComponent<TowerObj>().enabled = false;
        GetOGTowerMaterial(previewTower);
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
                ResetTowerMaterial(previewTower);
                previewTower.transform.parent = null;
                previewTower.GetComponent<TowerObj>().enabled = true;
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
        return new Vector3(player.transform.position.x, player.transform.position.y-1f, player.transform.position.z); // Place tower near the player at ground level
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
    void ResetTowerMaterial(GameObject tower)
    {
        Renderer[] renderers = tower.GetComponentsInChildren<Renderer>(); // Get all renderers in the tower object
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = towerOGMaterials[i];
        }
        towerOGMaterials.Clear();
    }
    void GetOGTowerMaterial(GameObject tower)
    {
        Renderer[] renderers = tower.GetComponentsInChildren<Renderer>(true); // Get all renderers in the tower object
        foreach (Renderer v in renderers)
        {
            print(v.gameObject);
            towerOGMaterials.Add(v.material);
        }
    }

    void GetTerrain() //function used to get the terrain data via raycast
    { 
        if(!previewTower) return;
        //1 unit up to offset the 1 unit down when displaying preview.
        Ray ray = new Ray(previewTower.transform.position + Vector3.up * 1f, Vector3.down);
        Debug.DrawRay(previewTower.transform.position + Vector3.up * 1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            //check to see if a terrain was hit
            if (hit.collider.GetType() == typeof(TerrainCollider))
            {
                terrain = hit.collider.gameObject.GetComponent<Terrain>(); //get terrain from collider
            }

        }
    }//End of GetTerrain() 

    void GenerateSplatMapData()
    {
        Collider towerCollider = GetComponentInChildren<Collider>(); //any collider
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
    }

    bool CanPlaceOnTerrain(float[,,] splatMapData, TerrainData terrainData, int forbiddenLayerIndex = 1)
    {
        //checks for layer that is not allowed for placement
        for (int z = 0; z < splatMapData.GetLength(0); z++) //height
        {
            for (int x = 0; x < splatMapData.GetLength(1); x++) //width
            {
                float weight = splatMapData[z, x, forbiddenLayerIndex];
                if (weight > 0.5f) //checks weight of splatmap data
                {
                    return false; //layer is present, so terrain cannot be placed
                }
            }
        }
        return true; //safe to place tower
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
        if (CanPlaceOnTerrain(splatMapData, terrainData, 1))//check to see if it can be placed
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

