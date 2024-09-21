using Godot;
using System;
using AbilityEnum;
using WeaponEnum;
namespace TurboITB;

public partial class Ability : Node
{
    public String ability_name;
    public String description;
    public AbilityType ability_type;
    public int turn_cooldown;
    public int encounter_cooldown;
    public int upgrade_level;
    public String[] class_restriction = null;
    // public WeaponType weapon_incompatability;
}
