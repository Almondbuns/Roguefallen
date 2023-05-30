using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TalentAIInfoType
{
    CombatMelee,
    Special
}

public enum TalentTarget
{
    Self,
    Tile,
    AdjacentTiles,
}

public enum TalentType
{
    Active,
    Substained,
    Passive
}

public class TalentAIInfo
{
    public TalentAIInfoType type;
    public int? player_range;
    public float use_probability = 1.0f;
}


public abstract class TalentPrototype
{
    public string name = "";
    public string icon = "";
    public string description = "";

    public TalentType type = TalentType.Active;

    public int cooldown;
    public int cooldown_start; // sometimes a talent should not be available the moment the monster is created
    public int cost_stamina;
    public int prepare_time;
    public int recover_time;

    public string prepare_message = "";
    public string action_message = "";

    public TalentTarget target;
    public int target_range;

    public TalentAIInfo ai_data;

    public abstract ActionData CreateAction(TalentInputData input);
}

