using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCData
{
    string unique_id;

    HashSet<string> flags;

    public void Save(BinaryWriter save)
    {
        save.Write(unique_id);
        save.Write(flags.Count);
        foreach (var v in flags)
        {
            save.Write(v);
        }
    }

    public void Load(BinaryReader save)
    {
        unique_id = save.ReadString();
        int size = save.ReadInt32();
        flags = new HashSet<string>(size);
        for (int i = 0; i < size; ++i)
        {
            flags.Add(save.ReadString());
        }
    }

    public NPCData(string unique_id)
    {
        this.unique_id = unique_id;
        flags = new();
    }

    public bool ContainsFlag(string flag)
    {
        return flags.Contains(flag);
    }

    public void AddFlag(string flag)
    {
        flags.Add(flag);
    }
}

    
