namespace EternalForest.Game;
using Godot;

public partial class GameManager : Node
{
    public bool IsNewGame { get; set; } // Si le jeu est en Solo

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
    }
}