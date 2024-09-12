using Godot;
using System;

public partial class LineShooter : Unit
{
	// line shooter shoots in a line that it is facing
	// it's attack stops on the first unit hit
    public override int[,] GetAttackRange()
    {
        LevelLoader level = (LevelLoader)GetNode("/root/LevelLoader");
		int[,] selection = new int[level.gridSize.X, level.gridSize.Y];
		
		// I am lazy so just going to do switch here
		switch (FacingDirection)
		{
			case 0: // north
				for(int iter = coordinates.Y - 1; iter >= 0; iter--)
				{
					selection[coordinates.X, iter] = 1;
					if (level.unitGrid[coordinates.X, iter] != null
					&& level.unitGrid[coordinates.X, iter] is Unit)
						break;
				}
				break;
			case 1: // east
				for(int iter = coordinates.X + 1; iter < level.gridSize.X; iter++)
				{
					selection[iter, coordinates.Y] = 1;
					if (level.unitGrid[iter, coordinates.Y] != null
					&& level.unitGrid[iter, coordinates.Y] is Unit)
						break;
				}
				break;
			case 2: // south
				for(int iter = coordinates.Y + 1; iter < level.gridSize.Y; iter++)
				{
					selection[coordinates.X, iter] = 1;
					if (level.unitGrid[coordinates.X, iter] != null
					&& level.unitGrid[coordinates.X, iter] is Unit)
						break;
				}
				break;
			case 3: // west
				for(int iter = coordinates.X - 1; iter > 0; iter--)
				{
					selection[iter, coordinates.Y] = 1;
					if (level.unitGrid[iter, coordinates.Y] != null
					&& level.unitGrid[iter, coordinates.Y] is Unit)
						break;
				}
				break;
		}

		return selection;
    }

    public override void Attack()
	{

	}
}
