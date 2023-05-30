using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class WeaponData
{
    public List<TalentData> attack_talents;
    

    public WeaponData(WeaponPrototype prototype)
    {
        attack_talents = new();

        if (prototype != null)
        {
            foreach (TalentPrototype talent in prototype.attack_talents)
            {
                attack_talents.Add(new TalentData(talent));
            }
        }
    }

    internal void Save(BinaryWriter save)
    {
        save.Write(attack_talents.Count);
        foreach (TalentData v in attack_talents)
            v.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        int size = save.ReadInt32();
        for (int i = 0; i < size; ++ i)
        {
            //TalentData v = new(null);
            attack_talents[i].Load(save);
            //attack_talents.Add(v);
        }
    }
}

public class ArmorData
{
    public int durability_current;

    internal void Save(BinaryWriter save)
    {
        save.Write(durability_current);
    }

    internal void Load(BinaryReader save)
    {
        durability_current = save.ReadInt32();
    }

    public ArmorData(ArmorPrototype prototype = null)
    {        
    }
}

public class UsableItemData
{
    public int number_of_uses;

    internal void Save(BinaryWriter save)
    {
        save.Write(number_of_uses);
    }

    internal void Load(BinaryReader save)
    {
        number_of_uses = save.ReadInt32();
    }
}

public class ItemData
{
    public static long id_counter = 0;
    public long id;

    public bool is_quest_item = false;
    public long quest_id = -1;

    ItemPrototype prototype;

    string name;

    public ItemQuality quality = ItemQuality.Normal;
    public List<ItemEffectData> quality_effects = new();

    public int x = -1;
    public int y = -1;

    public int amount = 1;

    public WeaponData weapon_data;
    public ArmorData armor_data;

    public UsableItemData usable_item_data;

    public List<TalentData> talents_when_consumed;
 
    public delegate void VoidHandler();
    public event VoidHandler HandleRemove;

    internal void Save(BinaryWriter save)
    {
        save.Write(id);
        save.Write(is_quest_item);
        save.Write(quest_id);

        save.Write(name);

        save.Write((int)quality);
        save.Write(quality_effects.Count);
        foreach (var v in quality_effects)
            v.Save(save);

        save.Write(prototype.GetType().Name);
        save.Write(prototype.level);

        save.Write(x);
        save.Write(y);

        save.Write(amount);

        if (weapon_data != null)
        {
            save.Write(true);
            weapon_data.Save(save);
        }
        else
            save.Write(false);

        if (armor_data != null)
        {
            save.Write(true);
            armor_data.Save(save);
        }
        else
            save.Write(false);

        if (usable_item_data != null)
        {
            save.Write(true);
            usable_item_data.Save(save);
        }
        else
            save.Write(false);

        save.Write(talents_when_consumed.Count);
        foreach (TalentData v in talents_when_consumed)
            v.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        id = save.ReadInt64();
        is_quest_item = save.ReadBoolean();
        quest_id = save.ReadInt64();

        name = save.ReadString();

        quality = (ItemQuality) save.ReadInt32();
        int size = save.ReadInt32();
        quality_effects = new(size);
        for (int i = 0; i < size; ++i)
        {
            ItemEffectData v = new();
            v.Load(save);
            quality_effects.Add(v);
        }

        Type a_type = Type.GetType(save.ReadString());

        int a_level = save.ReadInt32();
        prototype = (ItemPrototype)Activator.CreateInstance(a_type, a_level);

        x = save.ReadInt32();
        y = save.ReadInt32();

        amount = save.ReadInt32();

        bool b = save.ReadBoolean();

        int counter = 0;
        if (b == true)
        {
            weapon_data = new(prototype.weapon);
            weapon_data.Load(save);
            ++counter;
        }

        b = save.ReadBoolean();
        if (b == true)
        {
            armor_data = new();
            armor_data.Load(save);
        }

        b = save.ReadBoolean();
        if (b == true)
        {
            usable_item_data = new();
            usable_item_data.Load(save);
        }

        size = save.ReadInt32();
        counter = 0;
        for (int i = 0; i < size; ++i)
        {
            TalentData v = new(prototype.talents_when_consumed[counter]);
            v.Load(save);
            talents_when_consumed.Add(v);
            ++counter;
        }
    }

    public ItemData(ItemPrototype prototype, int x = -1, int y = -1)
    {
        id = id_counter;
        ++id_counter;

        this.prototype = prototype;
        this.x = x;
        this.y = y;

        talents_when_consumed = new();

        if (prototype != null)
        {
            name = prototype.name;

            float multiplier = 1;
            if (GameObject.Find("GameData").GetComponent<GameData>().player_data != null)
                multiplier = (100 + GameObject.Find("GameData").GetComponent<GameData>().player_data.GetCurrentAdditiveEffectAmount<EffectAddThrowingWeaponAmountRelative>()) / 100f;
            amount = (int) (UnityEngine.Random.Range(prototype.start_amount_min, prototype.start_amount_max + 1) * multiplier);
            

            if (prototype.weapon != null)
                weapon_data = new WeaponData(prototype.weapon);

            if (prototype.armor != null)
            {
                armor_data = new ArmorData(prototype.armor);
                armor_data.durability_current = GetMaxDurability();
            }

            if (prototype.usable_item != null)
            {
                usable_item_data = new UsableItemData();
                usable_item_data.number_of_uses = prototype.usable_item.max_number_of_uses;
            }

            foreach (TalentPrototype talent in prototype.talents_when_consumed)
            {
                talents_when_consumed.Add(new TalentData(talent));
            }

            if (prototype.qualities_possible.Contains(ItemQuality.Normal) == false)
            {
                if (prototype.qualities_possible.Count > 0)
                    SetQuality(prototype.qualities_possible[0]);
            }
        }
    }

    internal void OnRemoveFromMap()
    {
        HandleRemove?.Invoke();
    }

    public static ItemData GetRandomItem(int x, int y, int level)
    {
        List<Type> item_types = new List<Type>
        {
            //typeof(ItemSword1H),
            //typeof(ItemAxe1H),
            typeof(ItemHammer1H),
            typeof(ItemMace1H),
            typeof(ItemFlail1H),
            typeof(ItemBootsHeavy),
            //typeof(ItemBootsMedium),
            //typeof(ItemBootsLight),
            typeof(ItemChestHeavy),
            //typeof(ItemChestMedium),
            //typeof(ItemChestLight),
            typeof(ItemHandsHeavy),
            //typeof(ItemHandsMedium),
            //typeof(ItemHandsLight),
            //typeof(ItemSword2H),
            //typeof(ItemAxe2H),
            typeof(ItemHammer2H),
            //typeof(ItemSpear1H),
            //typeof(ItemSpear2H),
            typeof(ItemHeadHeavy),
            //typeof(ItemHeadMedium),
            //typeof(ItemHeadLight),
            typeof(ItemMeatHorn),
            typeof(ItemHealthPotion),
            typeof(ItemFirebomb),
            typeof(ItemStaminaPotion),
            typeof(ItemManaPotion),
            typeof(ItemAlmondBun),
            typeof(ItemBlueberries),
            typeof(ItemThrowingKnife),
            typeof(ItemAcidFlask),
            typeof(ItemPoemOfReturn),
            typeof(ItemPoemOfAWalk),
            typeof(ItemPoemOfAJourney),
            //typeof(ItemFluteOfHealing),
        };

        int rand = UnityEngine.Random.Range(0, item_types.Count);
        return new ItemData((ItemPrototype) Activator.CreateInstance(item_types[rand], level), x , y);
    }

    internal int GetGoldValue()
    {
        int value = amount * prototype.gold_value;
        switch (quality)
        {
            case ItemQuality.Magical1:
                value *= 3;
                break;
            case ItemQuality.Magical2:
                value *= 6;
                break;
            case ItemQuality.Unique:
                value *= 20;
                break;
            case ItemQuality.Artefact:
                value *= 100;
                break;
        }
        return value;
    }

    internal List<EffectData> GetEffectsWhenEquipped()
    {
        List<EffectData> effects = new();
        foreach(ItemEffectData ied in quality_effects)
        {
            if (ied.trigger == ItemEffectTrigger.Equipped)
                effects.Add(ied.effect);
        }
        foreach (EffectData effect in prototype.effects_when_equipped)
            effects.Add(effect);

        return effects;
    }

    public string GetIcon()
    {
        return prototype.icon;
    }

    internal string GetName()
    {
        return name;
    }

    internal int GetLevel()
    {
        return prototype.level;
    }

    internal int GetTier()
    {
        return prototype.tier;
    }

    internal object GetItemType()
    {
        return prototype.type;
    }

    internal int GetWeight()
    {
        return amount * prototype.weight;
    }

    internal List<EffectData> GetEffectsWhenConsumed()
    {
        return prototype.effects_when_consumed;
    }

    internal ItemPrototype GetPrototype()
    {
        return prototype;
    }

    internal void SetQuality(ItemQuality quality)
    {
        if (quality == ItemQuality.Magical1)
        {
            if (prototype.qualities_possible.Exists(x => x == ItemQuality.Magical1) == false)
                return;

            if (prototype.quality_effects_possible.Count == 0) return;

            quality_effects.Clear();
            name = prototype.name;

            int index = UnityEngine.Random.Range(0, prototype.quality_effects_possible.Count);

            ItemEffectData effect = prototype.quality_effects_possible[index];
            quality_effects.Add(effect);
            if (effect.type == ItemEffectType.Prefix)
                name = effect.name_presuffix + " " + name;
            else
                name = name + " " + effect.name_presuffix;
        }

        if (quality == ItemQuality.Magical2)
        {
            if (prototype.qualities_possible.Exists(x => x == ItemQuality.Magical2) == false)
                return;

            var prefix_list = prototype.quality_effects_possible.FindAll(x => x.type == ItemEffectType.Prefix);
            var suffix_list = prototype.quality_effects_possible.FindAll(x => x.type == ItemEffectType.Suffix);

            if (prefix_list.Count == 0 || suffix_list.Count == 0) return;

            quality_effects.Clear();
            name = prototype.name;

            int index = UnityEngine.Random.Range(0, prefix_list.Count);
            ItemEffectData effect = prefix_list[index];
            quality_effects.Add(effect);
            int? exlude_index = effect.exclude_index;
            name = effect.name_presuffix + " " + name;

            if (exlude_index.HasValue)
                suffix_list.RemoveAll(x => x.exclude_index == exlude_index);
            if (suffix_list.Count == 0) return;

            index = UnityEngine.Random.Range(0, suffix_list.Count);
            effect = suffix_list[index];
            quality_effects.Add(effect);
            name = name + " " + effect.name_presuffix;
        }

        if (quality == ItemQuality.Unique)
        {
            if (prototype.qualities_possible.Exists(x => x == ItemQuality.Unique) == false)
                return;

            var list = new List<ItemEffectData> (prototype.quality_effects_possible);

            if (list.Count < 4) return;
            quality_effects.Clear();

            name = "The " + prototype.name + " \"" + ItemData.GetRandomItemName(this) + "\"";

            for (int i = 0; i < 4; ++i)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                ItemEffectData effect = list[index];
                quality_effects.Add(effect);
                list.Remove(effect);
                int? exlude_index = effect.exclude_index;
                if (exlude_index.HasValue)
                    list.RemoveAll(x => x.exclude_index == exlude_index);
                if (list.Count == 0) return;
            }            
        }

        this.quality = quality;
        if (armor_data != null) armor_data.durability_current = GetMaxDurability();
    }

    internal int GetWeaponAttackTime()
    {
        int attack_time = 100;
        if (prototype.weapon == null)
            return attack_time;

        attack_time = prototype.weapon.attack_time;

        foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddAttackTime))
        {
            attack_time += (int)v.effect.amount;
        }

        return attack_time;
    }

    internal List<(DamageType type, int damage_min, int damage_max, int armor_penetration)> GetWeaponDamage()
    {
        List<(DamageType type, int damage_min, int damage_max, int penetration)> damage = new();
        if (prototype.weapon == null)
            return damage;
        
        foreach(var prototype_damage in prototype.weapon.damage)
        {
            DamageType type = prototype_damage.type;
            int damage_min = prototype_damage.damage_min;
            foreach(var v in quality_effects.FindAll(x => x.effect is EffectAddMinWeaponDamage))
            {
                damage_min += (int) v.effect.amount;
            }

            int damage_max = prototype_damage.damage_max;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddMaxWeaponDamage))
            {
                damage_max += (int)v.effect.amount;
            }

            if (prototype_damage.type == DamageType.FIRE)
            {
                foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponFireDamage))
                {
                    damage_min += (int)v.effect.amount;
                    damage_max += (int)v.effect.amount;
                }
            }

            if (prototype_damage.type == DamageType.ICE)
            {
                foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponIceDamage))
                {
                    damage_min += (int)v.effect.amount;
                    damage_max += (int)v.effect.amount;
                }
            }

            if (prototype_damage.type == DamageType.LIGHTNING)
            {
                foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponLightningDamage))
                {
                    damage_min += (int)v.effect.amount;
                    damage_max += (int)v.effect.amount;
                }
            }

            int penetration = prototype_damage.armor_penetration;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponPenetration))
            {
                penetration += (int)v.effect.amount;
            }

            damage.Add((type, damage_min, damage_max, penetration));
        }

        if (damage.Exists(x => x.type == DamageType.FIRE) == false && quality_effects.Exists(x => x.effect is EffectAddWeaponFireDamage) == true)
        {
            int amount = 0;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponFireDamage))
            {
                amount += (int)v.effect.amount;                
            }
            damage.Add((DamageType.FIRE, amount, amount, 0));
        }

        if (damage.Exists(x => x.type == DamageType.ICE) == false && quality_effects.Exists(x => x.effect is EffectAddWeaponIceDamage) == true)
        {
            int amount = 0;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponIceDamage))
            {
                amount += (int)v.effect.amount;
            }
            damage.Add((DamageType.ICE, amount, amount, 0));
        }

        if (damage.Exists(x => x.type == DamageType.LIGHTNING) == false && quality_effects.Exists(x => x.effect is EffectAddWeaponLightningDamage) == true)
        {
            int amount = 0;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddWeaponLightningDamage))
            {
                amount += (int)v.effect.amount;
            }
            damage.Add((DamageType.LIGHTNING, amount, amount, 0));
        }

        return damage;
    }

    internal int GetArmor(ArmorType type)
    {
        if (type == ArmorType.PHYSICAL)
        {
            int amount = prototype.armor.armor_physical;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddArmorPhysical))
            {
                amount += (int)v.effect.amount;
            }
            return amount;
        }

        if (type == ArmorType.ELEMENTAL)
        {
            int amount = prototype.armor.armor_elemental;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddArmorElemental))
            {
                amount += (int)v.effect.amount;
            }
            return amount;
        }

        if (type == ArmorType.MAGICAL)
        {
            int amount = prototype.armor.armor_magical;
            foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddArmorMagical))
            {
                amount += (int)v.effect.amount;
            }
            return amount;
        }

        return 0;
    }

    internal int GetMaxDurability()
    {
        int amount = prototype.armor.durability_max;
        foreach (var v in quality_effects.FindAll(x => x.effect is EffectAddArmorDurability))
        {
            amount += (int)v.effect.amount;
        }
        return amount;      
    }

    public void Regenerate(int relative_armor_durability_effect_amount = 0)
    {
        if (armor_data != null)
        {
            armor_data.durability_current = GetMaxDurability();
            if (relative_armor_durability_effect_amount > 0)
            armor_data.durability_current = (int) (armor_data.durability_current * ((100 + relative_armor_durability_effect_amount) / 100.0f));
        }

        if (usable_item_data != null)
        {
            usable_item_data.number_of_uses = prototype.usable_item.max_number_of_uses;
        }
    }

    public bool HasPlayerRequiredAttributes()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        if (prototype.required_attributes.strength > player.GetStrength())
            return false;

        if (prototype.required_attributes.vitality > player.GetVitality())
            return false;

        if (prototype.required_attributes.dexterity > player.GetDexterity())
            return false;

        if (prototype.required_attributes.constitution > player.GetConstitution())
            return false;

        if (prototype.required_attributes.intelligence > player.GetIntelligence())
            return false;

        if (prototype.required_attributes.willpower > player.GetWillpower())
            return false;

        return true;        
    }

    static string GetRandomItemName(ItemData item)
    {
        string name = "";
        List<string> syllables = new List<string>
        {
            "a",
            "yo",
            "gur",
            "da",
            "mow",
            "rekt",
            "con",
            "nee",
            "uru",
            "dash",
            "fam",
            "tic",
            "pon",
            "leen",
            "ari",
            "oni",
            "ezri",
            "int",
            "al",
            "mond",
            "bun"
        };

        int number_of_syllables = UnityEngine.Random.Range(2,5);

        for (int i = 0; i < number_of_syllables; ++i)
        {
            name += syllables[UnityEngine.Random.Range(0,syllables.Count)];
        }

        name = name.ToUpper()[0] + name.Substring(1);
        return name;

    }
}

