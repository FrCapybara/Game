using Godot;

public partial class GameMultiplayer : Node2D
{
    [Export] private PackedScene _playerScene; // Scène du joueur (préfabriqué)
    private const int Port = 138; // Port d'écoute du serveur
    ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
    public static bool IsServer {get; set;}

    public override void _Ready()
    {
        if (IsServer)
        {
            var error = peer.CreateServer(Port);
            if (error != Error.Ok)
            {
                return;
            }

            Multiplayer.MultiplayerPeer = peer;

            Multiplayer.PeerConnected += id =>
            {
                SpawnPlayer((int)id);
            };
            SpawnHost();
        }
        else if (!IsServer)
        {
            var error = peer.CreateClient("localhost", Port);
            if (error != Error.Ok)
            {
                return;
            }

            Multiplayer.MultiplayerPeer = peer;
        }
    }
    public void SpawnPlayer(int id)
    {
        Player player = (Player) _playerScene.Instantiate();
        GD.Print("Spawning player: " + id);
        player.Name = id.ToString();
        AddChild(player, true); // Ajoute le nœud avec un nom unique
        if (player.IsMultiplayerAuthority())
        {
            player.GetNode<Camera2D>("Camera2D").Enabled = true;
            player.GetNode<Camera2D>("Camera2D").MakeCurrent();
        }
        else
        {
            player.GetNode<Camera2D>("Camera2D").Enabled = false;
        }
    }

    public void SpawnHost(int id = 1)
    {
        GD.Print("Spawning host: " + id);
        SpawnPlayer(id);
    }
}