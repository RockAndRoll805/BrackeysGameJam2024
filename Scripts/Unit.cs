using Godot;
using System;

public partial class Unit : Node2D
{
	[Export]
	public int FacingDirection { get; set; } = 0; // 0 North, 1 East, 2 South, 3 West
	public int coordinateX, coordinateY;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Rotate(Mathf.DegToRad(90 * (int)FacingDirection));
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
		GD.Print("clicked");
		((LevelLoader)GetNode("/root/LevelLoader")).HighlightGrid(GetAttackRange());
	}
}
