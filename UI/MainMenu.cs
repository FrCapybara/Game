using Godot;
using System;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        VBoxContainer buttonContainer = GetNode<VBoxContainer>("VBoxContainer");
        
        Button quitButton = buttonContainer.GetNode<Button>("Quitter");
        Button soloButton = buttonContainer.GetNode<Button>("Solo");
        Button multiButton = buttonContainer.GetNode<Button>("Multi");
        
        quitButton.Pressed += OnQuitPressed;
        soloButton.Pressed += OnSoloPressed;
        multiButton.Pressed += OnMultiPressed;
    }
    private void OnSoloPressed()
    {
        GetTree().ChangeSceneToFile("res://UI/SingleplayerMenu.tscn"); // Charge le menu solo
    }

    private void OnMultiPressed()
    {
        GetTree().ChangeSceneToFile("res://UI/MultiplayerMenu.tscn"); // Charge le menu multijoueur
    }
    
    private void OnQuitPressed()
    {
        GetTree().Quit(); // Quitte le jeu
    }
}