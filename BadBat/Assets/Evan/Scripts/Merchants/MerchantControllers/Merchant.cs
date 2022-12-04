using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    //Merchant Settings
    public string merchantName;
    public List<SO_ShopItems> myShopInventory = new List<SO_ShopItems>();
    private List<GameObject> displayedThumbnails = new List<GameObject>();
    
    //UI References
    public Transform shopUIParent;
    public GameObject shopItemPrefab;
    public Transform shopItemParent;

    public SO_ShopItems currentPreviewItem;
    public TMP_Text itemPreviewName;
    public TMP_Text itemPreviewDescription;
    public Image itemPreviewSprite;
    public TMP_Text itemPreviewCostCash;
    public TMP_Text itemPreviewCostParts;

    private void Start() {
        OpenShop();
    }

    public void OpenShop() {
        //Spawn in thumbnails for all items I sell
        foreach (SO_ShopItems item in myShopInventory) {
            var newItem = Instantiate(shopItemPrefab, shopItemParent);
            newItem.GetComponent<ShopItemThumbnail>().InitShopItem(item, this);
            displayedThumbnails.Add(newItem);
        }
        PreviewItem(displayedThumbnails[0]);
        FindObjectOfType<EventSystem>().firstSelectedGameObject = displayedThumbnails[0].GetComponentInChildren<Button>().gameObject; //Used for controller navigation
        shopUIParent.gameObject.SetActive(true);
    }

    public void CloseShop() {
        foreach (var thumbnail in displayedThumbnails) {
            Destroy(thumbnail.gameObject);
        }
        
        shopUIParent.gameObject.SetActive(false);
    }

    //Takes in a thumbnail gameobject and previews the SO_ShopItem attached to it
    public void PreviewItem(GameObject toPreview) {
        var shopItem = toPreview.GetComponent<ShopItemThumbnail>().GetMyItem();

        currentPreviewItem = shopItem;
        itemPreviewName.text = shopItem.itemName;
        itemPreviewDescription.text = shopItem.itemDescription;
        itemPreviewSprite.sprite = shopItem.itemSprite;
        itemPreviewCostCash.text = shopItem.itemCostCash.ToString();
        itemPreviewCostParts.text = shopItem.itemCostParts.ToString();
        
        //Add info here for currency checking and disabling of buttons
    }

    //Previews whatever SO_ShopItem is handed to it
    public void PreviewItem(SO_ShopItems toPreview) { //Extension of method above
        foreach (var item in displayedThumbnails) {
            if (item.GetComponent<ShopItemThumbnail>().GetMyItem() == toPreview) {
                PreviewItem(item); //Calls original method back
            }
        }
    }

    public void PurchaseItem(SO_ShopItems itemToBuy) {
        Debug.Log("Purchase Item: " + itemToBuy.itemName);
        
        GetComponent<IPurchase>().Purchase(itemToBuy);
    }

    public void PurchaseItem() {
        PurchaseItem(currentPreviewItem);
    }
}
