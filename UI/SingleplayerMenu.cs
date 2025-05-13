namespace EternalForest.UI;
using EternalForest.Game;
using System.Collections.Generic;

using Godot;

public partial class SingleplayerMenu : Control
{
    public override void _Ready()
    {
        
        VBoxContainer buttonContainer = GetNode<VBoxContainer>("VBoxContainer");
        
        Button newWorldButton = buttonContainer.GetNode<Button>("Nouveau");
        Button continueButton = buttonContainer.GetNode<Button>("Continuer");
        Button backButton = buttonContainer.GetNode<Button>("Retour");

        newWorldButton.Pressed += OnNewWorldPressed;
        continueButton.Pressed += OnContinuePressed;
        backButton.Pressed += OnBackPressed;
    }

    private void OnNewWorldPressed()
    {
        GetNode<GameManager>("/root/GameManager").IsNewGame = true;
        GetTree().ChangeSceneToFile("res://scenes/GameSolo.tscn");
    }

    private void OnContinuePressed()
    {
        string savePath = "user://autosaves/";
        DirAccess directory = DirAccess.Open(savePath);
    
        if (directory == null || !directory.FileExists(GetLatestSaveFile(savePath)))
        {
            // Aucune sauvegarde trouvée, afficher un message à l'utilisateur
            GD.Print("Aucune sauvegarde trouvée !");
            // Vous pourriez ajouter ici un popup ou une notification
            return;
        }
    
        // Une sauvegarde existe, charger le jeu
        GetNode<GameManager>("/root/GameManager").IsNewGame = false;
        GetTree().ChangeSceneToFile("res://scenes/GameSolo.tscn");
    }

    private void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://UI/main_menu.tscn"); //retour au main menu
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
    
        // Trier par date (plus récent en dernier)
        saveFiles.Sort();
    
        // Retourner le fichier le plus récent ou une chaîne vide s'il n'y en a pas
        return saveFiles.Count > 0 ? saveFiles[saveFiles.Count - 1] : string.Empty;
    }
}