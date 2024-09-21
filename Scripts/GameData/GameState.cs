using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameState : Node
{
    private static System.Random stat_random = new System.Random();
    private static System.Random floor_random = new System.Random();
    private static System.Random combat_random = new System.Random();
    private static System.Random enemy_gen_random = new System.Random();
    private static System.Random merchant_random = new System.Random();

    public static PlayerCharacter[] character_arr = new PlayerCharacter[3];

    public int floor, gold = 0;

    public void CheckFailureState()
    {
        if (IsPartyEmpty())
        {
            // TODO
        }
    }

    public void AssignPartyMember(int index, PlayerCharacter character)
    {
        character_arr[index] = character;
    }

    /// <summary>
    /// Checks if party is empty
    /// </summary>
    /// <returns>Boolean</returns>
    public bool IsPartyEmpty()
    {
        return GetPartySize() == 0;
    }

    /// <summary>
    /// Gets the amount of characters in a party
    /// </summary>
    /// <returns>Count of characters</returns>
    public int GetPartySize()
    {
        int count = 0;
        for (int x = 0; x < character_arr.Length; x++)
        {
            if (character_arr[x] != null)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Gets the combined weighted luck value of all party members
    /// </summary>
    /// <returns>Integer of luck of all party members</returns>
    public int GetPartyLuck()
    {
        int luck_total = 0;
        for (int x = 0; x < character_arr.Length; x++)
        {
            if (character_arr[x] != null)
            {
                luck_total += character_arr[x].luck;
            }
        }

        return (int)(character_arr.Length / GetPartySize()) * luck_total;
    }

    /// <summary>
    /// Random generation used for leveling up
    /// </summary>
    /// <returns>Random double from 0-1</returns>
    public double RandomStat()
    {
        return stat_random.NextDouble();
    }

    /// <summary>
    /// Random generation used for generating floors
    /// </summary>
    /// <returns>Random double from 0-1</returns>
    public double RandomFloor()
    {
        return floor_random.NextDouble();
    }

    /// <summary>
    /// Random generation used for randomness during combat encounters
    /// </summary>
    /// <returns>Random double from 0-1</returns>
    public double RandomCombat()
    {
        return combat_random.NextDouble();
    }

    /// <summary>
    /// Random generation used for generating enemies
    /// </summary>
    /// <returns>Random double from 0-1</returns>
    public double RandomEnemy()
    {
        return enemy_gen_random.NextDouble();
    }

    /// <summary>
    /// Random generation used for generating merchant items
    /// </summary>
    /// <returns>Random double from 0-1</returns>
    public double RandomMerchant()
    {
        return merchant_random.NextDouble();
    }

    /// <summary>
    /// Initializes new instance of random class
    /// </summary>
    /// <param name="seed">Seed value for random generation</param>
    public static void UpdateSeed(int seed)
    {
        GD.Print("update " + seed);
        stat_random = new System.Random(seed);
        floor_random = new System.Random(seed);
        combat_random = new System.Random(seed);
        enemy_gen_random = new System.Random(seed);
        merchant_random = new System.Random(seed);
    }
}
