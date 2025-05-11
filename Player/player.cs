using EternalForest.Health;
using Godot;

public partial class player : CharacterBody2D
{
	[Export] public string PlayerName { get; set; }
	[Export] private player_input _input = null; // varibale pour stocker les entrees du joueur 
	[Export] private Character _character = null; // variable pour le personnage joueur
	public Vector2 BodyPosition => _character.GlobalPosition;

	public override void _Process(double delta)
	{
		_character.SetMovementInput(_input.MovementInput);
	} 
	
	[Export] private int maxHealth = 10; // Santé max je le met à 10 pour l'instant
	[Export] private HealthBar _healthBar = null; // Barre de vie
	private int _currentHealth;

	public override void _Ready()
	{
		_currentHealth = maxHealth; // On remet la santé au max
		_healthBar.UpdateHealthBar(_currentHealth);//On met à jour la barre de vie
		_animation = GetNode<AnimationPlayer>("Weapon/Attack");
	}
	private AnimationPlayer _animation;
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("space"))
		{
			GD.Print("Spacebar pressed!"); // Debug message

			if (_animation != null)
			{
				_animation.Stop();
				_animation.Play("Attack");
				GD.Print("Playing Attack animation.");
			}
			else
			{
				GD.PrintErr("AnimationPlayer is null!");
			}
		}
	}
}	
