using Godot;
using System;

public class PlayerController : KinematicBody2D
{
	// Variables
	private AnimatedSprite _sprite;
	private int speed = 2; // The player's move speed
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}
	
	// Called every tick to process physics
	public override void _PhysicsProcess(float delta)
	{
		MovementControl();
		Gravity();
		GD.Print(Position);
	}
	
	// Called during PhysicsProcess to get movement input
	private void MovementControl()
	{
		if (Input.IsActionPressed("right"))
		{
			_sprite.FlipH = false;
			if (_sprite.Animation != "Move") _sprite.Play("Move");
			Position = new Vector2(Position.x + speed, Position.y);
		} if (Input.IsActionPressed("left"))
		{
			_sprite.FlipH = true;
			if (_sprite.Animation != "Move") _sprite.Play("Move");
			Position = new Vector2(Position.x - speed, Position.y);
		}
	}
	
	// Called during PhysicsProcess to calculate gravity on the player
	private void Gravity()
	{
		var collision = MoveAndCollide(new Vector2(0, 1));
	}
}
