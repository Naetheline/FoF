using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 *  Keep track of a value for a caracteristic of a character. (ie. Strength)
 *  The Basevalue can be altered via modificators from various sources (equipements, potions...)
 *  
 *  Decorated by CompStat and use by CharacterStat and its sub-classes.
 */
[System.Serializable]
public class Stat 
{
    [SerializeField]
     protected float baseValue;

    public Stat(float  value = 10f)
    {
        baseValue = value;
    }

    protected List<float> modifiers = new List<float>();

     public virtual float GetValue()
    {
        float totalValue = baseValue;
        foreach (int mod in modifiers)
        {
            totalValue += mod;
        }
        return totalValue;
    }

   unsafe public void SetValue(float newBaseValue)
    {
        baseValue = newBaseValue;
    }
    
 
    public void AddModificator(float value)
    {
        if (value != 0)
        {
            modifiers.Add(value);
        }
    }
    public void RemoveModificator(float value)
    {
        if (value != 0)
        {
            modifiers.Remove(value);
        }
    }
}
