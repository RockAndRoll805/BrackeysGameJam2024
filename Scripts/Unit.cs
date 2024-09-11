using Godot;
using System;

public partial class Unit : Node2D
{
	[Export]
	public int FacingDirection { get; set; } = 0; // 0 North, 1 East, 2 South, 3 West
	protected (int x, int y) coordinates = (0, 0);
	protected bool is_selected = false;

	// GETTERS and SETTERS
	public (int x, int y) Coordinates { get => coordinates; set => coordinates = value; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Rotate(Mathf.DegToRad(90 * (int)FacingDirection));
		
		// Add signals to class
		GetNode<Button>("Button").Pressed += SelectUnit;
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
		// GD.Print("clicked");
		is_selected = true;
		((LevelLoader)GetNode("/root/LevelLoader")).HighlightGrid(GetAttackRange());
	}
}
