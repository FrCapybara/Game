using Godot;
using System;

public partial class Slot : Panel
{
	private PackedScene ItemClass;
	private Node item = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Preload the Item scene
		ItemClass = ResourceLoader.Load<PackedScene>("res://item.tscn");

		// Create a new RandomNumberGenerator for randomness
		var random = new RandomNumberGenerator();
		random.Randomize();

		// Check the random condition
		if (random.Randi() % 2 == 0)
		{
			// Instance the Item scene and add it to the scene tree
			item = ItemClass.Instantiate();
			AddChild(item);
		}
	}
	
	public void PickFromSlot()
	{
		if (item != null)
		{
			RemoveChild(item);
			Node inventoryMode = FindParent("Inventory");
			if (inventoryMode != null)
			{
				inventoryMode.AddChild(item);
			}
			item = null;
			
		}
	}

	public void PutIntsSlot(Node newItem)
	{
		if (newItem != null)
		{
			item = newItem;
			Node inventoryMode = FindParent("Inventory");
			if (inventoryMode != null)
			{
				inventoryMode.RemoveChild(item);
			}
			AddChild(item);
			
		}
	}
}
