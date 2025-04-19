using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    [SerializeField] private int goldAmount = 0;
    [SerializeField] private Text goldText;

    //This could be used later to save the amount of Gold a player has either throughout the game progresses
    // or once the player replays the game.
    /*
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set this as the global instance
            DontDestroyOnLoad(gameObject); // Keep across scenes
            LoadGold(); // Load saved gold amount
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GoldManager
        }
    }
    */

    private void Start()
    {
        UpdateGoldUI(); // Update UI when game starts
    }

    //UI method to add gold
    public void AddGold(int amount)
    {
        goldAmount += amount;
        UpdateGoldUI();
        SaveGold(); // Save gold after updating
    }

    //UI method for once gold is spent subtracting from gold amount total
    public void SpendGold(int amount)
    {
        if (goldAmount >= amount)
        {
            goldAmount -= amount;
            UpdateGoldUI();
            SaveGold(); // Save gold after spending
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    //Updates the UI text in the game
    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + goldAmount;
        }
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt("Gold", goldAmount); // Save gold
        PlayerPrefs.Save();
    }

    private void LoadGold()
    {
        goldAmount = PlayerPrefs.GetInt("Gold", 0); // Load saved gold or 0 if none
    }

    public void ResetGold() // Optional: Reset gold for a new game
    {
        goldAmount = 0;
        SaveGold();
        UpdateGoldUI();
    }

    private void Update()
    {
        //This is used just for testing if text is updated
        if (Input.GetKeyDown(KeyCode.Z))  
        {
            AddGold(100); // Add 100 gold when pressing 'Z'
            Debug.Log("Added 100 gold!");
        }
    }

}
