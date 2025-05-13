using Godot;
using System.Collections.Generic;
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
        else
        {
            // Charger la sauvegarde la plus récente
            LoadLatestSave();
        }
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
                _mapGen.GenerateNewMap();
                _floorGen.GenerateNewFloor();

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