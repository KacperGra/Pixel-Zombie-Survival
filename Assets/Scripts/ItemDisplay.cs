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
            if (item.index != ShopItem.AmmoBox)
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
            switch (item.index)
            {
                case ShopItem.Rifle:
                    player.HaveRifle = true;
                    break;
                case ShopItem.Shotgun:
                    player.HaveShotgun = true;
                    break;
                case ShopItem.AmmoBox:
                    player.ammoNumber += 30;
                    break;
                default:
                    break;
            }
        }     
    }
}
