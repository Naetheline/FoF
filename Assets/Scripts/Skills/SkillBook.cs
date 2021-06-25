using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * Skillbook UI and content.
 * 
 */
public class SkillBook : MonoBehaviour
{
    public const int MAX_SKILLS = 10;

    [SerializeField]
    private GameObject skillSlotPrefab;

    private GameObject book;
    private GameObject bookContent;


    private void Start()
    {
        book = GameObject.Find("SkillsPanel");
        bookContent = GameObject.Find("SkillsContent");
        if (bookContent == null)
        {
            Debug.LogError("No SkillsContent content found...");
        }
        for (int i = 0; i < MAX_SKILLS; i++)
        {
            GameObject slot = Instantiate(skillSlotPrefab, bookContent.transform);
            slot.SendMessage("SetType", SkillSlot.SlotType.BOOK);

            if (i == 0)
            {
                slot.GetComponentInChildren<SlotContentSkill>().Content = new Fireball("FIRE BALL ! ", 80, 3f, 10f,GameObject.Find("PlayerCharacter").GetComponent<PlayerStats>());
            }
        }
        book.SetActive(false);

    }

    public void OpenBook()
    {
        book.SetActive(true);
    }

    public void CloseBook()
    {
        book.SetActive(false);
    }

}
