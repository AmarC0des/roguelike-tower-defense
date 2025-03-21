using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;  // Assign the red bar UI Image
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;  // Start at full health
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent negative health
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent overhealing
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;  // Adjust the fill amount
    }

    //If statements in the method are for testing for now
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10f);  // Lose 10 HP when pressing Space
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(5f);  // Heal 5 HP when pressing H
        }

        UpdateHealthBar();
    }

}
