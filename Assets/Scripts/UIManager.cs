using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject TowerPlaceUI;
    public GameObject LevelUpUI;  // New Level-Up Panel

    public TMP_Text goldCountText, enemyCountText, waveCountText, charLevelText, XPText;
    public TMP_Text strengthText, speedText, pointsText;

    public Button strengthButton, speedButton, closeButton;

    private int strength, speed, points, charLevel, xp, xpRequired;

    void Start()
    {
        //TowerPlaceUI.SetActive(false);
        LevelUpUI.SetActive(false);  // Hide level-up menu initially

        strengthButton.onClick.AddListener(() => UpgradeStat("strength"));
        speedButton.onClick.AddListener(() => UpgradeStat("speed"));
        closeButton.onClick.AddListener(CloseLevelUpMenu);
    }

    // Just for testing for now
    void Update()
    {
        // Press "M" to toggle Level-Up screen
        if (Input.GetKeyDown(KeyCode.M))
        {
            LevelUpUI.SetActive(!LevelUpUI.activeSelf);
        }
    }

    public void StatsUpdateUI(int points)
    {
        strengthText.text = "Strength: " + strength;
        speedText.text = "Speed: " + speed;
        pointsText.text = "Available Points: " + points;

        strengthButton.interactable = points > 0;
        speedButton.interactable = points > 0;
    }

    public void UpdateXPUI(int xp, int xpRequired)
    {
        XPText.text = "XP: " + xp + " / " + xpRequired; // Show XP progress 
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

    public void UpdatePoints(int updatedPoints)
    {
        points = updatedPoints;
        StatsUpdateUI(points);  // Update the UI with the new points
    }



    public void UpgradeStat(string stat)
    {
        if (points > 0)
        {
            if (stat == "strength") strength++;
            else if (stat == "speed") speed++;

            points--;
            StatsUpdateUI(points);
        }
    }

    void OpenLevelUpMenu()
    {
        LevelUpUI.SetActive(true);
    }

    void CloseLevelUpMenu()
    {
        LevelUpUI.SetActive(false);
    }
}
