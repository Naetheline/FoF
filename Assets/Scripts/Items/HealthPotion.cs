using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  A HealthPotion is an item with a strenght that heals the player when used.
 * 
 * 
 */
public class HealthPotion : Item
{

    int strength;

    public HealthPotion(string name, int value) : base(ItemType.CONSUMABLE, name, GameManager.itemSpriteDict["hp"])
    {
        strength = value;
    }

    public override bool TryToUse(PlayerStats player)
    {
       if(player.currentHP < player.HPMax.GetValue() * 10)
        {
            player.Heal(strength);
           
            return true;
        }
        return false;
    }

    public override string GetTooltip()
    {
        return base.GetTooltip() + "\n Heals " + strength + " hp.";
    }
}
