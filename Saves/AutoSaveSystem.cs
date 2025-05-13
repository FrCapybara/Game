using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EternalForest.Game;

public partial class AutoSaveSystem : Node
{
    [Export]
    private float _saveIntervalSeconds = 30.0f; // 5 minutes (300 secondes)
    
    [Export]
    private string _savePath = "user://autosaves/";
    
    [Export]
    private int _maxAutoSaves = 5; // Nombre maximum de sauvegardes automatiques à conserver
    
    private float _timeSinceLastSave = 0.0f;
    private bool _isSaving = false;
    
    public override void _Ready()
    {
        // Créer le répertoire de sauvegarde s'il n'existe pas
        DirAccess directory = DirAccess.Open("user://");
        if (directory != null && !directory.DirExists("autosaves"))
        {
            directory.MakeDirRecursive("autosaves");
        }
        
        GD.Print("Système d'auto-sauvegarde initialisé. Interval: " + _saveIntervalSeconds + " secondes");
    }
    
    public override void _Process(double delta)
    {
        if (_isSaving) return;
        
        _timeSinceLastSave += (float)delta;
        
        // Vérifier si l'intervalle de sauvegarde est atteint
        if (_timeSinceLastSave >= _saveIntervalSeconds)
        {
            _timeSinceLastSave = 0;
            PerformAutoSave();
        }
    }
    
    private async void PerformAutoSave()
    {
        _isSaving = true;
        
        // Générer un nom de fichier basé sur la date et l'heure
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filename = _savePath + "autosave_" + timestamp + ".save";
        
        GD.Print("Sauvegarde automatique en cours: " + filename);
        
        // Créer et sauvegarder les données du jeu
        var saveData = CollectGameData();
        SaveGame(saveData, filename);
        
        // Rotation des sauvegardes anciennes
        await RotateOldSaves();
        
        _isSaving = false;
        GD.Print("Sauvegarde automatique terminée");
    }
    
    private GameSaveData CollectGameData()
    {
        var saveData = new GameSaveData();
        
        // Récupérer le joueur
        Node2D player = GetTree().Root.GetNode<Node2D>("GameSolo/Player");
        if (player != null)
        {
            saveData.PlayerPosition = player.GlobalPosition;
            
            // Récupérer la santé du joueur si possible
            if (player.HasMethod("GetHealth"))
            {
                saveData.PlayerHealth = (float)player.Call("GetHealth");
            }
            else
            {
                saveData.PlayerHealth = 100.0f; // Valeur par défaut
            }
        }
        
        // Marquer que la carte et le sol ont été générés
        saveData.MapGenerated = true;
        saveData.FloorGenerated = true;
        
        // Ajouter d'autres données si nécessaire
        saveData.CurrentLevel = "main"; // Ou une autre valeur pertinente
        
        return saveData;
    }
    
    private void SaveGame(GameSaveData saveData, string filename)
    {
        // Sérialiser et sauvegarder les données
        FileAccess file = FileAccess.Open(filename, FileAccess.ModeFlags.Write);
        if (file != null)
        {
            // Convertir les données en JSON
            string jsonData = Json.Stringify(saveData.ToJson());
            file.StoreString(jsonData);
            file.Close();
        }
        else
        {
            GD.Print("Erreur lors de la création du fichier de sauvegarde: " + filename);
        }
    }
    
    private async Task RotateOldSaves()
    {
        DirAccess directory = DirAccess.Open(_savePath);
        if (directory == null) return;
        
        // Collecter tous les fichiers de sauvegarde automatique
        var saveFiles = new List<string>();
        
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
        
        // Trier par date (plus récent en dernier)
        saveFiles.Sort();
        
        // Supprimer les sauvegardes les plus anciennes si nous avons dépassé le maximum
        while (saveFiles.Count > _maxAutoSaves)
        {
            string oldestFile = _savePath + saveFiles[0];
            GD.Print("Suppression de l'ancienne sauvegarde: " + oldestFile);
            DirAccess.RemoveAbsolute(oldestFile);
            saveFiles.RemoveAt(0);
            
            // Petite pause pour éviter de surcharger le système de fichiers
            await ToSignal(GetTree().CreateTimer(0.1), "timeout");
        }
    }
}