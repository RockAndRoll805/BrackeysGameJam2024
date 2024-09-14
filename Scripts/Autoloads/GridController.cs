using Godot;
using System;
using System.Runtime.CompilerServices;

namespace TurboITB;
public partial class GridController : Node
{
	public static (int X, int Y) GridSize = (0, 0);
	// public static (int X, int Y) GridSize{get => gridSize; set => gridSize = value;}
	public static Node[,] UnitGrid;
	public static Grid CurrentLevel;

	private static Unit selectedUnit = null;
	public static Unit SelectedUnit
	{
		get
		{
			return selectedUnit;
		}
		set
		{
			selectedUnit = value;
			CurrentLevel.UpdateHighlight();
		}
	}

	// public static void HighlightGrid(int[,] selection)
	// {
	// 	for (int y = 0; y < GridSize.X; y++)
	// 	{
	// 		string printer = "";
	// 		for (int x = 0; x < GridSize.Y; x++)
	// 			printer += selection[x, y] + " ";
	// 		GD.Print(printer);
	// 	}
	// }
}
