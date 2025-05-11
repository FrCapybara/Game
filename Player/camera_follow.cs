using Godot;

public partial class camera_follow : Node2D
{
	[Export] public Node2D _objectToFollow = null; // l'objet que la camera doit suivre

	public override void _Ready()
	{
		// position de la camera surle joeuru au lancement 
		if (_objectToFollow != null)
		{
			Position = _objectToFollow.Position;
		}
	}
	public override void _Process(double delta)
	{
		if (_objectToFollow != null)
		{
			Position = _objectToFollow.Position.Round(); // mise anjour de la position de la camera sur l'objet a suivre 
		}
	}
}


