using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFirebomb : ActorPrototype
{
    public ProjectileFirebomb(int level) : base(level)
    {
        name = "Firebomb";
        icon = "images/objects/firebomb";

        if (level <= 4)
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.FIRE, 5, 0) },
                damage_radius = 1,
                explosion_on_impact = true,
            };
        }
        else
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.FIRE, 10, 0) },
                damage_radius = 1,
                explosion_on_impact = true,
            };
        }

        stats.health_max = 5;

        stats.body_armor.Add(new ArmorStats { body_part = "Projectile", percentage = 100, armor = (0, 0, 0), durability_max = 0 });

        stats.movement_time = 50;
        stats.to_hit = 20;
    }
}

public class ProjectileThrowingKnife: ActorPrototype
{
    public ProjectileThrowingKnife(int level) : base(level)
    {
        name = "Throwing Knife";
        icon = "images/objects/throwing_knife";

        if (level <= 4)
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.PIERCE, 7, 4) },
                damage_radius = 0,
            };
        }
        else
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.PIERCE, 14, 7) },
                damage_radius = 0,
            };
        }

        stats.health_max = 5;

        stats.body_armor.Add(new ArmorStats { body_part = "Projectile", percentage = 100, armor = (0, 0, 0), durability_max = 0 });

        stats.movement_time = 25;
        stats.to_hit = 20;
    }
}

public class ProjectileArrow: ActorPrototype
{
    public ProjectileArrow(int level) : base(level)
    {
        name = "Arrow";
        icon = "images/objects/arrow";

        if (level <= 4)
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.PIERCE, 3, 2) },
                damage_radius = 0,
            };
        }
        else
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.PIERCE, 3, 2) },
                damage_radius = 0,
            };
        }

        stats.health_max = 2;

        stats.body_armor.Add(new ArmorStats { body_part = "Projectile", percentage = 100, armor = (0, 0, 0), durability_max = 0 });

        stats.movement_time = 10;
        stats.to_hit = 10;
    }
}

public class ProjectileAcidFlask : ActorPrototype
{
    public ProjectileAcidFlask(int level) : base(level)
    {
        name = "Acid Flask";
        icon = "images/objects/acid_flask";

        if (level <= 4)
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.DURABILITY, 15, 0) },
                damage_radius = 0,
                explosion_on_impact = true,
            };
        }
        else
        {
            projectile = new ProjectilePrototype
            {
                damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.DURABILITY, 25, 0) },
                damage_radius = 0,
                explosion_on_impact = true,
            };
        }

        stats.health_max = 5;

        stats.body_armor.Add(new ArmorStats { body_part = "Projectile", percentage = 100, armor = (0, 0, 0), durability_max = 0 });

        stats.movement_time = 50;
        stats.to_hit = 20;
    }
}

public class ItemProjectile : ActorPrototype
{
    public ItemProjectile(int level) : base(0)
    {
      //Needs to exist because of loading
    }

    public void CreateProjectile(ItemData item)
    {
        name = item.GetName();
        icon = item.GetPrototype().icon;

        projectile = new ProjectilePrototype
        {
            damage = new List<(DamageType type, int amount, int penetration)> { (DamageType.PIERCE, 4 * item.GetPrototype().weight, 0) },
            damage_radius = 0,
            explosion_on_impact = false,
        };

        stats.health_max = 100;

        stats.body_armor.Add(new ArmorStats { body_part = "Projectile", percentage = 100, armor = (0, 0, 0), durability_max = 0 });

        stats.movement_time = 50;
        stats.to_hit = 20;
    }
}
