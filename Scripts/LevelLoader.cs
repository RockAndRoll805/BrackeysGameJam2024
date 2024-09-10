using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class LevelLoader : Node
{
	private int currentLevelCount = 0;
	public Node[,] unitGrid;
	private Node[,] terrainGrid;
	public int gridSizeX = 0;
	public int gridSizeY = 0;

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
			if (child is Unit)
				((Unit)child).Attack();
		}
	}

	// currently this is support for 4k displays
	private void ResizeWindow()
	{
		Camera2D camera = (Camera2D)GetNode("Camera2D");
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
			Node nextLevelNode = nextLevel.Instantiate();
			AddChild(nextLevelNode);
			AlignGridAndPopulate();
		}
		else
		{
			// todo load win screen
		}
	}

	public void HighlightGrid(int[,] selection)
	{
		for (int y = 0; y < gridSizeX; y++)
		{
			string printer = "";
			for (int x = 0; x < gridSizeY; x++)
				printer += selection[x, y] + " ";
			GD.Print(printer);
		}
	}

	
	private void AlignGridAndPopulate()
	{
		unitGrid = new Node[gridSizeX, gridSizeY];
		Node currentLevel = GetNode("Level" + currentLevelCount);
		foreach(Node child in currentLevel.GetChildren())
		{
			if (child is Unit)
			{
				// Align Grid - I am too lazy to align sprites in the editor so this does it programmatically
				Node2D childUnit = (Node2D)child;
				float x = childUnit.Position.X;
				float y = childUnit.Position.Y;
				float newX = Mathf.Ceil(x / 64) * 64 - 32;
				float newY = Mathf.Ceil(y / 64) * 64 - 32;
				childUnit.Position = new Vector2(newX, newY);

				// Populate Grid
				// take X Y coordinates and turn it into position in 2d list
				// the grid in the editor is aligned with the center at 0,0 so add half grid size to not go negative
				float gridX = ((newX - 32) / 64) + gridSizeX / 2;
				float gridY = ((newY - 32) / 64) + gridSizeY / 2;
				unitGrid[(int)gridX, (int)gridY] = childUnit;
				((Unit)childUnit).coordinateX = (int)gridX;
				((Unit)childUnit).coordinateY = (int)gridY;
			}
		}
	}
}
