using Godot;
using System;

public partial class HealthComponent : Node {

    [Export] int max_health = 100;
    private int curr_health;

    [Export] bool can_overheal = false; // determines whether or not the character can be overhealed (in which case health will decay at a constant rate back to max_health)

    public int MaxHealth { get => max_health; set => max_health = value; }
    public int CurrHealth { get => curr_health; set => curr_health = value; }

    public override void _Ready() {
        
    }

    public override void _Process(double delta) {

        if(curr_health <= 0) { // unit has died
            GD.Print("Unit Died!");
            this.GetParent().QueueFree();
        }


    }

    public void InitializeHealth() {
        curr_health = max_health;
    }

    public void Damage(int amount) {
        curr_health -= amount;
    }

    public void Heal(int amount) {
        curr_health =+ amount;
    }

}