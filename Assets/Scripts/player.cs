using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class player : MonoBehaviour
{
    [SerializeField] private float playerHealth = 500;
    [SerializeField] private float playerMana;
    private float maxHealth = 500;
    [SerializeField] private float maxMana = 100;
    public float bossDamage = 5;
    public float mobDamage = 5;
    public float manaCost = 5;
    public Image healthLiqued;
    public Image manaLiqued;
    private bool pauseMenu;
    public GameObject pauseMenuObj;
    public GameObject deathMenuObj;
    public Magic magic;
    public Button returnToMainMenu;
    public Button returnToGame;

    private void Start()
    {
        magic = FindObjectOfType<CharaterAttack>().magicType;
        playerHealth = maxHealth;
        playerMana = maxMana;
        manaCost = magic.ManaCost;
    }

    private void Update()
    {
        healthLiqued.fillAmount = maxHealth / playerHealth;
        manaLiqued.fillAmount = maxMana / playerMana;
        PauseGame();
        PlayerManaCheck();
    }

    public int healthGain;
    public int manaGain;

    /// <summary>
    /// Much like the AI code this will compare an object and do an action if the requirements are met
    /// This will depelat the players health by the damage amount specified
    /// This will also add Health and Mana to the player
    /// It is all determind by what object the player collied with
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health"))
        {
            maxHealth += healthGain;
            print("Player Healed " + maxHealth);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Mana"))
        {
            maxMana += manaGain;
            print("Player Gained Mana " + maxMana);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            maxHealth -= bossDamage;
            print("Player Health " + maxHealth);
        }

        if (other.gameObject.CompareTag("Mob"))
        {
            maxHealth -= mobDamage;
            print("Mob did " + mobDamage + "Damage and you have " + maxHealth);
        }

        if (maxHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PlayerHealthCheck();
        }
    }

    /// <summary>
    /// Finds a Magic and finds the mana cost of that magic and deducts it from the payers mana but only if the conditions are met.
    /// </summary>
    public void DeductMana()
    {
        //magic = GetComponent<CharaterAttack>().magicType;
        //manaCost = magic.ManaCost;

        if (FindObjectOfType<CharaterAttack>().castable == true)
        {
            maxMana -= manaCost;
            print("You used " + manaCost + " mana");
        }
    }

    /// <summary>
    /// Pauses the game on a key press and toggles on a hidden UI menu
    /// </summary>
    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && maxHealth > 0)
        {
            pauseMenu = !pauseMenu;
        }

        if (pauseMenu)
        {
            pauseMenuObj.SetActive(true);
        }
        else
        {
            pauseMenuObj.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the players mana is able to support a magic cast by find the players current mana and and the mana need to cast the magic type
    /// </summary>
    void PlayerManaCheck()
    {
        if (playerMana <= 0 || manaCost > maxMana)
        {
            FindObjectOfType<CharaterAttack>().castable = false;
            print("You Dont have enough Mana");
        }
        else
        {
            FindObjectOfType<CharaterAttack>().castable = true;
        }
    }

    /// <summary>
    /// This Unlocks the curser and sets the game time to be frozen and allows the player to return to the main menu
    /// </summary>
    void PlayerHealthCheck()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathMenuObj.SetActive(true);
        Time.timeScale = 0;
        FindObjectOfType<CharaterAttack>().ableToAttack = false;
        FindObjectOfType<CharaterAttack>().castable = false;
    }
}
