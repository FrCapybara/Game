using Godot;
using System.Collections.Generic;
using EternalForest.Game;
using System;

public partial class GameSolo : Node2D
{
	[Export] private map_gen map_summer;
	[Export] private map_gen map_spring;
	[Export] private map_gen map_autumn;
	[Export] private map_gen map_winter;
	
	[Export] private floor_gen floor_summer;
	[Export] private floor_gen floor_spring;
	[Export] private floor_gen floor_autumn;
	[Export] private floor_gen floor_winter;
	
	[Export] private Player player;
	
	public int Width { get; private set; } = 3328;
	public int Height { get; private set; } = 3328;

	public List<(int, int)> VisitedChunk = new List<(int, int)>();
	
	(int,int) InitialChunk = (0, 0);

	private (int, int) InitialCoord = (0, 0);
	
	public int ChunkSize { get; private set; } = 3328;
	
	public int ChunkSizeX { get; private set; } = 3328;
	
	public int ChunkSizeY { get; private set; } = 3328;

	public override void _Ready()
	{
		if (GetNode<GameManager>("/root/GameManager").IsNewGame)
		{
			map_summer.GenerateNewMap(0, 0);
			floor_summer.GenerateNewFloor(0, 0);
			VisitedChunk.Add((0, 0));
			Generate8();
		}
		else
		{
			// Charger la sauvegarde la plus récente
			LoadLatestSave();
		}

		// Ajouter la hotbar au CanvasLayer
		CanvasLayer canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
		if (canvasLayer != null)
		{
			PackedScene hotbarScene = ResourceLoader.Load<PackedScene>("res://Inven/inventory_hotbar.tscn");
			if (hotbarScene != null)
			{
				Node hotbarInstance = hotbarScene.Instantiate();
				canvasLayer.AddChild(hotbarInstance);
			}
		}
	}
	
	public void Generate8()
	{
		Random random = new Random();
		(floor_gen, map_gen) chunk = (null, null);
		
		if (!VisitedChunk.Contains((InitialChunk.Item1 - 1, InitialChunk.Item2)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1 - 1, InitialChunk.Item2));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1 - Width, InitialCoord.Item2);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1 - Width, InitialCoord.Item2);
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1 - 1, InitialChunk.Item2 - 1)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1 - 1, InitialChunk.Item2 - 1));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1 - Width, InitialCoord.Item2 - Width);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1 - Width, InitialCoord.Item2 - Width);
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1 - 1, InitialChunk.Item2 + 1)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1 - 1, InitialChunk.Item2 + 1));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1 - Width, InitialCoord.Item2 + Width);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1 - Width, InitialCoord.Item2 + Width);
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1 + 1, InitialChunk.Item2)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1 + 1, InitialChunk.Item2));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1 + Width, InitialCoord.Item2);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1 + Width, InitialCoord.Item2);
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1 + 1, InitialChunk.Item2 - 1)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1 + 1, InitialChunk.Item2 -1));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1 + Width, InitialCoord.Item2 - Width);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1 + Width, InitialCoord.Item2 - Width);
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1 + 1, InitialChunk.Item2 + 1)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1 + 1, InitialChunk.Item2 + 1));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1 + Width, InitialCoord.Item2 + Width);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1 + Width, InitialCoord.Item2 + Width);
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1, InitialChunk.Item2 + 1)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1, InitialChunk.Item2 + 1));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1, InitialCoord.Item2 + Width);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1, InitialCoord.Item2 + Width);
			
		}
		if (!VisitedChunk.Contains((InitialChunk.Item1, InitialChunk.Item2 - 1)))
		{
			int randomNumber = random.Next(1, 5);
			chunk = GetChunk(randomNumber);
			VisitedChunk.Add((InitialChunk.Item1, InitialChunk.Item2 - 1));
			chunk.Item2.GenerateNewMap(InitialCoord.Item1, InitialCoord.Item2 - Width);
			chunk.Item1.GenerateNewFloor(InitialCoord.Item1, InitialCoord.Item2 - Width);
		}
	}
	
	public override void _Process(double delta)
	{
		Vector2 position = player.GlobalPosition;

		bool moved = false;
		
		if (position.X > ChunkSize)
		{
			ChunkSize += Width;
			InitialCoord.Item1 += Width;
			InitialChunk.Item1 += 1;
			moved = true;
		}
		else if (position.X < ChunkSize - Width)
		{
			ChunkSize -= Width;
			InitialCoord.Item1 -= Width;
			InitialChunk.Item1 -= 1;
			moved = true;
		}

		if (position.Y > ChunkSizeY)
		{
			ChunkSizeY += Width;
			InitialCoord.Item2 += Width;
			InitialChunk.Item2 += 1;
			moved = true;
		}
		else if (position.Y < ChunkSizeY - Width)
		{
			ChunkSizeY -= Width;
			InitialCoord.Item2 -= Width;
			InitialChunk.Item2 -= 1;
			moved = true;
		}
		if (moved)
		{
			Generate8();
		}
	}

	private (floor_gen, map_gen) GetChunk(int n)
	{
		(floor_gen, map_gen) res = (null, null);
		switch(n)
		{
			case 1:
				res = (floor_summer, map_summer);
				break;
			case 2:
				res = (floor_spring, map_spring);
				break;
			case 3:
				res =  (floor_autumn, map_autumn);
				break;
			case 4:
				res =  (floor_winter, map_winter);
				break;
		}
		return res;
	}

	private void LoadLatestSave()
	{
		string savePath = "user://autosaves/";
		string latestSaveFile = GetLatestSaveFile(savePath);
	
		if (string.IsNullOrEmpty(latestSaveFile))
		{
			GD.Print("Erreur: Impossible de trouver une sauvegarde à charger");
			return;
		}
	
		string fullPath = savePath + latestSaveFile;
		GD.Print("Chargement de la sauvegarde: " + fullPath);
	
		FileAccess file = FileAccess.Open(fullPath, FileAccess.ModeFlags.Read);
		if (file != null)
		{
			string jsonData = file.GetAsText();
			file.Close();
		
			var json = new Json();
			Error error = json.Parse(jsonData);
			if (error == Error.Ok)
			{
				var data = json.Data.AsGodotDictionary();
				GameSaveData saveData = GameSaveData.FromJson(data);

				// Régénérer le terrain avant d'appliquer les données de sauvegarde
				map_summer.GenerateNewMap(0, 0);
				floor_summer.GenerateNewFloor(0, 0);

				// Appliquer les données chargées
				ApplySaveData(saveData);
			}
			else
			{
				GD.Print("Erreur lors de l'analyse JSON: " + error.ToString());
			}
		}
		else
		{
			GD.Print("Erreur lors de l'ouverture du fichier: " + fullPath);
		}
	}

	private string GetLatestSaveFile(string savePath)
	{
		var saveFiles = new List<string>();
		DirAccess directory = DirAccess.Open(savePath);
	
		if (directory == null) return string.Empty;
	
		// Collecter tous les fichiers de sauvegarde automatique
		directory.ListDirBegin();
		string fileName = directory.GetNext();
	
		while (!string.IsNullOrEmpty(fileName))
		{
			if (!directory.CurrentIsDir() && fileName.EndsWith(".save") && fileName.StartsWith("autosave_"))
			{
				saveFiles.Add(fileName);
			}
			fileName = directory.GetNext();
		}
	
		directory.ListDirEnd();
	
		if (saveFiles.Count == 0)
			return string.Empty;
	
		// Trier par date (en format "autosave_yyyy-MM-dd_HH-mm-ss.save")
		saveFiles.Sort((a, b) => 
		{
			// Extraire les timestamps des noms de fichiers
			string timeA = a.Replace("autosave_", "").Replace(".save", "");
			string timeB = b.Replace("autosave_", "").Replace(".save", "");
			return string.Compare(timeA, timeB);
		});
	
		// Retourner le fichier le plus récent (le dernier après tri)
		return saveFiles[saveFiles.Count - 1];
	}

	private void ApplySaveData(GameSaveData saveData)
	{
		// Récupérer le joueur
		Node2D player = GetNode<Node2D>("Player");
		if (player != null)
		{
			player.GlobalPosition = saveData.PlayerPosition;
		
			// Utiliser Call pour appeler une méthode sur le nœud
			if (player.HasMethod("SetHealth"))
			{
				player.Call("SetHealth", saveData.PlayerHealth);
			}
		
			GD.Print($"Position du joueur chargée: {saveData.PlayerPosition}, Santé: {saveData.PlayerHealth}");
		}
		else
		{
			GD.Print("Erreur: Impossible de trouver le nœud Player");
		}
	}
}
