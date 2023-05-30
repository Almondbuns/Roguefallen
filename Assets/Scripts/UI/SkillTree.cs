using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public GameObject prefab_skill_talent;

    public SkillData skill;

    public List<GameObject> buttons;

    public void Setup()
    {
        buttons = new List<GameObject>();

        transform.Find("NoviceButton").GetComponent<Button>().onClick.AddListener(() => BuyExpertise(SkillExpertiseLevel.NOVICE));
        transform.Find("AdeptButton").GetComponent<Button>().onClick.AddListener(() => BuyExpertise(SkillExpertiseLevel.ADEPT));
        transform.Find("ExpertButton").GetComponent<Button>().onClick.AddListener(() => BuyExpertise(SkillExpertiseLevel.EXPERT));
        transform.Find("MasterButton").GetComponent<Button>().onClick.AddListener(() => BuyExpertise(SkillExpertiseLevel.MASTER));

        int counter = 0;
        foreach (SkillExpertiseData skill_expertise in skill.expertises)
        {
            int expertise_level = 0;
            switch (skill_expertise.level)
            {
                case SkillExpertiseLevel.NOVICE:
                    expertise_level = 0;
                    break;

                case SkillExpertiseLevel.ADEPT:
                    expertise_level = 1;
                    break;

                case SkillExpertiseLevel.EXPERT:
                    expertise_level = 2;
                    break;

                case SkillExpertiseLevel.MASTER:
                    expertise_level = 3;
                    break;

            }

            int skill_counter = 0;

            foreach (SkillTalentData skill_talent in skill_expertise.talents)
            {
                GameObject skill_button = GameObject.Instantiate(prefab_skill_talent, this.transform, false);
                skill_button.name = "Skill Talent Button " + counter;
                buttons.Add(skill_button);

                skill_button.GetComponent<RectTransform>().localPosition = new Vector3(25 + 160 * skill_counter - (160 * (skill_expertise.talents.Count - 1)) / 2, -210 + 150 * expertise_level);

                if (skill_talent.talent != null)
                {
                    Texture2D texture = Resources.Load<Texture2D>(skill_talent.talent.prototype.icon);
                    skill_button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    SkillTalentData skill_talent_closure = skill_talent;
                    skill_button.GetComponent<Button>().onClick.AddListener(() => BuySkillTalent(skill_talent_closure));
                    skill_button.GetComponent<TalentButton>().talent_data = skill_talent.talent;
                }
                ++counter;
                ++skill_counter;
            }
        }

        Refresh();
    }

    public void Refresh()
    {
        if (skill.expertises.Find(x => x.level == SkillExpertiseLevel.NOVICE).is_unlocked == true)
        {
            transform.Find("NoviceBG").GetComponent<Image>().color = new Color(1,1,1,.1f);
            transform.Find("NoviceButton").GetComponent<Image>().color = Color.white;

            transform.Find("NoviceButton").GetComponent<Tooltip>().text = "Skill Expertise: Novice.<br><br><color=green> Expertise level is already unlocked.</color>";
        }
        else
        {
            transform.Find("NoviceBG").GetComponent<Image>().color = new Color(.0f,.0f,.0f,.25f);
            transform.Find("NoviceButton").GetComponent<Image>().color = Color.grey;
            transform.Find("NoviceButton").GetComponent<Tooltip>().text = "Skill Expertise: Novice.<br><br><color=yellow> Expertise level can be unlocked with 1 expertise point.</color>";
        }

        if (skill.expertises.Find(x => x.level == SkillExpertiseLevel.ADEPT).is_unlocked == true)
        {
            transform.Find("AdeptBG").GetComponent<Image>().color = new Color(1, 1, 1, .1f);
            transform.Find("AdeptButton").GetComponent<Image>().color = Color.white;
            transform.Find("AdeptButton").GetComponent<Tooltip>().text = "Skill Expertise: Adept.<br><br><color=green> Expertise level is already unlocked.</color>";
        }
        else
        {
            transform.Find("AdeptBG").GetComponent<Image>().color = new Color(.0f, .0f, .0f, .25f);
            transform.Find("AdeptButton").GetComponent<Image>().color = Color.grey;
            transform.Find("AdeptButton").GetComponent<Tooltip>().text = "Skill Expertise: Adept.<br><br><color=yellow> Expertise level can be unlocked with 2 expertise points.</color>";
        }

        if (skill.expertises.Find(x => x.level == SkillExpertiseLevel.EXPERT).is_unlocked == true)
        {
            transform.Find("ExpertBG").GetComponent<Image>().color = new Color(1, 1, 1, .1f);
            transform.Find("ExpertButton").GetComponent<Image>().color = Color.white;
            transform.Find("ExpertButton").GetComponent<Tooltip>().text = "Skill Expertise: Expert.<br><br><color=green> Expertise level is already unlocked.</color>";
        }
        else
        {
            transform.Find("ExpertBG").GetComponent<Image>().color = new Color(.0f, .0f, .0f, .25f);
            transform.Find("ExpertButton").GetComponent<Image>().color = Color.grey;
            transform.Find("ExpertButton").GetComponent<Tooltip>().text = "Skill Expertise: Expert.<br><br><color=yellow> Expertise level can be unlocked with 3 expertise points.</color>";
        }

        if (skill.expertises.Find(x => x.level == SkillExpertiseLevel.MASTER).is_unlocked == true)
        {
            transform.Find("MasterBG").GetComponent<Image>().color = new Color(1, 1, 1, .1f);
            transform.Find("MasterButton").GetComponent<Image>().color = Color.white;
            transform.Find("MasterButton").GetComponent<Tooltip>().text = "Skill Expertise: Master.<br><br><color=green> Expertise level is already unlocked.</color>";
        }
        else
        {
            transform.Find("MasterBG").GetComponent<Image>().color = new Color(.0f, .0f, .0f, .25f);
            transform.Find("MasterButton").GetComponent<Image>().color = Color.grey;
            transform.Find("MasterButton").GetComponent<Tooltip>().text = "Skill Expertise: Master.<br><br><color=yellow> Expertise level can be unlocked with 4 expertise points.</color>";
        }
        
        int counter = 0;
        foreach (SkillExpertiseData skill_expertise in skill.expertises)
        {
            foreach (SkillTalentData skill_talent in skill_expertise.talents)
            {
                GameObject skill_button = buttons[counter];
               
                if (skill_expertise.is_unlocked == false)
                    skill_button.GetComponent<Image>().color = Color.gray;
                else
                    skill_button.GetComponent<Image>().color = Color.white;

                if (skill_talent.talent != null)
                {
                    if (skill_talent.is_unlocked == false)
                        skill_button.transform.Find("Selection").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    else
                        skill_button.transform.Find("Selection").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
                ++counter;
            }
        }
    }

   public void BuyExpertise(SkillExpertiseLevel level)
    {
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        bool success = game_data.player_data.BuyExpertise(skill, level);
        if (success)
            GameObject.Find("UI").GetComponent<UI>().Refresh();
    }

    public void BuySkillTalent(SkillTalentData talent)
    {
        Debug.Log(talent.description);
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        bool success = game_data.player_data.BuySkillTalent(skill, talent);
        if (success)
            GameObject.Find("UI").GetComponent<UI>().Refresh();
    }
}
