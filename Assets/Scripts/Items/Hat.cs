using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


/*
 *  A Hat is an item that cannot be used, but can be equiped in hat slots.
 *  An equiped hat changes the character stats (armor).
 * 
 */
public class Hat : Item
{
    public enum hatType { TOP, FEDORA, WITCH }

    private hatType typeHat;
    public hatType TypeHat
    {
        get
        {
            return typeHat;
        }
    }

    private float physArmorModifier;
    public float PhysArmorModifier
    {
        get
        {
            return physArmorModifier;
        }
    }
    private float magicArmorModifier;
    public float MagicArmorModifier
    {
        get
        {
            return magicArmorModifier;
        }
    }



    public Hat(string name, Sprite icon, float phys, float magic, hatType type = hatType.TOP)
        : base(ItemType.HAT, name, icon)
    {
        physArmorModifier = phys;
        magicArmorModifier = magic;
        typeHat = type;
    }

    public override string GetTooltip()
    {

        StringBuilder sb = new StringBuilder();
        sb.Append(base.GetTooltip());
        sb.Append("\nGives" + physArmorModifier + " physical armor.");
        sb.Append("\nGives" + magicArmorModifier + " magical armor.");

        return sb.ToString();
    }

}
