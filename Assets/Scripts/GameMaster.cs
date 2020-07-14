using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public float restartDelay = 1f;
    public GameObject gameOverText;
    public GameObject gameOverButton;
    public Text roundText;
    public Text zombiesKilledText;
    public int roundNumber = 1;

    public float timeAfterRound;
    private float currentTime;
    public Text timeText;

    private int spawnedZombies = 0;
    private int zombiesAlive = 0;
    private int zombiesKilled = 0;

    [HideInInspector]
    public int zombiesKilledOverall = 0;

    public AudioClip completeTheTurnSound;
    public AudioClip restartSound;

    [HideInInspector]
    public bool IsGameOver = false;

    [HideInInspector]
    public bool SpawnZombies = true;

    [HideInInspector]
    public bool IsBossFight = false;

    private void Start()
    {
        spawnedZombies = 0;
        gameOverText.SetActive(false);
        gameOverButton.SetActive(false);
        timeText.enabled = false;
        zombiesKilledText.enabled = false;
    }

    private void Update()
    {
        if(IsGameOver == false)
        {
            roundText.text = "Round: " + roundNumber.ToString();
            timeText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
            RoundLogic();    
        }
        else
        {
            SpawnZombies = false;
        }
    }

    private void RoundLogic()
    {
        int zombiesInRound = 3 + Mathf.CeilToInt(Mathf.Pow(1.6f, roundNumber));

        if((float)roundNumber % 5f == 0f)
        {
            IsBossFight = true;
            zombiesInRound = roundNumber / 5;
        }
        else
        {
            IsBossFight = false;
        }

        // Spawn zombies till the round limit
        if(spawnedZombies == zombiesInRound)
        {
            SpawnZombies = false;
        }
        else if (currentTime <= 0f && spawnedZombies == 0)
        {
            SpawnZombies = true;
        }

        // If all Zombies killed this round round number increase
        if (zombiesKilled == zombiesInRound && zombiesAlive == 0)
        {
            FindObjectOfType<ZombieSpawner>().timeToSpawn -= 0.15f;
            zombiesKilled = 0;
            spawnedZombies = 0; // Reset number of spawned zombies
            ++roundNumber;
            AudioSource.PlayClipAtPoint(completeTheTurnSound, new Vector3(0f, 0f, 0f));
            timeText.enabled = true;            
            currentTime = timeAfterRound;
        }
        if (timeText.enabled == true)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        if (currentTime <= 0f) // If time end next round begin
        {
            timeText.enabled = false;
        }
    }

    public void GameOver()
    {
        if (IsGameOver == false)
        {
            IsGameOver = true;
            gameOverText.SetActive(true);
            gameOverButton.SetActive(true);
            zombiesKilledText.text = "Zombies killed: " + zombiesKilledOverall.ToString();
            zombiesKilledText.enabled = true;
        }
    }

    private void Restart()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClick()
    {
        AudioSource.PlayClipAtPoint(restartSound, new Vector3(0f, 0f, 0f));
        Invoke("Restart", restartDelay);
    }

    public void SpawnZombie()
    {
        ++spawnedZombies;
        ++zombiesAlive;
    }

    public void ZombieDeath()
    {
        --zombiesAlive;
        ++zombiesKilled;
    }
}
