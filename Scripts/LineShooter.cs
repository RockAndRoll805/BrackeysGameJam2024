using Godot;
using System;

public partial class LineShooter : Unit
{
	// line shooter shoots in a line that it is facing
	// it's attack stops on the first unit hit
    public override int[,] GetAttackRange()
    {
        LevelLoader level = (LevelLoader)GetNode("/root/LevelLoader");
		int[,] selection = new int[level.gridSizeX, level.gridSizeY];
		
		// I am lazy so just going to do switch here
		switch (FacingDirection)
		{
			case 0: // north
				for(int iter = coordinateY - 1; iter >= 0; iter--)
				{
					selection[coordinateX, iter] = 1;
					if (level.unitGrid[coordinateX, iter] != null
					&& level.unitGrid[coordinateX, iter] is Unit)
						break;
				}
				break;
			case 1: // east
				for(int iter = coordinateX + 1; iter < level.gridSizeX; iter++)
				{
					selection[iter, coordinateY] = 1;
					if (level.unitGrid[iter, coordinateY] != null
					&& level.unitGrid[iter, coordinateY] is Unit)
						break;
				}
				break;
			case 2: // south
				for(int iter = coordinateY + 1; iter < level.gridSizeY; iter++)
				{
					selection[coordinateX, iter] = 1;
					if (level.unitGrid[coordinateX, iter] != null
					&& level.unitGrid[coordinateX, iter] is Unit)
						break;
				}
				break;
			case 3: // west
				for(int iter = coordinateX - 1; iter > 0; iter--)
				{
					selection[iter, coordinateY] = 1;
					if (level.unitGrid[iter, coordinateY] != null
					&& level.unitGrid[iter, coordinateY] is Unit)
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
