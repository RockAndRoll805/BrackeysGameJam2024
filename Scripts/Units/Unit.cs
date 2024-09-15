using Godot;
using System;

namespace TurboITB;

public partial class Unit : Node2D
{
	[Export]
	public int facing_direction = 0; // the direction the unit is facing -- 0 North, 1 East, 2 South, 3 West
	protected (int X, int Y) coordinates = (0, 0); // the position of the unit on the grid in terms of (X,Y) coordinates
	protected bool is_selected = false;

	// gameplay variables
	[Export]
	protected int min_range = 0; // the minimum range of the unit
	[Export] 
	protected int max_range = 5; // the maximum range of the unit

	// gameplay assets


	// GETTERS and SETTERS
	public (int X, int Y) Coordinates { get => coordinates; set => coordinates = value; }
	public int FacingDirection { get => facing_direction; set => facing_direction = value; } // 0 North, 1 East, 2 South, 3 West
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Rotate(Mathf.DegToRad(90 * (int)facing_direction));
		
		// Add signals to class
		GetNode<Button>("Button").Pressed += SelectUnit;
	}

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

	public void SelectUnit()
	{
		if (GridController.SelectedUnit == this)
		{
			GridController.CurrentLevel.GetNodeOrNull("Sprite2D")?.QueueFree();

			this.Modulate = new Color(1f,1f,1f, 1f);
			GridController.SelectedUnit = null;
		}
		else
		{
			Sprite2D ghost = new Sprite2D();
			ghost.Scale = this.GetNode<Sprite2D>("Sprite2D").Scale;
			ghost.Texture = this.GetNode<Sprite2D>("Sprite2D").Texture;
			ghost.Position = this.Position;
			ghost.Name = "Sprite2D";
			GridController.CurrentLevel.AddChild(ghost);

			if (GridController.SelectedUnit != null)
				GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
			this.Modulate = new Color(1f,1f,1f, 0.8f);
			GridController.SelectedUnit = this;
		}
	}
}
