using Godot;
using System;

public class Platform : KinematicBody2D
{
	// Node Reference
	private Position2D _pointA;
	private Position2D _pointB;
	
	// Enums
	[Flags] private enum PlatformType
	{
		Static,
		Horizontal,
		Vertical
	}
	
	// Variables
	[Export] private PlatformType platformType = PlatformType.Static;
	private bool reverse = false;
	[Export] private int speed = 2;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pointA = GetNode<Position2D>("PointA");
		_pointB = GetNode<Position2D>("PointB");
		
		if (platformType == PlatformType.Static)
		//ToDo: Fix platform to get them to work properly
	}
	
	// Called every tick to process physics
	public override void _PhysicsProcess(float delta)
	{
		Movement();
	}
	
	// Called during _PhysicsProcess to conduct movement
	private void Movement()
	{
		var velocity = FromAToB();
		MoveAndSlide(velocity);
	}
	
	// Called during Movement to determine which direction to move in
	private Vector2 FromAToB()
	{
		var dir = new Vector2(0, 0);
		
		if ((Vector2.Distance(Position, _pointA.Position) <= 1) ||
		(Vector2.Distance(Position, _pointB.Position) <= 1))
		{
			reverse = !reverse;
		}
		
		if (reverse)
		{
			dir = _pointA.Position - _pointB.Position;
		} else
		{
			dir = _pointB.Position - _pointA.Position;
		}
		
		dir = speed * dir.Normalize();
		
		return dir;
	}
}
