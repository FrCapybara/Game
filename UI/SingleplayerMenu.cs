namespace EternalForest.UI;
using EternalForest.Game;

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
		GD.Print("pas encore");
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://UI/main_menu.tscn"); //retour au main menu
	}
}
