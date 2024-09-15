using Godot;
using System;
using System.Threading.Tasks;

namespace TurboITB;

public partial class Unit : Node2D
{
	[Export]
	public int facing_direction = 0; // the direction the unit is facing -- 0 North, 1 East, 2 South, 3 West
	protected (int X, int Y) coordinates = (0, 0); // the position of the unit on the grid in terms of (X,Y) coordinates
	protected bool is_selected = false;

	// gameplay variables
	[Export]
	protected int min_range = 0; // the minimum range of the unit
	[Export]
	protected int max_range = 5; // the maximum range of the unit

	// gameplay assets
	

	// GETTERS and SETTERS
	public (int X, int Y) Coordinates { get => coordinates; set => coordinates = value; }
	public int FacingDirection { get => facing_direction; set => facing_direction = value; } // 0 North, 1 East, 2 South, 3 West
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GetNode<Sprite2D>("Sprite2D").Rotate(Mathf.DegToRad(90 * (int)facing_direction));
		GetNode<Sprite2D>("Sprite2D").Frame = FacingDirection;
		
		// Add signals to class
		GetNode<Button>("Button").Pressed += SelectUnit;
	}

	public void HighlightAttack()
	{
		int[,] attackRange = GetAttackRange();
		for (int y = 0; y < GridController.GridSize.Y; y++)
		{
			for (int x = 0; x < GridController.GridSize.X; x++)
			{
				ColorRect rect = new ColorRect();
				rect.MouseFilter = Control.MouseFilterEnum.Ignore;
				rect.Size = new Vector2(64, 64);
				rect.Position = new Vector2(64 * (x - 2), 64 * (y - 2));

				switch(attackRange[x, y])
				{
					case(1):
						rect.Color = new Color(1f, 0f, 0f, 0.2f);
						GridController.CurrentLevel.AddChild(rect);
						break;
					case(2):
						rect.SelfModulate = new Color(1f, 0f, 0f, 0.5f);
						GridController.CurrentLevel.AddChild(rect);
						break;
				}
			}
		}
	}

	public virtual int[,] GetAttackRange()
	{
		GD.Print("this is not override");
		return null;
	}

	// HACK HACKHACK!!! It's midnight!!
	public virtual async void Attack()
	{
		int[,] attackArr = GetAttackRange();
		(int x, int y) pos = (-1,-1);

		for (int y = 0; y < GridController.GridSize.Y; y++)
			for (int x = 0; x < GridController.GridSize.X; x++)
				if(attackArr[x, y] == 2)
					pos = (x, y);
		GD.Print(pos);
		if (pos == (-1,-1))
		{
			switch(FacingDirection)
			{
				case(0): // north
					pos = (coordinates.X, 0);
					break;
				case(1): // east
					pos = (GridController.GridSize.Y - 1, coordinates.Y);
					break;
				case(2): // south
					pos = (coordinates.X, GridController.GridSize.X - 1);
					break;
				case(3): // west
					pos = (0, coordinates.Y);
					break;
			}
		}
		else
		{
			GridController.UnitGrid[pos.x, pos.y].Free();
			GridController.UnitGrid[pos.x, pos.y] = null;
		}

		int diffX = pos.x - coordinates.X;
		int diifY = pos.y - coordinates.Y;
		Sprite2D sprite = new Sprite2D();
		sprite.Name = "Bullet";
		sprite.Texture = GD.Load<Texture2D>("res://Sprites/UI/highlight circle.png");
		this.AddChild(sprite);
		Tween tween = sprite.CreateTween();
		tween.TweenProperty(sprite, "position", new Vector2(diffX * 64, diifY * 64), 0.5f);
		tween.TweenCallback(Callable.From(sprite.QueueFree));

	}

	public void SelectUnit()
	{
		if (GridController.SelectedUnit is not null)
		{
			GridController.CurrentLevel.GetNodeOrNull("Sprite2D")?.QueueFree();

			GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
			GridController.SelectedUnit = null;
		}
		else
		{
			// Sprite2D ghost = new Sprite2D();
			// ghost.Scale = this.GetNode<Sprite2D>("Sprite2D").Scale;
			// ghost.Texture = this.GetNode<Sprite2D>("Sprite2D").Texture;
			// ghost.Rotation = this.GetNode<Sprite2D>("Sprite2D").Rotation;
			// ghost.Position = this.Position;
			// ghost.Name = "Sprite2D";
			// ghost.Frame = this.FacingDirection;
			Sprite2D ghost = new() {
				Scale = this.GetNode<Sprite2D>("Sprite2D").Scale,
				Texture = this.GetNode<Sprite2D>("Sprite2D").Texture,
				Hframes = 4,
				Rotation = GetNode<Sprite2D>("Sprite2D").Rotation,
				Position = this.Position,
				Name = "Sprite2D",
				Frame = this.FacingDirection
			};

			GridController.CurrentLevel.AddChild(ghost);

			if (GridController.SelectedUnit != null)
				GridController.SelectedUnit.Modulate = new Color(1f,1f,1f, 1f);
			this.Modulate = new Color(1f,1f,1f, 0.8f);
			GridController.SelectedUnit = this;
		}
	}
}
