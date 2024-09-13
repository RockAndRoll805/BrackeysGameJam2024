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
	[Export] 
	protected Texture2D range_circle_sprite;
	protected Sprite2D range_circle;

	// GETTERS and SETTERS
	public (int X, int Y) Coordinates { get => coordinates; set => coordinates = value; }
	public int FacingDirection { get => facing_direction; set => facing_direction = value; } // 0 North, 1 East, 2 South, 3 West
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Rotate(Mathf.DegToRad(90 * (int)facing_direction));
		
		// Add signals to class
		GetNode<Button>("Button").Pressed += SelectUnit;
       
	   	// Initialize Range Indicator
	    range_circle = new()
        {
            Texture = range_circle_sprite,
            Modulate = new(1.0f, 1.0f, 1.0f, 0.5f),
            GlobalPosition = this.GlobalPosition,
            Scale = new(max_range, max_range),
			Visible = false,
        };
		this.AddChild(range_circle);
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

        // display range circle
        range_circle.Visible = true;


        ((LevelLoader)GetNode("/root/LevelLoader")).HighlightGrid(GetAttackRange());
	}
}
