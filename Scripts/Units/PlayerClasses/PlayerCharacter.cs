using Godot;
using System;
namespace TurboITB;
public partial class PlayerCharacter : Unit
{
    // leveled stat growth
    public float hp_growth = 0f, power_growth = 0f, defense_growth = 0f, resistance_growth = 0f, skill_growth = 0f, speed_growth = 0f, luck_growth = 0f;

    public int exp = 0;
    // public Weapon equipped_weapon;
    public Ability ability_offense, ability_defense, ability_utility, ability_ultimate;
    public Ability[] ability_arr;

    public PlayerCharacter()
    {
        ability_arr = new Ability[4]{ability_offense, ability_defense, ability_utility, ability_ultimate};
    }
    public override void Death()
    {
        
    }

    private void LevelUp()
    {

    }
}
