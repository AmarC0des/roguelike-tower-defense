using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public enum GameState { Set_Up, Wave, Progression, Victory, Gameover }
    public GameState currentState;
    public TMP_Text stateText;


    void Start()
    {
        ChangeState(GameState.Set_Up);
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Set_Up:
                HandlePlanning();
                stateText.text = "Planning Phase";
                break;
            case GameState.Wave:
                HandleAttacking();
                stateText.text = "Wave Phase";
                break;
            case GameState.Progression:
                stateText.text = "Progression Phase";
                break;
            case GameState.Victory:
                HandleWin();
                stateText.text = "Victory Phase";
                break;
            case GameState.Gameover:
                HandleLose();
                stateText.text = "End Phase";
                break;
        }
    }

    void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game State changed to: " + currentState);
    }

    void HandlePlanning()
    {
        // Player places towers, prepares for wave
        if (Input.GetKeyDown(KeyCode.Space)) // Example trigger to start wave
        {
            ChangeState(GameState.Wave);
        }
    }

    void HandleAttacking()
    {
        // Enemy wave attacks, player defends
        if (AllEnemiesDefeated()) // Placeholder function
        {
            ChangeState(GameState.Victory);
        }
        else if (PlayerLost()) // Placeholder function
        {
            ChangeState(GameState.Gameover);
        }
    }

    void HandleWin()
    {
        // Prepare next level
        Debug.Log("Wave cleared! Proceeding to next level.");
        ChangeState(GameState.Set_Up);
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
        ChangeState(GameState.Set_Up);
    }
}
