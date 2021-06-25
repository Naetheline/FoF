using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 *  This class represent the inventory of the player
 *  Inventory Panel and inventory content are the UI element.
 *  Each item is manage throught slots of the inventory.
 * 
 */ 

public class Inventory : MonoBehaviour
{
    public const int MAX_ITEMS = 20;

    [SerializeField]
    private GameObject slotPrefab;

    private GameObject InventoryPanel;
    private GameObject InventoryContent;

    

    private void Start()
    {
        InventoryPanel = GameObject.Find("InventoryPanel");
        InventoryContent = GameObject.Find("InventoryContent");
        if(InventoryContent == null)
        {
            Debug.LogError("No Inventory content found...");
        }
        for (int i = 0; i < MAX_ITEMS; i++)
        {
            GameObject slot = Instantiate(slotPrefab, InventoryContent.transform);
            slot.SendMessage("SetType", Item.ItemType.ANY);
        }
        InventoryPanel.SetActive(false);

    }

    public void OpenInventory()
    {
        InventoryPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        InventoryPanel.SetActive(false);
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < InventoryContent.transform.childCount; i++)
        {
            Slot slot = InventoryContent.transform.GetChild(i).GetComponent<Slot>();
            if(slot.isAcceptable(itemToAdd))
            {
               if( slot.transform.GetComponentInChildren<SlotContentItem>().TryAssignItem(itemToAdd))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
