using Godot;
using System;

public partial class Item : Node2D
{
	public string Name { get; set; }
	public Texture Icon { get; set; }
	
	// Stackable item properties
	public int Quantity { get; set; } = 1;
	public int MaxStackSize { get; set; } = 99;
	
	// Item category for filtering/sorting
	public string ItemType { get; set; } = "Misc";
	
	public Item()
	{
		// Default initialization
	}
	
	public Item(string name, Texture icon, int quantity = 1, int maxStackSize = 99)
	{
		Name = name;
		Icon = icon;
		Quantity = quantity;
		MaxStackSize = maxStackSize;
	}
	
	// Combine items of the same type
	public bool TryStackWith(Item otherItem)
	{
		if (!CanStackWith(otherItem))
			return false;
			
		int totalQuantity = Quantity + otherItem.Quantity;
		
		if (totalQuantity <= MaxStackSize)
		{
			// Full stack merge
			Quantity = totalQuantity;
			otherItem.Quantity = 0;
			return true;
		}
		else
		{
			// Partial merge - source stack overflows
			Quantity = MaxStackSize;
			otherItem.Quantity = totalQuantity - MaxStackSize;
			return false;
		}
	}
	
	// Check if items can be stacked together
	public bool CanStackWith(Item otherItem)
	{
		if (otherItem == null) return false;
		return Name == otherItem.Name && ItemType == otherItem.ItemType;
	}
	
	// Take some items from this stack to create a new one
	public Item SplitStack(int amount)
	{
		if (amount <= 0 || amount >= Quantity)
			return null;
			
		Item splitItem = new Item(Name, Icon, amount, MaxStackSize);
		splitItem.ItemType = this.ItemType;
		
		Quantity -= amount;
		
		return splitItem;
	}
	
	// Helper to check if stack is depleted
	public bool IsEmpty()
	{
		return Quantity <= 0;
	}
	
	// Create duplicate item stack
	public Item Clone()
	{
		Item cloned = new Item(Name, Icon, Quantity, MaxStackSize);
		cloned.ItemType = this.ItemType;
		return cloned;
	}
	
	// Debug output
	public override string ToString()
	{
		return $"{Name} x{Quantity} (Max: {MaxStackSize})";
	}
	
	public override void _Ready()
	{
		// Ready setup
	}

	public override void _Process(double delta)
	{
		// Process updates
	}
}
