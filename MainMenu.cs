using Godot;
using System;

public partial class MainMenu : Control
{
	private VBoxContainer _mainButtons;
	private Panel _optionsPanel;
	private Button _startButton;
	private Button _optionsButton;
	private Button _exitButton;
	private Button _backButton;
	private HSlider _volumeSlider;

	public override void _Ready()
	{
		_mainButtons = GetNode<VBoxContainer>("MainButtons");
		_optionsPanel = GetNode<Panel>("OptionsPanel");

		_startButton = _mainButtons.GetNode<Button>("StartButton");
		_optionsButton = _mainButtons.GetNode<Button>("OptionsButton");
		_exitButton = _mainButtons.GetNode<Button>("ExitButton");
		_backButton = _optionsPanel.GetNode<Button>("BackButton");

		_volumeSlider = _optionsPanel.GetNode<HSlider>("VolumeSlider");
		_volumeSlider.ValueChanged += OnVolumeSliderChanged;

		_startButton.Pressed += OnStartButtonPressed;
		_optionsButton.Pressed += OnOptionsButtonPressed;
		_exitButton.Pressed += OnExitButtonPressed;
		_backButton.Pressed += OnBackButtonPressed;

		_optionsPanel.Visible = false;

		// Установка значения по умолчанию (громкость 100%)
		_volumeSlider.Value = 0; // 0 дБ — максимальная громкость
	}

	private void OnStartButtonPressed() 
	{
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}

	private void OnOptionsButtonPressed()
	{
		_mainButtons.Visible = false;
		_optionsPanel.Visible = true;
	}

	private void OnBackButtonPressed()
	{
		_mainButtons.Visible = true;
		_optionsPanel.Visible = false;
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}

	private void OnVolumeSliderChanged(double value)
	{
		// Изменяет громкость master канала
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), (float)value);
	}
}
