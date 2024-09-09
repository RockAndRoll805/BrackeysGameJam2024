using Godot;
using System;

public partial class Unit : Node2D
{
	[Export]
	public int FacingDirection { private get; set; } = 0; // 0 North, 1 East, 2 South, 3 West
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Rotate(Mathf.DegToRad(90 * (int)FacingDirection));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
