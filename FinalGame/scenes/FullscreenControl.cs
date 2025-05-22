using Godot;
using System;

public partial class FullscreenControl : CheckButton
{
	public override void _Ready()
	{
		// Подписываемся на переключение
		Toggled += OnFullscreenToggled;

		// Устанавливаем начальное состояние галочки в зависимости от текущего режима
		ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
	}

	private void OnFullscreenToggled(bool toggled)
	{
		if (toggled)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			GD.Print("Fullscreen ON");
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			GD.Print("Fullscreen OFF");
		}
	}
}
