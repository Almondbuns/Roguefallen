using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum DiseaseType
{
    CommonDisease,
}


public abstract class DiseasePrototype
{
    public string name;
    public string icon;

    public DiseaseType type;
    public List<List<EffectData>> severeness_effects;

    public DiseasePrototype()
    {
        severeness_effects= new List<List<EffectData>>();
        type = DiseaseType.CommonDisease;
    }
}

public abstract class PoisonPrototype
{
    public string name;
    public string icon;

    public int max_duration = 10000; //Poisons are always time limited
    public List<EffectData> effects;

    public PoisonPrototype()
    {
        effects= new List<EffectData>();
    }
}
