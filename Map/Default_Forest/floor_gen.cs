using Godot;
using System;

public partial class floor_gen : Node
{
	[Export] private PackedScene topLeftScene;
	[Export] private PackedScene topScene;
	[Export] private PackedScene topRightScene;
	[Export] private PackedScene leftScene;
	[Export] private PackedScene rightScene;
	[Export] private PackedScene downLeftScene;
	[Export] private PackedScene downScene;
	[Export] private PackedScene downRightScene;
	[Export] private PackedScene floorScene;
    [Export] private PackedScene floor2Scence;
    [Export] private PackedScene floor3Scence;
    [Export] private PackedScene floor4Scene;
    [Export] private PackedScene floor5Scene;
	
	private int mapWidth = 9984;  // On definit la largeur 
	private int mapHeight = 9984; // on definit la hauteur 
	private int tilesize = 32;
    
	public void GenerateNewFloor()
	{
		GenerateFloor();
	}
	private void GenerateFloor()
        {
            Random random = new Random();
            int horizontal = 312;
            int vertical = 312;
    
            for (int x = 0; x < horizontal; x++)
            {
                if (x == 0)
                {
                    PlaceTile(topLeftScene, x, 0);
                }
    
                if (x == horizontal - 1)
                {
                    PlaceTile(topRightScene, x, 0);
                }
                if (x != 0 && x != horizontal - 1)
                {
                    PlaceTile(topScene, x, 0);
                }
            }
    
            for (int y = 1; y < vertical-1; y++)
            {
                for (int x = 0; x < horizontal; x++)
                {
                    if (x == 0)
                    {
                        PlaceTile(leftScene, 0, y);
                    }
    
                    if (x == horizontal - 1 )
                    {
                        PlaceTile(rightScene, x, y);
                    }
                    if (x != 0 && x != horizontal - 1)
                    {
                        int n = random.Next(1, 6);
                        if (n == 1)
                        {
                            PlaceTile(floorScene, x, y);
                        }

                        if (n == 2)
                        {
                            PlaceTile(floor2Scence, x, y);
                        }

                        if (n == 3)
                        {
                            PlaceTile(floor3Scence, x, y);
                        }

                        if (n == 4)
                        {
                            PlaceTile(floor4Scene, x, y);
                        }

                        if (n == 5)
                        {
                            PlaceTile(floor5Scene, x, y);
                        }
                    }
                }
            }
    
            for (int x = 0; x < horizontal; x++)
            {
                if (x == 0)
                {
                    PlaceTile(downLeftScene, x, vertical - 1);
                }
    
                if (x == horizontal - 1)
                {
                    PlaceTile(downRightScene, x, vertical - 1);
                }
                if (x != 0 && x != horizontal - 1)
                {
                    PlaceTile(downScene, x, vertical - 1);
                }
            }
        }
        
        private void PlaceTile(PackedScene scene, int x, int y)
        {
            Node2D instance = (Node2D)scene.Instantiate();
            instance.Position = new Vector2(x * tilesize, y * tilesize);
            AddChild(instance);
        }
}
