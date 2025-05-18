using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	public int Score { get; set; } = 0;
	public bool GameRunning { get; set; } = false;
	public int HighScore { get; set; } = 0;

	public override void _Ready()
	{
		Instance = this;
		Score = 0;
		GameRunning = false;
	}
}
