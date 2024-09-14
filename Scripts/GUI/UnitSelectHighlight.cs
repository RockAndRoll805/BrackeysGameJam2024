using Godot;
using System;
using System.Diagnostics.Metrics;

namespace TurboITB;

// This whole class is a hack because I don't have AE
// TODO: replace using this class with an animated sprite
public partial class UnitSelectHighlight : Node2D
{

	Sprite2D topLeft, topRight, bottomLeft, bottomRight;

	double elapsedTime = 0.0;
	int counter = 0;
	const double timer = 1.0 / 60; // 60 fps

	public override void _Ready()
	{
		topLeft = GetNode<Sprite2D>("TopLeft");
		topRight = GetNode<Sprite2D>("TopRight");
		bottomLeft = GetNode<Sprite2D>("BottomLeft");
		bottomRight = GetNode<Sprite2D>("BottomRight");
	}

	public override void _Process(double delta)
	{
		elapsedTime += delta;
		
		if (elapsedTime >= timer)
		{
			bottomLeft.Position = new Vector2(bottomLeft.Position.X, bottomLeft.Position.Y - 2);
			topLeft.Position = new Vector2(topLeft.Position.X + 2, topLeft.Position.Y);
			topRight.Position = new Vector2(topRight.Position.X, topRight.Position.Y + 2);
			bottomRight.Position = new Vector2(bottomRight.Position.X - 2, bottomRight.Position.Y);

			counter += 2;
			if (counter > 55)
			{
				counter = 0;
				bottomLeft.Position = new Vector2(-28, 28);
				topLeft.Position = new Vector2(-28, -28);
				topRight.Position = new Vector2(28, -28);
				bottomRight.Position = new Vector2(28, 28);
			}
			elapsedTime = 0.0;
		}
	}
}
