using Godot;
using System;

public partial class InventoryGui : Control
{
	private bool _isOpen = false;
	

	
	public void Open()
	{
		_isOpen = true;
		Visible = true;
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
	
	
}
