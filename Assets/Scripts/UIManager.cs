using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject TowerPlaceUI;
    public GameObject LevelUpUI;  // New Level-Up Panel
    public GameObject OpeningScreenUI;

    public TMP_Text goldCountText, enemyCountText, waveCountText, charLevelText, XPText;
    public TMP_Text strengthText, speedText, pointsText;

    public Button strengthButton, speedButton, closeButton;

    private int strength, speed, points;

    void Start()
    {
        LevelUpUI.SetActive(false);  // Hide level-up menu initially
        // TowerPlaceUI.SetActive(false); Commmented it out cause not sure how to use it but as long as
        //                              an object is assigned in unity to TowerPlaceUI everything works fine

        strengthButton.onClick.AddListener(() => UpgradeStat("strength"));
        speedButton.onClick.AddListener(() => UpgradeStat("speed"));
        closeButton.onClick.AddListener(CloseLevelUpMenu);
    }

    // UI for the stats in the level up screen
    public void StatsUpdateUI(int points)
    {
        strengthText.text = "Strength: " + strength;
        speedText.text = "Speed: " + speed;
        pointsText.text = "Available Points: " + points;

        strengthButton.interactable = points > 0; // If points is 0 buttons become unclickable
        speedButton.interactable = points > 0;
    }

    public void UpdateXPUI(int xp, int xpRequired)
    {
        XPText.text = "XP: " + xp + " / " + xpRequired; // Show XP progress 
    }

    public void UpdateGoldUI(int gold)
    {
        goldCountText.text = gold.ToString();
    }

    public void UpdateEnemyCountUI(int enemyCount)
    {
        enemyCountText.text = enemyCount.ToString();
    }

    public void UpdateWaveCountUI(int waveCount)
    {
        waveCountText.text = waveCount.ToString();
    }

    public void UpdateLevelCountUI(int charLevel)
    {

        charLevelText.text = charLevel.ToString();
    }

    public void UpdatePoints(int updatedPoints)
    {
        points = updatedPoints;
        StatsUpdateUI(points);  // Update the UI with the new points
    }


    // Method that works with buttons to add points to specific stat
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

    // Method that works with continue button to close the level up screen.
    void CloseLevelUpMenu()
    {
        LevelUpUI.SetActive(false);
    }
}
