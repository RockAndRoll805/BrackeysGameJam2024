using Godot;
using System;
using System.Threading.Tasks;

namespace TurboITB;

public partial class Unit : Node2D
{
	public string unit_name;

    public StatusEffect[] status_arr;

    // public Weapon weapon = null;

	// leveled stats
    protected int power = 0, defense = 0, resistance = 0, skill = 0, speed = 0, luck = 0;
    protected int hp_base = 0, hp_current = 0, hp_mod_add = 0, hp_mod_mult = 1;

    // unleveled stats modifiers
    protected int accuracy_mod, evasion_mod = 0, crit_chance_mod = 0, crit_damage_mod = 0;

    /// <summary>
    /// Maximum health value after modifiers
    /// </summary>
    /// <value></value>
    protected int hp_max
    {
        get
        {
            int ret = (hp_base + hp_mod_add) * hp_mod_mult;
            return (ret <= 0) ? 1 : ret;
        }
    }

    protected int evasion { get => evasion_mod + skill; }


    /// <summary>
    /// Set unit's hp to a value
    /// </summary>
    /// <param name="heatlh">New HP value</param>
    public void SetHp(int heatlh)
    {
        hp_current = heatlh;
    }

    /// <summary>
    /// Apply heal without accuracy check. Can not overheal
    /// </summary>
    /// <param name="amount">Health to be restored</param>
    public void Heal(int amount)
    {
        hp_current += amount;
        if (hp_base > hp_max) { hp_current = hp_max; }
    }

    /// <summary>
    /// Apply damage without performing accuracy check
    /// </summary>
    /// <param name="amount">Damage to be inflicted</param>
    public void Damage(int amount)
    {
        amount -= defense;
        hp_current -= (amount > 0) ? amount : 0;
        if (hp_current <= 0){ Death(); }
    }

    public int Attack(Unit[] unit_arr)
    {
        return 0;
    }

    public int HealAttack(Unit[] unit_arr)
    {
        return 0;
    }

    public virtual void Death()
    {
        
    }


	protected (int X, int Y) coordinates = (0, 0); // the position of the unit on the grid in terms of (X,Y) coordinates
	public (int X, int Y) Coordinates { get => coordinates; set => coordinates = value; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Add signals to class
		GetNode<Button>("Button").Pressed += SelectUnit;
	}

	// TODO: replace this with a better system or rewrite in grid is set to isometric
	public void HighlightAttack()
	{
		int[,] attackRange = GetAttackRange();
		for (int y = 0; y < GridController.GridSize.Y; y++)
		{
			for (int x = 0; x < GridController.GridSize.X; x++)
			{
				ColorRect rect = new ColorRect();
				rect.MouseFilter = Control.MouseFilterEnum.Ignore;
				rect.Size = new Vector2(64, 64);
				rect.Position = new Vector2(64 * (x - 2), 64 * (y - 2));

				switch(attackRange[x, y])
				{
					case(1):
						rect.Color = new Color(1f, 0f, 0f, 0.2f);
						GridController.CurrentLevel.AddChild(rect);
						break;
					case(2):
						rect.SelfModulate = new Color(1f, 0f, 0f, 0.5f);
						GridController.CurrentLevel.AddChild(rect);
						break;
				}
			}
		}
	}

	public virtual int[,] GetAttackRange()
	{
		GD.Print("this is not override");
		return null;
	}

	public virtual void Attack()
	{

	}

	// TODO: replace this with a better system or rewrite in grid is set to isometric
	public void SelectUnit()
	{
		if (GridController.SelectedUnit is not null)
		{
			GridController.CurrentLevel.GetNodeOrNull("Sprite2D")?.QueueFree();

			GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
			GridController.SelectedUnit = null;
		}
		else
		{
			Sprite2D ghost = new() {
				Scale = this.GetNode<Sprite2D>("Sprite2D").Scale,
				Texture = this.GetNode<Sprite2D>("Sprite2D").Texture,
				Hframes = 4,
				Rotation = GetNode<Sprite2D>("Sprite2D").Rotation,
				Position = this.Position,
				Name = "Sprite2D",
				// Frame = this.FacingDirection
			};

			GridController.CurrentLevel.AddChild(ghost);

			if (GridController.SelectedUnit is not null)
				GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
			this.Modulate = new Color(1f,1f,1f, 0.8f);
			GridController.SelectedUnit = this;
		}
	}
}
