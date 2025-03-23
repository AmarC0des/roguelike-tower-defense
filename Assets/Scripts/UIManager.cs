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
    public static UIManager Instance { get; private set; } //Using Singleton 

    public GameObject TowerPlaceUI;

    public TMP_Text goldCountText;
    public TMP_Text enemyCountText;
    public TMP_Text waveCountText;
    public TMP_Text charLevelText;

    public int goldCount;
    public int enemyCount;
    public int waveCount;
    public int charLevel;

    void Awake()
    {
        //checks if there is an instance of UIManager 
        if (Instance == null) //If not, make one
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);//If there is one, destroy it
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TowerPlaceUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGold(int gold)
    {
        goldCount += gold;
        goldCountText.text = "Gold: " + goldCount;
    }
    public void UpdateEnemyCount()
    {
        enemyCount++;
        enemyCountText.text = "Enemies: " + enemyCount;
    }
    public void UpdateWaveCount()
    {
        waveCount++;
        waveCountText.text = "Wave: " + waveCount;
    }
    public void UpdateLevelCount()
    {
        charLevel++;
        charLevelText.text = "Character Level: " + charLevel;
    }


}
