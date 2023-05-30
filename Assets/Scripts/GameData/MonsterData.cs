using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Drawing;

public class MonsterData : ActorData
{
    public AIData ai;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        ai.Save(save);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        ai = new DumbAI(this);
        ai.Load(save);
    }

    public MonsterData(int x, int y, ActorPrototype prototype) : base(x,y,prototype)
    {
        if (prototype != null)
            ai = new DumbAI(this);
    }

    public override void SelectNextAction()
    {
        ActionData action = null;
        if (ai != null)
            action = ai.SelectNextAction();

        if (action != null)
            current_action = action;
    }

    public override void OnKill()
    {
        if (is_dead == true) //only count deaths once
        return;

        GameLogger.Log("<color=#FF0000>The " + prototype.name + " dies.</color>");
        base.OnKill();

        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        if (prototype.stats.kill_experience > 0)
        {
            GameLogger.Log("<color=#3333FF>" + player.prototype.name + " gains " + prototype.stats.kill_experience + " experience.</color>");
            player.GainExperience(prototype.stats.kill_experience);
        }
    }
}
