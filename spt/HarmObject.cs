using Godot;
using System;

public class HarmObject : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("HarmObjects");
	}
}
