using Godot;
using System;

public partial class Platform : CharacterBody2D
{	
	// Enums
	[Flags] private enum PlatformType
	{
		Static,
		Dynamic
	}
	
	// Signals
	[Signal] private delegate void moving(Vector2 vel);
	
	// Variables
	private bool aToB = true;
	private Vector2 dir = new Vector2(0, 0);
	[Export] private PlatformType platformType = PlatformType.Static;
	[Export] private Vector2 pointA = new Vector2(0, 0);
	[Export] private Vector2 pointB = new Vector2(0, 0);
	private int speed = 2;
	public Vector2 velocity = new Vector2(0, 0);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Platforms");
	}
	
	// Called every tick to process physics
	public override void _PhysicsProcess(float delta)
	{
		GetDir();
		Movement();
	}
	
	private void GetDir()
	{
		var endPoint = new Vector2(0, 0);
		
		if (aToB)
		{
			if (Position.DistanceTo(pointB) >= speed) endPoint = pointB;
			else
			{
				endPoint = pointA;
				aToB = false;
			}
		} else
		{
			if (Position.DistanceTo(pointA) >= speed) endPoint = pointA;
			else
			{
				endPoint = pointB;
				aToB = true;
			}
		}
		
		var direction = Position.DirectionTo(endPoint);
		if (Math.Abs(direction.x) > Math.Abs(direction.y)) 
		{
			dir = new Vector2(Math.Sign(direction.x), 0);
		} else
		{
			dir = new Vector2(0, Math.Sign(direction.y));
		}
	}
	
	private void Movement()
	{
		if (platformType == PlatformType.Dynamic)
		{
			velocity = dir * speed;
			EmitSignal("moving", velocity);
			MoveAndCollide(velocity);
		}
	}
}
