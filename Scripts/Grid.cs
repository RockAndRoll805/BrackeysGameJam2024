using Godot;
using System;
using System.Dynamic;

namespace TurboITB;

public partial class Grid : Node
{
	[Export] public int GridSizeX { private get; set; } = 0;
	[Export] public int GridSizeY { private get; set; } = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		((LevelLoader)GetParent()).gridSize.X = GridSizeX;
		((LevelLoader)GetParent()).gridSize.Y = GridSizeY;
	}
}
