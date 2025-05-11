using Godot;
using System;

public partial class Item : Node2D
{
	public string Name { get; set; }
	public Texture Icon { get; set; }

	public Item()
	{
		// Initialisation par défaut
	}

	public Item(string name, Texture icon)
	{
		Name = name;
		Icon = icon;
	}
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
