using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  Decorator of the Stat class
 * 
 *  A CompStat is a Stat with its own modifier but is base value is based on an other Stat.
 */

[System.Serializable]
public class CompStat : Stat
{
    public Stat caracteristic;

    public CompStat( Stat baseStatistic)
    {
        caracteristic = baseStatistic;
    }

    public override float GetValue()
    {
        float carac = 0;
        if (caracteristic != null)
        {
            carac = caracteristic.GetValue();
        }

        return carac + base.GetValue();
    }
}
