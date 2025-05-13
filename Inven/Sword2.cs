using Godot;
using System;

public partial class Sword2 : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var random = new RandomNumberGenerator();
		random.Randomize();

		// Get the TextureRect node
		TextureRect textureRect = GetNode<TextureRect>("TextureRect");

		// Load a texture based on a random condition
		if (random.Randi() % 2 == 0)
		{
			
		}
		else
		{
			textureRect.Texture = ResourceLoader.Load<Texture2D>("res://Inven/Sword.png"); // Assuming you want a different texture here
		}
		
	}
}
