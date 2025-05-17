using Godot;
using System;

public partial class Index : Node2D
{
    [Export] public Node2D Player;

    public override void _Ready()
    {
        // Si le joueur n'est pas défini dans l'inspecteur, essaye de le trouver automatiquement
        if (Player == null)
            Player = GetParent().GetNode<Node2D>("player"); // adapte ce chemin si besoin
    }

    public override void _Process(double delta)
    {
        if (Player == null)
            return;

        // Si le joueur est plus bas (Y plus grand), il doit être devant l'objet
        if (Player.Position.Y > Position.Y)
            ZIndex = Player.ZIndex - 1;  // objet derrière le joueur
        else
            ZIndex = Player.ZIndex + 1;  // objet devant le joueur
    }
}
