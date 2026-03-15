using UnityEngine;

public class NPCFlagCheck : GameConditionPrototype
{
    public string npc_name;
    public string flag;

    public NPCFlagCheck(string npc_name, string flag)
    {
        this.npc_name = npc_name;
        this.flag = flag;
    }

    public override bool Evaluate()
    {
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        NPCData npc_data;
        bool npc_exists = game_data.npcs.TryGetValue(npc_name, out npc_data);
        if (npc_exists == false) return false;
        return npc_data.ContainsFlag(flag);
    }
}
