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
    public bool show_amount_info = true;

    public DamageType damage_type;
    public float amount;
    public int duration;
    public EffectDataExecutionTime execution_time;

    public virtual EffectData Copy()
    {
        EffectData the_copy = (EffectData) Activator.CreateInstance(this.GetType());

        the_copy.name = this.name;
        the_copy.icon = this.icon;
        the_copy.show_amount_info = this.show_amount_info;
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
        save.Write(show_amount_info);

        save.Write((int)damage_type);
        save.Write(amount);
        save.Write(duration);
        save.Write((int) execution_time);
    }

    internal virtual void Load(BinaryReader save)
    {
        name = save.ReadString();
        icon = save.ReadString();
        show_amount_info = save.ReadBoolean();

        damage_type = (DamageType)save.ReadInt32();
        amount = save.ReadSingle();
        duration = save.ReadInt32();
        execution_time = (EffectDataExecutionTime) save.ReadInt32();
    }
}

public class ActorEffectSpecialCommand : EffectData
{
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
        name = "Move";
        icon = "images/effects/effect_slowdown";
    }
   
}

public class EffectConfusion : EffectData
{
    public EffectConfusion()
    {
        name = "Confused";
        icon = "images/effects/effect_confusion";
        show_amount_info = false;
    }
}

public class EffectExhaustion : EffectData
{
    public EffectExhaustion()
    {
        name = "Exhausted";
        icon = "images/effects/effect_exhaustion";
        show_amount_info = false;
    }
}

public class EffectPassiveBlock : EffectData
{
    public EffectPassiveBlock()
    {
        name = "Passive Block";
        icon = "images/effects/effect_block_active";
    }
}

public class EffectActiveBlock : EffectData
{
    public EffectActiveBlock()
    {
        name = "Active Block";
        icon = "images/effects/effect_block_active";
        show_amount_info = false;
    }
}

public class EffectParryChance : EffectData
{
    public EffectParryChance()
    {
        name = "Parry Stance";
        icon = "images/effects/effect_parry";
        show_amount_info = false;
    }
}

public class EffectParryChanceBonus : EffectData
{
    public EffectParryChanceBonus()
    {
        name = "Parry Stance Bonus";
        icon = "images/effects/effect_parry";
        show_amount_info = false;
    }
}

public class EffectCounterStrikeVulnerable : EffectData
{
    public EffectCounterStrikeVulnerable()
    {
        name = "Vulnerable";
        icon = "images/effects/effect_confusion";
        show_amount_info = false;
    }
}

public class EffectBashDamageRelative : EffectData
{
    public EffectBashDamageRelative()
    {
        name = "Bash Damage";
        icon = "images/effects/effect_confusion";
        show_amount_info = false;
    }
}

public class EffectDazeTime : EffectData
{
    public EffectDazeTime()
    {
        name = "Daze Time";
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
        show_amount_info = false;
    }

}

public class EffectStun : EffectData
{
    public EffectStun()
    {
        name = "Stunned";
        icon = "images/effects/effect_slowdown";
        show_amount_info = false;
    }

}

public class EffectAddHitpoints : EffectData
{
    public EffectAddHitpoints()
    {
        name = "Heal";
        icon = "images/effects/effect_heal";
        show_amount_info = false;
    }

}

public class EffectSubstractHitpoints : EffectData
{
    public EffectSubstractHitpoints()
    {
        name = "Damage";
        icon = "images/effects/effect_heal";
        show_amount_info = false;
    }

}

public class EffectAddStamina : EffectData
{
    public EffectAddStamina()
    {
        name = "Refresh Stamina";
        icon = "images/effects/effect_heal";
        show_amount_info = false;
    }

}

public class EffectSubstractStamina : EffectData
{
    public EffectSubstractStamina()
    {
        name = "Lose Stamina";
        icon = "images/effects/effect_hurt";
        show_amount_info = false;
    }

}

public class EffectAddMana : EffectData
{
    public EffectAddMana()
    {
        name = "Refresh Mana";
        icon = "images/effects/effect_heal";
        show_amount_info = false;
    }

}

public class EffectAddRelativeTrapSpotting : EffectData
{
    public EffectAddRelativeTrapSpotting()
    {
        name = "Trap Spotting Chance";
        icon = "";
        show_amount_info = false;
    }
}

public class EffectMonsterStats : EffectData
{
    public EffectMonsterStats()
    {
        name = "See Monster Attributes";
        icon = "";
        show_amount_info = false;
    }
}

public class EffectMonsterAdvancedStats : EffectData
{
    public EffectMonsterAdvancedStats()
    {
        name = "See Advanced Monster Attributes";
        icon = "";
        show_amount_info = false;
    }
}

public class EffectMonsterArmor : EffectData
{
    public EffectMonsterArmor()
    {
        name = "See Monster Armor";
        icon = "";
        show_amount_info = false;
    }
}

public class EffectMonsterResistances: EffectData
{
    public EffectMonsterResistances()
    {
        name = "See Monster Resistances";
        icon = "";
        show_amount_info = false;
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
        name = "Disease Res";
        icon = "";
    }
}

public class EffectAddMaxPoisonResistance: EffectData
{
    public EffectAddMaxPoisonResistance()
    {
        name = "Poison Res";
        icon = "";
    }
}
public class EffectAddMaxInsanityResistance: EffectData
{
    public EffectAddMaxInsanityResistance()
    {
        name = "Insanity Res";
        icon = "";
    }
}

public class EffectAddMinWeaponDamage : EffectData
{
    public EffectAddMinWeaponDamage()
    {
        name = "Minimum Damage";
        icon = "";
    }
}

public class EffectAddAttackTime : EffectData
{
    public EffectAddAttackTime()
    {
        name = "Attack Time";
        icon = "";
    }
}

public class EffectAddMaxWeaponDamage : EffectData
{
    public EffectAddMaxWeaponDamage()
    {
        name = "Maximum Damage";
        icon = "";
    }
}

public class EffectAddWeaponPenetration : EffectData
{
    public EffectAddWeaponPenetration()
    {
        name = "Penetration";
        icon = "";
    }
}

public class EffectAddWeaponFireDamage : EffectData
{
    public EffectAddWeaponFireDamage()
    {
        name = "Fire Damage";
        icon = "";
    }
}

public class EffectAddWeaponIceDamage : EffectData
{
    public EffectAddWeaponIceDamage()
    {
        name = "Ice Damage";
        icon = "";
    }
}

public class EffectAddWeaponLightningDamage : EffectData
{
    public EffectAddWeaponLightningDamage()
    {
        name = "Lightning Damage";
        icon = "";
    }
}

public class EffectAddArmorPhysical: EffectData
{
    public EffectAddArmorPhysical()
    {
        name = "Physical Armor";
        icon = "";
    }
}

public class EffectAddArmorElemental : EffectData
{
    public EffectAddArmorElemental()
    {
        name = "Elemental Armor";
        icon = "";
    }
}

public class EffectAddArmorMagical : EffectData
{
    public EffectAddArmorMagical()
    {
        name = "Magic Armor";
        icon = "";
    }
}

public class EffectAddArmorDurability : EffectData
{
    public EffectAddArmorDurability()
    {
        name = "Durability";
        icon = "";
    }
}

public class EffectAddRelativeArmorDurability : EffectData
{
    public EffectAddRelativeArmorDurability()
    {
        name = "% Durability";
        icon = "";
    }
}

public class EffectAddThrowingWeaponDamageRelative  : EffectData
{
    public EffectAddThrowingWeaponDamageRelative ()
    {
        name = "% Throwing Weapon Damage";
        icon = "";
    }
}

public class EffectAddThrowingWeaponAmountRelative  : EffectData
{
    public EffectAddThrowingWeaponAmountRelative ()
    {
        name = "% Throwing Weapon Amount";
        icon = "";
    }
}


