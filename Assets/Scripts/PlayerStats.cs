using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    float currentHealth;
    [SerializeField]
    float maxHealth = 100f;
    [SerializeField]
    public GameObject healthbar;

    // DAMAGE VARIABLES
    float damageToTake = 0;
    float damageStep = 0;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        if(damageToTake != 0)
            DamageOverTime(damageStep);
        else
            damageStep = 0;
    }
    private void LateUpdate()
    {
        GameController controler = GameObject.Find("UI").GetComponent<GameController>();
        if (currentHealth == 0 && controler.gameState != GameController.GameState.EndGame)
        {
           controler.EndGame();
        }
    }
    public void TakeDamage(float damage)
    {
        damageToTake = damage;
        damageStep = damage * Time.fixedDeltaTime;
    }
    // Animate healthbar
    void DamageOverTime(float damage)
    {
        currentHealth -= damage;
        damageToTake -= damage;
        damageToTake = Convert.ToInt32(damageToTake * 100) * 0.01f;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (healthbar != null)
        {
            int temp = (int)Mathf.Round(currentHealth);
            healthbar.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = temp.ToString();
            if(damageToTake==0)
                healthbar.GetComponent<Slider>().value = temp;
            else
                healthbar.GetComponent<Slider>().value = currentHealth;
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    
}
