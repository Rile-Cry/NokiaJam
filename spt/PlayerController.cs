using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	// Resource References
	private AudioStream _jump = (AudioStream) ResourceLoader.Load("res://snd/jump.wav");
	private AudioStream _hurt = (AudioStream) ResourceLoader.Load("res://snd/hurt.wav");
	private AudioStream _pickup = (AudioStream) ResourceLoader.Load("res://snd/pickup.wav");
	
	// Node References
	private AnimatedSprite2D _health;
	private CharacterBody2D _platform;
	private AnimatedSprite2D _sprite;
	private AudioStreamPlayer _stream;
	private Timer _invuln;
	
	// Signals
	[Signal] public delegate void addItem(string new_item);
	[Signal] public delegate void removeItem(string item);
	[Signal] public delegate void useKey();
	
	// Variables
	private Vector2 dir = new Vector2(0, 0);
	private bool invuln = false;
	private bool grounded = false; // Determines if the player touched the ground or not
	private int health = 3; // Hold the value of the player's health
	private int healthMax = 3; // The maximum health of the player
	private bool jumping = false; // Determines if the player is jumping or not
	private int jumpPoint = 5; // The point during the jump that the player is at
	private int jumpPointMax = 5; // The max point for jumpPoint
	private bool jumpSound = true; // Tells when the jump sound can be played
	private bool onPlatform = false;
	private Vector2 platVel = new Vector2(0, 0);
	private int speed = 2; // The player's move speed
	
	// Called when the body_entered signal is recieved
	public void ChangeHealth()
	{
		if (!invuln)
		{
			health -= 1;
			if (health == 0) GetTree().ReloadCurrentScene();
			_stream.Stream = _hurt;
			_stream.Play();
			invuln = true;
			_invuln.Start(1);
		}
	}
	
	// Catches the timeout from the Invulnerability timer
	public void OnInvulnTimeout()
	{
		invuln = false;
	}
	
	// The reciever for the Platform's 'moving' signal
	public void PlatformMoving(Vector2 vel)
	{
		platVel = vel;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_health = GetNode<AnimatedSprite2D>("HealthBar");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_stream = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		_invuln = GetNode<Timer>("Invuln");
		_invuln.Connect("timeout", new Callable(this, "OnInvulnTimeout"));
	}
	
	// Called every tick to process physics
	public override void _PhysicsProcess(float delta)
	{
		ManageHealth();
		MovementControl();
		MovementResult();
		Gravity();
	}
	
	// Called during PhysicsProcess to determine and change player health
	public void ManageHealth()
	{
		_health.Frame = healthMax - health;
	}
	
	// Called during PhysicsProcess to get movement input
	private void MovementControl()
	{
		if (Input.IsActionPressed("right"))
		{
			_sprite.FlipH = false;
			if (_sprite.Animation != "Move") _sprite.Play("Move");
			dir = new Vector2(speed, 0);
		} else if (Input.IsActionPressed("left"))
		{
			_sprite.FlipH = true;
			if (_sprite.Animation != "Move") _sprite.Play("Move");
			dir = new Vector2(-speed, 0);
		} else
		{
			dir = new Vector2(0, 0);
			_sprite.Play("Idle");
		}
		
		JumpControl();
	}
	
	// Called during MovementControl to make the player jump.
	private void JumpControl()
	{
		if (Input.IsActionPressed("jump"))
		{
			jumping = true;
			_stream.Stream = _jump;
			if (jumpSound) _stream.Play();
			jumpSound = false;
			
			if (onPlatform)
			{
				OffPlatform();
			}
		}
		
		if (jumping && jumpPoint > 0 && grounded)
		{
			dir = dir + (new Vector2(0, -jumpPoint));
			jumpPoint -= 1;
		} else if (jumping && jumpPoint <= 0)
		{
			jumpPoint = jumpPointMax;
			jumping = false;
			grounded = false;
		}
	}
	
	// Called during PhysicsProcess to gie the result of MovementControl
	private void MovementResult()
	{
		var collisionInfo = MoveAndCollide(dir + platVel);
		if (collisionInfo != null)
		{
			var collider = collisionInfo.Collider as Node;
			if (collider.IsInGroup("HarmObjects")) ChangeHealth();
			if (collider.IsInGroup("Keys"))
			{
				_stream.Stream = _pickup;
				_stream.Play();
				EmitSignal("addItem", "key");
				collider.QueueFree();
			}
		}
		
		platVel = new Vector2(0, 0);
	}
	
	// Called during PhysicsProcess to calculate gravity on the player
	private void Gravity()
	{
		var collisionInfo = MoveAndCollide(new Vector2(0, 1));
		
		if (collisionInfo != null)
		{
			var collider = collisionInfo.Collider as Node;
			
			if (collisionInfo.GetPosition().y > Position.y)
			{
				jumping = false;
				jumpSound = true;
				grounded = true;
			}
			
			if (collider.IsInGroup("HarmObjects")) ChangeHealth();
			if (collider.IsInGroup("Platforms")) 
			{
				if (!onPlatform)
				{
					_platform = (CharacterBody2D) collider;
					_platform.Connect("moving", new Callable(this, "PlatformMoving"));
					onPlatform = true;
				}
			}
		} else
		{
			if (onPlatform) OffPlatform();
		}
	}
	
	private void OffPlatform()
	{
		_platform.Disconnect("moving", new Callable(this, "PlatformMoving"));
		_platform = null;
		onPlatform = false;
	}
}
