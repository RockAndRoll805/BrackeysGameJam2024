using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class Highlight : Node
{
	Sprite2D topLeft;
	Sprite2D topRight;
	Sprite2D bottomLeft;
	Sprite2D bottomRight;

	double elapsedTime = 0.0;
	int counter = 0;
	const double timer = 1.0 / 60;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		topLeft = (Sprite2D)GetNode("TopLeft");
		topRight = (Sprite2D)GetNode("TopRight");
		bottomLeft = (Sprite2D)GetNode("BottomLeft");
		bottomRight = (Sprite2D)GetNode("BottomRight");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;
		
		if (elapsedTime >= timer)
		{
			bottomLeft.Position = new Vector2(bottomLeft.Position.X, bottomLeft.Position.Y - 1);
			topLeft.Position = new Vector2(topLeft.Position.X + 1, topLeft.Position.Y);
			topRight.Position = new Vector2(topRight.Position.X, topRight.Position.Y + 1);
			bottomRight.Position = new Vector2(bottomRight.Position.X - 1, bottomRight.Position.Y);

			if (++counter > 55)
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
