using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData
{
    public AIPersonality personality;

    internal virtual void Save(System.IO.BinaryWriter save)
    {
        save.Write((int)personality);
    }

    internal virtual void Load(System.IO.BinaryReader save)
    {
        personality = (AIPersonality)save.ReadInt32();
    }

    public virtual ActionData SelectNextAction()
    {
        return new WaitAction(50);
    }
}

public enum AIPersonality
{
    Normal,
    HitAndRun,
    Passive,
}

public class DumbAI : AIData
{
    public ActorData actor_data;

    public (int x, int y)? current_target;

    internal override void Save(System.IO.BinaryWriter save)
    {
        base.Save(save);

        save.Write(current_target.HasValue);

        if (current_target.HasValue)
        {
            save.Write(current_target.Value.x);
            save.Write(current_target.Value.y);
        }
    }

    internal override void Load(System.IO.BinaryReader save)
    {
        base.Load(save);

        bool b = save.ReadBoolean();
        if (b == true)
        {
            current_target = (save.ReadInt32(), save.ReadInt32());
        }
        else
        {
            current_target = null;
        }
    }

    public DumbAI(ActorData actor_data)
    {
        this.actor_data = actor_data;
        personality = actor_data.prototype.monster.ai_personality;
    }

    public override ActionData SelectNextAction()
    {   
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();

        if (personality == AIPersonality.Passive)
            return new WaitAction(1000);

        int detection_range = 8;
        if (personality == AIPersonality.HitAndRun)
            detection_range = 16;

        if ((Mathf.Abs(game_data.player_data.X - actor_data.X) > detection_range || Mathf.Abs(game_data.player_data.Y - actor_data.Y) > detection_range))
            return new WaitAction(100);

        //TODO: input depending on talent
        TalentInputData input = new()
        {
            source_actor = actor_data,
            target_actor = game_data.player_data,
            target_tiles = new List<(int, int)> { (game_data.player_data.X, game_data.player_data.Y) },
            local_data = game_data.current_map
        };

        //Only look at talents that are ready to use (no cooldown, enough stamina or magic, ...)
        var usable_talents = new List<TalentData>();
        foreach (TalentData talent in actor_data.talents)
        { 
            if (actor_data.CanUseTalent(talent))
                usable_talents.Add(talent);
        }

        //If ready prioritize special talents
        var special_talents = usable_talents.FindAll(t => t.prototype.ai_data.type == TalentAIInfoType.Special);
        foreach(TalentData talent in special_talents)
        {
            //Only select talent if player range is within limits
            if (talent.prototype.ai_data.player_range.HasValue == true)
            {
                if (MathF.Max(MathF.Abs(game_data.player_data.X - actor_data.X),MathF.Abs(game_data.player_data.Y - actor_data.Y)) > talent.prototype.ai_data.player_range)
                continue;
            }

            if (UnityEngine.Random.value > talent.prototype.ai_data.use_probability)
                continue;

            if (talent.prototype.target == TalentTarget.Self)
            {
                bool success = actor_data.ActivateTalent(talent, input);
                return null;
            }

            if (talent.prototype.target == TalentTarget.Tile)
            {
                //Player must be in line of sight
                var test_line = Algorithms.LineofSight((actor_data.X, actor_data.Y), (game_data.player_data.X,game_data.player_data.Y));
                if (game_data.current_map.IsLineofSightBlocked(test_line) == true) continue;

                input.target_tiles = new List<(int, int)> {(game_data.player_data.X, game_data.player_data.Y)};
                bool success = actor_data.ActivateTalent(talent, input);
                return null;
            }
        }

        //Move to target
        if (current_target.HasValue)
        {
            if (actor_data.X == current_target.Value.x && actor_data.Y == current_target.Value.y)
            {
                current_target = null;
            }
        }
        
        int target_x, target_y;

        if (current_target.HasValue)
        {
            {
                target_x = current_target.Value.x;
                target_y = current_target.Value.y;
            }
        }
        else
        {
            target_x = game_data.player_data.X;
            target_y = game_data.player_data.Y;
        }
      
        //Do melee attacks if player is next to actor instead of moving
        if (game_data.player_data.X >= actor_data.X - 1 && game_data.player_data.Y >= actor_data.Y - 1
            && game_data.player_data.X <= actor_data.X + (actor_data.prototype.tile_width - 1) + 1
            && game_data.player_data.Y <= actor_data.Y + (actor_data.prototype.tile_height - 1) + 1)
        {
            if (usable_talents.Count > 0)
            {
                TalentData random_talent = usable_talents[UnityEngine.Random.Range(0, usable_talents.Count)];
                bool success = actor_data.ActivateTalent(random_talent, input);
                if (success == true)
                {
                    if (actor_data.is_currently_hidden == true)
                        actor_data.SetHidden(false);
                }
                if (personality == AIPersonality.HitAndRun)
                {
                    //Run
                    bool done = false;
                    int tries = 0;
                    while (done == false && tries < 1000)
                    {
                        int random_x = UnityEngine.Random.Range(-16, 17);
                        int random_y = UnityEngine.Random.Range(-16, 17);

                        if (game_data.current_map.CanBeMovedInByActor(actor_data.X + random_x, actor_data.Y + random_y, actor_data))
                            current_target = (actor_data.X + random_x, actor_data.Y + random_y); 
                        ++tries;
                    }
                }

                return null;
            }
        }

        //Move to player

        Path path = Algorithms.AStar(game_data.current_map,(actor_data.X, actor_data.Y), 
            (target_x, target_y), false, false, actor_data);
        

        if (path == null || path.path.Count < 1)
            return new WaitAction(50);


        if (actor_data.prototype.can_move == true && game_data.current_map.CanBeMovedInByActor(path.path[0].x, path.path[0].y,actor_data))
        {
            if (actor_data.current_effects.Find(x => x.effect is EffectStun) != null)
                return new WaitAction(50);

            return new MoveAction(actor_data, path.path[0].x, path.path[0].y, actor_data.GetMovementTime());
        }
        
        return new WaitAction(50);
    }
}
