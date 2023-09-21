using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerHealth : MonoBehaviour
{
    private static PlayerHealth instance = null;

    public static PlayerHealth Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("PlayerHealth not found");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("Multiple PlayerHealth found");
            Destroy(this);
            return;
        }
    }

    public Text deathMessage;
    public float health;
    public float maxHealth;
    public Slider healthSlider;
    public GameObject deathScreen;

    public float healingAmount = 5;
    public bool healing = true;
    private float tempHealth = 0;
    private float timer = 0;
    public float healingTime = 5;

    private void Update()
    {
        if (health <= 0)
        {
            death("You died");
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthSlider.value = health / maxHealth;

        if (health < maxHealth)
        {
            if (health >= tempHealth)
            {
                timer += Time.deltaTime;
                if (timer > healingTime && healing)
                {
                    health += Time.deltaTime * healingAmount;
                }
            }
            else
            {
                timer = 0;
            }
            tempHealth = health;
        }
    }

    public void death(string str)
    {
        if (Time.timeScale != 0)
        {
            //deathScreen.SetActive(true);
            Time.timeScale = 0;
            //deathMessage.text = str;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}