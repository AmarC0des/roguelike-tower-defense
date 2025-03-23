/* UI Manager Class
 * ----------------------------
 * Handles the UI for the game.
 * 
 * 
 * Modified by Camron Carr 3/22/25:
 * -Basic setup of script
 * -Added Functions for updating UI elements
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject TowerPlaceUI;

    public TMP_Text goldCountText;
    public TMP_Text enemyCountText;
    public TMP_Text waveCountText;
    public TMP_Text charLevelText;

    // Start is called before the first frame update
    void Start()
    {
        TowerPlaceUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGoldUI(int gold)
    {
        goldCountText.text = "Gold: " + gold;
    }
    public void UpdateEnemyCountUI(int enemyCount)
    {
        enemyCountText.text = "Enemies: " + enemyCount;
    }
    public void UpdateWaveCountUI(int waveCount)
    {
        waveCountText.text = "Wave: " + waveCount;
    }
    public void UpdateLevelCountUI(int charLevel)
    {
        charLevelText.text = "Character Level: " + charLevel;
    }


}
