using Godot;
using System;

public partial class PauseMenu : Control
{
	private Button _resumeButton;
	private Button _restartButton;
	private Button _quitButton;
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		// Путь до кнопок через VBoxContainer
		var vbox = GetNode<VBoxContainer>("PanelContainer/VBoxContainer");
		_resumeButton = vbox.GetNode<Button>("Resume");
		_restartButton = vbox.GetNode<Button>("Restart");
		_quitButton = vbox.GetNode<Button>("Quit");

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_resumeButton.Pressed += OnResumePressed;
		_restartButton.Pressed += OnRestartPressed;
		_quitButton.Pressed += OnQuitPressed;
		
		
		
		Visible = false; // Меню скрыто при старте
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel")) // по Esc
		{
			if (Visible)
				HidePauseMenu();
			else
				ShowPauseMenu();
		}
	}

	private void ShowPauseMenu()
	{
		Visible = true;
		GetTree().Paused = true;
		_animationPlayer?.Play("blur");
	}

	private void HidePauseMenu()
	{
		Visible = false;
		GetTree().Paused = false;
		_animationPlayer?.Play("blur");
	}

	private void OnResumePressed() => HidePauseMenu();

	private void OnRestartPressed()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	private void OnQuitPressed() => GetTree().Quit();
}
