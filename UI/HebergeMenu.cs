using Godot;
using EternalForest.Game;

public partial class HebergeMenu : Control
{
	public override void _Ready()
	{
		Button continuerButton = GetNode<Button>("VBoxContainer/Continuer");
		Button nouveauButton = GetNode<Button>("VBoxContainer/Nouveau");
		Button retourButton = GetNode<Button>("VBoxContainer/Retour");
		
		continuerButton.Pressed += OnContinuerPressed;
		nouveauButton.Pressed += OnNouveauPressed;
		retourButton.Pressed += OnRetourPressed;
	}
	
	private async void OnNouveauPressed()
	{
		GD.Print("Pas encore");
	}
	
	private void OnContinuerPressed()
	{
		GD.Print("Pas encore");
	}

	private void OnRetourPressed()
	{
		GetTree().ChangeSceneToFile("res://UI/MultiplayerMenu.tscn"); // retour au menu multi
	}
}
