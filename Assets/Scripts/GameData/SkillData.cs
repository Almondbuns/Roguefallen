using System.Collections.Generic;
using System;
using System.IO;

public class SkillTreeData
{
    public List<SkillData> skills;

    public SkillTreeData()
    {
        skills = new List<SkillData>();
    }

    internal void Save(BinaryWriter save)
    {
        save.Write(skills.Count);
        foreach (var v in skills)
            v.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        int size = save.ReadInt32();
        skills = new(size);
        for (int i = 0; i < size; ++i)
        {
            SkillData v = new();
            v.Load(save);
            skills.Add(v);
        }
    }

    internal void Tick()
    {
        foreach (SkillData skill in skills)
            skill.Tick();
    }

    internal List<TalentData> GetUnlockedTalents()
    {
        List<TalentData> talent_list = new List<TalentData>();

        foreach (SkillData skill in skills)
        {
            foreach (SkillExpertiseData sed in skill.expertises)
            {
                if (sed.is_unlocked == false) continue;

                foreach (SkillTalentData skd in sed.talents)
                {
                    if (skd.is_unlocked == false) continue;
                    if (skd.talent != null)
                        talent_list.Add(skd.talent);
                }
            }
        }

        return talent_list;
    }

    internal List<SkillTalentData> GetUnlockedSkillTalents()
    {
        List<SkillTalentData> talent_list = new List<SkillTalentData>();

        foreach (SkillData skill in skills)
        {
            foreach (SkillExpertiseData sed in skill.expertises)
            {
                if (sed.is_unlocked == false) continue;

                foreach (SkillTalentData skd in sed.talents)
                {
                    if (skd.is_unlocked == false) continue;
                    if (skd.talent != null)
                        talent_list.Add(skd);
                }
            }
        }

        return talent_list;
    }
}

public class SkillData
{
    public string name;
    public string icon;

    public List<SkillExpertiseData> expertises;

    internal void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(icon);
        save.Write(expertises.Count);
        foreach (var v in expertises)
            v.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        name = save.ReadString();
        icon = save.ReadString();
        
        int size = save.ReadInt32();
        expertises = new(size);
        for (int i = 0; i < size; ++i)
        {
            SkillExpertiseData v = new();
            v.Load(save);
            expertises.Add(v);
        }
    }

    public SkillData()
    {
        expertises = new();
    }

    internal void Tick()
    {
        foreach (SkillExpertiseData expertise in expertises)
            expertise.Tick();
    }

    public List<TalentData> GetAvailableTalents()
    {
        List<TalentData> list = new();
        return list;
    }
}

public enum SkillExpertiseLevel
{
    NOVICE,
    ADEPT,
    EXPERT,
    MASTER
}

public class SkillExpertiseData
{
    public SkillExpertiseLevel level;
    public bool is_unlocked = false;
    public List<SkillTalentData> talents;

    internal void Save(BinaryWriter save)
    {
        save.Write((int)level);
        save.Write(is_unlocked);

        save.Write(talents.Count);
        foreach (var v in talents)
            v.Save(save);
    }
    internal void Load(BinaryReader save)
    {
        level = (SkillExpertiseLevel)save.ReadInt32();
        is_unlocked = save.ReadBoolean();

        int size = save.ReadInt32();
        talents = new(size);
        for (int i = 0; i < size; ++i)
        {
            SkillTalentData v = new();
            v.Load(save);
            talents.Add(v);
        }
    }
    public SkillExpertiseData()
    {
        talents = new();
    }

    internal void Tick()
    {
        foreach (SkillTalentData skill in talents)
            skill.Tick();
    }

}

public enum SkillTalentRequirement
{
    None,
    BluntWeapon,
    AxeWeapon,
    HeavyArmor,
}

public class SkillTalentData
{
    public SkillTalentRequirement requirement;
    public TalentData talent;
    public int current_level = 0;
    public int max_level = 1;
    public bool is_unlocked = false;
    public string description;

    internal void Save(BinaryWriter save)
    {
        save.Write((int) requirement);
        save.Write(talent.prototype.GetType().ToString());
        talent.Save(save);
        save.Write(current_level);
        save.Write(max_level);
        save.Write(is_unlocked);
        save.Write(description);
    }

    internal void Load(BinaryReader save)
    {
        requirement = (SkillTalentRequirement) save.ReadInt32();
        talent = new TalentData((TalentPrototype) Activator.CreateInstance(Type.GetType(save.ReadString())));
        talent.Load(save);
        current_level = save.ReadInt32();
        max_level = save.ReadInt32();
        is_unlocked = save.ReadBoolean();
        description = save.ReadString();
    }

    internal void Tick()
    {
        if (talent != null)
            talent.Tick();
    }
}