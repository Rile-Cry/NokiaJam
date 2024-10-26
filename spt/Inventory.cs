using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node2D
{
	// Node References
	private Panel _hoveringSlot;
	private PackedScene _itemScene;
	private Node2D _player;
	
	// Variables
	private int currentSlot = 0;
	
	// Items Dictionary
	private Dictionary<string, string> _items = new Dictionary<string, string>()
	{
		{"key", "res://img/itm/key.png"},
		{"keyHover", "res://img/itm/keyHover.png"};
	};
	
	// Inventory
	private Dictionary<string, string> _inventory = new Dictionary<string, string>()
	{
		{"Slot1", null},
		{"Slot2", null},
		{"Slot3", null},
		{"Slot4", null},
		{"Slot5", null}
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hovingSlot = GetSlot();
		_itemScene = (PackedScene) GD.Load("res://scn/ui/Item.tscn");
		_player = (Node2D) GetParent();
		_player.Connect("addItem", new Callable(this, "AddItem"));
		_player.Connect("removeItem", new Callable(this, "RemoveItem"));
	}
	
	private Panel GetSlot()
	{
		var path = $"GridContainer/Slot{currentSlot + 1}";
		return GetNode(path);
	}
	
	// Called when the signal addItem is recieved
	private void AddItem(string new_item)
	{
		var slot = GrabEmptySlot();
		if (slot != null)
		{
			var item = _itemScene.Instance();
			slot.AddChild(item);
			var texture = (Texture2D) ResourceLoader.Load(_items[new_item]);
			item.GetNode<TextureRect>("TextureRect").Texture2D = texture;
		}
	}
	
	// Called during AddItem to find an empty slot
	private Panel GrabEmptySlot()
	{
		foreach (KeyValuePair<string, string> kvp in _inventory)
		{
			if (kvp.Value == null) return GetNode<Panel>("GridContainer/" + kvp.Key);
		}
		
		return null;
	}
	
	// Called when the signal removeItem is recieved
	private void RemoveItem(string item)
	{
		var slot = GrabSlot();
		if (slot.GetChildren() == 1)
		{
			var child = slot.GetChild(0);
			slot.RemoveChild(child);
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		CheckHover();
	}
	
	private void CheckHover()
	{
		foreach ()
	}
}
