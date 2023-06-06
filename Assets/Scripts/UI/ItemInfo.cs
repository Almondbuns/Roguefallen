using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemInfo : MonoBehaviour
{
    public ItemData item_data;
    public GameObject prefab_item_effects;
    public GameObject prefab_weapon_data;
    public GameObject prefab_weapon_damage_type;
    public GameObject prefab_armor_data;
    public GameObject prefab_talent_data;
    public GameObject prefab_requirements;
    public GameObject prefab_usable_item_data;
    RectTransform rect;
    public Vector2 start_position;
    
    public void Create()
    {
        if (item_data == null)
        {
            Debug.Log("Error: Start ItemInfo without ItemData");
            return;
        }

        Texture2D texture = Resources.Load<Texture2D>(item_data.GetIcon());
        transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        string quality_string = "";
        switch (item_data.quality)
        {
            case ItemQuality.Normal:
                quality_string = "<color=#ffffff>";
                break;
            case ItemQuality.Magical1:
                quality_string = "<color=#6666ff>";
                break;
            case ItemQuality.Magical2:
                quality_string = "<color=#FFFF00ff>";
                break;
            case ItemQuality.Unique:
                quality_string = "<color=#00aa00>";
                break;
            case ItemQuality.Artefact:
                quality_string = "<color=#aa2222>";
                break;
        }
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = quality_string + item_data.GetName() + "</color>";
        if (item_data.GetTier() > 0)
            transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text += " +" + item_data.GetTier();
        transform.Find("Type").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetItemType().ToString();
        if (item_data.amount > 1)
            transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.amount.ToString();
        else
            transform.Find("Amount").gameObject.SetActive(false);
      
        transform.Find("Weight").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetWeight().ToString();
        transform.Find("Gold").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetGoldValue().ToString();
        int height = 95;

        if (item_data.GetEffectsWhenConsumed().Count > 0)
        {
            
            GameObject consume_effects = GameObject.Instantiate(prefab_item_effects, transform, false);
            consume_effects.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
            height += 20;
            consume_effects.transform.Find("EffectSource").GetComponent<TMPro.TextMeshProUGUI>().text = "When Consumed";
            consume_effects.transform.Find("Effects").GetComponent<TMPro.TextMeshProUGUI>().text = "";

            (string new_text, int new_height) = AddEffects(item_data.GetEffectsWhenConsumed());
            height += new_height;
            consume_effects.transform.Find("Effects").GetComponent<TMPro.TextMeshProUGUI>().text = new_text;
        }

        int counter = 0;
        foreach (TalentPrototype talent in item_data.GetPrototype().talents_when_consumed)
        {
            GameObject talent_info = GameObject.Instantiate(prefab_talent_data, transform, false);
            talent_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height);
            talent_info.GetComponent<TalentInfo>().talent_data = item_data.talents_when_consumed[counter];
            talent_info.GetComponent<TalentInfo>().check_screen_position = false;
            height += 250;
            ++counter;
        }

        if (item_data.GetPrototype().usable_item != null)
        {
            GameObject usable_info = GameObject.Instantiate(prefab_usable_item_data, transform, false);
            usable_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
            height += 60;

            usable_info.transform.Find("NumberOfUses").GetComponent<CurrentMaxBar>().SetValues(item_data.usable_item_data.number_of_uses, item_data.GetPrototype().usable_item.max_number_of_uses);

            if (item_data.GetPrototype().usable_item.effects_when_used.Count > 0)
            {
                GameObject use_effects = GameObject.Instantiate(prefab_item_effects, transform, false);
                use_effects.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
                height += 25;
                use_effects.transform.Find("EffectSource").GetComponent<TMPro.TextMeshProUGUI>().text = "When Used";
                use_effects.transform.Find("Effects").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                
                (string new_text, int new_height) = AddEffects(item_data.GetPrototype().usable_item.effects_when_used);
                height += new_height;
                use_effects.transform.Find("Effects").GetComponent<TMPro.TextMeshProUGUI>().text += new_text;
            }
        }

        if (item_data.GetEffectsWhenEquipped().Count > 0)
        {

            GameObject equip_effect = GameObject.Instantiate(prefab_item_effects, transform, false);
            equip_effect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height);
            height += 25;
            equip_effect.transform.Find("EffectSource").GetComponent<TMPro.TextMeshProUGUI>().text = "When Equipped";
            equip_effect.transform.Find("Effects").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            
            (string new_text, int new_height) = AddEffects(item_data.GetEffectsWhenEquipped());
            height += new_height;
            equip_effect.transform.Find("Effects").GetComponent<TMPro.TextMeshProUGUI>().text += new_text;
        }

        if (item_data.GetPrototype().weapon != null)
        {
            GameObject weapon_info = GameObject.Instantiate(prefab_weapon_data, transform, false);
            weapon_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
            height += 60;
            WeaponPrototype weapon_data = item_data.GetPrototype().weapon;
            string equip_type_string;
            if (weapon_data.equip_type == WeaponEquipType.ONEHANDED)
                equip_type_string = "1H";
            else
                equip_type_string = "2H";
            weapon_info.transform.Find("Type").GetComponent<TMPro.TextMeshProUGUI>().text = weapon_data.sub_type.ToString() + " (" + equip_type_string + ")";

            weapon_info.transform.Find("AttackTime").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = weapon_data.attack_time.ToString();

            foreach (var damage_per_type in item_data.GetWeaponDamage())
            {
                GameObject damage_info = GameObject.Instantiate(prefab_weapon_damage_type, transform, false);
                damage_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
                height += 25;

                damage_info.GetComponent<TMPro.TextMeshProUGUI>().text = damage_per_type.type.ToString();
                damage_info.transform.Find("Damage").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text =
                    damage_per_type.damage_min + "-" + damage_per_type.damage_max;
                damage_info.transform.Find("ArmorPenetration").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = damage_per_type.armor_penetration.ToString();
            }

            foreach (TalentData attack_talent in item_data.weapon_data.attack_talents)
            {
                GameObject talent_info = GameObject.Instantiate(prefab_talent_data, transform, false);
                talent_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height);
                talent_info.GetComponent<TalentInfo>().talent_data = attack_talent;
                talent_info.GetComponent<TalentInfo>().check_screen_position = false;
                height += 250;
                
            }
        }

        if (item_data.GetPrototype().armor != null)
        {
            GameObject armor_info = GameObject.Instantiate(prefab_armor_data, transform, false);
            armor_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
            height += 85;
            ArmorPrototype prototype_armor = item_data.GetPrototype().armor;

            armor_info.transform.Find("Type").GetComponent<TMPro.TextMeshProUGUI>().text = prototype_armor.sub_type.ToString();
            armor_info.transform.Find("ArmorPhysical").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = 
                item_data.GetArmor(ArmorType.PHYSICAL).ToString();
            armor_info.transform.Find("ArmorElemental").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text =
                item_data.GetArmor(ArmorType.ELEMENTAL).ToString();
            armor_info.transform.Find("ArmorMagical").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text =
                item_data.GetArmor(ArmorType.MAGICAL).ToString();
            armor_info.transform.Find("Durability").GetComponent<CurrentMaxBar>().SetValues(item_data.armor_data.durability_current, item_data.GetMaxDurability());
        }

         if (item_data.GetPrototype().shield != null)
        {
            foreach (TalentData talent in item_data.shield_data.talents)
            {
                GameObject talent_info = GameObject.Instantiate(prefab_talent_data, transform, false);
                talent_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height);
                talent_info.GetComponent<TalentInfo>().talent_data = talent;
                talent_info.GetComponent<TalentInfo>().check_screen_position = false;
                height += 250;       
            }
        }
       
        if (item_data.GetPrototype().required_attributes.strength != 0 
            || item_data.GetPrototype().required_attributes.vitality != 0
            || item_data.GetPrototype().required_attributes.dexterity != 0
            || item_data.GetPrototype().required_attributes.constitution != 0
            || item_data.GetPrototype().required_attributes.intelligence != 0
            || item_data.GetPrototype().required_attributes.willpower != 0)
            {
                GameObject requirement_info = GameObject.Instantiate(prefab_requirements, transform, false);
                requirement_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-height);
                height += 30;

                PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;

                if (item_data.GetPrototype().required_attributes.strength > 0)
                {
                    requirement_info.transform.Find("Strength").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetPrototype().required_attributes.strength.ToString();
                    if(item_data.GetPrototype().required_attributes.strength > player.GetStrength())
                        requirement_info.transform.Find("Strength").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0,0);
                }
                if (item_data.GetPrototype().required_attributes.vitality > 0)
                {
                    requirement_info.transform.Find("Vitality").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetPrototype().required_attributes.vitality.ToString();
                    if(item_data.GetPrototype().required_attributes.vitality > player.GetVitality())
                        requirement_info.transform.Find("Vitality").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0,0);
                }
                if (item_data.GetPrototype().required_attributes.dexterity > 0)
                {
                    requirement_info.transform.Find("Dexterity").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetPrototype().required_attributes.dexterity.ToString();
                    if(item_data.GetPrototype().required_attributes.dexterity > player.GetDexterity())
                        requirement_info.transform.Find("Dexterity").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0,0);
                }
                if (item_data.GetPrototype().required_attributes.constitution> 0)
                {
                    requirement_info.transform.Find("Constitution").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetPrototype().required_attributes.constitution.ToString();
                    if(item_data.GetPrototype().required_attributes.constitution > player.GetConstitution())
                        requirement_info.transform.Find("Constitution").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0,0);
                }
                if (item_data.GetPrototype().required_attributes.intelligence > 0)
                {
                    requirement_info.transform.Find("Intelligence").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetPrototype().required_attributes.intelligence.ToString();
                    if(item_data.GetPrototype().required_attributes.intelligence > player.GetIntelligence())
                        requirement_info.transform.Find("Intelligence").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0,0);
                }
                if (item_data.GetPrototype().required_attributes.willpower > 0)
                {
                    requirement_info.transform.Find("Willpower").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetPrototype().required_attributes.willpower.ToString();
                    if(item_data.GetPrototype().required_attributes.willpower > player.GetWillpower())
                        requirement_info.transform.Find("Willpower").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0,0);
                }
            }


        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x,height + 5);

        start_position.x = Mouse.current.position.x.ReadValue();
        start_position.y = Mouse.current.position.y.ReadValue();
    }

    public (string text, int height) AddEffects(List<EffectData> effect_list)
    {
        int height = 0;
        string text = "";
        foreach (EffectData effect in effect_list)
        {
            height += 25;
            string effect_string;
            if (effect.amount > 0)
                effect_string = "+";
            else
                effect_string = "";

            switch (effect)
            {
                default:
                    effect_string = "";
                    break;

                case EffectAddHitpoints:
                    effect_string += effect.amount + " hitpoints";
                    break;

                case EffectAddStamina:
                    effect_string += effect.amount + " stamina";
                    break;

                case EffectAddMana:
                    effect_string += effect.amount + " mana";
                    break;

                case EffectAddStrength:
                    effect_string += effect.amount + " strength";
                    break;

                case EffectAddDexterity:
                    effect_string += effect.amount + " dexterity";
                    break;

                case EffectAddVitality:
                    effect_string += effect.amount + " vitality";
                    break;

                case EffectAddIntelligence:
                    effect_string += effect.amount + " intelligence";
                    break;

                case EffectAddWillpower:
                    effect_string += effect.amount + " willpower";
                    break;

                case EffectAddConstitution:
                    effect_string += effect.amount + " constitution";
                    break;

                case EffectAddMovementTime:
                    effect_string += effect.amount + " move time";
                    break;

                case EffectAddAttackTime:
                    effect_string += effect.amount + " attack time";
                    break;

                case EffectAddMinWeaponDamage:
                    effect_string += effect.amount + " minimum damage";
                    break;

                case EffectAddMaxWeaponDamage:
                    effect_string += effect.amount + " maximum damage";
                    break;

                case EffectAddWeaponPenetration:
                    effect_string += effect.amount + " armor penetration";
                    break;

                case EffectAddWeaponFireDamage:
                    effect_string += effect.amount + " fire damage";
                    break;

                case EffectAddWeaponIceDamage:
                    effect_string += effect.amount + " ice damage";
                    break;

                case EffectAddWeaponLightningDamage:
                    effect_string += effect.amount + " lightning damage";
                    break;

                case EffectAddArmorPhysical:
                    effect_string += effect.amount + " physical armor";
                    break;

                case EffectAddArmorElemental:
                    effect_string += effect.amount + " elemental armor";
                    break;

                case EffectAddArmorMagical:
                    effect_string += effect.amount + " magical armor";
                    break;

                case EffectAddArmorDurability:
                    effect_string += effect.amount + " durability";
                    break;
                case EffectAddToHit:
                    effect_string += effect.amount + " to hit";
                    break;
                case EffectAddDodge:
                    effect_string += effect.amount + " dodge";
                    break;
                case EffectAddMaxDiseaseResistance:
                    effect_string += effect.amount + " disease resistance";
                    break;
                case EffectAddMaxPoisonResistance:
                    effect_string += effect.amount + " poison resistance";
                    break;
                case EffectAddMaxInsanityResistance:
                    effect_string += effect.amount + " insanity resistance";
                    break;
                case EffectPassiveBlock:
                    effect_string += effect.amount + " % block chance";
                    break;
                case EffectParryChanceBonus:
                    effect_string += effect.amount + " % parry chance";
                    break;
                case EffectBashDamageRelative:
                    effect_string += effect.amount + " % bash damage";
                    break;
            }


            if (effect.execution_time == EffectDataExecutionTime.CONTINUOUS)
                effect_string += " each turn";

            if (effect.duration > 0)
            {
                effect_string += "\nfor " + effect.duration / 100 + " turns";
                height += 25;
            }

            text += effect_string + "\n";
        }
        return (text, height);
    }
  
    public void CheckScreenPosition()
    {
        rect = GetComponent<RectTransform>();

        if (rect.position.x < 0)
        {
            rect.position = new Vector3(0,rect.position.y,rect.position.z);
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
            rect.position = new Vector3(rect.position.x, rect.sizeDelta.y + 10 , rect.position.z);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Mouse.current.position.ReadValue().x - start_position.x) > 100
        || Mathf.Abs(Mouse.current.position.ReadValue().y - start_position.y) > 100)
        {
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(item_data);
        }
    }

    void FixedUpdate()
    {
        CheckScreenPosition();
    }
}
