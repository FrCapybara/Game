using Godot;

public partial class player_input : Node
{
	[Export] private string _leftAxis = "left";
	[Export] private string _rightAxis = "right";
	[Export] private string _upAxis = "up";
	[Export] private string _downAxis = "down";
	private Vector2 _movementInput = Vector2.Zero; 
	public Vector2 MovementInput => _movementInput;
	
	public override void _Process(double delta)
	{
		//mise a jour des entrees de mouvement selon les touches
		_movementInput = new Vector2(
			Input.GetAxis(_leftAxis, _rightAxis), //mouvement Horizontal gauche droite
			Input.GetAxis(_upAxis, _downAxis)); //mouvement vertical haut bas 
	}
}
