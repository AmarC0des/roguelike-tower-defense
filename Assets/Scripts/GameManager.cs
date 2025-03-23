using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public enum GameState { Set_Up, Wave, Progression, Victory, Gameover }
    public GameState currentState;
    public GameState nextState;
    //Managers
    public TowerPlacementManager towerManager;


    public TMP_Text stateText;


    void Start()
    {
        currentState = GameState.Set_Up;
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Set_Up:
                towerManager.enabled = true;
                HandlePlanning();
                stateText.text = "Planning Phase";
                nextState = GameState.Wave;
                break;
            case GameState.Wave:
                towerManager.enabled = false;
                HandleAttacking();
                stateText.text = "Wave Phase";
                nextState = GameState.Progression;
                break;
            case GameState.Progression:
                stateText.text = "Progression Phase";
                nextState = GameState.Victory;
                break;
            case GameState.Victory:
                HandleWin();
                stateText.text = "Victory Phase";
                nextState = GameState.Gameover;
                break;
            case GameState.Gameover:
                HandleLose();
                stateText.text = "GameOver Phase";
                nextState = GameState.Set_Up;
                break;
        }
    }

    public void ChangeState()
    {
        currentState = nextState;
        Debug.Log("Game State changed to: " + currentState);
    }

    void HandlePlanning()
    {
        // Player places towers, prepares for wave
        if (Input.GetKeyDown(KeyCode.Space)) // Example trigger to start wave
        {
            ChangeState();
        }
    }

    void HandleAttacking()
    {
        // Enemy wave attacks, player defends
        if (AllEnemiesDefeated()) // Placeholder function
        {
            ChangeState();
        }
        else if (PlayerLost()) // Placeholder function
        {
            ChangeState();
        }
    }

    void HandleWin()
    {
        // Prepare next level
        Debug.Log("Wave cleared! Proceeding to next level.");
        
    }

    void HandleLose()
    {
        // Handle game over
        Debug.Log("Game Over! Restarting...");
        RestartGame();
    }

    bool AllEnemiesDefeated()
    {
        // Logic to check if all enemies are defeated
        return false;
    }

    bool PlayerLost()
    {
        // Logic to check if player has lost, in which health reaches 0
        return false;
    }

    void RestartGame()
    {
        // Restart game logic
       // ChangeState();
    }
}
