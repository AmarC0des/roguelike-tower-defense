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
 * Modified Eric Nunez 3/31/2025
 * - Added xp logic and UI for leveling up screen that is linked with XP.
 * - Points are added for the stats to upgrade when you level up.
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
using TMPro;
public class GameManager : MonoBehaviour
{
    public enum GameState { StartGame, SetUp, Wave, Progression, WinGame, Gameover, TitleScreen }
    public GameState currentState;
    public GameState nextState;
    //Managers
    public UIManager uiManager;
    public TowerPlacementManager towerManager;


    public TMP_Text stateText;
    public int xp, xpRequired, points;
    public int goldCount;
    public int enemyCount;
    public int waveCount;
    public int charLevel;


    void Start()
    {
        nextState = GameState.StartGame;
        ChangeState();
    }

    // Used for testing purposes making sure xp is connected with level up logic
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<GameManager>().GainXP(50);
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
        xpRequired = CalculateXPRequirement(charLevel);

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
        // Logic to check if all enemies are defeated
        return false;
    }

    public void PlayerDied()
    {
        nextState = GameState.Gameover;
        ChangeState();
    }

    void ToTitleScreen()
    {
        //Goes to TitleScreen
    }

    // Gain XP and check if leveling up is needed
    public void GainXP(int amount)
    {
        xp += amount;

        while (xp >= xpRequired)
        {
            LevelUp();
        }

        uiManager.UpdateXPUI(xp, xpRequired);
    }

    // Trigger level-up when XP threshold is reached
    public void LevelUp()
    {
        charLevel++; // Increase character level
        points = 3; // Give player 3 stat points upon leveling up
        xp = 0; // Reset XP after leveling up
        xpRequired = CalculateXPRequirement(charLevel); // Calculate the XP required for next level
    
        uiManager.UpdatePoints(points);  // Update points after leveling up
        uiManager.UpdateLevelCountUI(charLevel);  // Update character level UI
        uiManager.StatsUpdateUI(points);  // Update stats (Strength, Speed, Available Points)

        uiManager.LevelUpUI.SetActive(true); // Show the level-up screen
    }

    // Calculate XP required for the next level (scales per level)
    int CalculateXPRequirement(int charLevel)
    {
        return 100 + (charLevel - 1) * 50;
    }
    

    private void UpdateUI()
    {
        uiManager.UpdateGoldUI(goldCount);
        uiManager.UpdateEnemyCountUI(enemyCount);
        uiManager.UpdateWaveCountUI(waveCount);
        uiManager.UpdateLevelCountUI(charLevel);
        uiManager.UpdateXPUI(xp, xpRequired);
    }
}