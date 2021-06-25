using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefabItem;

    public static Dictionary<string, Sprite> itemSpriteDict;
    public static Dictionary<string, Sprite> skillSpriteDict;



    private void Awake()
    {

        itemSpriteDict = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Items/");
        foreach (Sprite item in sprites)
        {
            itemSpriteDict.Add(item.name, item);
        }

        skillSpriteDict = new Dictionary<string, Sprite>();
        sprites = Resources.LoadAll<Sprite>("Skills/");
        foreach (Sprite skill in sprites)
        {
            skillSpriteDict.Add(skill.name, skill);
        }
    }


    void Start()
    {
       GameObject go = Instantiate(prefabItem, new Vector3(-5, 0.5f, 5), Quaternion.identity);
        go.SendMessage("SetItem", new Weapon( "sword", itemSpriteDict["short-sword"], 5f, 5f));

        go = Instantiate(prefabItem, new Vector3(-4, 0.5f, -1), Quaternion.identity);
        go.SendMessage("SetItem", new Weapon("axe", itemSpriteDict["axe"], 3f, 3f, Weapon.weaponType.AXE));

        go = Instantiate(prefabItem, new Vector3(-3, 0.5f, -3), Quaternion.identity);
        go.SendMessage("SetItem", new Hat("fedora", itemSpriteDict["fedora"], 30, 5, Hat.hatType.FEDORA));
        go = Instantiate(prefabItem, new Vector3(-5, 0.5f, -3), Quaternion.identity);
        go.SendMessage("SetItem", new Hat("witch hat", itemSpriteDict["witch-hat"], 0, 50, Hat.hatType.WITCH));
        go = Instantiate(prefabItem, new Vector3(-4, 0.5f, -2), Quaternion.identity);
        go.SendMessage("SetItem", new HealthPotion("small health potion", 20));
        go = Instantiate(prefabItem, new Vector3(-4, 0.5f, -3), Quaternion.identity);
        go.SendMessage("SetItem", new HealthPotion("small health potion", 20));
        go = Instantiate(prefabItem, new Vector3(-3, 0.5f, -1), Quaternion.identity);
        go.SendMessage("SetItem", new HealthPotion("small health potion", 20));
        go = Instantiate(prefabItem, new Vector3(-3, 0.5f, -2), Quaternion.identity);
        go.SendMessage("SetItem", new HealthPotion("small health potion", 20));

    }

    public void NewGame()
    {

    }
}
