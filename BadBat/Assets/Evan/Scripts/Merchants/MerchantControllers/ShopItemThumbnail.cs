using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ShopItemThumbnail : MonoBehaviour
{
    public Image itemSprite;
    public TMP_Text itemName;
    private Merchant myMerchant;
    private SO_ShopItems myItem;
    
    public void InitShopItem(SO_ShopItems item, Merchant merchant) {
        myItem = item;
        itemSprite.sprite = item.itemSprite;
        itemName.text = item.itemName;
        myMerchant = merchant;
    }

    public SO_ShopItems GetMyItem() {
        return myItem;
    }

    public void TumbnailSelect() {
        
    }

    public void ThumbnailPressed() {
        myMerchant.PreviewItem(myItem);
    }

    public void ThumbnailHover() {
        
    }
}
