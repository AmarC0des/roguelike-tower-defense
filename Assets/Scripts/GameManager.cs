/* Game Manager Class
 * ----------------------------
 * Class used to run the game. Handles all 
 * game logic and variables needed to access 
 * game state or statistics. 
 * 
 * 
 * Modified Eric Nunez 3/21/25:
 * -Implemented State Machine
 * 
 * Modified Camron Carr 3/22/25:
 * -Setting up some UI references
 * -Started working on Set_Up Phase functions
 * -Added state UI debugger of sorts
 * -Moved state handling to its own function so that it is called each frame. 
 * 
 * NOTES:
 * When game manager is made, we might need to update how the tile spawns enemies.
 * 
 * Its possible we might run a spawns per tile in which case the current method is fine
 * and we can just update the spawn location to the furthest tile that way enemy spawns
 * will seem less predictable. This is the idea I am leaning toward.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { StartGame, SetUp, Wave, Progression, WinGame, Gameover, TitleScreen }
    public GameState currentState;
    public GameState nextState;
    //Managers
    public UIManager uiManager;
    public TowerPlacementManager towerManager;
    public PathManager pathManager;
    public PlayerController player;

    public TMP_Text stateText;

    public Collider castleCol;
    public int maxCastleHp;
    public int curCastleHp;

    public int xp, xpRequired, points;
    public int goldCount;
    public int maxEnemyCount;
    public int waveCount;
    public int charLevel;
    public int charHP;
    public int charMaxHP;

    public int power;
    public float speed;

    public bool canSpawnBoss;

    public GameObject finalBoss;
    public AudioClip battle;
    public AudioClip planning;
    


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("I'm Instanced");
        }
    }

    void Start()
    {
        nextState = GameState.StartGame;
        ChangeState();
        
    }

    void Update()
    {
        if (xp >= xpRequired)
        {
            GainXP(50);
        }
        if (currentState == GameState.Wave && maxEnemyCount <= 0)
        {
            ChangeState();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            CastleTakeDamage(10);
        }
        
    }

    public void ChangeState()
    {
        currentState = nextState;
        Debug.Log("Game State changed to: " + currentState);
        CheckState();

    }

    private void CheckState()
    {
        switch (currentState)
        {
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.SetUp:
                HandlePlanning();
                break;
            case GameState.Wave:
                HandleWave();
                break;
            case GameState.Progression:
                HandleProgression();
                break;
            case GameState.WinGame:
                ToWinScreen();
                break;
            case GameState.Gameover:
                ToGameOverScreen();
                break;
            case GameState.TitleScreen: //ryan t
                ToTitleScreen();
                break;
        }
    }

    private void HandleStartGame()
    {
        pathManager.ConnectPaths();
        curCastleHp = maxCastleHp;
        stateText.text = "Game Started";
        nextState = GameState.SetUp;
        goldCount = 50;
        maxEnemyCount = 0;
        waveCount = 0;
        charLevel = 1;
        power = 5;
        speed = 3f;
        xpRequired = CalculateXPRequirement(charLevel);
        uiManager.OpeningScreenUI.SetActive(true);
        UpdateUI();
        GetComponent<AudioSource>().clip = planning;
        GetComponent<AudioSource>().Play();
    }

    private void HandlePlanning()
    {
        uiManager.OpeningScreenUI.SetActive(false);
        waveCount++;
        towerManager.enabled = true;
        uiManager.TowerPlaceUI.SetActive(true);
        stateText.text = "Planning Phase";
        nextState = GameState.Wave;

        UpdateUI();
        
    }

    private void HandleWave()
    {
        maxEnemyCount = pathManager.GetMaxEnemyCount();
        towerManager.enabled = false;
        uiManager.TowerPlaceUI.SetActive(false);
        stateText.text = "Wave Phase";
        if (waveCount == 15)
        {
            SpawnBoss();
        }
        nextState = GameState.Progression;
        UpdateUI();
        GetComponent<AudioSource>().clip = battle;
        GetComponent<AudioSource>().Play();
    }

    private void HandleProgression()
    {
        pathManager.ShowTileSelection();  // Show tile selection UI to the player
        stateText.text = "Progress Phase";
        nextState = GameState.SetUp;

        UpdateUI();
        GetComponent<AudioSource>().clip = planning;
        GetComponent<AudioSource>().Play();
    }

    private void HandleWinGame()
    {
        stateText.text = "Victory Phase";
        nextState = GameState.TitleScreen;

    }
    private void HandleGameOver()
    {
        stateText.text = "Game Over Phase";
        SceneManager.LoadScene("EndScreen");
    }

    public void PlayerDied()
    {
        nextState = GameState.Gameover;
        ChangeState();
    }

    void ToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    void ToWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }
    void ToGameOverScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public void UpdateUI()
    {
        uiManager.UpdateGoldUI(goldCount);
        uiManager.UpdateEnemyCountUI(maxEnemyCount);
        uiManager.UpdateWaveCountUI(waveCount);
        uiManager.UpdateLevelCountUI(charLevel);
        uiManager.UpdateXPUI(xp, xpRequired);
    }

    public void CastleTakeDamage(float damage)
    {
        curCastleHp -= Mathf.RoundToInt(damage);
        Debug.Log(curCastleHp);
        if (curCastleHp <= 0)
        {
            nextState = GameState.Gameover;
            ChangeState();
            SceneManager.LoadScene("EndScreen");
        }
    }
  
    public void GainXP(int amount)
    {
        xp += amount;

        if (xp >= xpRequired)
        {
            LevelUp();
        }

        uiManager.UpdateXPUI(xp, xpRequired);
    }
    // Trigger level-up when XP threshold is reached
    public void LevelUp()
    {
        Time.timeScale = 0f;
        charLevel++; // Increase character level
        points = 3; // Give player 3 stat points upon leveling up
        xp = 0; // Reset XP after leveling up
        xpRequired = CalculateXPRequirement(charLevel); // Calculate the XP required for next level

        uiManager.UpdatePoints(points);  // Update points after leveling up
        uiManager.UpdateLevelCountUI(charLevel);  // Update character level UI
        uiManager.StatsUpdateUI(points);  // Update stats (Strength, Speed, Available Points)

        uiManager.LevelUpUI.SetActive(true); // Show the level-up screen
    }

    public void incPower()
    {
        power++;
    }
    public void incSpeed()
    {
        speed += .5f;
    }

    public void DoneLeveling()
    {
        Time.timeScale = 1f;
        uiManager.LevelUpUI.SetActive(true); // Show the level-up screen
        SetSpeed(speed);
    }
    public void SetSpeed(float spdValue) //invert this make it a mehtod of gm
    {
        player.speed = spdValue;
    }
    // Calculate XP required for the next level (scales per level)
    int CalculateXPRequirement(int charLevel)
    {
        return 50 + (charLevel - 1) * 50;
    }


    public void SpawnBoss()
    {
        GameObject boss;
        boss = Instantiate(finalBoss, pathManager.path.m_Waypoints[0].position, Quaternion.identity);
        boss.GetComponent<CinemachineDollyCart>().m_Path = pathManager.path;
    }
    public void AbleToSpawnBoss()
    {
        canSpawnBoss = true;
        SpawnBoss();
    }

    public void RepairCastle()
    {
        if (goldCount >= 50)
        {
            goldCount -= 50;
            curCastleHp = maxCastleHp;
            UpdateUI();
        }
    }
}
