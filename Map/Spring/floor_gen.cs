using Godot;
using System;

public partial class floor_gen : Node
{
	[Export] private PackedScene floor;
    
	public void GenerateNewFloor(int x, int y)
	{
		PlaceTile(floor, x, y);
	}
	 
    private void PlaceTile(PackedScene scene, int x, int y)
    {
        Node2D instance = (Node2D)scene.Instantiate();
        instance.Position = new Vector2(x, y);
        AddChild(instance);
    }
}
