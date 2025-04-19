using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab; // The actual tower prefab to place
    private GameObject previewTower; // The transparent preview version of the tower
    public Material transparentMaterial; // Transparent material for preview
    public Material solidMaterial; // Solid material for the final tower
    public float placementDistance = 2f; // Distance from the player where the tower will be placed

    private bool isPlacing = false; // Tracks whether the player is currently previewing a tower

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
            UpdatePreviewPosition();
        }
    }

    // Starts the tower placement preview
    void StartPreview()
    {
        isPlacing = true; // Enable placement mode
        previewTower = Instantiate(towerPrefab, GetPlacementPosition(), Quaternion.identity); // Create the preview tower
        
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
            previewTower = null; // Clear reference to preview tower
        }
    }

    // Calculates the position where the tower should be placed
    Vector3 GetPlacementPosition()
    {
        return new Vector3(transform.position.x + placementDistance, 0f, transform.position.z); // Place tower near the player at ground level
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
}
