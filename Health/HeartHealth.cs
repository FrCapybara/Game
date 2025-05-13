using Godot;
using System;

public partial class HeartHealth : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	private PackedScene HeartGui;
	private Node Heart;
	
	public void SetMaxHealth(int health)
	{
		for (int i = 0; i <= health; i++)
		{
			var heart = HeartGui.Instantiate();
			Heart.AddChild(heart);
		}
	}
	public override void _Ready()
	{
		HeartGui = ResourceLoader.Load<PackedScene>("res://Health/panel.tscn");
		Heart = GetNode<Node>("CanvasLayer/Heart");
		SetMaxHealth(10);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
