using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InventoryGui : Control
{
	public static InventoryGui Instance { get; private set; }
	
	private bool _isOpen = false;
	private GridContainer _gridContainer;
	private List<Item> _items;
	private Item _draggedItem;
	
	// Variables pour la hotbar
	private List<Item> _hotbarItems = new List<Item>(9);
	private int _selectedHotbarSlot = 0;
	public int SelectedHotbarSlot => _selectedHotbarSlot;
	
	// Pour bouger les elts
	private Control _draggedItemVisual;
	private Sprite2D _draggedItemSprite;
	private Label _draggedItemLabel;
	private InventoryHotbar _hotbarUI;
	
	[Export] private int _maxSlots = 63;
	
	public override void _Ready()
	{
		Instance = this;
		Visible = false;
		SetProcessInput(true);
		_gridContainer = GetNode<GridContainer>("NinePatchRect/GridContainer");
		_items = new List<Item>();
		
		// Initialiser la hotbar
		for (int i = 0; i < 9; i++) _hotbarItems.Add(null);
		
		// Pour bouger les items le visuel
		_draggedItemVisual = new Control { Visible = false, MouseFilter = Control.MouseFilterEnum.Ignore };
		AddChild(_draggedItemVisual);
		
		_draggedItemSprite = new Sprite2D { Scale = new Vector2(1.2f, 1.2f) };
		_draggedItemVisual.AddChild(_draggedItemSprite);
		
		_draggedItemLabel = new Label { Scale = new Vector2(0.9f, 0.9f) };
		_draggedItemLabel.AddThemeColorOverride("font_color", Colors.White);
		_draggedItemLabel.AddThemeColorOverride("font_shadow_color", Colors.Black);
		_draggedItemLabel.AddThemeConstantOverride("shadow_offset_x", 1);
		_draggedItemLabel.AddThemeConstantOverride("shadow_offset_y", 1);
		_draggedItemVisual.AddChild(_draggedItemLabel);
		
		////////////////////////////////////////////////////////////////////////// On ajoute ici les items
		AddItem(new Item("Sword", ResourceLoader.Load<Texture2D>("res://Inven/Items/SwordFer.png"), 2, 5));
		AddItem(new Item("Sword", ResourceLoader.Load<Texture2D>("res://Inven/Items/SwordFer.png"), 3, 5));
		AddItem(new Item("Sword", ResourceLoader.Load<Texture2D>("res://Inven/Items/SwordFer.png"), 1, 5));
		AddItem(new Item("Stick", ResourceLoader.Load<Texture2D>("res://Inven/Items/Stick.png"), 2, 64));
		AddItem(new Item("Stick", ResourceLoader.Load<Texture2D>("res://Inven/Items/Stick.png"), 25, 64));
		AddItem(new Item("IronLingot", ResourceLoader.Load<Texture2D>("res://Inven/Items/IronLingot.png"), 20, 64));
		AddItem(new Item("Diamond", ResourceLoader.Load<Texture2D>("res://Inven/Items/Diamond.png"), 20, 64));
		AddItem(new Item("Pickaxe2", ResourceLoader.Load<Texture2D>("res://Inven/Items/Pickaxe2.png"), 1, 1));
		AddItem(new Item("CobbleStone", ResourceLoader.Load<Texture2D>("res://Inven/Items/CobbleStone.png"), 20, 64));
		UpdateUI();
		
	}
	
	public void Open() { _isOpen = true; Visible = true; UpdateUI(); }
	public void Close() { _isOpen = false; Visible = false; }
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("E"))
		{
			if (_isOpen) Close(); else Open();
		}
		
		// Choisir ou aller sur la hotbar touche 1 à 9
		if (@event is InputEventKey key && key.Pressed && key.Keycode >= Key.Key1 && key.Keycode <= Key.Key9)
			SelectHotbarSlot((int)key.Keycode - (int)Key.Key1);
		
		// Pour passer sur la HB
		if (@event is InputEventMouseButton scroll && scroll.Pressed)
		{
			if (scroll.ButtonIndex == MouseButton.WheelUp) SelectHotbarSlot(_selectedHotbarSlot == 0 ? 8 : _selectedHotbarSlot - 1);
			else if (scroll.ButtonIndex == MouseButton.WheelDown) SelectHotbarSlot(_selectedHotbarSlot == 8 ? 0 : _selectedHotbarSlot + 1);
		}
		
		if (_draggedItem != null && @event is InputEventMouseMotion)
			_draggedItemVisual.GlobalPosition = GetViewport().GetMousePosition();
	}
	
	public void SelectHotbarSlot(int slot) { _selectedHotbarSlot = slot; _hotbarUI?.UpdateUI(); }
	public void RegisterHotbarUI(InventoryHotbar hotbarUI) { _hotbarUI = hotbarUI; _hotbarUI?.UpdateUI(); }
	
	public bool AddItem(Item item)
	{
		if (item == null) return false;
		
		// Stacke les elts existants
		for (int i = 0; i < _items.Count; i++)
			if (_items[i].CanStackWith(item) && _items[i].TryStackWith(item)) return true;
		
		// Ajoute les nv elts si on y arrive
		if (item.Quantity > 0 && _items.Count < _maxSlots)
		{
			_items.Add(item);
			return true;
		}
		return false;
	}
	
	
	public Item GetHotbarItem(int slot) => (slot >= 0 && slot < 9) ? _hotbarItems[slot] : null;
	public Item GetSelectedHotbarItem() => GetHotbarItem(_selectedHotbarSlot);
	
	public void SetHotbarItem(int slot, Item item)
	{
		if (slot >= 0 && slot < 9) { _hotbarItems[slot] = item; _hotbarUI?.UpdateUI(); }
	}
	
	public Item TakeHotbarItem(int slot)
	{
		if (slot >= 0 && slot < 9)
		{
			Item item = _hotbarItems[slot];
			_hotbarItems[slot] = null;
			_hotbarUI?.UpdateUI();
			return item;
		}
		return null;
	}
	
	public void UpdateUI()
	{
		// Enleve tt enfants existants
		foreach (Node child in _gridContainer.GetChildren()) child.QueueFree();

		// Ajoute elts dans l'inv
		for (int i = 0; i < _maxSlots; i++)
		{
			Panel panel = new Panel { CustomMinimumSize = new Vector2(34, 34), Name = $"Panel{i}" };
			int slotIndex = i;
			panel.GuiInput += (@event) => OnSlotInput(@event, slotIndex);
			
			// ajoute des slots
			panel.AddChild(new Sprite2D {
				Texture = ResourceLoader.Load<Texture2D>("res://Inven/SlotInv.png"),
				Position = new Vector2(17, 17),
				Scale = new Vector2(2, 2),
				RegionEnabled = true,
				RegionRect = new Rect2(105, 7, 17, 16)
			});
			
			// ajoute l'icon s'il y a un item
			if (i < _items.Count && _items[i] != null)
			{
				// calculer la pos de l'item 
				Vector2 itemPosition = panel.CustomMinimumSize / 2 + new Vector2(6, -9);
				panel.AddChild(new Sprite2D {
					Texture = _items[i].Icon as Texture2D, 
					Position = itemPosition,
					Scale = new Vector2(1.5f, 1.5f)
				});
				
				// ajoute label quantité s'il y a + de 1
				if (_items[i].Quantity > 1)
				{
					// calcul la pos du label
					Vector2 labelPosition = panel.CustomMinimumSize / 2 + new Vector2(8, -2);
					Label label = new Label {
						Text = _items[i].Quantity.ToString(),
						Position = labelPosition,
						Scale = new Vector2(0.9f, 0.9f)
					};
					label.AddThemeColorOverride("font_color", Colors.White);
					label.AddThemeColorOverride("font_shadow_color", Colors.Black);
					label.AddThemeConstantOverride("shadow_offset_x", 1);
					label.AddThemeConstantOverride("shadow_offset_y", 1);
					panel.AddChild(label);
				}
			}
			
			_gridContainer.AddChild(panel);
		}
	}
	
	private void OnSlotInput(InputEvent @event, int slotIndex)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed)
		{
			if (mouse.ButtonIndex == MouseButton.Left && slotIndex < _items.Count && _items[slotIndex] != null)
				StartDragging(_items[slotIndex], slotIndex);
			else if (mouse.ButtonIndex == MouseButton.Right && slotIndex < _items.Count && _items[slotIndex]?.Quantity > 1)
				SplitStack(slotIndex);
		}
		if (@event is InputEventMouseButton release && !release.Pressed && _draggedItem != null)
			DropItem(slotIndex);
	}
	
	public void HandleHotbarSlotInput(InputEvent @event, int slotIndex)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed)
		{
			if (mouse.ButtonIndex == MouseButton.Left)
			{
				if (_draggedItem != null) DropHotbarItem(slotIndex);
				else if (slotIndex < _hotbarItems.Count && _hotbarItems[slotIndex] != null)
					StartDraggingFromHotbar(_hotbarItems[slotIndex], slotIndex);
			}
			else if (mouse.ButtonIndex == MouseButton.Right && slotIndex < _hotbarItems.Count && _hotbarItems[slotIndex]?.Quantity > 1)
				SplitHotbarStack(slotIndex);
		}
		if (@event is InputEventMouseButton release && !release.Pressed && _draggedItem != null)
			DropHotbarItem(slotIndex);
	}
	
	private void StartDragging(Item item, int fromSlot)
	{
		if (_draggedItem != null) return;
		
		_draggedItem = item;
		_items[fromSlot] = null;
		SetDraggedItemVisual(item);
		UpdateUI();
	}
	
	private void StartDraggingFromHotbar(Item item, int fromSlot)
	{
		if (_draggedItem != null) return;
		
		_draggedItem = item;
		_hotbarItems[fromSlot] = null;
		SetDraggedItemVisual(item);
		_hotbarUI?.UpdateUI();
	}
	
	private void SetDraggedItemVisual(Item item)
	{
		_draggedItemSprite.Texture = item.Icon as Texture2D;
		_draggedItemLabel.Text = item.Quantity.ToString();
		_draggedItemLabel.Visible = item.Quantity > 1;
		_draggedItemVisual.Visible = true;
		_draggedItemVisual.GlobalPosition = GetViewport().GetMousePosition();
	}
	
	//les slots maj
	private void DropItem(int toSlot)
	{
		if (_draggedItem == null) return;
		
		if (toSlot >= _maxSlots || toSlot < 0)
		{
			toSlot = FindBestSlot(_draggedItem);
			if (toSlot == -1) return;
		}
		
		while (_items.Count <= toSlot) _items.Add(null);
		
		if (toSlot < _items.Count && _items[toSlot] != null)
		{
			if (_items[toSlot].CanStackWith(_draggedItem))
			{
				if (!_items[toSlot].TryStackWith(_draggedItem))
					(_items[toSlot], _draggedItem) = (_draggedItem, _items[toSlot]);
				else _draggedItem = null;
			}
			else (_items[toSlot], _draggedItem) = (_draggedItem, _items[toSlot]);
		}
		else
		{
			_items[toSlot] = _draggedItem;
			_draggedItem = null;
		}
		
		_draggedItemVisual.Visible = false;
		UpdateUI();
	}
	
	// On mets les items dans la HB ici
	private void DropHotbarItem(int toSlot)
	{
		if (_draggedItem == null) return;
		
		if (toSlot < 0 || toSlot >= 9)
		{
			AddItem(_draggedItem);
			_draggedItem = null;
			_draggedItemVisual.Visible = false;
			return;
		}
		
		if (_hotbarItems[toSlot] != null)
		{
			if (_hotbarItems[toSlot].CanStackWith(_draggedItem))
			{
				if (!_hotbarItems[toSlot].TryStackWith(_draggedItem))
					(_hotbarItems[toSlot], _draggedItem) = (_draggedItem, _hotbarItems[toSlot]);
				else _draggedItem = null;
			}
			else (_hotbarItems[toSlot], _draggedItem) = (_draggedItem, _hotbarItems[toSlot]);
		}
		else
		{
			_hotbarItems[toSlot] = _draggedItem;
			_draggedItem = null;
		}
		
		_draggedItemVisual.Visible = false;
		_hotbarUI?.UpdateUI();
	}
	// Pour enlever/diviser/séparer les items 
	private void SplitStack(int slotIndex)
	{
		Item item = _items[slotIndex];
		if (item?.Quantity <= 1) return;
		
		int splitAmount = item.Quantity > 5 ? 1 : item.Quantity / 2;
		Item splitItem = item.SplitStack(splitAmount);
		
		if (splitItem != null)
		{
			int emptySlot = FindEmptySlot();
			if (emptySlot >= 0)
			{
				while (_items.Count <= emptySlot) _items.Add(null);
				_items[emptySlot] = splitItem;
				UpdateUI();
			}
			else item.TryStackWith(splitItem);
		}
	}
	
	// Pour séparer les items sur la HB 
	private void SplitHotbarStack(int slotIndex)
	{
		Item item = _hotbarItems[slotIndex];
		if (item?.Quantity <= 1) return;
		
		int splitAmount = item.Quantity > 5 ? 1 : item.Quantity / 2;
		Item splitItem = item.SplitStack(splitAmount);
		
		if (splitItem != null)
		{
			int emptySlot = Array.FindIndex(_hotbarItems.ToArray(), i => i == null);
			
			if (emptySlot >= 0) _hotbarItems[emptySlot] = splitItem;
			else if (!AddItem(splitItem)) item.TryStackWith(splitItem);
			
			_hotbarUI?.UpdateUI();
		}
	}
	
	private int FindBestSlot(Item item) => _items.FindIndex(i => i?.CanStackWith(item) == true && i.Quantity < i.MaxStackSize);
	private int FindEmptySlot() => Enumerable.Range(0, _maxSlots).FirstOrDefault(i => i >= _items.Count || _items[i] == null, -1);
	public bool HasDraggedItem() => _draggedItem != null;
}
