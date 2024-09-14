using Godot;
using System;
using System.Dynamic;
using System.Runtime.Versioning;

namespace TurboITB;

public partial class Grid : Node
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

		// this is in Grid and not unit because the level is loaded after all of the children are
		foreach(Node child in this.GetChildren())
		{
			if (child is Unit unit)
				unit.HighlightAttack();
		}
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
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventKey)
			if (eventKey.Pressed && eventKey.ButtonIndex <= MouseButton.Right
			&& GridController.SelectedUnit != null)
			{
				GridController.SelectedUnit.GetNode<Button>("Button").ReleaseFocus();
				GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
				GridController.SelectedUnit = null;
			}
	}

	public void UpdateHighlight()
	{
		mouseCellCoordinates = GetMouseCellLocation();
		MoveHighlightedCell();
	}

	// get the X Y coordinates of the mouse in terms of what cell the mouse is in
	private (int x, int y) GetMouseCellLocation()
	{
		int centerX = GetViewport().GetWindow().Size.X / 2;
		int centerY = GetViewport().GetWindow().Size.Y / 2;

		float mouseFromCenterX = GetViewport().GetMousePosition().X - centerX;
		float mouseFromCenterY = GetViewport().GetMousePosition().Y - centerY;

		int mouseCellX = (int)((mouseFromCenterX + Math.CopySign(64, mouseFromCenterX)) / 64);
		int mouseCellY = (int)((mouseFromCenterY + Math.CopySign(64, mouseFromCenterY)) / 64);

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
		int newXPos = (int)(mouseCellCoordinates.x * 64 - Math.CopySign(32, mouseCellCoordinates.x));
		int newYPos = (int)(mouseCellCoordinates.y * 64 - Math.CopySign(32, mouseCellCoordinates.y));
		highlightedCell.Position = new Vector2(newXPos, newYPos);
	}
}
