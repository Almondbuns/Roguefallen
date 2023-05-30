using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public PlayerData player_data = null;
    public PlayerData loaded_player_data = null;

    public GameObject active_effect_prefab;
    public GameObject resistance_prefab;

    public void Refresh()
    {
        if (player_data == null)
            return;

        //HP
        transform.Find("HPBar").GetComponent<CurrentMaxBar>().SetValues(player_data.health_current, player_data.GetHealthMax());
        transform.Find("StaminaBar").GetComponent<CurrentMaxBar>().SetValues(player_data.stamina_current, player_data.GetStaminaMax());
        transform.Find("ManaBar").GetComponent<CurrentMaxBar>().SetValues(player_data.mana_current, player_data.GetManaMax());
        if (player_data.player_stats.level <= player_data.experience_levels.Count)
            transform.Find("ExperienceBar").GetComponent<CurrentMaxBar>().SetValues(player_data.player_stats.experience, player_data.experience_levels[player_data.player_stats.level - 1]);

        transform.Find("ArmorChest").GetComponent<CurrentMaxBar>().SetValues(player_data.GetDurability("chest"), player_data.GetMaxDurability("chest"));
        transform.Find("ArmorHead").GetComponent<CurrentMaxBar>().SetValues(player_data.GetDurability("head"), player_data.GetMaxDurability("head"));
        transform.Find("ArmorHands").GetComponent<CurrentMaxBar>().SetValues(player_data.GetDurability("hands"), player_data.GetMaxDurability("hands"));
        transform.Find("ArmorFeet").GetComponent<CurrentMaxBar>().SetValues(player_data.GetDurability("feet"), player_data.GetMaxDurability("feet"));

        transform.Find("MovementTime").Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetMovementTime().ToString();

        foreach (Transform transform in transform.Find("EffectsPanel"))
            Destroy(transform.gameObject);

        int effect_display_counter = 0;
        int vertical_position = -10;

        foreach(DiseaseData disease in player_data.current_diseases)
        {
            foreach (EffectData effect in disease.GetCurrentEffects())
            {
                GameObject go = GameObject.Instantiate(active_effect_prefab, transform.Find("EffectsPanel"), false);
                go.GetComponent<RectTransform>().localPosition = new Vector3(60 * effect_display_counter, vertical_position, 0);
                if (disease.prototype.icon != null)
                {
                    Texture2D texture = Resources.Load<Texture2D>(disease.prototype.icon);
                    go.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                go.GetComponent<Tooltip>().text = disease.prototype.name + "<br>" + effect.name + ": " + effect.amount;
                go.transform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                ++effect_display_counter;
            }
        }

        foreach (PoisonData poison in player_data.current_poisons)
        {
            foreach (EffectData effect in poison.GetCurrentEffects())
            {
                GameObject go = GameObject.Instantiate(active_effect_prefab, transform.Find("EffectsPanel"), false);
                go.GetComponent<RectTransform>().localPosition = new Vector3(60 * effect_display_counter, vertical_position, 0);
                if (poison.prototype.icon != null)
                {
                    Texture2D texture = Resources.Load<Texture2D>(poison.prototype.icon);
                    go.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                go.GetComponent<Tooltip>().text = poison.prototype.name + "<br>" + effect.name + ": " + effect.amount;
                go.transform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text = (poison.prototype.max_duration - poison.duration).ToString();
                ++effect_display_counter;
            }
        }

        List<TalentData> unlocked_talents = player_data.player_stats.skill_tree.GetUnlockedTalents();
        foreach (long talent_id in player_data.current_substained_talents_id)
        {
            foreach (TalentData talent in unlocked_talents.FindAll(x => x.id == talent_id))
            {
                if (talent.prototype is TalentSubstainedEffects)
                {
                    foreach (EffectData effect in ((TalentSubstainedEffects)(talent.prototype)).substained_effects)
                    {
                        GameObject go = GameObject.Instantiate(active_effect_prefab, transform.Find("EffectsPanel"), false);
                        go.GetComponent<RectTransform>().localPosition = new Vector3(60 * effect_display_counter, vertical_position, 0);
                        if (effect.icon != null && effect.icon != "")
                        {
                            Texture2D texture = Resources.Load<Texture2D>(effect.icon);
                            go.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        }
                        go.GetComponent<Tooltip>().text = talent.prototype.name + "<br>" + effect.name + ": " + effect.amount;
                        go.transform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text = "SUB";
                        ++effect_display_counter;
                    }
                }
            }
        }
       
        foreach (ActorEffectData effect in player_data.current_effects)
        {
            GameObject go = GameObject.Instantiate(active_effect_prefab, transform.Find("EffectsPanel"), false);
            go.GetComponent<RectTransform>().localPosition = new Vector3(60 * effect_display_counter, vertical_position, 0);
            
            Texture2D texture = Resources.Load<Texture2D>(effect.effect.icon);
            if (effect.effect.icon != null && effect.effect.icon != "")
                go.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            go.GetComponent<Tooltip>().text = effect.effect.name;
            
            if (effect.effect.amount != 0) 
                go.GetComponent<Tooltip>().text += ": " + effect.effect.amount;

            go.transform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text = (effect.effect.duration - effect.current_tick).ToString();
            ++effect_display_counter;
        }

        if (effect_display_counter > 0)
            vertical_position -= 80;

        foreach (var resistance in player_data.meter_resistances.resistances)
        {
            if (resistance.Value > 0)
            {
                GameObject go = GameObject.Instantiate(resistance_prefab, transform.Find("EffectsPanel"), false);
                go.GetComponent<RectTransform>().localPosition = new Vector3(0, vertical_position - 60, 0);
                
                if (resistance.Key == DamageType.DISEASE)
                {
                    go.transform.Find("Bar").GetComponent<CurrentMaxBar>().SetValues(resistance.Value, player_data.GetMaxDiseaseResistance());
                    Texture2D texture = Resources.Load<Texture2D>("images/effects/effect_disease");
                    go.transform.Find("Type").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                else if (resistance.Key == DamageType.POISON)
                {
                    go.transform.Find("Bar").GetComponent<CurrentMaxBar>().SetValues(resistance.Value, player_data.GetMaxPoisonResistance());
                    Texture2D texture = Resources.Load<Texture2D>("images/effects/effect_poison");
                    go.transform.Find("Type").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }   
                else if (resistance.Key == DamageType.INSANITY)
                {
                    go.transform.Find("Bar").GetComponent<CurrentMaxBar>().SetValues(resistance.Value, player_data.GetMaxInsanityResistance());
                }
                vertical_position -= 60;
            }
        }
    }
}
