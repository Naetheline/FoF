using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Item.ItemType itemAccepted;

    Action<Weapon, Weapon> WeaponChanged;
    Action<Hat, Hat> HatChanged;


    public bool isAcceptable(Item item)
    {

        return (item== null || itemAccepted == Item.ItemType.ANY || item.Type == itemAccepted);
        
    }

    public void SetType(Item.ItemType typeAccepted = Item.ItemType.ANY)
    {
        itemAccepted = typeAccepted;
    }


    public void OnDrop(PointerEventData eventData)
    {      
        SlotContentItem draggedContent = eventData.pointerDrag.GetComponent<SlotContentItem>();

        if(draggedContent == null)
        {
            return;
        }

        if (draggedContent.Content == null || isAcceptable(draggedContent.Content) )
        {
            SlotContentItem toSwap = this.GetComponentInChildren<SlotContentItem>();
            if (draggedContent.parentToReturnTo.gameObject.GetComponent<Slot>().isAcceptable(toSwap.Content))
            {
                // Combining stacks of item
                if(toSwap.Content != null && draggedContent.Content != null &&
                    toSwap.Content.Name == draggedContent.Content.Name && toSwap.Content.Stackable)
                {
                   int leftOver = draggedContent.Add(toSwap.qty);
                    if (leftOver == 0)
                    {
                        toSwap.EmptyContent();
                    }
                    else
                    {
                        toSwap.Add(-(toSwap.qty - leftOver));
                    }
                    
                }
                // If the parent to return to was a weapon slot or a hat slot we need to change equipment
                Slot parentSlot = draggedContent.parentToReturnTo.GetComponent<Slot>();
                
                if(parentSlot != null && parentSlot.itemAccepted == Item.ItemType.WEAPON)
                {
                    if (parentSlot.WeaponChanged != null)
                    {
                        Weapon newWeapon = (toSwap.Content != null) ? (Weapon)toSwap.Content : null;
                        Weapon old = (draggedContent.Content != null )? (Weapon)draggedContent.Content : null;
                        parentSlot.WeaponChanged( newWeapon, old);
                    }
                }
                else if (parentSlot != null && parentSlot.itemAccepted == Item.ItemType.HAT)
                {
                    if (parentSlot.HatChanged != null)
                    {
                        Hat newHat = (toSwap.Content != null) ? (Hat)toSwap.Content : null;
                        Hat old = (draggedContent.Content != null) ? (Hat)draggedContent.Content : null;
                        parentSlot.HatChanged(newHat, old);
                    }
                }
                    
                toSwap.transform.SetParent(draggedContent.parentToReturnTo);
                toSwap.transform.localPosition = Vector3.zero;
                draggedContent.parentToReturnTo = transform;

                // If the slot we drop the item in we need to  to change equipment
                if (itemAccepted == Item.ItemType.WEAPON)
                {
                    // Fire event with new item
                    if(WeaponChanged != null)
                    {
                        Weapon old = (toSwap.Content != null) ? (Weapon)toSwap.Content : null;
                        Weapon newWeapon = (draggedContent.Content != null) ? (Weapon)draggedContent.Content : null;
                        WeaponChanged(newWeapon, old);
                    }
                }
                
                 else if (itemAccepted == Item.ItemType.HAT)
                {
                    // Fire event with new item
                    if(HatChanged != null)
                    {
                        Hat old = (toSwap.Content != null) ? (Hat)toSwap.Content : null;
                        Hat newHat = (draggedContent.Content != null) ? (Hat)draggedContent.Content : null;
                        HatChanged(newHat, old);
                    }
                } 
                
            }
        }
        
    }

    public void RegisterWeaponChanged(Action<Weapon, Weapon> callback)
    {
        WeaponChanged += callback;
    }

    public void UnregisterWeaponChanged(Action<Weapon, Weapon> callback)
    {
        WeaponChanged -= callback;
    }

    public void RegisterHatChanged(Action<Hat, Hat> callback)
    {
        HatChanged += callback;
    }

    public void UnregisterHatChanged(Action<Hat, Hat> callback)
    {
        HatChanged -= callback;
    }

}
