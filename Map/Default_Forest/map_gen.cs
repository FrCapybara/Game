using Godot;
using System;
using System.Collections.Generic;

public partial class map_gen : Node2D
{
    [Export] private PackedScene leavesScene; // On exporte la varibale pour la scene feuilles
    [Export] private PackedScene rockScene; // On exporte la varibale pour la scene rochers
    [Export] private PackedScene treeScene; //On exporte la varibale pour la scene arbres
    [Export] private PackedScene mushroomScene; // On exporte la varibale pour la scene champignons
    [Export] private PackedScene Orc1; // Scène de l'ennemi
    [Export] private PackedScene Orc2; // Scène de l'ennemi
    [Export] private PackedScene Orc3; // Scène de l'ennemi

    private int mapWidth = 9984;  // On definit la largeur 
    private int mapHeight = 9984; // on definit la hauteur 
    

    private int leavesMin = 100, leavesMax = 200;   // Quantite min et max de feuilles
    private int rockMin = 100, rockMax = 200;   // Quantite min et max de cailloux
    private int treeMin = 100, treeMax = 200;   // Quantite min et max arbre
    private int mushroomMin = 100, mushroomMax = 200;
    
    public void GenerateNewMap()
    {
        GenerateMap();
    }
    
    private void GenerateEnemies(PackedScene ennemy)
    {
        Random random = new Random();

        for (int i = 0; i < 20; i++)
        {
            Node2D enemyInstance = (Node2D)ennemy.Instantiate();
            Vector2 position = new Vector2(random.Next(0, mapWidth), random.Next(0, mapHeight));
            enemyInstance.Position = position;
            AddChild(enemyInstance);
        }
    }

    private void GenerateMap()
    {
        GenerateEnemies(Orc1);
        GenerateEnemies(Orc2);
        GenerateEnemies(Orc3);
        GenerateObjects(mushroomScene, mushroomMin, mushroomMax);
        GenerateObjects(leavesScene, leavesMin, leavesMax);
        GenerateObjects(rockScene, rockMin, rockMax);
        GenerateObjects(treeScene, treeMin, treeMax);
    }

    private void GenerateObjects(PackedScene scene, int minCount, int maxCount)
    {
        Random random = new Random();

        // determine le nombre d'objets a generer
        int objectCount = random.Next(minCount, maxCount);

        List<Rect2> occupiedSpaces = new List<Rect2>();

        for (int i = 0; i < objectCount; i++)
        {
            Node2D instance = (Node2D)scene.Instantiate();
            
            //on recupere la sprite dans instane
            var sprite = instance.GetNode<Sprite2D>("Sprite2D");
            //on recupere la taille de l'image de l'objet
            Vector2 spriteSize = sprite.RegionRect.Size * sprite.Scale;
            //on cree le vecteur pour les positions de chaque objet 
            Vector2 position;

            // on cherche a generer une position jusqua ce quelle soit valide 
            do
            {
                float x = random.Next((int)(spriteSize.X) , (int)(mapWidth - spriteSize.X * 2));
                float y = random.Next((int)(spriteSize.Y) , (int)(mapHeight - spriteSize.Y * 2));
                position = new Vector2(x, y);
            }
            while (IsOverlapping(position, spriteSize, occupiedSpaces));

            // placement de l'objet si ok
            instance.Position = position;
            AddChild(instance);

            // ajoute l'espace occupé par l'objet dans la liste
            occupiedSpaces.Add(new Rect2(position - spriteSize, spriteSize * 2));
        }
    }

    private bool IsOverlapping(Vector2 position, Vector2 size, List<Rect2> occupiedSpaces)
    {
        // cree un Rect2 pour le nouvel objet
        Rect2 newRect = new Rect2(position - size, size * 2);

        foreach (var rect in occupiedSpaces)
        {
            // verifie si les deux rectangles se chevauchent
            if (newRect.Intersects(rect))
            {
                return true; // chevauchement 
            }
        }

        // pas de chevauchement
        return false;
    }
}