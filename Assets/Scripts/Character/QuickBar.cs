using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The QuickBar is compose of slots of the COSUMABLE type.
 *  If the player usew the shortcut for a quickslot (see InputManager)
 *  it will try to use the item containing by the slot.
 * 
 */
public class QuickBar : MonoBehaviour
{
    public const int MAX_QUICK_ITEMS = 4;

    [SerializeField]
    private GameObject slotPrefab;

    private GameObject quickbarContent;

    private GameObject[] qSlots;

    private PlayerStats player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("PlayerCharacter").GetComponent<PlayerStats>();

        quickbarContent = GameObject.Find("QuickBarContent");

        qSlots = new GameObject[QuickBar.MAX_QUICK_ITEMS];

        for (int i = 0; i < MAX_QUICK_ITEMS; i++)
        {
            GameObject slot = Instantiate(slotPrefab, quickbarContent.transform);
            slot.SendMessage("SetType", Item.ItemType.CONSUMABLE);
            qSlots[i] = slot;
        }
    }

    public void UseSlot(int slot)
    {
        qSlots[slot].GetComponentInChildren<SlotContentItem>().UseOne(player);
    }

   
}
