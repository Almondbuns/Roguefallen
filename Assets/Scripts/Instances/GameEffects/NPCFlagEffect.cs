using UnityEngine;

public class NPCSetFlagEffect : GameEffectPrototype
{
    public string npc_name;
    public string flag;

    public NPCSetFlagEffect(string npc_name, string flag)
    {
        this.npc_name = npc_name;
        this.flag = flag;
    }

    public override void ApplyEffect()
    {      
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        NPCData npc_data;
        bool npc_exists = game_data.npcs.TryGetValue(npc_name, out npc_data);
        if (npc_exists == false)
        {
            npc_data = new NPCData(npc_name);
            game_data.npcs.Add(npc_name, npc_data);
        }
        
        npc_data.AddFlag(flag);
    }
}
