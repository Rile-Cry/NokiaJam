using Godot;
using System;

public partial class Door : Area2D
{
	// Node Reference
	private AnimatedSprite2D _anim;
	private CharacterBody2D _player;
	
	// Resource Reference
	[Export] private string nextScene = "testLevel";
	private bool unlocked = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Connect("body_entered", new Callable(this, "EnterPlayer"));
		this.Connect("body_exited", new Callable(this, "ExitPlayer"));
		_anim.Connect("animation_finished", new Callable(this, "OpenDoor"));
	}
	
	private void EnterPlayer(Node body)
	{
		if (body.Name == "Player")
		{
			_player = (CharacterBody2D) body;
			_player.Connect("useKey", new Callable(this, "UnlockDoor"));
		}
	}
	
	private void UnlockDoor()
	{
		_anim.Play("unlocking");
	}
	
	private void ExitPlayer(Node body)
	{
		if (body.Name == "Player")
		{
			_player.Disconnect("useKey", new Callable(this, "UnlockDoor"));
			_player = null;
		}
	}
	
	private void OpenDoor()
	{
		if (_anim.Animation == "unlocking") _anim.Play("open");
	}
	
	public override void _Process(float delta)
	{
		GetInput();
	}
	
	private void GetInput()
	{
		if (unlocked)
		{
			if (_player != null && Input.IsActionPressed("enter"))
			{
				GetTree().ChangeSceneToFile($"res://scn/lvl/{nextScene}");
			}
		}
	}
}
