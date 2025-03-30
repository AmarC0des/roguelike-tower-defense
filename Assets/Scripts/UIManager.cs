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

    private int strength, speed, points, charLevel;

    void Start()
    {
        ResetStats();
        //TowerPlaceUI.SetActive(false);
        LevelUpUI.SetActive(false);  // Hide level-up menu initially

        LoadStats();
        UpdateUI();

        strengthButton.onClick.AddListener(() => UpgradeStat("strength"));
        speedButton.onClick.AddListener(() => UpgradeStat("speed"));
        closeButton.onClick.AddListener(CloseLevelUpMenu);
    }

    void UpdateUI()
    {
        strengthText.text = "Strength: " + strength;
        speedText.text = "Speed: " + speed;
        pointsText.text = "Available Points: " + points;


    }

    public void UpdateGoldUI(int gold)
    {
        PlayerPrefs.SetInt("gold", gold);
        goldCountText.text = "Gold: " + gold;
    }

    public void UpdateEnemyCountUI(int enemyCount)
    {
        PlayerPrefs.SetInt("enemyCount", enemyCount);
        //enemyCountText.text = "Enemies: " + enemyCount;
    }

    public void UpdateWaveCountUI(int waveCount)
    {
        PlayerPrefs.SetInt("waveCount", waveCount);
        //waveCountText.text = "Wave: " + waveCount;
    }

    public void UpdateLevelCountUI(int level)
    {
        charLevel = level;
        PlayerPrefs.SetInt("charLevel", charLevel);
        //charLevelText.text = "Character Level: " + charLevel;
    }

    void UpgradeStat(string stat)
    {
        if (points > 0)
        {
            if (stat == "strength") strength++;
            else if (stat == "speed") speed++;

            points--;
            SaveStats();
            UpdateUI();
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

    void SaveStats()
    {
        PlayerPrefs.SetInt("strength", strength);
        PlayerPrefs.SetInt("speed", speed);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("charLevel", charLevel);
        PlayerPrefs.Save();
    }

    void LoadStats()
    {
        strength = PlayerPrefs.GetInt("strength", 0);
        speed = PlayerPrefs.GetInt("speed", 0);
        points = PlayerPrefs.GetInt("points", 5);
        charLevel = PlayerPrefs.GetInt("charLevel", 1);
    }

    void ResetStats()
    {
        PlayerPrefs.SetInt("strength", 0);
        PlayerPrefs.SetInt("speed", 0);
        PlayerPrefs.SetInt("points", 5);
        PlayerPrefs.Save();

        LoadStats();
        UpdateUI();
    }
}
