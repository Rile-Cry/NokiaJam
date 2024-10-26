using Godot;
using System;

public partial class Key : CharacterBody2D
{
	// Node References
	private AnimationPlayer _anim;
	
	// Called during group "Keys" for removing the key
	private void PickupKey()
	{
		QueueFree();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_anim.Play("Default");
		AddToGroup("Keys");
	}
}
