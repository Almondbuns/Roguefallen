using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum WeaponSubType
{
    SWORD,
    AXE,
    BLUNT,
    SPEAR
}

public enum WeaponEquipType
{
    ONEHANDED,
    TWOHANDED,
}

public class WeaponPrototype
{
    public WeaponSubType sub_type;
    public WeaponEquipType equip_type;

    public int attack_time = 100;

    public List<(DamageType type, int damage_min, int damage_max, int armor_penetration)> damage;

    public List<TalentPrototype> attack_talents;

    public WeaponPrototype()
    {
        attack_talents = new List<TalentPrototype>();
        damage = new();
    }
}

public enum ShieldSubType
{
    LIGHT,
    MEDIUM,
    HEAVY
}

public class ShieldPrototype
{
    public ShieldSubType sub_type;
    public List<TalentPrototype> talents;

    public ShieldPrototype()
    {
        talents = new();
    }
}

public enum ArmorSubType
{
    LIGHT,
    MEDIUM,
    HEAVY

}

public enum ItemType
{
    WEAPON,
    SHIELD,
    ARMOR_HEAD,
    ARMOR_FEET,
    ARMOR_HANDS,
    ARMOR_CHEST,
    AMULET,
    RING,
    CONSUMABLE,
    QUEST_ITEM,
    OTHER
}

public class ArmorPrototype
{
    public ArmorSubType sub_type;
    public int armor_physical;
    public int armor_elemental;
    public int armor_magical;

    public int durability_max;
}

public class RequiredAttributes
{
    public int strength;
    public int vitality;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int willpower;
};

public enum ItemQuality
{
    Normal,
    Magical1,
    Magical2,
    Unique,
    Artefact
}

public enum ItemEffectType
{
    Prefix,
    Suffix
}

public enum ItemEffectTrigger
{
    Equipped
}

public enum ItemEffectTarget
{
    Player,
    Item
}
public class ItemEffectData
{
    public int? exclude_index; //Do not choose two effects of the same exclude index;
    public ItemEffectType type;

    public string name_presuffix;
    public EffectData effect;
    public ItemEffectTrigger trigger;
    public ItemEffectTarget target = ItemEffectTarget.Player;

    internal void Save(BinaryWriter save)
    {
        save.Write(exclude_index.HasValue);
        if (exclude_index.HasValue)
            save.Write(exclude_index.Value);

        save.Write((int)type);
        save.Write(name_presuffix);
        save.Write(effect.GetType().Name);
        effect.Save(save);
        save.Write((int) trigger);
    }

    internal void Load(BinaryReader save)
    {
        bool b = save.ReadBoolean();
        if (b == true)
            exclude_index = save.ReadInt32();

        type = (ItemEffectType)save.ReadInt32();
        name_presuffix = save.ReadString();
        Type e_type = Type.GetType(save.ReadString());
        effect = (EffectData) Activator.CreateInstance(e_type);
        effect.Load(save);
        trigger = (ItemEffectTrigger)save.ReadInt32();
    }

}

public class UsableItemPrototype
{
    public int max_number_of_uses;
    public List<EffectData> effects_when_used;
    public List<TalentPrototype> talents_when_used;

    public UsableItemPrototype()
    {
        talents_when_used = new List<TalentPrototype>();
        effects_when_used = new List<EffectData>();
    }
}

public class ItemPrototype
{
    public string name;
    public string icon;
    public ItemType type;

    public int weight;
    public int gold_value = 10;

    public int level = 0; // internal leveling system (always in difficulty level)
    public int tier = 0; // external level system (may vary depending on item type)

    public bool is_stackable = false;
    public int stack_max = 1;

    public int start_amount_min = 1;
    public int start_amount_max = 1;

    public List<EffectData> effects_when_consumed;
    public List<TalentPrototype> talents_when_consumed;
    
    public List<EffectData> effects_when_equipped;

    public WeaponPrototype weapon;

    public ShieldPrototype shield;
    public ArmorPrototype armor;
    public UsableItemPrototype usable_item;

    public RequiredAttributes required_attributes;

    public List<ItemQuality> qualities_possible = new() { ItemQuality.Normal };
    public List<ItemEffectData> quality_effects_possible = new();

    public ItemPrototype(int level)
    {
        this.level = level;

        effects_when_consumed = new List<EffectData>();
        talents_when_consumed = new();
        effects_when_equipped = new List<EffectData>();
        required_attributes = new RequiredAttributes();
    }
}