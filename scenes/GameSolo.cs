using Godot;
using System;
using EternalForest.Game;

public partial class GameSolo : Node2D
{
	[Export] private map_gen _mapGen;
	[Export] private floor_gen _floorGen;

	public override void _Ready()
	{
		if (GetNode<GameManager>("/root/GameManager").IsNewGame)
		{
			_mapGen.GenerateNewMap();
			_floorGen.GenerateNewFloor();
		}
	}
}
