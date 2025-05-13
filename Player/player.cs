using Godot;
using EternalForest.Game;

public partial class Player : CharacterBody2D
{
	[Export] private int _speed = 300;
	[Export] private AnimatedSprite2D _sprite;
	[Export] private Camera2D _camera; // Référence à la caméra
	
	
	[Export] private string _leftAxis = "left";
	[Export] private string _rightAxis = "right";
	[Export] private string _upAxis = "up";
	[Export] private string _downAxis = "down";

	private Vector2 _movementInput = Vector2.Zero;
	private string _lastDirection = "R";

	public override void _EnterTree()
	{
		if (GetNode<GameManager>("/root/GameManager").IsNewGame)
		{
			
		}
		else
		{
			int id = int.Parse(GetName());
			SetMultiplayerAuthority(id);
		}
		
	}
	
	public override void _Ready()
	{
		// Si ce n'est pas le joueur local, on désactive le traitement des entrées
		if (!IsMultiplayerAuthority())
		{
			SetProcess(false);
			SetPhysicsProcess(false);
			if (_camera != null)
			{
				_camera.Enabled = false; // Désactive la caméra pour les autres joueurs
			}

			return;
		}
		if (IsMultiplayerAuthority())
		{
			_camera.Enabled = true;
			_camera.MakeCurrent(); // Fait de cette caméra la caméra active
		}
		
		
	}
	
	

	public override void _Process(double delta)
	{
		if (!IsMultiplayerAuthority()) return;
		
		_movementInput = new Vector2(
			Input.GetAxis(_leftAxis, _rightAxis),
			Input.GetAxis(_upAxis, _downAxis)
		).Normalized();

		Velocity = _movementInput * _speed;
		MoveAndSlide();

		UpdateAnimation();
	}

	private void UpdateAnimation()
	{
		if (_movementInput.X > 0)
		{
			_lastDirection = "R";
			_sprite.Play("walk_right");
		}
		else if (_movementInput.X < 0)
		{
			_lastDirection = "L";
			_sprite.Play("walk_left");
		}
		else if (_movementInput.Y != 0)
		{
			_sprite.Play(_lastDirection == "R" ? "walk_right" : "walk_left");
		}
		else
		{
			_sprite.Stop();
		}
	}
	
}
