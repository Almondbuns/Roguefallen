using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public class TalentInputData
{
    public TalentData talent;
    public ActorData source_actor;
    public ActorData target_actor;
    public List<(int, int)> target_tiles;
    public MapData local_data;
 
    public ItemData item;

    public TalentInputData()
    {
        target_tiles = new();    
    }
}

public class TalentData
{
    public static long id_counter = 0;
    public long id;

    public int cooldown_current;
    public TalentPrototype prototype; // reference

    internal void Save(BinaryWriter save)
    {
        save.Write(id);
        save.Write(cooldown_current);
        //save.Write(prototype.GetType().Name);
    }

    internal void Load(BinaryReader save)
    {
        id = save.ReadInt64();
        cooldown_current = save.ReadInt32();
        //Type a_type = Type.GetType(save.ReadString());
        //prototype = (TalentPrototype) Activator.CreateInstance(a_type);
    }

    public TalentData(TalentPrototype prototype)
    {
        id = id_counter;
        ++id_counter;

        this.prototype = prototype;
        if (prototype != null)
            cooldown_current = prototype.cooldown_start;
    }

    public bool IsUsable()
    {
        if (cooldown_current <= 0) return true;

        return false;
    }

    public void Tick()
    {
        if (cooldown_current > 0)
            --cooldown_current;
    }

    internal ActionData CreateAction(TalentInputData input)
    {
        input.talent = this;
        return prototype.CreateAction(input);
    }
}
