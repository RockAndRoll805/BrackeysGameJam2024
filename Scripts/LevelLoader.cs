using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TurboITB;

public partial class LevelLoader : Node
{
	private int currentLevelCount = 0;
	public Node[,] unitGrid;
	private Node[,] terrainGrid;
	public (int X, int Y) gridSize = (0, 0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadNextLevel();
		GetTree().GetRoot().SizeChanged += ResizeWindow;
		ResizeWindow();
	}

	public void DoAttack()
	{
		foreach(Node child in GetChildren())
		{
			if (child is Unit unit)
				unit.Attack();
		}
	}

	// currently this is support for 4k displays
	private void ResizeWindow()
	{
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		if (viewportSize.X > 1920 && viewportSize.Y > 1080)
			camera.Zoom = new Vector2(viewportSize.X / 1920, viewportSize.Y / 1080);
	}

	private void LoadNextLevel()
	{
		Node currentLevel = GetNodeOrNull("Level" + currentLevelCount);
		if (currentLevel != null)
		{
			RemoveChild(currentLevel);
		}

		currentLevelCount++;
		string levelScene = "res://Scenes/Levels/Level" + currentLevelCount + ".tscn";

		if (ResourceLoader.Exists(levelScene))
		{
			PackedScene nextLevel = GD.Load<PackedScene>(levelScene);
			Grid nextLevelNode = (Grid)nextLevel.Instantiate();
			AddChild(nextLevelNode);
			GridController.CurrentLevel = nextLevelNode;
			AlignGridAndPopulate();
			GridController.CurrentLevel.HighlightAttacks();
		}
		else
		{
			// todo load win screen
		}
	}

	
	private void AlignGridAndPopulate()
	{
		GridController.UnitGrid = new Node[GridController.GridSize.X, GridController.GridSize.Y];
		Node currentLevel = GetNode("Level" + currentLevelCount);
		foreach(Node child in currentLevel.GetChildren())
		{
			if (child is Unit unit)
			{
				// Align Grid - I am too lazy to align sprites in the editor so this does it programmatically
				float x = unit.Position.X;
				float y = unit.Position.Y;
				float newX = Mathf.Ceil(x / 64) * 64 - 32;
				float newY = Mathf.Ceil(y / 64) * 64 - 32;
				unit.Position = new Vector2(newX, newY);
				
				// Populate Grid
				// take X Y coordinates and turn it into position in 2d list
				// the grid in the editor is aligned with the center at 0,0 so add half grid size to not go negative
				float gridX = ((newX - 32) / 64) + GridController.GridSize.X / 2;
				float gridY = ((newY - 32) / 64) + GridController.GridSize.Y / 2;
				GridController.UnitGrid[(int)gridX, (int)gridY] = unit;
				unit.Coordinates = ((int)gridX, (int)gridY);
			}
		}
	}
}
