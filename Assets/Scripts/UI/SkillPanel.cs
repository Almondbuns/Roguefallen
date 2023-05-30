using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public GameObject prefab_skill_select_button;
    public GameObject prefab_skill_tree;

    GameObject current_skill_tree;
    public List<GameObject> skill_list;

    // Start is called before the first frame update
    void Start()
    {
        skill_list = new List<GameObject>();

        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        SkillTreeData skill_tree = player_data.player_stats.skill_tree;

        int counter = 0;
        foreach (SkillData skill in skill_tree.skills)
        {
            GameObject skill_button = GameObject.Instantiate(prefab_skill_select_button, this.transform, false);
            if (skill.icon != null)
            {
                Texture2D texture = Resources.Load<Texture2D>(skill.icon);
                skill_button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            skill_button.name = "Skill Button " + skill.name;
            skill_button.GetComponent<RectTransform>().localPosition = new Vector3(-700 + 150 * (counter % 4), 50 - 180 * (counter / 4));
            int closure_counter = counter;
            skill_button.GetComponent<Button>().onClick.AddListener(() => SwitchSkillTree(closure_counter));
            if (counter == 0)
                skill_button.transform.Find("Selection").GetComponent<Image>().color = new Color(1, 1, 1, 1);

            skill_button.GetComponent<Tooltip>().text = "Skill: " + skill.name;

            skill_list.Add(skill_button);
            ++counter;
        }

        current_skill_tree = GameObject.Instantiate(prefab_skill_tree, this.transform, false);
        current_skill_tree.GetComponent<RectTransform>().localPosition = new Vector3(370,-10);
        current_skill_tree.GetComponent<SkillTree>().skill = skill_tree.skills[0];
        current_skill_tree.GetComponent<SkillTree>().Setup();

        Refresh();

    }
    internal void Refresh()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        transform.Find("AttributePoints").Find("PointsToSpend").Find("Points").GetComponent<TextMeshProUGUI>().text = player_data.player_stats.attribute_points.ToString();
        transform.Find("ExpertisePoints").Find("PointsToSpend").Find("Points").GetComponent<TextMeshProUGUI>().text = player_data.player_stats.skill_expertise_points.ToString();
        transform.Find("TalentPoints").Find("PointsToSpend").Find("Points").GetComponent<TextMeshProUGUI>().text = player_data.player_stats.talent_points.ToString();

        current_skill_tree.GetComponent<SkillTree>().Refresh();

        transform.Find("Strength").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.strength.ToString();
        transform.Find("Strength").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetStrength().ToString();
        transform.Find("Vitality").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.vitality.ToString();
        transform.Find("Vitality").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetVitality().ToString();
        transform.Find("Dexterity").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.dexterity.ToString();
        transform.Find("Dexterity").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetDexterity().ToString();
        transform.Find("Constitution").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.constitution.ToString();
        transform.Find("Constitution").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetConstitution().ToString();
        transform.Find("Intelligence").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.intelligence.ToString();
        transform.Find("Intelligence").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetIntelligence().ToString();
        transform.Find("Willpower").Find("Base").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.player_stats.willpower.ToString();
        transform.Find("Willpower").Find("Current").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetWillpower().ToString();
       
    }

    public void SwitchSkillTree(int counter)
    {
        Destroy(current_skill_tree);

        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        SkillTreeData skill_tree = player_data.player_stats.skill_tree;

        current_skill_tree = GameObject.Instantiate(prefab_skill_tree, this.transform, false);
        current_skill_tree.GetComponent<RectTransform>().localPosition = new Vector3(370, -10);
        current_skill_tree.GetComponent<SkillTree>().skill = skill_tree.skills[counter];
        current_skill_tree.GetComponent<SkillTree>().Setup();

        foreach(GameObject button in skill_list)
            button.transform.Find("Selection").GetComponent<Image>().color = new Color(1, 1, 1, 0);


        skill_list[counter].transform.Find("Selection").GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

       public void AddStrength()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.strength += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddVitality()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.vitality += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddDexterity()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.dexterity += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddConstitution()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.constitution += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddItelligence()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.intelligence += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

    public void AddWillpower()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        if (player_data.player_stats.attribute_points <= 0)
            return;

        player_data.player_stats.willpower += 1;
        player_data.player_stats.attribute_points -= 1;

        Refresh();
    }

}
