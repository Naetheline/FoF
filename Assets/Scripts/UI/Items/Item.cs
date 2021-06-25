using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ITooltip
{
    public enum ItemType { ANY, WEAPON, CONSUMABLE, HAT };
    private string name;
    public string Name
    {
        get { return name; }
    }
    private ItemType type;
    public ItemType Type
    {
        get { return type; }
    }
    private bool stackable;
    public bool Stackable
    {
        get { return stackable; }
    }
    public Sprite icon;


    public Item(ItemType iType, string iName, Sprite iIcon)
    {
        type = iType;
        name = iName;
        icon = iIcon;

        stackable = type == ItemType.CONSUMABLE;
    }

    public virtual bool TryToUse(PlayerStats player)
    {
        if (type == ItemType.WEAPON)
        {
        }
        else
        {
            Debug.Log("Using item : " + name);
        }
        return true;
    }

    public virtual string GetTooltip()
    {
        return name;
    }
}
