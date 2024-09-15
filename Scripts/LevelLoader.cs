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
		GetNode<Button>("AttackButton").Pressed += DoAttack;
		GetNode<Button>("ResetLevelButton").Pressed += ResetLevel;

		LoadNextLevel();
		GetTree().GetRoot().SizeChanged += ResizeWindow;
		ResizeWindow();
	}

	public void DoAttack()
	{
		foreach(Node child in GridController.CurrentLevel.GetChildren())
		{
			if (child is not null && child is Unit unit)
			{
				unit.Attack();
			}
				
		}
	}

	public void ResetLevel()
	{
		GetNode<Grid>("Level" + currentLevelCount).Free();
		GridController.UnitGrid = null;
		GridController.SelectedUnit = null;
		GridController.CurrentLevel = null;
		LoadCurrentLevel();
	}

	// currently this is support for 4k displays
	private void ResizeWindow()
	{
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		if (viewportSize.X > 1920 && viewportSize.Y > 1080)
			camera.Zoom = new Vector2(viewportSize.X / 1920, viewportSize.Y / 1080);
	}

	private bool LoadCurrentLevel()
	{
		string levelScene = "res://Scenes/Levels/Level" + currentLevelCount + ".tscn";
		if (!ResourceLoader.Exists(levelScene))
			return false;
		PackedScene nextLevel = GD.Load<PackedScene>(levelScene);
		Grid nextLevelNode = (Grid)nextLevel.Instantiate();
		GD.Print("did we make it here?");
		GridController.CurrentLevel = nextLevelNode;
		AddChild(nextLevelNode);
		AlignGridAndPopulate();
		GridController.CurrentLevel.HighlightAttacks();
		return true;
	}

	private void LoadNextLevel()
	{
		Node currentLevel = GetNodeOrNull("Level" + currentLevelCount);
		if (currentLevel != null)
		{
			currentLevel.QueueFree();
		}

		currentLevelCount++;

		if (!LoadCurrentLevel())
		{
			// todo load win screen
		}
	}

	
	private void AlignGridAndPopulate()
	{
		GridController.UnitGrid = new Node[GridController.GridSize.X, GridController.GridSize.Y];
		Node currentLevel = GetNode("Level" + currentLevelCount);
		int counter = 1;
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

				RichTextLabel attackOrder = new RichTextLabel();
				attackOrder.Size = new Vector2(64, 64);
				attackOrder.PushColor(new Color(0, 1, 0, 1));
				attackOrder.PushBold();
				attackOrder.AddText(counter++ + "");
				attackOrder.Position = new Vector2(-32, -32);
				attackOrder.MouseFilter = Control.MouseFilterEnum.Ignore;
				attackOrder.ZIndex = 1;
				GD.Print(unit.Name);
				unit.AddChild(attackOrder);
			}
		}
	}
}
