using Godot;
using System;
using System.Diagnostics;

public partial class LevelLoader : Node
{
	private int currentLevelCount = 0;
	private int minGridSize = 64;
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

	}

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
		string levelScene = "res://Levels/Level" + currentLevelCount + ".tscn";

		if (ResourceLoader.Exists(levelScene))
		{
			PackedScene nextLevel = GD.Load<PackedScene>(levelScene);
			Node nextLevelNode = nextLevel.Instantiate();
			AddChild(nextLevelNode);
			AlignGrid();
		}
		else
		{
			// todo load win screen
		}
	}

	private void AlignGrid()
	{
		Node currentLevel = GetNode("Level" + currentLevelCount);
		foreach(Node child in currentLevel.GetChildren())
		{
			if (child is Unit)
			{
				Node2D childUnit = (Node2D)child;
				float x = childUnit.Position.X;
				float y = childUnit.Position.Y;
				float newX = Mathf.Ceil(x / 64) * 64 - 32;
				float newY = Mathf.Ceil(y / 64) * 64 - 32;
				childUnit.Position = new Vector2(newX, newY);
			}
		}
	}
}
