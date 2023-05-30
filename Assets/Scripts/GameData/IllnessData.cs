using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class DiseaseData
{
    public DiseasePrototype prototype;
    public int severeness;

    public DiseaseData(DiseasePrototype prototype = null)
    {
        this.prototype = prototype;
        severeness = 0;
    }

    public void Save(BinaryWriter save)
    {
        save.Write(prototype.GetType().ToString());
        save.Write(severeness);
    }

    public void Load(BinaryReader save)
    {
        prototype = (DiseasePrototype) Activator.CreateInstance(Type.GetType(save.ReadString()));
        severeness = save.ReadInt32();
    }

    public List<EffectData> GetCurrentEffects()
    {
        return prototype.severeness_effects[severeness];
    }
}

public class PoisonData
{
    public PoisonPrototype prototype;
    public int duration;

    public PoisonData(PoisonPrototype prototype = null)
    {
        this.prototype = prototype;
        duration = 0;
    }

    public void Save(BinaryWriter save)
    {
        save.Write(prototype.GetType().ToString());
        save.Write(duration);
    }

    public void Load(BinaryReader save)
    {
        prototype = (PoisonPrototype) Activator.CreateInstance(Type.GetType(save.ReadString()));
        duration = save.ReadInt32();
    }

    public List<EffectData> GetCurrentEffects()
    {
        return prototype.effects;
    }

    public void Tick(ActorData actor)
    {
        duration += 1;

        foreach(EffectData effect in prototype.effects)
        {
            if (effect.execution_time == EffectDataExecutionTime.CONTINUOUS && duration % 100 == 0)
            {
                actor.DoEffectOnce(effect);
            }
        }
    }
}