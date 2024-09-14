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

	public override void _Process(double delta)
	{
		// if(is_selected) {
		// 	// do whatever if unit is selected

		// 	// check to see if unit is still selected and update accordingly
		// 	if(GridController.SelectedUnit.GetInstanceId != this.GetInstanceId)
		// 	{
		// 		is_selected = false; 
		// 	}
		// }

	}

	public void HighlightAttack()
	{
		GD.Print(GridController.GridSize.X);
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
		//GridController.SelectedUnit = GridController.SelectedUnit == this ? null : this;
		if (GridController.SelectedUnit == this)
		{
			this.Modulate = new Color(1f,1f,1f, 1f);
			GridController.SelectedUnit = null;
		}
		else
		{
			if (GridController.SelectedUnit != null)
				GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
			this.Modulate = new Color(1f,1f,1f, 0.8f);
			GridController.SelectedUnit = this;
		}
		// GridController.HighlightGrid(GetAttackRange());
		// is_selected = true;
	}
}
