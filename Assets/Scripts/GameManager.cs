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


    public TMP_Text stateText;

    public Collider castleCol;
    public int maxCastleHp;
    public int curCastleHp;

    public int goldCount;
    public int enemyCount;
    public int waveCount;
    public int charLevel;

    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Stores spawned enemies


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
        curCastleHp = maxCastleHp;
        nextState = GameState.StartGame;
        ChangeState();
    }

    void Update()
    {

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
                HandleWinGame();
                break;
            case GameState.Gameover:
                HandleGameOver();
                break;
        }
    }

    private void HandleStartGame()
    {
        stateText.text = "Game Started";
        nextState = GameState.SetUp;
        goldCount = 0;
        enemyCount = 0;
        waveCount = 0;
        charLevel = 1;

        UpdateUI();

    }

    private void HandlePlanning()
    {
        waveCount++;
        towerManager.enabled = true;
        uiManager.TowerPlaceUI.SetActive(true);
        stateText.text = "Planning Phase";
        nextState = GameState.Wave;

        UpdateUI();
    }

    private void HandleWave()
    {
        towerManager.enabled = false;
        uiManager.TowerPlaceUI.SetActive(false);
        stateText.text = "Wave Phase";
        nextState = GameState.Progression;

        UpdateUI();
    }

    private void HandleProgression()
    {
        charLevel++;
        stateText.text = "Progress Phase";
        nextState = GameState.SetUp;

        UpdateUI();
    }

    private void HandleWinGame()
    {
        stateText.text = "Victory Phase";
        nextState = GameState.TitleScreen;

    }
    private void HandleGameOver()
    {
        stateText.text = "Game Over Phase";
        nextState = GameState.TitleScreen;
    }

    bool AllEnemiesDefeated()
    {

        return false;
    }

    public void PlayerDied()
    {
        nextState = GameState.Gameover;
        ChangeState();
    }

    void ToTitleScreen()
    {

    }

    private void UpdateUI()
    {
        uiManager.UpdateGoldUI(goldCount);
        uiManager.UpdateEnemyCountUI(enemyCount);
        uiManager.UpdateWaveCountUI(waveCount);
        uiManager.UpdateLevelCountUI(charLevel);
    }

    public void CastleTakeDamage(float damage)
    {
        curCastleHp -= Mathf.RoundToInt(damage);
        Debug.Log(curCastleHp);
    }
    public void PlayerTakeDamage(float damage)
    {

    }
}
