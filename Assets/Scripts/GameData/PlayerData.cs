using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
//using Newtonsoft.Json.Linq;

public enum PlayerTalentSourceType
{
    None,
    Item,
    Skill,
}

public struct PlayerTalentSource
{
    public PlayerTalentSourceType type;
    public ItemData item;
}

public class PlayerData : ActorData
{
    public PlayerStatsData player_stats;
    public int gold_amount = 100;
    
    //public InventoryData inventory;

    public List<EquipmentSlotData> equipment;
    public List<int> experience_levels;

    public List<(PlayerTalentSource source, TalentData talent)> usable_talents; // do not save, will be regenerated on every turn (references)
    public List<QuestData> active_quests;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        player_stats.Save(save);
        save.Write(gold_amount);

        save.Write(equipment.Count);
        foreach (var v in equipment)
            v.Save(save);
        
        save.Write(experience_levels.Count);
        foreach (int v in experience_levels)
            save.Write(v);

        //save.Write(active_quests.Count);
        //foreach (QuestData v in active_quests)
        //    v.Save(save);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        player_stats.Load(save);
        gold_amount = save.ReadInt32();

        int size = save.ReadInt32();
        equipment = new(size);
        for(int i = 0; i < size; ++i)
        {
            EquipmentSlotData v = new();
            v.Load(save);
            equipment.Add(v);
        }

        size = save.ReadInt32();
        experience_levels = new(size);
        for (int i = 0; i < size; ++i)
        {
            experience_levels.Add(save.ReadInt32());
        }

        /*size = save.ReadInt32();
        active_quests = new(size);
        for (int i = 0; i < size; ++i)
        {
            QuestData v = new();
            v.Load(save);
            active_quests.Add(v);
        }*/
    }

    public PlayerData(int starting_level) : base(20,20, null)
    {
        prototype = new PlayerPrototype(starting_level);        

        player_stats = new PlayerStatsData(starting_level);
        inventory = new InventoryData(15);
        equipment = new List<EquipmentSlotData>();
        usable_talents = new();
        active_quests = new();
     
        health_current = GetHealthMax();
        stamina_current = GetStaminaMax();
        mana_current = GetManaMax();


        foreach(ArmorStats armor_stats in prototype.stats.body_armor)
            body_armor.Add(new ArmorStatsData(armor_stats));

        foreach (DamageType type in prototype.stats.meter_resistances.resistances.Keys)
        {
            meter_resistances.resistances[type] = 0;
        }

        prototype.stats.movement_time = 50;

        inventory.AddItem(new ItemData(new ItemPoemOfReturn(starting_level)));
        inventory.AddItem(new ItemData(new ItemFluteOfHealing(starting_level)));
        
        if (starting_level > 1)
        {
            inventory.AddItem(new ItemData(new ItemFluteOfHealing(starting_level)));
            inventory.AddItem(new ItemData(new ItemFirebomb(starting_level)));
            inventory.AddItem(new ItemData(new ItemThrowingKnife(starting_level)));
            inventory.AddItem(new ItemData(new ItemAcidFlask(starting_level)));
            inventory.AddItem(new ItemData(new ItemStaminaPotion(starting_level)));
            inventory.AddItem(new ItemData(new ItemMeatHorn(starting_level)));
        }
        
        equipment.Add(new EquipmentSlotData {name = "Head", item_type = new List<ItemType> {ItemType.ARMOR_HEAD} });
        equipment.Add(new EquipmentSlotData { name = "Chest", item_type = new List<ItemType> { ItemType.ARMOR_CHEST } });
        equipment.Add(new EquipmentSlotData { name = "Hands", item_type = new List<ItemType> { ItemType.ARMOR_HANDS } });
        equipment.Add(new EquipmentSlotData { name = "Feet", item_type = new List<ItemType> { ItemType.ARMOR_FEET } });
        equipment.Add(new EquipmentSlotData { name = "Neck", item_type = new List<ItemType> { ItemType.AMULET } });
        equipment.Add(new EquipmentSlotData { name = "Finger 1", item_type = new List<ItemType> { ItemType.RING } });
        equipment.Add(new EquipmentSlotData { name = "Finger 2", item_type = new List<ItemType> { ItemType.RING } });
        equipment.Add(new EquipmentSlotData { name = "Weapon", item_type = new List<ItemType> { ItemType.WEAPON} });
         equipment.Add(new EquipmentSlotData { name = "Shield", item_type = new List<ItemType> { ItemType.SHIELD } });
        //equipment.Add(new EquipmentSlotData { name = "Weapon 2L", item_type = new List<ItemType> { ItemType.WEAPON, ItemType.SHIELD } });
        //equipment.Add(new EquipmentSlotData { name = "Weapon 2R", item_type = new List<ItemType> { ItemType.WEAPON, ItemType.SHIELD } });

        equipment.Find(x => x.name == "Chest").item = new ItemData(new ItemChestHeavy(starting_level));
        equipment.Find(x => x.name == "Weapon").item = new ItemData(new ItemMace1H(starting_level));

        if (starting_level > 1)
        {
            equipment.Find(x => x.name == "Hands").item = new ItemData(new ItemHandsHeavy(starting_level));
            equipment.Find(x => x.name == "Feet").item = new ItemData(new ItemBootsHeavy(starting_level));
            equipment.Find(x => x.name == "Head").item = new ItemData(new ItemHeadHeavy(starting_level));

            equipment.Find(x => x.name == "Finger 1").item = new ItemData(new ItemRing(starting_level));
            equipment.Find(x => x.name == "Finger 2").item = new ItemData(new ItemRing(starting_level));
            equipment.Find(x => x.name == "Neck").item = new ItemData(new ItemAmulet(starting_level));

            equipment.Find(x => x.name == "Shield").item = new ItemData(new ItemShieldHeavy(starting_level));

            //Since we do not know which quality levels are allowed at a level just try to increase
            equipment.Find(x => x.name == "Chest").item.SetQuality(ItemQuality.Magical1);
            equipment.Find(x => x.name == "Hands").item.SetQuality(ItemQuality.Magical1); 
            equipment.Find(x => x.name == "Feet").item.SetQuality(ItemQuality.Magical1); 
            equipment.Find(x => x.name == "Head").item.SetQuality(ItemQuality.Magical1); 
            equipment.Find(x => x.name == "Weapon").item.SetQuality(ItemQuality.Magical1);  
            equipment.Find(x => x.name == "Finger 1").item.SetQuality(ItemQuality.Magical1); 
            equipment.Find(x => x.name == "Finger 2").item.SetQuality(ItemQuality.Magical1); 
            equipment.Find(x => x.name == "Neck").item.SetQuality(ItemQuality.Magical1); 
            equipment.Find(x => x.name == "Shield").item.SetQuality(ItemQuality.Magical1); 

            equipment.Find(x => x.name == "Chest").item.SetQuality(ItemQuality.Magical2);
            equipment.Find(x => x.name == "Hands").item.SetQuality(ItemQuality.Magical2); 
            equipment.Find(x => x.name == "Feet").item.SetQuality(ItemQuality.Magical2); 
            equipment.Find(x => x.name == "Head").item.SetQuality(ItemQuality.Magical2); 
            equipment.Find(x => x.name == "Weapon").item.SetQuality(ItemQuality.Magical2);
            equipment.Find(x => x.name == "Finger 1").item.SetQuality(ItemQuality.Magical2); 
            equipment.Find(x => x.name == "Finger 2").item.SetQuality(ItemQuality.Magical2); 
            equipment.Find(x => x.name == "Neck").item.SetQuality(ItemQuality.Magical2); 
            equipment.Find(x => x.name == "Shield").item.SetQuality(ItemQuality.Magical2); 
            
            equipment.Find(x => x.name == "Chest").item.SetQuality(ItemQuality.Unique);
            equipment.Find(x => x.name == "Hands").item.SetQuality(ItemQuality.Unique); 
            equipment.Find(x => x.name == "Feet").item.SetQuality(ItemQuality.Unique); 
            equipment.Find(x => x.name == "Head").item.SetQuality(ItemQuality.Unique); 
            equipment.Find(x => x.name == "Weapon").item.SetQuality(ItemQuality.Unique);
            equipment.Find(x => x.name == "Finger 1").item.SetQuality(ItemQuality.Unique); 
            equipment.Find(x => x.name == "Finger 2").item.SetQuality(ItemQuality.Unique); 
            equipment.Find(x => x.name == "Neck").item.SetQuality(ItemQuality.Unique);   
            equipment.Find(x => x.name == "Shield").item.SetQuality(ItemQuality.Unique); 
        }

        experience_levels = new List<int>
        {
            100,
            300,
            600,
            1000,
            1500,
            2100,
            2800,
            3600,
            4500,
            5500,
            6600
        };

        if (starting_level > 1)
            player_stats.experience = experience_levels[starting_level - 2];

    }

    internal ItemData GetMainWeapon()
    {
        ItemData main_weapon = null;
        
        foreach(EquipmentSlotData slot in equipment)
        {
            if (slot.name == "Weapon")
                return slot.item;
        }

        return main_weapon;
    }

    public void AddQuest(QuestData quest_data)
    {
        active_quests.Add(quest_data);
        GameLogger.Log("The Player accepted the quest: " + quest_data.name + ".");
    }

    public override int GetAttackTime()
    {
        int value = prototype.stats.attack_time;

        value += GetCurrentAdditiveEffectAmount<EffectAddAttackTime>();

        return value;
    }

    public int GetStrength()
    {
        int value = player_stats.strength;

        value += GetCurrentAdditiveEffectAmount<EffectAddStrength>();

        return value;
    }

    public override int GetCurrentAdditiveEffectAmount<T>()
    {
        int value = 0;

        value += base.GetCurrentAdditiveEffectAmount<T>();

        foreach (EquipmentSlotData slot in equipment)
        {
            if (slot.item != null)
            {
                foreach (EffectData effect in slot.item.GetPrototype().effects_when_equipped)
                {
                    if (effect is T)
                        value += (int)effect.amount;
                }

                foreach (ItemEffectData effect in slot.item.quality_effects)
                {
                    if (effect.trigger == ItemEffectTrigger.Equipped && effect.target == ItemEffectTarget.Player && effect.effect is T)
                        value += (int)effect.effect.amount;
                }
            }
        }

        List<TalentData> unlocked_talents = player_stats.skill_tree.GetUnlockedTalents();

        foreach (long talent_id in current_substained_talents_id)
        {
            foreach (TalentData talent in unlocked_talents.FindAll(x => x.id == talent_id))
            {
                if (talent.prototype is TalentSubstainedEffects)
                {
                    foreach (EffectData effect in ((TalentSubstainedEffects)(talent.prototype)).substained_effects)
                    {
                        if (effect is T)
                            value += (int)effect.amount;
                    }
                }
            }
        }

        foreach (long talent_id in current_passive_talents_id)
        {
            foreach (TalentData talent in unlocked_talents.FindAll(x => x.id == talent_id))
            {
                if (talent.prototype is TalentPassiveEffects)
                {
                    foreach (EffectData effect in ((TalentPassiveEffects)(talent.prototype)).passive_effects)
                    {
                        if (effect is T)
                            value += (int)effect.amount;
                    }
                }
            }
        }

        return value;
    }

    public int GetVitality()
    {
        int value = player_stats.vitality;

        value += GetCurrentAdditiveEffectAmount<EffectAddVitality>();

        return value;
    }

    internal void GainExperience(int kill_experience)
    {
        player_stats.experience += kill_experience;
        base.OnExperienceGain(kill_experience);

        if (player_stats.level - 1 >= experience_levels.Count)
            return;

        if (player_stats.experience >= experience_levels[player_stats.level - 1])
            LevelUp();
    }

    internal void LevelUp()
    {
        player_stats.level += 1;
        player_stats.attribute_points += 5;
        player_stats.skill_expertise_points += 1;
        player_stats.talent_points += 1;

        base.OnLevelUp();

        health_current = GetHealthMax();
        stamina_current = GetStaminaMax();
        mana_current = GetManaMax();
    }

    public int GetDexterity()
    {
        int value = player_stats.dexterity;

        value += GetCurrentAdditiveEffectAmount<EffectAddDexterity>();

        return value;
    }

    public int GetConstitution()
    {
        int value = player_stats.constitution;

        value += GetCurrentAdditiveEffectAmount<EffectAddConstitution>();

        return value;
    }

    public int GetIntelligence()
    {
        int value = player_stats.intelligence;

        value += GetCurrentAdditiveEffectAmount<EffectAddIntelligence>();

        return value;
    }

    public int GetWillpower()
    {
        int value = player_stats.willpower;

        value += GetCurrentAdditiveEffectAmount<EffectAddWillpower>();

        return value;
    }

    public override int GetToHit()
    {
        int value = GetDexterity();

        value += GetCurrentAdditiveEffectAmount<EffectAddToHit>();

        return value;
    }

    public override int GetDodge()
    {
        int value = GetConstitution();
        value += GetCurrentAdditiveEffectAmount<EffectAddDodge>();
        return value;
    }

    public int GetMaxWeight()
    {
        return ((PlayerPrototype)prototype).max_weight + GetStrength();
    }

    public override int GetHealthMax()
    {
        int value = prototype.stats.health_max;
        value += 2 * GetVitality();
        //value += GetCurrentAdditiveEffectAmount<EffectAddHealth>();
        

        return value;
    }

    public override int GetStaminaMax()
    {
        int value = prototype.stats.stamina_max;
        value += 2 * GetConstitution();

        //value += GetCurrentAdditiveEffectAmount<EffectAddHealth>();

        return value;
    }

    public override int GetManaMax()
    {
        int value = prototype.stats.mana_max;
        value += 2 * GetWillpower();

        //value += GetCurrentAdditiveEffectAmount<EffectAddHealth>();

        return value;
    }

    public override int GetArmor(string body_part, ArmorType armor_type)
    {
        ArmorStats armor_stats = prototype.stats.body_armor.Find(x => x.body_part.ToLower().Equals(body_part.ToLower()));
        int current_armor = 0;
        if (armor_stats != null)
        {
            if (armor_type == ArmorType.PHYSICAL)
                current_armor = armor_stats.armor.physical;
            else if (armor_type == ArmorType.ELEMENTAL)
                current_armor = armor_stats.armor.elemental;
            else
                current_armor = armor_stats.armor.magical;
        }

        foreach (EquipmentSlotData equip in equipment)
        {
            if (equip.name.ToLower() != body_part.ToLower())
                continue;

            if (equip.item != null)
            {
                if (equip.item.GetPrototype().armor != null)
                {                    
                    current_armor += equip.item.GetArmor(armor_type);                    
                }
            }
        }

        if (armor_type == ArmorType.PHYSICAL)
            current_armor += GetCurrentAdditiveEffectAmount<EffectAddArmorPhysical>();
        else if (armor_type == ArmorType.ELEMENTAL)
            current_armor += GetCurrentAdditiveEffectAmount<EffectAddArmorElemental>();
        else if (armor_type == ArmorType.MAGICAL)
            current_armor += GetCurrentAdditiveEffectAmount<EffectAddArmorMagical>();

        return current_armor;
    }

    public override int ReduceDurability(string body_part, int value)
    {
        int negative_durability = value;

        foreach (EquipmentSlotData equip in equipment)
        {
            if (equip.name.ToLower() != body_part.ToLower())
                continue;

            if (equip.item != null)
            {
                if (equip.item.GetPrototype().armor != null)
                {
                    negative_durability = - Mathf.Min(0, equip.item.armor_data.durability_current - value);
                    equip.item.armor_data.durability_current = Mathf.Max(0, equip.item.armor_data.durability_current - value);
                }
            }
        }

        return negative_durability;
    }

    public int GetDurability(string body_part)
    {
        int durability = 0;

        foreach (EquipmentSlotData equip in equipment)
        {
            if (equip.name.ToLower() != body_part.ToLower())
                continue;

            if (equip.item != null)
            {
                if (equip.item.GetPrototype().armor != null)
                {
                    durability += equip.item.armor_data.durability_current;
                }
            }
        }

        return durability;
    }

    public override int GetMaxDurability(string body_part)
    {
        int max_durability = prototype.stats.body_armor.Find(x => x.body_part.ToLower().Equals(body_part.ToLower())).durability_max;
        foreach (EquipmentSlotData equip in equipment)
        {
            if (equip.name.ToLower() != body_part.ToLower())
                continue;

            if (equip.item != null)
            {
                if (equip.item.GetPrototype().armor != null)
                {
                    max_durability += equip.item.GetMaxDurability();
                    int relative_armor_durability_effect_amount = GetCurrentAdditiveEffectAmount<EffectAddRelativeArmorDurability>();
                    if (relative_armor_durability_effect_amount > 0)
                    max_durability = (int) (max_durability * ((100 + relative_armor_durability_effect_amount) / 100.0));
                }
            }
        }

        return max_durability;
    }

    public bool AddItem(ItemData item)
    {
        if (item.GetPrototype().name == "Gold")
        {
            gold_amount += item.amount;
            return true;
        }

        if (item.GetWeight() + GetCurrentWeight() > GetMaxWeight())
        {
            GameLogger.Log("The Player cannot pick up " + item.GetName() + " because they are overburdened.");
            return false;
        }

        bool success = inventory.AddItem(item);
        return success;
    }

    public void PrepareEquip(InventorySlotData inventory_data, EquipmentSlotData equipment_data)
    {
        int inventory_index = inventory.slots.IndexOf(inventory_data);
        int equipment_index = equipment.IndexOf(equipment_data);

        current_action = new EquipItemAction(this, inventory_index, equipment_index, prototype.stats.usage_time);
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartCoroutine("ContinueTurns");
        return;
    }

    internal bool Equip(int inventory_slot_index, int equipment_slot_index)
    {
        InventorySlotData inventory_data = inventory.slots[inventory_slot_index];
        EquipmentSlotData equipment_data = equipment[equipment_slot_index];

        if (inventory_data.item == null) return false;
        if (equipment_data.item != null) return false;
        if (equipment_data.item_type.Contains(inventory_data.item.GetPrototype().type) == false) return false;

        if (inventory_data.item.HasPlayerRequiredAttributes() == false) return false;

        equipment_data.item = inventory_data.item;
        inventory_data.item = null;

        ReevaluateTalents();

        return true;
    }

    public void PrepareUnequip(EquipmentSlotData equipment_data, InventorySlotData inventory_data)
    {
        int inventory_index = inventory.slots.IndexOf(inventory_data);
        int equipment_index = equipment.IndexOf(equipment_data);

        current_action = new UnequipItemAction(this, inventory_index, equipment_index, prototype.stats.usage_time);
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartCoroutine("ContinueTurns");
        return;
    }

    internal bool UnEquip(int inventory_slot_index, int equipment_slot_index)
    {
        InventorySlotData inventory_data = inventory.slots[inventory_slot_index];
        EquipmentSlotData equipment_data = equipment[equipment_slot_index];

        if (inventory_data.item != null) return false;
        if (equipment_data.item == null) return false;

        inventory_data.item = equipment_data.item;
        equipment_data.item = null;

        ReevaluateTalents();

        return true;
    }

    public void ReevaluateTalents()
    {
        usable_talents.Clear();
        foreach (EquipmentSlotData equipment_slot in equipment)
        {
            if (equipment_slot.item == null)
                continue;

            if (equipment_slot.item.weapon_data != null)
            {
                foreach(TalentData talent in equipment_slot.item.weapon_data.attack_talents)
                {
                    PlayerTalentSource pts = new()
                    {
                        type = PlayerTalentSourceType.Item,
                        item = equipment_slot.item,
                    };
                    usable_talents.Add((pts, talent));
                }
            }

            if (equipment_slot.item.shield_data != null)
            {
                foreach(TalentData talent in equipment_slot.item.shield_data.talents)
                {
                    PlayerTalentSource pts = new()
                    {
                        type = PlayerTalentSourceType.Item,
                        item = equipment_slot.item,
                    };
                    usable_talents.Add((pts, talent));
                }
            }
        }

        List<SkillTalentData> skill_talents = player_stats.skill_tree.GetUnlockedSkillTalents();
    
        foreach(SkillTalentData skill_talent in skill_talents)
        {       
            //Check Requirements
            if (skill_talent.requirement == SkillTalentRequirement.BluntWeapon)
            {
                ItemData main_weapon = GetMainWeapon();
                if (main_weapon == null || main_weapon.weapon_data == null || main_weapon.GetPrototype().weapon.sub_type != WeaponSubType.BLUNT)
                    continue;
            }
            
            if (skill_talent.talent.prototype.type == TalentType.Passive) continue;

            PlayerTalentSource pts = new()
            {
                type = PlayerTalentSourceType.Skill,
            };
            usable_talents.Add((pts, skill_talent.talent));     
        }

        foreach (InventorySlotData inventory_slot in inventory.slots)
        {
            if (inventory_slot.item == null)
                continue;

            if (inventory_slot.item.talents_when_consumed != null)
            {
                foreach (TalentData talent in inventory_slot.item.talents_when_consumed)
                {
                    PlayerTalentSource pts = new()
                    {
                        type = PlayerTalentSourceType.Item,
                        item = inventory_slot.item,
                    };
                    usable_talents.Add((pts, talent));
                }

            }

            if (inventory_slot.item.GetPrototype().effects_when_consumed != null && inventory_slot.item.GetPrototype().effects_when_consumed.Count > 0)
            {
                PlayerTalentSource pts = new()
                {
                    type = PlayerTalentSourceType.Item,
                    item = inventory_slot.item,
                };
                TalentData talent = new TalentData(
                    new ItemTalentGainConsumeEffects()
                    {
                        name = "Consume " + inventory_slot.item.GetName(),
                        icon = inventory_slot.item.GetPrototype().icon,
                        description = "Consumes " + inventory_slot.item.GetName() + ".",
                        prepare_time = prototype.stats.usage_time / 2 - 1,
                        recover_time = prototype.stats.usage_time / 2,
                    }
                );

                 usable_talents.Add((pts, talent));
            }

            if (inventory_slot.item.usable_item_data != null && inventory_slot.item.usable_item_data.number_of_uses > 0 && inventory_slot.item.GetPrototype().usable_item.effects_when_used.Count > 0)
            {
                PlayerTalentSource pts = new()
                {
                    type = PlayerTalentSourceType.Item,
                    item = inventory_slot.item,
                };
                TalentData talent = new TalentData(
                    new ItemTalentGainUsableEffects()
                    {
                        name = "Use " + inventory_slot.item.GetName(),
                        icon = inventory_slot.item.GetPrototype().icon,
                        description = "Use " + inventory_slot.item.GetName() + ".",
                        prepare_time = prototype.stats.usage_time / 2 - 1,
                        recover_time = prototype.stats.usage_time / 2,
                    }
                );

                 usable_talents.Add((pts, talent));
            }
        }
    }

    public void PrepareConsume(ItemData item_data)
    {
        current_action = new ConsumeItemAction(item_data, prototype.stats.usage_time);
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartCoroutine("ContinueTurns");
        return;
    }

     public void PrepareDrop(ItemData item_data, int amount = -1)
    {
        current_action = new DropItemAction(item_data, prototype.stats.usage_time, amount);
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartCoroutine("ContinueTurns");
        return;
    }

    public void Consume(ItemData item_data)
    {
        if (item_data == null)
            return;
            
        int slot_index = inventory.FindSlotIndex(item_data);
        if (slot_index == -1)
            return;

        foreach (EffectData effect in item_data.GetPrototype().effects_when_consumed)
        {
            AddEffect(effect);
        }

        inventory.RemoveItem(slot_index, 1);
        GameObject.Find("UI").GetComponent<UI>().Refresh();
    }

    public void Use(ItemData item_data)
    {
        int slot_index = inventory.FindSlotIndex(item_data);
        if (slot_index == -1)
            return;

        if (item_data.usable_item_data == null)
            return;

        if (item_data.usable_item_data.number_of_uses < 1)
            return;

        foreach (EffectData effect in item_data.GetPrototype().usable_item.effects_when_used)
        {
            AddEffect(effect);
        }

        item_data.usable_item_data.number_of_uses -= 1;

        GameObject.Find("UI").GetComponent<UI>().Refresh();
    }

    public void Drop(ItemData item_data, int amount = -1)
    {
        if (item_data == null)
            return;

        int slot_index = inventory.FindSlotIndex(item_data);
        if (slot_index == -1)
            return;
  
        if (amount == -1 || amount >= item_data.amount)
        {
            inventory.RemoveItem(slot_index);
            GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
            
            item_data.x = x;
            item_data.y = y;
            game_data.current_map.Add(item_data);
        }
        else
        {
            inventory.RemoveItem(slot_index, amount);
            GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
            ItemData new_item = new ItemData((ItemPrototype) Activator.CreateInstance(item_data.GetPrototype().GetType(), item_data.GetLevel()), x, y);
            new_item.amount = amount;
            game_data.current_map.Add(new_item);
        }

        GameObject.Find("UI").GetComponent<UI>().Refresh();
    }

    internal void RemoveItem(ItemData item, int amount)
    {
        inventory.RemoveItem(item, amount);
        GameObject.Find("UI").GetComponent<UI>().Refresh();
    }

    public override bool ActivateTalent(int index, TalentInputData input)
    {
        if (index < 0 || index >= usable_talents.Count)
            return false;

        var selected_talent = usable_talents[index];
        if (selected_talent.source.type == PlayerTalentSourceType.Item)
            input.item = selected_talent.source.item;

        return ActivateTalent(selected_talent.talent, input);
    }

    public override bool CanUseTalent(int index)
    {
        if (index < 0 || index >= usable_talents.Count)
            return false;

        TalentData talent_data = usable_talents[index].talent;

        if (talent_data.IsUsable() == false)
            return false;

        if (talent_data.prototype.cost_stamina > stamina_current)
        {
            if (talent_data.prototype.type == TalentType.Substained && current_substained_talents_id.Contains(talent_data.id))
            {

            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public override bool CanUseTalent(TalentData talent)
    {
        return CanUseTalent(usable_talents.IndexOf(usable_talents.Find(x => x.talent == talent)));
    }

    public override float Tick()
    {
        float wait_time = base.Tick();

        player_stats.Tick();

        foreach(var slot in equipment)
        {
            if (slot.item != null)
                slot.item.Tick();
        }

        foreach(var slot in inventory.slots)
        {
            if (slot.item != null)
                slot.item.Tick();
        }
        

        if (current_substained_talents_id.Count > 0)
        {
            if (GameObject.Find("GameData").GetComponent<GameData>().global_ticks % 100 == 0)
            {
                List<TalentData> unlocked_talents = player_stats.skill_tree.GetUnlockedTalents();
                foreach (long talent_id in current_substained_talents_id)
                {
                    foreach (TalentData talent in unlocked_talents.FindAll(x => x.id == talent_id))
                    {
                        if (talent.prototype is TalentSubstainedEffects)
                        {
                            foreach (EffectData effect in ((TalentSubstainedEffects)(talent.prototype)).substained_effects)
                            {
                                if (effect.execution_time == EffectDataExecutionTime.CONTINUOUS )
                                {
                                    
                                        DoEffectOnce(effect);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (current_passive_talents_id.Count > 0)
        {
            if (GameObject.Find("GameData").GetComponent<GameData>().global_ticks == 0)
            {
                List<TalentData> unlocked_talents = player_stats.skill_tree.GetUnlockedTalents();
                foreach (long talent_id in current_passive_talents_id)
                {
                    foreach (TalentData talent in unlocked_talents.FindAll(x => x.id == talent_id))
                    {
                        if (talent.prototype is TalentPassiveEffects)
                        {
                            foreach (EffectData effect in ((TalentPassiveEffects)(talent.prototype)).passive_effects)
                            {
                                if (effect.execution_time == EffectDataExecutionTime.CONTINUOUS )
                                {
                                    
                                        DoEffectOnce(effect);
                                
                                }
                            }
                        }
                    }
                }
            }
        }

        return wait_time;
    }

    public bool BuyExpertise(SkillData skill, SkillExpertiseLevel level)
    {
        SkillExpertiseData expertise = skill.expertises.Find(x => x.level == level);
        
        if (expertise.is_unlocked == true) //already bought
            return false;

        if (player_stats.skill_expertise_points < (int) level + 1) // not enough level up points
            return false;

        if (level >= SkillExpertiseLevel.ADEPT && skill.expertises.Find(x => x.level == level - 1).is_unlocked == false) // prelevel not bought
            return false;

        expertise.is_unlocked = true; 
        player_stats.skill_expertise_points -= (int) level + 1;

        return true;
    }

    internal bool BuySkillTalent(SkillData skill, SkillTalentData talent)
    {
        if (player_stats.talent_points <= 0) return false;

        foreach(SkillExpertiseData expertise in skill.expertises)
        {
            foreach(SkillTalentData skill_talent in expertise.talents)
            {
                if (skill_talent == talent)
                {
                    if (expertise.is_unlocked == false) return false;
                    if (skill_talent.is_unlocked == true) return false;

                    skill_talent.is_unlocked = true;
                    
                    //Passive talents are activated imidiately
                    if (skill_talent.talent != null && skill_talent.talent.prototype.type == TalentType.Passive)
                        current_passive_talents_id.Add(skill_talent.talent.id);

                    player_stats.talent_points--;
                }
            }
        }
        return true;
    }

    public override void MoveTo(int destination_x, int destination_y, bool is_teleport = false)
    {
        base.MoveTo(destination_x, destination_y, is_teleport);

        //Autopickup Gold
        MapData map = GameObject.Find("GameData").GetComponent<GameData>().current_map;

        foreach (ItemData item in map.items)
        {
            if (item.GetPrototype() is ItemGold && x == item.x && y == item.y)
            {
                CollectItemCommand c = new(item);
                c.Execute();
                break;
            }
        }      
    }

    public void RefreshAll()
    {
        CureAllPoisons();
        CureAllDiseases();

        health_current = GetHealthMax();
        stamina_current = GetStaminaMax();
        mana_current = GetManaMax();

        int relative_armor_durability_effect_amount = GetCurrentAdditiveEffectAmount<EffectAddRelativeArmorDurability>();

        foreach(var v in equipment)
        {
            if (v.item != null)
            {
                v.item.Regenerate(relative_armor_durability_effect_amount );
            }
        }

        foreach(var v in inventory.slots)
        {
            if (v.item != null)
            {
                v.item.Regenerate(relative_armor_durability_effect_amount );
            }

        }
    }

    public override int GetMaxDiseaseResistance()
    {
        int value = prototype.stats.meter_resistances.resistances[DamageType.DISEASE] + player_stats.vitality;
        value += GetCurrentAdditiveEffectAmount<EffectAddMaxDiseaseResistance>();
        return value;
    }

    public override int GetMaxPoisonResistance()
    {
        int value = prototype.stats.meter_resistances.resistances[DamageType.POISON] + player_stats.constitution;
        value += GetCurrentAdditiveEffectAmount<EffectAddMaxPoisonResistance>();
        return value;
    }

    public override int GetMaxInsanityResistance()
    {
        int value = prototype.stats.meter_resistances.resistances[DamageType.INSANITY] + player_stats.willpower;
        value += GetCurrentAdditiveEffectAmount<EffectAddMaxInsanityResistance>();
        return value;
    }

    public int GetCurrentWeight()
    {
        int value = inventory.GetWeight();

        foreach(var slot in equipment)
        {
            if (slot.item != null)
                value += slot.item.GetWeight();
        }

        return value;
    }

    public override void OnKill()
    {
        if (is_dead == true) //only count deaths once
        return;
        
        prototype.OnKill(this);

        is_dead = true;        

        GameObject.Find("UI").GetComponent<UI>().AddUIState(new UIStateDeathPanel());

    }

}