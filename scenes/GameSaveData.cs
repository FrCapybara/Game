using Godot;
namespace EternalForest.Game
{
    public class GameSaveData
    {
        public Vector2 PlayerPosition { get; set; }
        public float PlayerHealth { get; set; }
        public string CurrentLevel { get; set; }
        public bool MapGenerated { get; set; }
        public bool FloorGenerated { get; set; }
        
        public Godot.Collections.Dictionary ToJson()
        {
            var data = new Godot.Collections.Dictionary();
            
            data["player_position_x"] = PlayerPosition.X;
            data["player_position_y"] = PlayerPosition.Y;
            data["player_health"] = PlayerHealth;
            data["current_level"] = CurrentLevel;
            data["map_generated"] = MapGenerated;
            data["floor_generated"] = FloorGenerated;
            
            return data;
        }
        
        public static GameSaveData FromJson(Godot.Collections.Dictionary data)
        {
            var saveData = new GameSaveData();
            
            if (data.ContainsKey("player_position_x") && data.ContainsKey("player_position_y"))
            {
                saveData.PlayerPosition = new Vector2(
                    (float)data["player_position_x"],
                    (float)data["player_position_y"]
                );
            }
            
            if (data.ContainsKey("player_health"))
            {
                saveData.PlayerHealth = (float)data["player_health"];
            }
            
            if (data.ContainsKey("current_level"))
            {
                saveData.CurrentLevel = (string)data["current_level"];
            }
            
            if (data.ContainsKey("map_generated"))
            {
                saveData.MapGenerated = (bool)data["map_generated"];
            }
            
            if (data.ContainsKey("floor_generated"))
            {
                saveData.FloorGenerated = (bool)data["floor_generated"];
            }
            
            return saveData;
        }
    }
}