using Godot;
using System;

public partial class Item : Node2D
{
	public string Name { get; set; }
	public Texture2D Icon { get; set; } 
	
	// prop pour stacker item
	public int Quantity { get; set; } = 1;
	public int MaxStackSize { get; set; } = 99;
	
	// catégorie item pour faire un peu le ménage 
	public string ItemType { get; set; } = "Misc";
	
	public Item()
	{
		// Initialisation
	}
	
	public Item(string name, Texture2D icon, int quantity = 1, int maxStackSize = 99)
	{
		Name = name;
		Icon = icon;
		Quantity = quantity;
		MaxStackSize = maxStackSize;
	}
	
	// Combien les mm elts
	public bool TryStackWith(Item otherItem)
	{
		if (!CanStackWith(otherItem))
			return false;
			
		int totalQuantity = Quantity + otherItem.Quantity;
		
		if (totalQuantity <= MaxStackSize)
		{
			
			Quantity = totalQuantity;
			otherItem.Quantity = 0;
			return true;
		}
		else
		{
			// stacke pas trop sinon risque stack overflow
			Quantity = MaxStackSize;
			otherItem.Quantity = totalQuantity - MaxStackSize;
			return false;
		}
	}
	
	// vérifie si on peut stacker les items
	public bool CanStackWith(Item otherItem)
	{
		if (otherItem == null) return false;
		return Name == otherItem.Name && ItemType == otherItem.ItemType;
	}
	
	// prends un item pour le bouger à un autre endroit
	public Item SplitStack(int amount)
	{
		if (amount <= 0 || amount >= Quantity)
			return null;
			
		Item splitItem = new Item(Name, Icon, amount, MaxStackSize);
		splitItem.ItemType = this.ItemType;
		
		Quantity -= amount;
		
		return splitItem;
	}
	
	// Pour voir si c'est vide
	public bool IsEmpty()
	{
		return Quantity <= 0;
	}
	
	// juste clonner 
	public Item Clone()
	{
		Item cloned = new Item(Name, Icon, Quantity, MaxStackSize);
		cloned.ItemType = this.ItemType;
		return cloned;
	}
	

	public override void _Ready()
	{
		// Ready 
	}

	public override void _Process(double delta)
	{
		// Update 
	}
}
