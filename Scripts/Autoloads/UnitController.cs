using Godot;
using System;

namespace TurboITB;
public partial class UnitController : Node
{

	private static Unit selectedUnit = null;

	// Getters and Setters	
	public static Unit SelectedUnit { get => selectedUnit; set => selectedUnit = value; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
