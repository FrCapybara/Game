using Godot;
using System.Collections.Generic;

public partial class InventoryHotbar : Control
{
	private List<Panel> _slots = new List<Panel>();
	private GridContainer _container;
	
	public override void _Ready()
	{
		_container = GetNode<GridContainer>("GridContainer");
		CreateSlots();
		InventoryGui.Instance?.RegisterHotbarUI(this);
		SetProcessInput(true);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse)
		{
			if (mouse.Pressed && GetGlobalRect().HasPoint(mouse.GlobalPosition))
			{
				for (int i = 0; i < _slots.Count; i++)
				{
					if (_slots[i].GetGlobalRect().HasPoint(mouse.GlobalPosition))
					{
						InventoryGui.Instance.HandleHotbarSlotInput(@event, i);
						GetViewport().SetInputAsHandled();
						return;
					}
				}
			}
			else if (!mouse.Pressed && InventoryGui.Instance?.HasDraggedItem() == true && GetGlobalRect().HasPoint(mouse.GlobalPosition))
			{
				for (int i = 0; i < _slots.Count; i++)
				{
					if (_slots[i].GetGlobalRect().HasPoint(mouse.GlobalPosition))
					{
						
						InventoryGui.Instance.HandleHotbarSlotInput(@event, i);
						GetViewport().SetInputAsHandled();
						return;
					}
				}
			}
		}
	}
	
	private void CreateSlots()
	{
		for (int i = 0; i < 9; i++)
		{
			Panel slot = CreateSlot(i);
			_slots.Add(slot);
			_container.AddChild(slot);
		}
	}
	
	private Panel CreateSlot(int index)
	{
		Panel panel = new Panel { CustomMinimumSize = new Vector2(34, 34) };
		int slotIndex = index;
		
		panel.GuiInput += (@event) => {
			InventoryGui.Instance?.HandleHotbarSlotInput(@event, slotIndex);
		};
		
		panel.AddChild(new Sprite2D {
			Texture = ResourceLoader.Load<Texture2D>("res://Inven/SlotInv.png"),
			Position = new Vector2(17, 17),
			Scale = new Vector2(2, 2),
			RegionEnabled = true,
			RegionRect = new Rect2(105, 7, 17, 16)
		});
		
		return panel;
	}
	
	public void UpdateUI()
	{
		if (InventoryGui.Instance == null) return;
		
		for (int i = 0; i < 9; i++) UpdateSlot(i);
		UpdateSelection();
	}
	
	private void UpdateSlot(int slot)
	{
		if (slot >= _slots.Count) return;
		
		Panel panel = _slots[slot];
		
		// Enleve tts elts existants
		foreach (Node child in panel.GetChildren())
		{
			if (child is Sprite2D sprite && sprite.Texture != ResourceLoader.Load<Texture2D>("res://Inven/SlotInv.png"))
				child.QueueFree();
			else if (child is Label)
				child.QueueFree();
		}
		
		Item item = InventoryGui.Instance.GetHotbarItem(slot);
		if (item != null)
		{
			// ajoute icon de l'item
			Vector2 itemPosition = panel.CustomMinimumSize / 2 + new Vector2(6, -9);
			panel.AddChild(new Sprite2D {
				Texture = item.Icon as Texture2D,
				Position = itemPosition,
				Scale = new Vector2(1.5f, 1.5f)
			});
			
			// ajoute label quantité s'il y a + de 1
			if (item.Quantity > 1)
			{
				Vector2 labelPosition = panel.CustomMinimumSize / 2 + new Vector2(8, -2);
				Label label = new Label {
					Text = item.Quantity.ToString(),
					Position = labelPosition,
					Scale = new Vector2(0.9f, 0.9f),
					MouseFilter = Control.MouseFilterEnum.Ignore
				};
				label.AddThemeColorOverride("font_color", Colors.White);
				label.AddThemeColorOverride("font_shadow_color", Colors.Black);
				label.AddThemeConstantOverride("shadow_offset_x", 1);
				label.AddThemeConstantOverride("shadow_offset_y", 1);
				panel.AddChild(label);
			}
		}
	}
	
	private void UpdateSelection()
	{
		if (InventoryGui.Instance == null) return;
		
		int selectedSlot = InventoryGui.Instance.SelectedHotbarSlot;
		for (int i = 0; i < _slots.Count; i++)
			_slots[i].Modulate = i == selectedSlot ? new Color(1.2f, 1.2f, 0.8f) : Colors.White;
	}
}
