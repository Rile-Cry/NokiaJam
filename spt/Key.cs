using Godot;
using System;

public class Key : KinematicBody2D
{
	// Node Reference
	private Timer _timer;
	
	// Variables
	private bool canMove = true;
	private int dirCount = 1;
	private int dirCountMax = 2;
	[Export] private int floatDist = 1;
	private bool up = true;
	
	// Called during the Timer's timeout signal
	private void CanMove()
	{
		canMove = true;
	}
	
	// Called during group "Keys" for removing the key
	private void PickupKey()
	{
		QueueFree();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		AddToGroup("Keys");
		_timer.Connect("timeout", this, "CanMove");
	}
	
	// Called every tick to process physics
	public override void _PhysicsProcess(float delta)
	{
		Movement();
	}
	
	// Called during _PhysicsProcess to move the key
	private void Movement()
	{
		if (canMove)
		{
			if (dirCount == 0)
			{
				dirCount = dirCountMax;
				up = !up;
			}
			
			if (up)
			{
				Position = new Vector2(Position.x, Position.y - floatDist);
			} else
			{
				Position = new Vector2(Position.x, Position.y + floatDist);
			}
			
			dirCount -= 1;
			canMove = false;
			_timer.Start(1);
		}
	}
}
