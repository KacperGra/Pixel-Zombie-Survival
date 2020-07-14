using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject ZombieRegular;
    public GameObject ZombieSprinter;
    public GameObject ZombieTank;
    public GameObject ZombieGiant;
    private Vector2 screenBounds;

    [HideInInspector]
    public float timeToSpawn = 2f;
    private float currentTime = 0f;
    GameMaster gameMaster;
    
    [HideInInspector]
    public bool BossSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMaster.SpawnZombies == true)
        {
            if (gameMaster.IsBossFight == true && BossSpawned == false)
            {
                for(int i = 0; i < gameMaster.roundNumber / 5; ++i)
                {
                    SpawnZombie(ZombieGiant);
                }
                BossSpawned = true;
            }
            else if (gameMaster.IsBossFight == false)
            {
                if (currentTime >= timeToSpawn)
                {
                    if(gameMaster.roundNumber > 2)
                    {
                        int randomValue = Random.Range(0, 3);
                        switch(randomValue)
                        {
                            case 0:
                                SpawnZombie(ZombieRegular);
                                break;
                            case 1:
                                SpawnZombie(ZombieTank);
                                break;
                            case 2:
                                SpawnZombie(ZombieSprinter);
                                break;
                            default:
                                SpawnZombie(ZombieRegular);
                                break;
                        }
                    }
                    else
                    {
                        SpawnZombie(ZombieRegular);
                    }
                }
                currentTime += 1 * Time.deltaTime;
            }
        }
    }

    private Vector3 SetRandomPosition()
    {
        int spawnIndex = Random.Range(0, 4);
        if(spawnIndex == 0)
        {
            return new Vector3(Random.Range(-screenBounds.x - 0.2f, -screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        }
        else if(spawnIndex == 1)
        {
            return new Vector3(Random.Range(screenBounds.x, screenBounds.x + 0.2f), Random.Range(-screenBounds.y, screenBounds.y));
        }
        else if(spawnIndex == 2)
        {
            return new Vector3(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(screenBounds.y + 0.2f, screenBounds.y));
        }
        else
        {
            return new Vector3(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, -screenBounds.y - 0.2f));
        }
    }

    private void SpawnZombie(GameObject zombieType)
    {
        GameObject newZombie = Instantiate(zombieType) as GameObject;
        newZombie.transform.position = SetRandomPosition();
        currentTime = 0f;
        gameMaster.SpawnZombie();
    }
}
