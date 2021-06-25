using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// See quickBar, the idea is the same but for skills.

public class SkillsBar : MonoBehaviour
{
    public const int MAX_QUICK_SKILLS = 4;

    [SerializeField]
    private GameObject skillSlotPrefab;

    private GameObject skillsbarContent;

    private GameObject[] sSlots;

    // Start is called before the first frame update
    void Awake()
    {
        skillsbarContent = GameObject.Find("SkillsBarContent");

        sSlots = new GameObject[SkillsBar.MAX_QUICK_SKILLS];
        for (int i = 0; i < MAX_QUICK_SKILLS; i++)
        {
            GameObject slot = Instantiate(skillSlotPrefab, skillsbarContent.transform);
            slot.SendMessage("SetType", SkillSlot.SlotType.QUICKBAR);
            sSlots[i] = slot;
        }
    }

    public void UseSlot(int slot)
    {
        sSlots[slot].GetComponentInChildren<SlotContentSkill>().Use();
    }
}
