using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProjectileData : ActorData
{
    public List<(int x, int y)> path;
    public int path_index = 0;
    public bool is_shoot_by_player = false;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(path.Count);
        foreach (var v in path)
        {
            save.Write(v.x);
            save.Write(v.y);
        }

        save.Write(path_index);
        save.Write(is_shoot_by_player);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        int size = save.ReadInt32();
        path = new(size);
        for (int i = 0; i < size; ++i)
        {
            path.Add((save.ReadInt32(), save.ReadInt32()));
        }

        path_index = save.ReadInt32();
        is_shoot_by_player = save.ReadBoolean();
    }

    public ProjectileData(int x, int y, ActorPrototype prototype = null) : base(x,y,prototype)
    {
        path = new();
    }

    public override void TryToHit(int to_hit, List<(DamageType type, int damage, int armor_penetration)> damage, List<EffectData> effects, List<Type> diseases, List<Type> poisons)
    {

    }

    public List<(DamageType type, int amount, int penetration)> GetDamage()
    {
        List<(DamageType type, int amount, int penetration)> damage = new();
            
        float multiplier = 1f;
        if (is_shoot_by_player == true)
            multiplier = (100 + GameObject.Find("GameData").GetComponent<GameData>().player_data.GetCurrentAdditiveEffectAmount<EffectAddThrowingWeaponDamageRelative>()) / 100f;
        
        foreach(var v in prototype.projectile.damage)
        {
            damage.Add((v.type, (int) (v.amount * multiplier), v.penetration));
        }

        return damage;
    }

    public override void SelectNextAction()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        
        //If currently on a occupied tile or last tile in path explode
        if (path_index >= path.Count - 1 || map_data.IsAccessableTile(path[path_index].x, path[path_index].y, false, this) == false)
        {
            current_action = new ExplodeAction(this, prototype.projectile.damage_radius, GetDamage(), prototype.projectile.explosion_on_impact);
            return;
        }

        ++path_index;    
        if (map_data.CanBeMovedInByActor(path[path_index].x, path[path_index].y, this) == true)
        {
            current_action = new MoveAction(this, path[path_index].x, path[path_index].y, prototype.stats.movement_time); 
        }
        else
        {
            current_action = new MoveAndExplodeAction(this, path[path_index].x, path[path_index].y, prototype.stats.movement_time, prototype.projectile.damage_radius, GetDamage(), prototype.projectile.explosion_on_impact); 
        }             
    }

    public override void OnKill()
    {
        //if (prototype.projectile.explosion_on_impact == true)
            //GameLogger.Log("The " + prototype.name + " explodes.");
        base.OnKill();
    }
}

