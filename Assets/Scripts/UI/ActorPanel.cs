using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class ActorPanel : MonoBehaviour
{
    public ActorData actor_data = null;
    public ActorData loaded_actor_data = null;

    public GameObject active_effect_prefab;
    public GameObject body_part_prefab;
    public GameObject resistance_prefab;

    RectTransform rect;
    public Vector2 start_position;

    public void Refresh()
    {
        if (actor_data == null)
            return;

        //Icon
        if (actor_data != loaded_actor_data)
        {
            Texture2D texture = Resources.Load<Texture2D>(actor_data.prototype.icon);
            transform.Find("Image").Find("Icon").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            loaded_actor_data = actor_data;
        }

        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = actor_data.prototype.name;

        transform.Find("Level").GetComponent<TMPro.TextMeshProUGUI>().text = actor_data.prototype.stats.level.ToString();

        //HP
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        int monster_stats = player_data.GetCurrentAdditiveEffectAmount<EffectMonsterStats>();

        if (monster_stats > 0)
        {
            transform.Find("HPBar").GetComponent<CurrentMaxBar>().SetValues(actor_data.Health_current, actor_data.GetHealthMax());
            transform.Find("StaminaBar").GetComponent<CurrentMaxBar>().SetValues(actor_data.Stamina_current, actor_data.GetStaminaMax());
            transform.Find("ManaBar").GetComponent<CurrentMaxBar>().SetValues(actor_data.mana_current, actor_data.GetManaMax());
        }
        else
        {
            transform.Find("HPBar").GetComponent<CurrentMaxBar>().SetValues(0,0);
            transform.Find("StaminaBar").GetComponent<CurrentMaxBar>().SetValues(0,0);
            transform.Find("ManaBar").GetComponent<CurrentMaxBar>().SetValues(0,0);
        }

        // 10 - 225
        int height = -215;
        int monster_advanced_stats = player_data.GetCurrentAdditiveEffectAmount<EffectMonsterAdvancedStats>();

        if (monster_advanced_stats > 0)
        {
            transform.Find("ToHit").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = actor_data.prototype.stats.to_hit.ToString();
            transform.Find("Dodge").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = actor_data.prototype.stats.dodge.ToString();
            transform.Find("MovementTime").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = actor_data.prototype.stats.movement_time.ToString();
        }
        else
        {
            transform.Find("ToHit").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            transform.Find("Dodge").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            transform.Find("MovementTime").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    

        int monster_armor = player_data.GetCurrentAdditiveEffectAmount<EffectMonsterArmor>();

        if (monster_armor > 0)
        {
            foreach(ArmorStats armor_stats in actor_data.prototype.stats.body_armor)
            {
                GameObject go = GameObject.Instantiate(body_part_prefab, transform, false);
                go.GetComponent<RectTransform>().localPosition = new Vector3(10, height, 0);
                
                go.GetComponent<TMPro.TextMeshProUGUI>().text = armor_stats.body_part;
                go.transform.Find("Percentage").GetComponent<TMPro.TextMeshProUGUI>().text = armor_stats.percentage.ToString();
                go.transform.Find("Physical").GetComponent<TMPro.TextMeshProUGUI>().text = armor_stats.armor.physical.ToString();
                go.transform.Find("Elemental").GetComponent<TMPro.TextMeshProUGUI>().text = armor_stats.armor.elemental.ToString();
                go.transform.Find("Magical").GetComponent<TMPro.TextMeshProUGUI>().text = armor_stats.armor.magical.ToString();
                go.transform.Find("DurabilityBar").GetComponent<CurrentMaxBar>().SetValues(actor_data.body_armor.Find(x => x.body_part == armor_stats.body_part).durability_current, armor_stats.durability_max);
                height -= 25;
            }
        }
        else
        {
    
        }
        height -= 5;

        int monster_resistances = player_data.GetCurrentAdditiveEffectAmount<EffectMonsterResistances>();

        transform.Find("Resistances").GetComponent<RectTransform>().anchoredPosition = new Vector3(0, height, 0);
        height -= 30;

        if (monster_resistances > 0)
        {
            foreach (DamageType damage_type in Enum.GetValues(typeof(DamageType)))
            {
                if (actor_data.prototype.stats.probability_resistances.GetResistance(damage_type) == DamageTypeResistances.NORMAL)
                    continue;

                GameObject go = GameObject.Instantiate(resistance_prefab, transform, false);
                go.GetComponent<RectTransform>().localPosition = new Vector3(50, height, 0);

                go.GetComponent<TMPro.TextMeshProUGUI>().text = damage_type.ToString();
                go.transform.Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = actor_data.prototype.stats.probability_resistances.GetResistance(damage_type).ToString().Replace("_", " ");
                height -= 25;
            }
        }
        else
        {
           GameObject go = GameObject.Instantiate(resistance_prefab, transform, false);
            go.GetComponent<RectTransform>().localPosition = new Vector3(50, height, 0);

            go.GetComponent<TMPro.TextMeshProUGUI>().text = "Tip:";
            go.transform.Find("Resistance").GetComponent<TMPro.TextMeshProUGUI>().text = "Use Awareness Skills to unlock more info.";
            height -= 25;
        }

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, - height + 10);

        start_position.x = Mouse.current.position.x.ReadValue();
        start_position.y = Mouse.current.position.y.ReadValue();
    }

    public void CheckScreenPosition()
    {
        rect = GetComponent<RectTransform>();

        if (rect.position.x < 0)
        {
            rect.position = new Vector3(0, rect.position.y, rect.position.z);
            return;
        }

        if (rect.position.x + rect.sizeDelta.x > Screen.width)
        {
            rect.position = new Vector3(Screen.width - rect.sizeDelta.x, rect.position.y, rect.position.z);
            return;
        }

        if (rect.position.y > Screen.height)
        {
            rect.position = new Vector3(rect.position.x, Screen.height, rect.position.z);
            return;
        }

        if (rect.position.y - rect.sizeDelta.y < 0)
        {
            rect.position = new Vector3(rect.position.x, rect.sizeDelta.y + 10, rect.position.z);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Mouse.current.position.ReadValue().x - start_position.x) > 100
        || Mathf.Abs(Mouse.current.position.ReadValue().y - start_position.y) > 100)
        {
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(actor_data);
        }
    }

    void FixedUpdate()
    {
        CheckScreenPosition();
    }
}
