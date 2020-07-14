using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private int ammoInBox = 10;
    public int minimumAmmo = 5;
    public int maximumAmmo = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ammoInBox = Random.Range(minimumAmmo, maximumAmmo);
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.ammoNumber += ammoInBox;
            Destroy(gameObject);
        }
    }
}
