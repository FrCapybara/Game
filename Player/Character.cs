using Godot;

public partial class Character : CharacterBody2D
{
	[Export] private int _speed = 300; //varibale qui stock la vitesse du mouvement du personnage 
	[Export] private AnimatedSprite2D _sprite;
	private Vector2 _movementInput = Vector2.Zero; //stoke la direction de mouvement, initialisee a zéro
	private string LastDirection = "R"; 
	
	public override void _Ready()
	{
		Position = new Vector2(4500, 4500); // pour spawn le player au centre de la map
	}
	
	public void SetMovementInput(Vector2 input)
	{
		_movementInput = input.Normalized(); //normalise l'entrée pour un vitesse constante dans toutes les directions
	}
	public override void _PhysicsProcess(double delta) // méthode apelée à chaque frame physique(pour un mouvement précis)
	{
		Velocity = _movementInput * _speed; // calcule la vélocité en fonction de la direction et de la vitesse
		MoveAndSlide(); // applique le mouvement en gerant les collisions
		
		//cette parti du code permet de gerer l'animation en fonction de la direction
		if (_movementInput.X > 0)
		{
			LastDirection = "R";
			_sprite.Play("walk_right");
		}
		else if (_movementInput.X < 0)
		{
			LastDirection = "L";
			_sprite.Play("walk_left");
		}
		else if (_movementInput.Y != 0)
		{
			if (LastDirection == "R")
			{
				_sprite.Play("walk_right");
			}
			else
			{
				_sprite.Play("walk_left");
			}
		}
		else
		{
			_sprite.Stop();
		}
	}
	
}
