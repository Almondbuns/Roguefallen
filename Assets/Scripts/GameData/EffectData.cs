using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public enum EffectDataExecutionTime
{
    START,
    CONTINUOUS,
    END
}

//EffectData is called data instead of prototype because it is highly dynamic in creation details
//like amount and durations that must be saved with the target

public class EffectData
{
    // Use Copy!
    public string name;
    public string icon;

    public DamageType damage_type;
    public float amount;
    public int duration;
    public EffectDataExecutionTime execution_time;

    public virtual EffectData Copy()
    {
        EffectData the_copy = (EffectData) Activator.CreateInstance(this.GetType());

        the_copy.name = this.name;
        the_copy.icon = this.icon;
        the_copy.damage_type = this.damage_type;
        the_copy.amount = this.amount;
        the_copy.duration = this.duration;
        the_copy.execution_time = this.execution_time;

        return the_copy;
    }

    public virtual void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(icon);

        save.Write((int)damage_type);
        save.Write(amount);
        save.Write(duration);
        save.Write((int) execution_time);
    }

    internal virtual void Load(BinaryReader save)
    {
        name = save.ReadString();
        icon = save.ReadString();

        damage_type = (DamageType)save.ReadInt32();
        amount = save.ReadSingle();
        duration = save.ReadInt32();
        execution_time = (EffectDataExecutionTime) save.ReadInt32();
    }
}

public class ActorEffectSpecialCommand : EffectData
{
    //TODO Save/Load(!)
    public CommandData command;

    public override void Save(BinaryWriter save)
    {
        base.Save(save);
        save.Write(command.GetType().ToString());
        command.Save(save);
    }

    internal override void Load(BinaryReader save)
    {
       base.Load(save);
       string type = save.ReadString();
       command = (CommandData) Activator.CreateInstance(Type.GetType(type));
       command.Load(save);
    }


    public void Execute()
    {
        command.Execute();
    }

    public override EffectData Copy()
    {
        ActorEffectSpecialCommand effect = (ActorEffectSpecialCommand)base.Copy();
        effect.command = this.command;
        return effect;
    }
}

public class EffectAddMovementTime : EffectData
{
    public EffectAddMovementTime()
    {
        name = "Slow Down";
        icon = "images/effects/effect_slowdown";
    }
   
}

public class EffectConfusion : EffectData
{
    public EffectConfusion()
    {
        name = "Confusion";
        icon = "images/effects/effect_confusion";
    }
}

public class EffectPassiveBlock : EffectData
{
    public EffectPassiveBlock()
    {
        name = "Passive Block";
        icon = "images/effects/effect_confusion";
    }
}

public class EffectParry : EffectData
{
    public EffectParry()
    {
        name = "Parry Stance";
        icon = "images/effects/effect_confusion";
    }
}

public class EffectCounterStrikeVulnerable : EffectData
{
    public EffectCounterStrikeVulnerable()
    {
        name = "Counter Vulnerable";
        icon = "images/effects/effect_confusion";
    }
}

public class EffectRemoveMovementTimeRelative : EffectData
{
    public EffectRemoveMovementTimeRelative()
    {
        name = "Speed Up";
        icon = "images/effects/effect_speedup";
    }
   
}

public class EffectInterrupt : EffectData
{
    public EffectInterrupt()
    {
        name = "Interrupt";
        icon = "";
    }

}

public class EffectStun : EffectData
{
    public EffectStun()
    {
        name = "Stun";
        icon = "images/effects/effect_slowdown";
    }

}

public class EffectAddHitpoints : EffectData
{
    public EffectAddHitpoints()
    {
        name = "Heal";
        icon = "images/effects/effect_heal";
    }

}

public class EffectSubstractHitpoints : EffectData
{
    public EffectSubstractHitpoints()
    {
        name = "Damage";
        icon = "images/effects/effect_heal";
    }

}

public class EffectAddStamina : EffectData
{
    public EffectAddStamina()
    {
        name = "Refresh Stamina";
        icon = "images/effects/effect_heal";
    }

}

public class EffectSubstractStamina : EffectData
{
    public EffectSubstractStamina()
    {
        name = "Lose Stamina";
        icon = "images/effects/effect_hurt";
    }

}

public class EffectAddMana : EffectData
{
    public EffectAddMana()
    {
        name = "Refresh Mana";
        icon = "images/effects/effect_heal";
    }

}

public class EffectAddRelativeTrapSpotting : EffectData
{
    public EffectAddRelativeTrapSpotting()
    {
        name = "Trap Spotting Chance";
        icon = "";
    }
}

public class EffectMonsterStats : EffectData
{
    public EffectMonsterStats()
    {
        name = "See Monster Attributes";
        icon = "";
    }
}

public class EffectMonsterAdvancedStats : EffectData
{
    public EffectMonsterAdvancedStats()
    {
        name = "See Advanced Monster Attributes";
        icon = "";
    }
}

public class EffectMonsterArmor : EffectData
{
    public EffectMonsterArmor()
    {
        name = "See Monster Armor";
        icon = "";
    }
}

public class EffectMonsterResistances: EffectData
{
    public EffectMonsterResistances()
    {
        name = "See Monster Resistances";
        icon = "";
    }
}

public class EffectAddStrength : EffectData
{
    public EffectAddStrength()
    {
        name = "Strength";
        icon = "";
    }

}

public class EffectAddDexterity : EffectData
{
    public EffectAddDexterity()
    {
        name = "Dexterity";
        icon = "";
    }

}

public class EffectAddVitality : EffectData
{
    public EffectAddVitality()
    {
        name = "Vitality";
        icon = "";
    }

}

public class EffectAddIntelligence : EffectData
{
    public EffectAddIntelligence()
    {
        name = "Intelligence";
        icon = "";
    }

}

public class EffectAddConstitution : EffectData
{
    public EffectAddConstitution()
    {
        name = "Constitution";
        icon = "";
    }

}

public class EffectAddWillpower : EffectData
{
    public EffectAddWillpower()
    {
        name = "Willpower";
        icon = "";
    }

}

public class EffectAddToHit: EffectData
{
    public EffectAddToHit()
    {
        name = "To Hit";
        icon = "";
    }
}

public class EffectAddDodge: EffectData
{
    public EffectAddDodge()
    {
        name = "Dodge";
        icon = "";
    }
}

public class EffectAddMaxDiseaseResistance: EffectData
{
    public EffectAddMaxDiseaseResistance()
    {
        name = "Add Disease Resistance";
        icon = "";
    }
}

public class EffectAddMaxPoisonResistance: EffectData
{
    public EffectAddMaxPoisonResistance()
    {
        name = "Add Poison Resistance";
        icon = "";
    }
}
public class EffectAddMaxInsanityResistance: EffectData
{
    public EffectAddMaxInsanityResistance()
    {
        name = "Add Insanity Resistance";
        icon = "";
    }
}

public class EffectAddMinWeaponDamage : EffectData
{
    public EffectAddMinWeaponDamage()
    {
        name = "Add Minimum Damage";
        icon = "";
    }
}

public class EffectAddAttackTime : EffectData
{
    public EffectAddAttackTime()
    {
        name = "Add Attack Time";
        icon = "";
    }
}

public class EffectAddMaxWeaponDamage : EffectData
{
    public EffectAddMaxWeaponDamage()
    {
        name = "Add Maximum Damage";
        icon = "";
    }
}

public class EffectAddWeaponPenetration : EffectData
{
    public EffectAddWeaponPenetration()
    {
        name = "Add Armor Penetration";
        icon = "";
    }
}

public class EffectAddWeaponFireDamage : EffectData
{
    public EffectAddWeaponFireDamage()
    {
        name = "Add Fire Damage";
        icon = "";
    }
}

public class EffectAddWeaponIceDamage : EffectData
{
    public EffectAddWeaponIceDamage()
    {
        name = "Add Ice Damage";
        icon = "";
    }
}

public class EffectAddWeaponLightningDamage : EffectData
{
    public EffectAddWeaponLightningDamage()
    {
        name = "Add Lightning Damage";
        icon = "";
    }
}

public class EffectAddArmorPhysical: EffectData
{
    public EffectAddArmorPhysical()
    {
        name = "Add Physical Armor";
        icon = "";
    }
}

public class EffectAddArmorElemental : EffectData
{
    public EffectAddArmorElemental()
    {
        name = "Add Elemental Armor";
        icon = "";
    }
}

public class EffectAddArmorMagical : EffectData
{
    public EffectAddArmorMagical()
    {
        name = "Add Magic Armor";
        icon = "";
    }
}

public class EffectAddArmorDurability : EffectData
{
    public EffectAddArmorDurability()
    {
        name = "Add Durability";
        icon = "";
    }
}

public class EffectAddRelativeArmorDurability : EffectData
{
    public EffectAddRelativeArmorDurability()
    {
        name = "Add % Durability";
        icon = "";
    }
}

public class EffectAddThrowingWeaponDamageRelative  : EffectData
{
    public EffectAddThrowingWeaponDamageRelative ()
    {
        name = "Add % Throwing Weapon Damage";
        icon = "";
    }
}

public class EffectAddThrowingWeaponAmountRelative  : EffectData
{
    public EffectAddThrowingWeaponAmountRelative ()
    {
        name = "Add % Throwing Weapon Amount";
        icon = "";
    }
}


