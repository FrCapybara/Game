using Godot;

public partial class MultiplayerMenu : Control
{
    public override void _Ready()
    {
        Button retourButton = GetNode<Button>("VBoxContainer/Retour");
        Button hebergerButton = GetNode<Button>("VBoxContainer/Héberger");
        Button rejoindreButton = GetNode<Button>("VBoxContainer/Rejoindre");

        // Connexion des signaux avec les boutons
        retourButton.Pressed += OnRetourPressed;
        hebergerButton.Pressed += OnHebergerPressed;
        rejoindreButton.Pressed += OnRejoindrePressed;
    }

    private void OnHebergerPressed()
    {
        GetTree().ChangeSceneToFile("res://UI/HebergeMenu.tscn"); // charge le menu pour hébérger
    }

    private void OnRejoindrePressed()
    {
        GameMultiplayer.IsServer = false;
        GetTree().ChangeSceneToFile("res://scenes/GameMultiplayer.tscn"); // charge la scene multiplayer
    }
    
    private void OnRetourPressed()
    {
        GetTree().ChangeSceneToFile("res://UI/main_menu.tscn"); //pour retourner au menu principal 
    }
}