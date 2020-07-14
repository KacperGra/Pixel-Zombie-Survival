using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Item item;

    public Text nameText;
    public Image itemImage;
    public Text costText;
    public Text buttonText;
    public Player player;
    private bool IsBought = false;
    private bool transactionDone = false;

    public int indexID;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = item.name;
        itemImage.sprite = item.image;
        costText.text = item.cost.ToString();
    }

    public void OnClick()
    {
        if (player.goldNumber >= item.cost && IsBought == false)
        {
            player.goldNumber -= item.cost;
            if (item.name != "Ammo x30")
            {
                costText.text = "AVILABLE";
                costText.fontSize = 9;
                IsBought = true;
            }
            transactionDone = true;
        }
        else
        {
            transactionDone = false;
        }
        if (transactionDone == true)
        {
            switch (indexID)
            {
                case 1:
                    player.HaveRifle = true;
                    break;
                case 2:
                    player.HaveShotgun = true;
                    break;
                case 3:
                    player.ammoNumber += 30;
                    break;
                default:
                    break;
            }
        }     
    }
}
