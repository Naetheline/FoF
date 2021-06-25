using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Base class form which inherite every skills possible.
 * 
 *  TODO : add a target and a way to set it differently if it is the player launching the spell or a monster.
 * 
 */
public class Skill : ITooltip
{
    protected CharacterStats launcher;
    protected GameObject projectilePrefab;
    protected Camera cam = Camera.main;

    public Sprite icon;

    private float coolDown;
    public float CoolDown
    {
        get
        {
            return coolDown;
        }
    }

    private string name;
    public string Name
    {
        get { return name; }
    }

    private string description;
    public string Description
    {
        get { return description; }
    }

    private CharacterStats.DamageType type;
    public CharacterStats.DamageType Type
    {
        get { return type; }
    }

    private int attackPwr;
    public int AttackPwr
    {
        get { return attackPwr; }
    }

    public Skill(string n, Sprite i, CharacterStats.DamageType t, int a, float cd, CharacterStats creature)
    {
        name = n;
        icon = i;
        type = t;
        attackPwr = a;
        coolDown = cd;
        launcher = creature;

        projectilePrefab = Resources.Load<GameObject>("Prefabs/FireBall");
    }


    public virtual void Use()
    {
    }

    public string GetTooltip()
    {
        return string.Format("{0}\n Does {1} {2} dammage(s) to the target.", name, attackPwr, type);
    }
}
