using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class ActorEffectData
{
    public int current_tick;
    public EffectData effect;

    internal void Save(BinaryWriter save)
    {
        save.Write(current_tick);
        save.Write(effect.GetType().Name);
        effect.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        current_tick = save.ReadInt32();
        Type type = Type.GetType(save.ReadString());
        effect = (EffectData) Activator.CreateInstance(type);
        effect.Load(save);
    }
}

public class ArmorStatsData
{
    public string body_part;
    public int durability_current;

    internal void Save(BinaryWriter save)
    {
        save.Write(body_part);
        save.Write(durability_current);
    }

    internal void Load(BinaryReader save)
    {
        body_part = save.ReadString();
        durability_current = save.ReadInt32();
    }

    public ArmorStatsData(ArmorStats stats = null)
    {
        if (stats != null)
        {
            this.body_part = stats.body_part;
            this.durability_current = stats.durability_max;
        }
    }

    public int SubstractDamage(int value)
    {
        durability_current -= value;
        if (durability_current < 0)
        {
            int output = -durability_current;
            durability_current = 0;
            return output;
        }
        return 0;
    }
}

public class ActorMeterResistanceData
{
    public Dictionary<DamageType, int> resistances;

    public ActorMeterResistanceData()
    {
        resistances = new();
    }

    internal void Save(BinaryWriter save)
    {
        save.Write(resistances.Count);
        foreach (DamageType type in resistances.Keys)
        {
            save.Write((int) type);
            save.Write(resistances[type]);
        }
    }

    internal void Load(BinaryReader save)
    {
        int size = save.ReadInt32();
        for (int i = 0; i < size; ++ i)
        {
            DamageType dt = (DamageType) save.ReadInt32();
            int value = save.ReadInt32();
            if (resistances.ContainsKey(dt))
            {
                resistances[dt] = value;
            }
            else
            {
                resistances.Add(dt, value);
            }
            
        }
    }
}

public class ActorData
{
    public static long id_counter = 0;
    public long id;

    public bool is_quest_actor = false;
    public bool is_boss = false;
    public long quest_id = -1;

    public ActorPrototype prototype;

    public int x {get; private set;}
    public int y {get; private set;}

    public bool is_dead = false;
    public bool is_currently_hidden = false;

    public int health_current;
    public int stamina_current;
    public int mana_current;

    public ActorMeterResistanceData meter_resistances;
  
    public List<TalentData> talents;
    public List<ActorEffectData> current_effects;
    public List<long> current_substained_talents_id;
    public List<long> current_passive_talents_id;

    public List<DiseaseData> current_diseases;
    public List<PoisonData> current_poisons;
    
    public List<ArmorStatsData> body_armor;

    public ActionData current_action = null;

    public InventoryData inventory;

    public delegate void ActorDataHandler(ActorData actor);
    public delegate void DamageHandler(int damage, int absorb_armor);
    public delegate void IntHandler(int value);
    public delegate void IntStringHandler(int value, string type);
    public delegate void VoidHandler();
    public delegate void AttackedTilesHandler(List<AttackedTileData> tiles);
    public delegate void EffectHandler(bool gain, EffectData effect);

    public event ActorDataHandler HandleKill;
    public event ActorDataHandler HandlePrepareActionTick;
    public event ActorDataHandler HandleStartActionTick;
    public event ActorDataHandler HandleRecoverActionTick;
    public event ActorDataHandler HandleHasFinishedActionTick;
    public event DamageHandler HandleAbsorbDamage;
    public event IntStringHandler HandleHeal;
    public event IntStringHandler HandleDamage;
    public event IntHandler HandleExperienceGain;
    public event VoidHandler HandleDodge;
    public event VoidHandler HandleResist;
    public event VoidHandler HandleLevelUp;
    public event VoidHandler HandleParry;
    public event VoidHandler HandleBlock;
    public event VoidHandler HandleMovement;
     public event VoidHandler HandleTeleport;
    public event ActorDataHandler HandleUnhide;
    public event AttackedTilesHandler HandleMeleeAttack;
    public event EffectHandler HandleEffect;

    internal virtual void Save(BinaryWriter save)
    {
        save.Write(id);
        save.Write(is_quest_actor);
        save.Write(is_boss);
        save.Write(quest_id);

        save.Write(prototype.GetType().Name);
        save.Write(prototype.stats.level);

        save.Write(x);
        save.Write(y);

        save.Write(is_dead);
        save.Write(is_currently_hidden);

        save.Write(health_current);
        save.Write(stamina_current);
        save.Write(mana_current);

        meter_resistances.Save(save);

        save.Write(talents.Count);
        foreach(var v in talents)
        {
            v.Save(save);
        }

        save.Write(current_effects.Count);
        foreach (var v in current_effects)
        {
            v.Save(save);
        }

        save.Write(current_substained_talents_id.Count);
        foreach (var v in current_substained_talents_id)
        {
            save.Write(v);
        }

        save.Write(current_passive_talents_id.Count);
        foreach (var v in current_passive_talents_id)
        {
            save.Write(v);
        }

        save.Write(current_diseases.Count);
        foreach (var v in current_diseases)
        {
            v.Save(save);
        }

        save.Write(current_poisons.Count);
        foreach (var v in current_poisons)
        {
            v.Save(save);
        }

        save.Write(body_armor.Count);
        foreach (var v in body_armor)
        {
            v.Save(save);
        }

        if (current_action == null)
        {
            save.Write(false);
        }
        else
        {
            save.Write(true);
            save.Write(current_action.GetType().Name);
            current_action.Save(save);
        }

        if (inventory != null)
        {
            save.Write(true);
            inventory.Save(save);
        }
        else
        {
            save.Write(false);
        }
    }

    internal virtual void Load(BinaryReader save)
    {
        id = save.ReadInt64();
        is_quest_actor = save.ReadBoolean();
        is_boss = save.ReadBoolean();
        quest_id = save.ReadInt64();

        Type a_type = Type.GetType(save.ReadString());

        int a_level = save.ReadInt32();
        prototype = (ActorPrototype)Activator.CreateInstance(a_type, a_level);
     
        int x = save.ReadInt32();
        int y = save.ReadInt32();
        MoveTo(x,y);

        is_dead = save.ReadBoolean();
        is_currently_hidden = save.ReadBoolean();

        health_current = save.ReadInt32();
        //Debug.Log(health_current);
        stamina_current = save.ReadInt32();
        mana_current = save.ReadInt32();

        meter_resistances.Load(save);

        int size = save.ReadInt32();
        talents = new(size);
        for(int i = 0; i < size; ++i)
        {
            TalentData talent = new TalentData(null);
            talent.Load(save);
            talents.Add(talent);
        }

        size = save.ReadInt32();
    
        current_effects = new(size);
        for (int i = 0; i < size; ++i)
        {
            ActorEffectData effect = new ActorEffectData();
            effect.Load(save);
            current_effects.Add(effect);
        }

        size = save.ReadInt32();
        current_substained_talents_id = new(size);
        for (int i = 0; i < size; ++i)
        {
            long v = save.ReadInt64();
            current_substained_talents_id.Add(v);
        }

        size = save.ReadInt32();
        current_passive_talents_id = new(size);
        for (int i = 0; i < size; ++i)
        {
            long v = save.ReadInt64();
            current_passive_talents_id.Add(v);
        }

        size = save.ReadInt32();
        current_diseases = new(size);
        for (int i = 0; i < size; ++i)
        {
            DiseaseData v = new DiseaseData();
            v.Load(save);
            current_diseases.Add(v);
        }

        size = save.ReadInt32();
        current_poisons = new(size);
        for (int i = 0; i < size; ++i)
        {
            PoisonData v = new PoisonData();
            v.Load(save);
            current_poisons.Add(v);
        }

        size = save.ReadInt32();
        body_armor = new(size);
        for (int i = 0; i < size; ++i)
        {
            ArmorStatsData a = new ArmorStatsData();
            a.Load(save);
            body_armor.Add(a);
        }

        bool b = save.ReadBoolean();
        if (b == true)
        {
            Type type = Type.GetType(save.ReadString());
            current_action = (ActionData)Activator.CreateInstance(type);
            current_action.Load(save);
        }

        if (save.ReadBoolean() == true)
        {
            inventory = new InventoryData(prototype.inventory.size);
            inventory.Load(save);
        }

        //Postprocessing: Talent Prototypes of actor talents data are references!
        int counter = 0;
        foreach (TalentPrototype talent in prototype.talents)
        {
            talents[counter].prototype = talent;
            ++counter;
        }
        
    }

    public ActorData(int x, int y, ActorPrototype prototype = null)
    {
        id = id_counter;
        ++id_counter;

        this.x = x;
        this.y = y;
        
        talents = new List<TalentData>();
        current_effects = new();
        current_substained_talents_id = new();
        current_passive_talents_id = new();
        current_diseases = new();
        current_poisons = new();
        body_armor = new();

        meter_resistances = new();

        this.prototype = prototype;

        if (prototype != null)
        {
            if (prototype.is_hidden == true)
                is_currently_hidden = true;

            foreach (TalentPrototype talent in prototype.talents)
            {
                talents.Add(new TalentData(talent));
            }

            health_current = GetHealthMax();
            stamina_current = GetStaminaMax();
            mana_current = GetManaMax();

            foreach (ArmorStats armor_stats in prototype.stats.body_armor)
            {
                body_armor.Add(new ArmorStatsData(armor_stats));
            }

            foreach (DamageType type in prototype.stats.meter_resistances.resistances.Keys)
            {
                meter_resistances.resistances[type] = 0;
            }

            if (prototype.inventory != null)
            {
                inventory = new InventoryData(prototype.inventory.size);
            }
            
            prototype.OnCreation(this);
        }

    }

    public void SetHidden(bool value)
    {
        is_currently_hidden = value;

        HandleUnhide?.Invoke(this);
    }

    public virtual void MoveTo(int destination_x, int destination_y, bool is_teleport = false)
    {
        x = destination_x;
        y = destination_y;

        if (is_teleport == true)
            HandleTeleport?.Invoke();
        else
            HandleMovement?.Invoke();
    }

    public bool OnPlayerMovementHit()
    {
        return prototype.OnPlayerMovementHit(this);
    }

    public virtual int GetCurrentAdditiveEffectAmount<T>()
    {
        int value = 0;

        foreach (ActorEffectData effect in current_effects)
        {
            if (effect.effect is T)
                value += (int)effect.effect.amount;
        }
        
        foreach (DiseaseData disease in current_diseases)
        {
            foreach (EffectData effect in disease.GetCurrentEffects())
            {
                if (effect is T)
                    value += (int)effect.amount;
            }
        }

        foreach (PoisonData poison in current_poisons)
        {
            foreach (EffectData effect in poison.GetCurrentEffects())
            {
                if (effect is T)
                    value += (int)effect.amount;
            }
        }

        return value;
    }

    public virtual int GetToHit()
    {
        int value = prototype.stats.to_hit;
        
        value += GetCurrentAdditiveEffectAmount<EffectAddToHit>();

        return value;

    }

    public virtual int GetDodge()
    {
        int value = prototype.stats.dodge;
        value += GetCurrentAdditiveEffectAmount<EffectAddDodge>();
        return value;
    }

    public virtual int GetMaxDiseaseResistance()
    {
        int value = prototype.stats.meter_resistances.resistances[DamageType.DISEASE];
        value += GetCurrentAdditiveEffectAmount<EffectAddMaxDiseaseResistance>();
        return value;
    }

    public virtual int GetMaxPoisonResistance()
    {
        int value = prototype.stats.meter_resistances.resistances[DamageType.POISON];
        value += GetCurrentAdditiveEffectAmount<EffectAddMaxPoisonResistance>();
        return value;
    }

    public virtual int GetMaxInsanityResistance()
    {
        int value = prototype.stats.meter_resistances.resistances[DamageType.INSANITY];
        value += GetCurrentAdditiveEffectAmount<EffectAddMaxInsanityResistance>();
        return value;
    }

    public virtual int GetMovementTime()
    {
        int value = prototype.stats.movement_time;

        value += GetCurrentAdditiveEffectAmount<EffectAddMovementTime>();

        value = (int) Mathf.Max(0,(value * (100 - GetCurrentAdditiveEffectAmount<EffectRemoveMovementTimeRelative>()) / 100f));

        return value;
    }

    public virtual int GetAttackTime()
    {
        int value = prototype.stats.attack_time;

        value += GetCurrentAdditiveEffectAmount<EffectAddAttackTime>();

        return value;
    }

    public void DoMeleeAttack(List<AttackedTileData> attacked_tiles)
    {
        HandleMeleeAttack?.Invoke(attacked_tiles);
    }

    public virtual int GetUsageTime()
    {
        return prototype.stats.usage_time;
    }

    public virtual int GetArmor(string body_part, ArmorType armor_type)
    {
        int current_armor = 0;
        ArmorStats armor_stats = prototype.stats.body_armor.Find(x => x.body_part.ToLower().Equals(body_part.ToLower()));

        if (armor_stats != null)
        {
            if (armor_type == ArmorType.PHYSICAL)
                current_armor = armor_stats.armor.physical;
            else if (armor_type == ArmorType.ELEMENTAL)
                current_armor = armor_stats.armor.elemental;
            else if (armor_type == ArmorType.MAGICAL)
                current_armor = armor_stats.armor.magical;
            else current_armor = 0;
        }

        if (armor_type == ArmorType.PHYSICAL)
            current_armor += GetCurrentAdditiveEffectAmount<EffectAddArmorPhysical>();
        else if (armor_type == ArmorType.ELEMENTAL)
            current_armor += GetCurrentAdditiveEffectAmount<EffectAddArmorElemental>();
        else if (armor_type == ArmorType.MAGICAL)
            current_armor += GetCurrentAdditiveEffectAmount<EffectAddArmorMagical>();

        return current_armor;
    }

    public virtual int GetMaxDurability(string body_part)
    {
        return prototype.stats.body_armor.Find(x => x.body_part == body_part).durability_max;
    }

    public virtual void SelectNextAction()
    {
    }

    public virtual void OnKill()
    {
        if (is_dead == true) //only count deaths once
        return;
        
        prototype.OnKill(this);

        is_dead = true; // will be removed from map at end of tick
        HandleKill?.Invoke(this);

        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        foreach (QuestData quest in game_data.player_data.active_quests)
            quest.OnActorKill(this);

    }

    public virtual int ReduceDurability(string body_part, int value)
    {
        ArmorStatsData armor_stats = body_armor.Find(x => x.body_part == body_part);
        if (armor_stats != null)
            return armor_stats.SubstractDamage(value);
        
        return value;
    }

    public virtual void TryToHit(ActorData src_actor, int to_hit, List<(DamageType type , int damage, int armor_penetration)> damage, List<EffectData> effects, List<Type> diseases = null, List<Type> poisons = null)
    {
        //Parry
        int parry_chance = GetCurrentAdditiveEffectAmount<EffectParry>();
        if (parry_chance > 0)
        {
            int r = UnityEngine.Random.Range(0,100);
            if (r < parry_chance)
            {
                GameLogger.Log("The " + prototype.name + " parries the attack.");
                src_actor.AddEffect(new EffectCounterStrikeVulnerable(){amount = 1, duration = 300});
                HandleParry?.Invoke();
                return;
            }
        }

        //Only hit if not dodged
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 20 + GetDodge() - to_hit) //dodged!
        {
            GameLogger.Log("The " + prototype.name + " dodges.");
            HandleDodge?.Invoke();
            return;
        }

        int damage_sum = 0;
        int absorb_sum = 0;

        string message = "The " + prototype.name + " takes ";
        int counter = 0;
       
        //Hit random body_part (percentage based)
        int random = UnityEngine.Random.Range(1, 101);
        int body_part_sum = 0;
        string body_part = "";
        foreach (ArmorStats armor_stats in prototype.stats.body_armor)
        {
            if (body_part_sum < random)
            {
                body_part = armor_stats.body_part;
                body_part_sum += armor_stats.percentage;
            }
        }

        foreach (var damage_per_type in damage)
        {
            if (counter > 0)
                message += " and ";

            // First multiply damage with resistance
            int damage_multiplied = (int) (damage_per_type.damage * prototype.stats.probability_resistances.GetDamageMultiplyer(damage_per_type.type));
             //Increase damage if counter vulnerable
            if (GetCurrentAdditiveEffectAmount<EffectCounterStrikeVulnerable>() > 0)
            {
                damage_multiplied *= 2;        
            }

            ArmorType armor_type;
            string damage_type;
            switch(damage_per_type.type)
            {
                case DamageType.CRUSH:
                    damage_type = "crush";
                    armor_type = ArmorType.PHYSICAL;
                    break;
                case DamageType.SLASH:
                    damage_type = "slash";
                    armor_type = ArmorType.PHYSICAL;
                    break;
                case DamageType.PIERCE:
                    damage_type = "pierce";
                    armor_type = ArmorType.PHYSICAL;
                    break;
                case DamageType.FIRE:
                    damage_type = "fire";
                    armor_type = ArmorType.ELEMENTAL;
                    break;
                case DamageType.ICE:
                    damage_type = "ice";
                    armor_type = ArmorType.ELEMENTAL;
                    break;
                case DamageType.LIGHTNING:
                    damage_type = "lightning";
                    armor_type = ArmorType.ELEMENTAL;
                    break;
                case DamageType.DARK:
                    damage_type = "dark";
                    armor_type = ArmorType.MAGICAL;
                    break;
                case DamageType.MAGIC:
                    damage_type = "magic";
                    armor_type = ArmorType.MAGICAL;
                    break;
                case DamageType.DIVINE:
                    damage_type = "divine";
                    armor_type = ArmorType.MAGICAL;
                    break;
                case DamageType.DURABILITY:
                    damage_type = "durability";
                    armor_type = ArmorType.NONE;
                    break;
                case DamageType.DISEASE:
                    damage_type = "disease";
                    armor_type = ArmorType.NONE;
                    break;
                case DamageType.POISON:
                    damage_type = "poison";
                    armor_type = ArmorType.NONE;
                    break;
                default:
                    damage_type = "UNKNOWN";
                    armor_type = ArmorType.NONE;
                    break;
            }

            int damage_absorbed;
            int damage_taken;
            int output;

            if (damage_per_type.type == DamageType.DURABILITY)
            {
                damage_absorbed = damage_per_type.damage - ReduceDurability(body_part, damage_per_type.damage);
                damage_taken = 0;
                output = 0; 
            }
            else if (damage_per_type.type == DamageType.DISEASE)
            {
                if (prototype.can_catch_disease == true)
                {
                    meter_resistances.resistances[damage_per_type.type] += damage_per_type.damage;
                    damage_absorbed = damage_per_type.damage;
                    damage_taken = 0;
                    output = 0;
                }
                else
                {
                    continue;
                }
            }
            else if (damage_per_type.type == DamageType.POISON)
            {
                if (prototype.can_catch_poison == true)
                {
                    meter_resistances.resistances[damage_per_type.type] += damage_per_type.damage;
                    damage_absorbed = damage_per_type.damage;
                    damage_taken = 0;
                    output = 0;
                }
                else
                {
                    continue;
                }
            }
            else // all normal damage types
            {
                //Deal with possible blocks
                
                int active_block_chance = GetCurrentAdditiveEffectAmount<EffectActiveBlock>();
                int r_active = UnityEngine.Random.Range(0,100);
                int passive_block_chance = GetCurrentAdditiveEffectAmount<EffectPassiveBlock>();
                int r_passive = UnityEngine.Random.Range(0,100);
                int shield_armor = GetArmor("shield", armor_type);
                if (stamina_current >= 1 && shield_armor > 0 && active_block_chance > 0 && r_active < active_block_chance)
                {
                    damage_absorbed = Mathf.Min(damage_multiplied, Mathf.Max(0, shield_armor + GetArmor(body_part, armor_type) - damage_per_type.armor_penetration));
                    output = damage_multiplied - shield_armor + ReduceDurability("shield", shield_armor);
                    output = ReduceDurability(body_part, output);
                    HandleBlock?.Invoke();
                    GameLogger.Log("The " + prototype.name + " blocks.");
                    RemoveStamina(1);
                }
                else if (shield_armor > 0 && passive_block_chance > 0 && r_passive < passive_block_chance)
                {
                    damage_absorbed = Mathf.Min(damage_multiplied, Mathf.Max(0, shield_armor + GetArmor(body_part, armor_type) - damage_per_type.armor_penetration));
                    output = damage_multiplied - shield_armor + ReduceDurability("shield", shield_armor);
                    output = ReduceDurability(body_part, output);
                    HandleBlock?.Invoke();
                    GameLogger.Log("The " + prototype.name + " blocks.");
                }
                else
                {
                    damage_absorbed = Mathf.Min(damage_multiplied, Mathf.Max(0, GetArmor(body_part, armor_type) - damage_per_type.armor_penetration));
                    output = ReduceDurability(body_part, damage_absorbed);
                }
                damage_taken = damage_multiplied - damage_absorbed;
            }

            if (output > 0)
            {
                damage_taken += output;
                damage_absorbed -= output;
            }

            health_current -= damage_taken;
            damage_sum += damage_taken;
            absorb_sum += damage_absorbed;

            message += damage_taken + " (" + damage_absorbed + " absorbed)" + " " + damage_type;
            prototype.OnDamage(this, damage_per_type.type, damage_taken);

            ++counter;
        }

        message += " " + body_part.ToLower() + " damage.";

        if (damage_sum > 0 || absorb_sum > 0)
        {
            GameLogger.Log(message);
            HandleAbsorbDamage?.Invoke(damage_sum, absorb_sum);
        }

        if (damage_sum > 0 && current_effects.Find(x => x.effect is EffectStun) != null)
        {
            GameLogger.Log(prototype.name + " is no longer stunned.");
            foreach (ActorEffectData e in current_effects.FindAll(x => x.effect is EffectStun))
                RemoveEffect(e);

        }

        float effect_damage_probability = 1; // damage_sum / (damage_sum + absorb_sum);

        if (effects != null)
        {
            foreach (EffectData effect in effects)
            {
                if (effect.damage_type == DamageType.UNKNOWN)
                {
                    Debug.Log("Error: Damage Type in Effect " + effect.name + " not set.");
                    continue;
                }

                float effect_probability = effect_damage_probability * prototype.stats.probability_resistances.GetEffectProbability(effect.damage_type);
                float dice = UnityEngine.Random.Range(0, 1.0f);
                if (dice > effect_probability)
                {
                    GameLogger.Log("The " + prototype.name + " resists an effect.");
                    HandleResist?.Invoke();
                    continue;
                }

                AddEffect(effect.Copy());
                
            }
        }

        if (diseases != null)
        {
            if (meter_resistances.resistances[DamageType.DISEASE] > GetMaxDiseaseResistance())
            {
                if (diseases.Count > 0)
                {
                    //Get a random disease
                    int random_index = UnityEngine.Random.Range(0, diseases.Count);
                    bool found = false;
                    foreach (DiseaseData disease in current_diseases)
                    {
                        if (disease.prototype.GetType() == (diseases[random_index]))
                            found = true;
                    }

                    if (found == false)
                    {
                        DiseasePrototype proto = (DiseasePrototype)Activator.CreateInstance(diseases[random_index]);
                        DiseaseData illness = new DiseaseData(proto);
                        current_diseases.Add(illness);
                        GameLogger.Log("<color=#F17500>The " + prototype.name + " contracts " + illness.prototype.name + ".</color>");
                        meter_resistances.resistances[DamageType.DISEASE] = 0;
                    }
                }
            }
        }

        if (poisons != null)
        { 
            if (meter_resistances.resistances[DamageType.POISON] > GetMaxPoisonResistance())
            {                
                if (poisons.Count > 0)
                {
                    //Get a random poison - but only one of the same kind at the same time
                    int random_index = UnityEngine.Random.Range(0, poisons.Count);
                    bool found = false;
                    foreach (PoisonData poison in current_poisons)
                    {
                        if (poison.prototype.GetType() == (poisons[random_index]))
                            found = true;
                    }

                    if (found == false)
                    {
                        PoisonPrototype proto = (PoisonPrototype)Activator.CreateInstance(poisons[random_index]);
                        PoisonData poison = new PoisonData(proto);
                        current_poisons.Add(poison);
                        GameLogger.Log("<color=#F17500>The " + prototype.name + " contracts " + poison.prototype.name + ".</color>");
                        meter_resistances.resistances[DamageType.POISON] = 0;
                    }
                }
            }
        }

        if (health_current <= 0)
        {
            OnKill();
        }
    }

    public void OnEnterTile(ActorData actor)
    {
        prototype.OnEnterTile(this, actor);
    }

    internal void OnExperienceGain(int kill_experience)
    {
        //Only used by player - but event is actor event
        HandleExperienceGain?.Invoke(kill_experience);
    }

    internal void OnLevelUp()
    {
        //Only used by player - but event is actor event
        HandleLevelUp?.Invoke();
    }

    public virtual bool CanUseTalent(int index)
    {
        if (index < 0 || index >= talents.Count)
            return false;

        TalentData talent_data = talents[index];
        
        if (talent_data.IsUsable() == false)
            return false;

        if (talent_data.prototype.cost_stamina > stamina_current)
            return false;

        if (current_effects.Find(x => x.effect is EffectStun) != null)
            return false;

        return true;
    }

    public virtual bool CanUseTalent(TalentData talent)
    {
        return CanUseTalent(talents.IndexOf(talent));
    }

    public virtual bool ActivateTalent(int index, TalentInputData input)
    {
        if (index < 0 || index >= talents.Count)
            return false;

        return ActivateTalent(talents[index], input);
    }

    public virtual bool ActivateTalent(TalentData talent, TalentInputData input)
    {
        if (CanUseTalent(talent) == false)
            return false;

        if (talent.prototype.type == TalentType.Active)
        {
            stamina_current -= talent.prototype.cost_stamina;
            talent.cooldown_current = talent.prototype.cooldown;

            current_action = talent.CreateAction(input);
        }
        else if (talent.prototype.type == TalentType.Substained)
        {
            if (current_substained_talents_id.Contains(talent.id) == false)
            {
                stamina_current -= talent.prototype.cost_stamina;
            }
            talent.cooldown_current = talent.prototype.cooldown;

            current_action = talent.CreateAction(input);
        }
      
        return true;
    }

    public virtual float Tick()
    {
        if (is_dead == true && !(this is PlayerData))
            return 0;

        float wait_time = 0;
        
        if (current_action == null)
        {
            SelectNextAction();
        }

        foreach(TalentData talent in talents)
        {
            talent.Tick();
        }

        foreach(ActorEffectData effect in current_effects)
        {
            effect.current_tick += 1;
            if (effect.effect.execution_time == EffectDataExecutionTime.CONTINUOUS && effect.current_tick % 100 == 0)
            {
                DoEffectOnce(effect.effect);
            }
        }

        foreach(PoisonData poison in current_poisons)
        {
            poison.Tick(this);            
        }
        current_poisons.RemoveAll(x => x.duration > x.prototype.max_duration);

        List<ActorEffectData> effects_gone = current_effects.FindAll(x => x.current_tick > x.effect.duration);
        foreach(var effect in effects_gone)
        {
            if (effect.effect.execution_time == EffectDataExecutionTime.END)
                DoEffectOnce(effect.effect);
            RemoveEffect(effect);
        }

        if (GameObject.Find("GameData").GetComponent<GameData>().global_ticks % 100 == 0)
        {
            if (is_currently_hidden == true)
                CheckHidden();
        }

        if (current_action != null && current_action.current_tick == 0 && current_action.prepare_time > 0)
        {
            if (current_action.log == true)
            {
                string message = current_action.prepare_message;
                if (message != null)
                {
                    message = message.Replace("<name>", prototype.name);
                    GameLogger.Log("<color=#FF0000>" + message + "</color>");
                }
            }
        }


        if (current_action != null && current_action.current_tick == current_action.prepare_time)
        {
            HandleStartActionTick?.Invoke(this);
            if (current_action.log == true)
            {
                string message = current_action.action_message;
                if (message != null)
                {
                    message = message.Replace("<name>", prototype.name);
                    GameLogger.Log("<color=#FFFF00>" + message + "</color>");
                }
            }
            //wait_time += 0.5f;
        }

        if (current_action != null)
            wait_time += current_action.Tick();

        if (current_action != null && current_action.state == ActionState.PREPARE)
            HandlePrepareActionTick?.Invoke(this);
      
        if (current_action != null && current_action.state == ActionState.RECOVER)
            HandleRecoverActionTick?.Invoke(this);

        if (current_action != null && current_action.state == ActionState.FINISHED)
            HandleHasFinishedActionTick?.Invoke(this);

        if (current_action != null && current_action.HasFinished() == true)
        {
            SelectNextAction();
        }
        
        return wait_time; 
    }

    private void CheckHidden()
    {
        if (is_currently_hidden == false) return;
        
        MapData map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        if (map.tiles[x, y].visibility != Visibility.Active) return;

        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        int random = UnityEngine.Random.Range(0, 100) + (player.player_stats.perception - prototype.stats.stealth);
        
        int trap_spotting_factor = player.GetCurrentAdditiveEffectAmount<EffectAddRelativeTrapSpotting>();
        int trap_spotting_change = 5 + (int)(5 * (trap_spotting_factor / 100.0));

        if (random >= 100 - trap_spotting_change)
            SetHidden(false);
    }

    public void DoEffectOnce(EffectData effect)
    {
        switch (effect)
        {
            case EffectAddHitpoints e:         
                Heal((int)effect.amount);
                break;
            case EffectSubstractHitpoints e:         
                Damage((int)effect.amount);
                break;

            case EffectAddStamina e:
                RefreshStamina((int)effect.amount);
                break;
            
            case EffectSubstractStamina e:
                RemoveStamina((int)effect.amount);
                break;

            case EffectAddMana e:
                RefreshMana((int)effect.amount);
                break;

            case EffectInterrupt e:

                current_action = new WaitAction(20);
                break;

            case EffectStun e:

                current_action = new WaitAction(100);
                break;

            case ActorEffectSpecialCommand e:
                e.Execute();
                break;
        }
    }

    public void AddEffect(EffectData effect)
    {
        string text = "The " + prototype.name + " gains the effect: " + effect.name.ToLower();
        if (effect.amount != 0) 
            text += ": " + effect.amount;
        
        text += ".";

        GameLogger.Log(text);
        
        HandleEffect?.Invoke(true, effect);

        if (effect.execution_time == EffectDataExecutionTime.START || effect.execution_time == EffectDataExecutionTime.CONTINUOUS) 
            DoEffectOnce(effect);

        if (effect.duration > 0)
        {
            current_effects.Add(new ActorEffectData { current_tick = 0, effect = effect });
        }
    }

    public void RemoveEffect(ActorEffectData effect)
    {
        string text = "The "+ prototype.name +"  loses the effect: " + effect.effect.name.ToLower();
        if (effect.effect.amount != 0) 
            text += ": " + effect.effect.amount;
        
        text += ".";

        GameLogger.Log(text);

        HandleEffect?.Invoke(false, effect.effect);

        current_effects.Remove(effect);
    }


    public void Heal(int amount)
    {
        health_current = Mathf.Min(GetHealthMax(), health_current + amount);
        HandleHeal?.Invoke(amount, "Health");
    }

    public void Damage(int amount)
    {
        health_current -= amount;
        HandleDamage?.Invoke(amount, "Health");

        if (health_current <= 0)
        {
            OnKill();
        }
    }

    public void RefreshStamina(int amount)
    {
        stamina_current = Mathf.Min(GetStaminaMax(), stamina_current + amount);
        HandleHeal?.Invoke(amount, "Stamina");
    }

    public void RemoveStamina(int amount)
    {
        stamina_current = Mathf.Max(0, stamina_current - amount);
        HandleDamage?.Invoke(amount, "Stamina");
    }

    public void RefreshMana(int amount)
    {
        mana_current = Mathf.Min(GetManaMax(), mana_current + amount);
        HandleHeal?.Invoke(amount, "Mana");
    }

    public virtual int GetHealthMax()
    {
        int value = prototype.stats.health_max;

        //value += GetCurrentAdditiveEffectAmount<EffectAddHealth>();

        return value;
    }

    public virtual int GetStaminaMax()
    {
        int value = prototype.stats.stamina_max;

        //value += GetCurrentAdditiveEffectAmount<EffectAddHealth>();

        return value;
    }

    public virtual int GetManaMax()
    {
        int value = prototype.stats.mana_max;

        //value += GetCurrentAdditiveEffectAmount<EffectAddHealth>();

        return value;
    }

    public void CureAllPoisons()
    {
        this.current_poisons.Clear();
        meter_resistances.resistances[DamageType.POISON] = 0;
    }

    public void CureAllDiseases()
    {
        this.current_diseases.Clear();
        meter_resistances.resistances[DamageType.DISEASE] = 0;
        GameLogger.Log("The " + prototype.name + " feels very healthy.");
    }

    public void CureInsanity()
    {
        this.current_diseases.Clear();
        meter_resistances.resistances[DamageType.DISEASE] = 0;
        GameLogger.Log("The " + prototype.name + " feels very healthy.");
    }
}