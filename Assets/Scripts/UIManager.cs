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

    private int strength, speed, points = 5, charLevel, xp, xpRequired;

    void Start()
    {
        //TowerPlaceUI.SetActive(false);
        LevelUpUI.SetActive(false);  // Hide level-up menu initially
        
        StatsUpdateUI();

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

    void StatsUpdateUI()
    {
        strengthText.text = "Strength: " + strength;
        speedText.text = "Speed: " + speed;
        pointsText.text = "Available Points: " + points;

        strengthButton.interactable = points > 0;
        speedButton.interactable = points > 0;
    }

    public void UpdateXPUI(int xp, int charLevel)
    {
        xpRequired = CalculateXPRequirement(charLevel);
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


    void UpgradeStat(string stat)
    {
        if (points > 0)
        {
            if (stat == "strength") strength++;
            else if (stat == "speed") speed++;

            points--;
            StatsUpdateUI();
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


    // Gain XP and check if leveling up is needed
    public void GainXP(int amount)
    {
        xp += amount;

        while (xp >= xpRequired)
        {
            xp -= xpRequired;
            LevelUp();
        }

        StatsUpdateUI();
    }

    // Trigger level-up when XP threshold is reached
    void LevelUp()
    {
        charLevel++;
        points += 3; // Reward stat points
        xpRequired = CalculateXPRequirement(charLevel);

        StatsUpdateUI();
        LevelUpUI.SetActive(true); // Show level-up screen
    }

    // Calculate XP required for the next level (scales per level)
    int CalculateXPRequirement(int level)
    {
        return 100 + (level - 1) * 50;
    }
}
