using Godot;
using System;

namespace TurboITB;

public partial class LineShooter : Unit
{
	// line shooter shoots in a line that it is facing
	// it's attack stops on the first unit hit


	// TODO: remove or rewrite this enemy
	public override int[,] GetAttackRange()
	{
		int[,] selection = new int[GridController.GridSize.X, GridController.GridSize.Y];
		int FacingDirection = 0;
		switch (FacingDirection)
		{
			case 0: // north
				for(int iter = coordinates.Y - 1; iter >= 0; iter--)
				{
					selection[coordinates.X, iter] = 1;
					if (GridController.UnitGrid[coordinates.X, iter] != null
					&& GridController.UnitGrid[coordinates.X, iter] is Unit)
					{
						selection[coordinates.X, iter] = 2;
						break;
					}
				}
				break;
			case 1: // east
				for(int iter = coordinates.X + 1; iter < GridController.GridSize.X; iter++)
				{
					selection[iter, coordinates.Y] = 1;
					if (GridController.UnitGrid[iter, coordinates.Y] != null
					&& GridController.UnitGrid[iter, coordinates.Y] is Unit)
					{
						selection[iter, coordinates.Y] = 2;
						break;
					}
				}
				break;
			case 2: // south
				for(int iter = coordinates.Y + 1; iter < GridController.GridSize.Y; iter++)
				{
					selection[coordinates.X, iter] = 1;
					if (GridController.UnitGrid[coordinates.X, iter] != null
					&& GridController.UnitGrid[coordinates.X, iter] is Unit)
					{
						selection[coordinates.X, iter] = 2;
						break;
					}
				}
				break;
			case 3: // west
				for(int iter = coordinates.X - 1; iter > 0; iter--)
				{
					selection[iter, coordinates.Y] = 1;
					if (GridController.UnitGrid[iter, coordinates.Y] != null
					&& GridController.UnitGrid[iter, coordinates.Y] is Unit)
					{
						selection[coordinates.X, iter] = 2;
						break;
					}
				}
				break;
		}

		return selection;
	}

	// public override void Attack()
	// {

	// }
}
