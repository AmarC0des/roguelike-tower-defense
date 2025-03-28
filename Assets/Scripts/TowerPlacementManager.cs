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



using UnityEngine;

public class TowerPlacementManager: MonoBehaviour
{
    public GameObject towerPrefab; // The actual tower prefab to place
    private GameObject previewTower; // The transparent preview version of the tower
    public Material transparentMaterial; // Transparent material for preview
    public Material cannotPlaceMat; //transparent mat turns red when not able to be placed.
    public Material solidMaterial; // Solid material for the final tower
    //public float placementDistance = 2f; // Distance from the player where the tower will be placed 

    [SerializeField]
    private Terrain terrain;
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
                GetTerrain();
            }
            else
            {
                ConfirmPlacement(); // Confirm and finalize tower placement
            }
        }
      

        // While in preview mode, keep updating the preview tower's position
        if (isPlacing && previewTower != null)
        {
            UpdatePreviewPosition();
            GenerateSplatMapData();
            CheckTerrainLayers(splatMapData, terrainData);
        }
    }

    // Starts the tower placement preview
    public void StartPreview()
    {
        isPlacing = true; // Enable placement mode
        previewTower = Instantiate(towerPrefab, player.transform, worldPositionStays: false); // Create the preview tower
        player.GetComponent<PlayerController>().DetachCharacter();
        // Apply the transparent material to make it look like a preview
        SetTowerMaterial(previewTower, transparentMaterial);
    }

    // Updates the preview tower's position to always appear near the player
    void UpdatePreviewPosition()
    {
        if (previewTower != null)
        {
            previewTower.transform.position = GetPlacementPosition();
        }
    }

    // Confirms the placement, making the tower solid and stopping preview mode
    void ConfirmPlacement()
    {
        if (previewTower != null)
        {
            isPlacing = false; // Disable placement mode
            SetTowerMaterial(previewTower, solidMaterial); // Apply the solid material to finalize the tower
            previewTower.transform.parent = null;
            previewTower = null; // Clear reference to preview tower
            player.GetComponent<PlayerController>().AttachCharacter();
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



    void GetTerrain() //Function used to get the terrain data via raycast
    {
       
        Ray ray = new Ray(transform.position, Vector3.down); //typical raycast
        if (Physics.Raycast(ray, out RaycastHit hit,10f))
        {
            Debug.Log(hit);
            //Check to see if a terrain was hit
            if (hit.collider.GetType() == typeof(TerrainCollider))
            {
                terrain = hit.collider.gameObject.GetComponent<Terrain>(); //get terrain from collider
                Debug.Log(terrain.gameObject.name);
            }
            
        }
    }//End of GetTerrain() 

    private Vector3 sampledAreaMin; // For Gizmo
    private Vector3 sampledAreaMax; // For Gizmo

    void GenerateSplatMapData()
    {
       
        Collider collider = GetComponentInChildren<Collider>();

        // Use the collider's bounds to define the sampled area
        Bounds bounds = collider.bounds;
        Vector3 boundsMin = bounds.min; // Bottom-left corner in world space
        Vector3 boundsMax = bounds.max; // Top-right corner in world space

        terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        // Convert bounds to terrain local coordinates
        Vector3 localMin = boundsMin - terrainPos;
        Vector3 localMax = boundsMax - terrainPos;

        // Convert to splat map coordinates
        int splatX = Mathf.FloorToInt(localMin.x / terrainData.size.x * terrainData.alphamapWidth);
        int splatZ = Mathf.FloorToInt(localMin.z / terrainData.size.z * terrainData.alphamapHeight);
        int splatWidth = Mathf.CeilToInt((localMax.x - localMin.x) / terrainData.size.x * terrainData.alphamapWidth);
        int splatHeight = Mathf.CeilToInt((localMax.z - localMin.z) / terrainData.size.z * terrainData.alphamapHeight);

        // Clamp to valid range to avoid out-of-bounds errors
        splatX = Mathf.Clamp(splatX, 0, terrainData.alphamapWidth - splatWidth);
        splatZ = Mathf.Clamp(splatZ, 0, terrainData.alphamapHeight - splatHeight);
        splatWidth = Mathf.Min(splatWidth, terrainData.alphamapWidth - splatX);
        splatHeight = Mathf.Min(splatHeight, terrainData.alphamapHeight - splatZ);

        // Sample the splat map area under the object
        splatMapData = terrainData.GetAlphamaps(splatX, splatZ, splatWidth, splatHeight);

        // Store sampled area bounds for Gizmo drawing
        sampledAreaMin = terrainPos + new Vector3(
            splatX * terrainData.size.x / terrainData.alphamapWidth,
            boundsMin.y,
            splatZ * terrainData.size.z / terrainData.alphamapHeight
        );
        sampledAreaMax = terrainPos + new Vector3(
            (splatX + splatWidth) * terrainData.size.x / terrainData.alphamapWidth,
            boundsMax.y,
            (splatZ + splatHeight) * terrainData.size.z / terrainData.alphamapHeight
        );

    }
    // Check if the object can be placed (returns true if valid, false if "Sandy" is present)
    void CheckTerrainLayers(float[,,] splatMapData, TerrainData terrainData)
    {
        
        // Track presence of each layer
        bool[] layerPresent = new bool[terrainData.alphamapLayers];
        for (int x = 0; x < splatMapData.GetLength(1); x++) // Width
        {
            for (int z = 0; z < splatMapData.GetLength(0); z++) // Height
            {
                for (int layer = 0; layer < terrainData.alphamapLayers; layer++)
                {
                    if (splatMapData[z, x, layer] > .95f) // Any weight > 0 means the layer is present
                    {
                        layerPresent[layer] = true;
                    }
                }
            }
        }

        if (layerPresent[1] == true)
        {
            SetTowerMaterial(previewTower, cannotPlaceMat);
        }

        // Build list of present layers
        int presentCount = 0;
        for (int i = 0; i < layerPresent.Length; i++)
        {
            if (layerPresent[i]) presentCount++;
        }

        string[] presentLayers = new string[presentCount];
        int index = 0;
        for (int i = 0; i < layerPresent.Length; i++)
        {
            if (layerPresent[i])
            {
                presentLayers[index] = terrainData.terrainLayers[i].name;
                index++;
            }
        }

        // Log all present layers
        Debug.Log("Layers present under object: " + (presentCount > 0 ? string.Join(", ", presentLayers) : "None"));
    }

    void OnDrawGizmos()
    {
        if (terrain == null) return;

        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null) return;

        Gizmos.color = Color.yellow; // Yellow wireframe for the sampled area

        // Define the corners of the sampled area
        Vector3 bottomLeft = new Vector3(sampledAreaMin.x, sampledAreaMin.y, sampledAreaMin.z);
        Vector3 bottomRight = new Vector3(sampledAreaMax.x, sampledAreaMin.y, sampledAreaMin.z);
        Vector3 topLeft = new Vector3(sampledAreaMin.x, sampledAreaMax.y, sampledAreaMax.z);
        Vector3 topRight = new Vector3(sampledAreaMax.x, sampledAreaMax.y, sampledAreaMax.z);

        // Draw the wireframe box
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);

        // Draw vertical lines to show height
        Gizmos.DrawLine(bottomLeft, bottomLeft + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
        Gizmos.DrawLine(bottomRight, bottomRight + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
        Gizmos.DrawLine(topLeft, topLeft + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
        Gizmos.DrawLine(topRight, topRight + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));

        // Draw top rectangle
        Gizmos.DrawLine(bottomLeft + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y),
                        bottomRight + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
        Gizmos.DrawLine(bottomRight + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y),
                        topRight + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
        Gizmos.DrawLine(topRight + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y),
                        topLeft + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
        Gizmos.DrawLine(topLeft + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y),
                        bottomLeft + Vector3.up * (sampledAreaMax.y - sampledAreaMin.y));
    }
}
   
