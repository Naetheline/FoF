
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Interactable {

    private Item item;

   public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<SpriteRenderer>().sprite = newItem.icon;
    }

    public override void MoveToInteraction(CharacterControl cc, PlayerStats player)
    {
        base.MoveToInteraction(cc, player);
        cc.agent.stoppingDistance = 0.5f;
    }

    public override void Interact(CharacterControl cc,PlayerStats player)
    {
        base.Interact(cc, player);
        if (player.GetComponent<Inventory>().AddItem(item))
        {
            Destroy(this.gameObject);
        }
    }

    
}
