using System.Collections.Generic;
using Godot;

namespace EternalForest.Health;

public abstract partial class HealthBar : HBoxContainer
{
	[Export] private int MaxHealth = 10; // vie max 
	[Export] private Texture2D coeurMinecraft; //  texture coeur full
	[Export] private Texture2D halfcoeur; //  texture coeur moit
	[Export] private Texture2D coeurvide; // texture vide

	private HBoxContainer _heartContainer;
	private List<TextureRect> _hearts = new List<TextureRect>();
	
	public override void _Ready()
	{
		_heartContainer = GetNode<HBoxContainer>("HBoxContainer"); // on cherche le HBox
		
		
		
		// Initialise la bar avec le nb de coeur max
		for (int i = 0; i < MaxHealth / 2; i++)
		{
			var heart = new TextureRect();
			heart.Texture = coeurMinecraft;
			_hearts.Add(heart);
			_heartContainer.AddChild(heart);
		}
	}

	public void UpdateHealthBar(int currentHealth)
	{
		 for (int i = 0; i < _hearts.Count; i++)
		 {
			 var heart = _hearts[i];
			 int healthForHeart = Mathf.Clamp(currentHealth - (i * 2), 0, 2);

			 if (healthForHeart == 2)
			 {
				 heart.Texture = coeurMinecraft;
			 }
			 else if (healthForHeart == 1)
			 {
				 heart.Texture = halfcoeur;
			 }
			 else
			 {
				 heart.Texture = coeurvide;
			 }
		 }
	}
}
