using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryGui : Control
{
	private bool _isOpen = false;
	private GridContainer _gridContainer;
	private List<Item> _items;
	private Item _draggedItem;
	private Panel _draggedPanel;
	
	public void Open()
	{
		_isOpen = true;
		Visible = true;
		UpdateUI();
	}

	public void Close()
	{
		_isOpen = false;
		Visible = false;
	}

	
	public override void _Ready()
	{
		Visible = false;
		SetProcessInput(true);
		_gridContainer = GetNode<GridContainer>("GridContainer");
		AddItem(new Item("Sword", (Texture)ResourceLoader.Load("res://Inven/New Piskel.png")));
		_items = new List<Item>();
		
		UpdateUI();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("E"))
		{
			if (_isOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}
		
		
	}
	
	public void AddItem(Item item)
	{
		_items.Add(item);
		UpdateUI();
	}
	
	public void UpdateUI()
	{
		// Supprimer tous les enfants existants
		foreach (Node child in _gridContainer.GetChildren())
		{
			child.QueueFree();
		} 

		// Ajouter des éléments d'inventaire
		for (int i = 0; i < _items.Count; i++)
		{
			Panel panel = new Panel();
			panel.Name = "Panel" + i;
			_gridContainer.AddChild(panel);

			// Ajouter un Sprite2D pour l'icône de l'élément
			Sprite2D sprite = new Sprite2D();
			sprite.Texture = (Texture2D)_items[i].Icon;
			panel.AddChild(sprite);

			// Ajouter un Label pour le nom de l'élément
			Label label = new Label();
			label.Text = _items[i].Name;
			panel.AddChild(label);
			
			
		}
	}
	
	
}
