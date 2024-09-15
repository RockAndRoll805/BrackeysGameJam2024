using Godot;
using System;
using System.Dynamic;
using System.Runtime.Versioning;

namespace TurboITB;

public partial class Grid : Node2D
{
	private (int x, int y) mouseCellCoordinates;
	private Node2D highlightedCell;
	[Export] public int GridSizeX { private get; set; } = 0;
	[Export] public int GridSizeY { private get; set; } = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GridController.GridSize.X = GridSizeX;
		GridController.GridSize.Y = GridSizeY;

		PackedScene highlight = GD.Load<PackedScene>("res://Scenes/GUI/UnitSelectHighlight.tscn");
		highlightedCell = (Node2D)highlight.Instantiate();
		this.AddChild(highlightedCell);
	}

	public override void _Process(double delta)
	{
		(int x, int y) newMouseCellCoordinates = GetMouseCellLocation();
		
		// move the highlighter to new coordinates
		if (newMouseCellCoordinates != mouseCellCoordinates)
		{
			mouseCellCoordinates = newMouseCellCoordinates;
			if (GridController.SelectedUnit == null)
			{
				MoveHighlightedCell();
			}
		}

		// make selected unit follow the mouse
		if(this.GetNodeOrNull<Sprite2D>("Sprite2D") != null)
		{
			GetNodeOrNull<Sprite2D>("Sprite2D").Position = ConvertCellLocToXY(GetMouseCellLocation());
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// clicking off of a unit to deselect
		if (@event is InputEventMouseButton eventKey)
			if (eventKey.Pressed && eventKey.ButtonIndex <= MouseButton.Right
			&& GridController.SelectedUnit != null)
			{
				// remove unit from former position in grid
				GridController.UnitGrid[GridController.SelectedUnit.Coordinates.X, GridController.SelectedUnit.Coordinates.Y] = null;

				// update position to new location
				Node2D spritePos = this.GetNodeOrNull<Sprite2D>("Sprite2D");
				int gridX = (int)(((spritePos.Position.X - 32) / 64) + GridController.GridSize.X / 2);
				int gridY = (int)(((spritePos.Position.Y - 32) / 64) + GridController.GridSize.Y / 2);
				GridController.SelectedUnit.Coordinates =  (gridX, gridY);
				GridController.SelectedUnit.Position = spritePos.Position;

				GridController.UnitGrid[gridX, gridY] = GridController.SelectedUnit;

				// return selected unit to normal
				GridController.SelectedUnit.GetNode<Button>("Button").ReleaseFocus();
				GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);

				// clean up
				GridController.SelectedUnit = null;
				this.GetNodeOrNull<Sprite2D>("Sprite2D")?.QueueFree();

				HighlightAttacks();
			}
	}

	public void HighlightAttacks()
	{
		foreach(Node child in GetChildren())
			if (child is ColorRect rect)
				child.QueueFree();

		foreach(Node child in this.GetChildren())
			if (child is Unit unit)
				unit.HighlightAttack();
	}

	public void UpdateCursorHighlight()
	{
		mouseCellCoordinates = GetMouseCellLocation();
		MoveHighlightedCell();
	}

	// get the X Y coordinates of the mouse in terms of what cell the mouse is in
	private (int x, int y) GetMouseCellLocation()
	{
		int mouseCellX = (int)((GetGlobalMousePosition().X + Math.CopySign(64, GetGlobalMousePosition().X)) / 64);
		int mouseCellY = (int)((GetGlobalMousePosition().Y + Math.CopySign(64, GetGlobalMousePosition().Y)) / 64);

		// clamp coordinates to be within the grid
		(int min, int max) rangeX = (GridController.GridSize.X / -2, GridController.GridSize.X / 2);
		(int min, int max) rangeY = (GridController.GridSize.Y / -2, GridController.GridSize.Y / 2);
		return (
			(mouseCellX < rangeX.min) ? rangeX.min : (mouseCellX > rangeX.max) ? rangeX.max : mouseCellX,
			(mouseCellY < rangeY.min) ? rangeY.min : (mouseCellY > rangeY.max) ? rangeY.max : mouseCellY
			);
	}

	private void MoveHighlightedCell()
	{
		highlightedCell.Position = ConvertCellLocToXY((mouseCellCoordinates.x, mouseCellCoordinates.y));
	}

	private Vector2 ConvertCellLocToXY((int x, int y) mouseLocation)
	{
		int newXPos = (int)(mouseLocation.x * 64 - Math.CopySign(32, mouseLocation.x));
		int newYPos = (int)(mouseLocation.y * 64 - Math.CopySign(32, mouseLocation.y));
		return new Vector2(newXPos, newYPos);
	}
}
