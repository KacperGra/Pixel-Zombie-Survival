using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmoBox : MonoBehaviour
{
    public GameObject ammoBoxPrefab;
    private Vector2 screenBounds;
    public float timeToSpawn = 10f;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameMaster>().IsGameOver == false)
        {
            if (currentTime >= timeToSpawn)
            {
                SpawnBox();
                currentTime = 0f;
            }
            currentTime += 1 * Time.deltaTime;
        }
    }

    private Vector3 SetRandomPosition()
    {
        return new Vector3(Random.Range(-screenBounds.x + 1, screenBounds.x - 1), Random.Range(-screenBounds.y + 1, screenBounds.y - 1));
    }

    private void SpawnBox()
    {
        GameObject newZombie = Instantiate(ammoBoxPrefab) as GameObject;
        newZombie.transform.position = SetRandomPosition();
    }
}
