using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
	// preload obstacles
	PackedScene DyingStudScene = GD.Load<PackedScene>("res://scenes/DyingStud.tscn");
	PackedScene CryingStudScene = GD.Load<PackedScene>("res://scenes/CryingStud.tscn");
	PackedScene PapersScene = GD.Load<PackedScene>("res://scenes/Papers.tscn");

	List<PackedScene> obstacleTypes;
	List<Node2D> obstacles = new();
	int[] papersHeights = { 250, 200, 310 };

	// game variables
	readonly Vector2I PlayerStartPos = new Vector2I(150, 485);
	readonly Vector2I CamStartPos = new Vector2I(576, 324);
	int difficulty;
	const int MaxDifficulty = 2;
	int score;
	const int ScoreModifier = 10;
	int highScore;
	float speed;
	const float StartSpeed = 8.0f;
	const int MaxSpeed = 25;
	const int SpeedModifier = 5000;
	Vector2I screenSize;
	int groundHeight;
	bool gameRunning;
	Node2D lastObs;
	int papersSpawnTimer = 1;

	public override void _Ready()
	{
		screenSize = GetWindow().Size;
		groundHeight = GetNode<Sprite2D>("Ground/Sprite2D").Texture.GetHeight();

		GetNode<Button>("GameOver/Button").Pressed += NewGame;

		obstacleTypes = new List<PackedScene> { PapersScene, DyingStudScene, CryingStudScene };

		NewGame();
	}

	void NewGame()
	{
		score = 0;
		ShowScore();
		gameRunning = false;
		GetTree().Paused = false;
		difficulty = 0;

		foreach (var obs in obstacles)
			obs.QueueFree();
		obstacles.Clear();

		var player = GetNode<Node2D>("Player");
		player.Position = PlayerStartPos;
		player.Set("velocity", new Vector2I(0, 0));

		GetNode<Camera2D>("Camera2D").Position = CamStartPos;
		GetNode<Node2D>("Ground").Position = new Vector2I(0, 0);

		GetNode<Label>("HUD/StartLabel").Show();
		GetNode<CanvasLayer>("GameOver").Hide();
	}

	public override void _Process(double delta)
	{
		if (gameRunning)
		{
			speed = StartSpeed + score / (float)SpeedModifier;
			if (speed > MaxSpeed)
				speed = MaxSpeed;

			AdjustDifficulty();
			GenerateObs();

			var player = GetNode<Node2D>("Player");
			var camera = GetNode<Camera2D>("Camera2D");

			player.Position += new Vector2((float)speed, 0);
			camera.Position += new Vector2((float)speed, 0);

			score += (int)speed;
			ShowScore();

			var ground = GetNode<Node2D>("Ground");
			if (camera.Position.X - ground.Position.X > screenSize.X * 1.5f)
				ground.Position += new Vector2(screenSize.X, 0);

			for (int i = obstacles.Count - 1; i >= 0; i--)
			{
				if (obstacles[i].Position.X < camera.Position.X - screenSize.X)
					RemoveObs(obstacles[i]);
			}
		}
		else
		{
			if (Input.IsActionPressed("ui_accept"))
			{
				gameRunning = true;
				GetNode<Label>("HUD/StartLabel").Hide();
			}
		}
	}

	void GenerateObs()
	{
		if (obstacles.Count == 0 || lastObs.Position.X < score + (int)GD.RandRange(300, 500))
		{
			var obsType = obstacleTypes[(int)GD.Randi() % obstacleTypes.Count];
			var obs = obsType.Instantiate<Node2D>();
			if (obs == null)
				return;

			int obsX = screenSize.X + score + 100;
			int obsY = screenSize.Y - groundHeight + 5;
			lastObs = obs;
			AddObs(obs, obsX, obsY);
		}

		if (papersSpawnTimer <= 0)
		{
			if ((GD.Randi() % 2) == 0)
			{
				var obs = PapersScene.Instantiate<Node2D>();
				if (obs == null)
					return;

				int obsX = screenSize.X + score + 100;
				int obsY = papersHeights[(int)GD.Randi() % papersHeights.Length];
				AddObs(obs, obsX, obsY);
				papersSpawnTimer = 100;
			}
		}
		else
		{
			papersSpawnTimer--;
		}
	}

	void AddObs(Node2D obs, int x, int y)
	{
		obs.Position = new Vector2I(x, y);
		obs.Connect("body_entered", new Callable(this, nameof(HitObs)));
		AddChild(obs);
		obstacles.Add(obs);
	}

	void RemoveObs(Node2D obs)
	{
		obs.QueueFree();
		obstacles.Remove(obs);
	}

	void HitObs(Node body)
	{
		if (body.Name == "Player")
		{
			GameOver();
		}
	}

	void ShowScore()
	{
		GetNode<Label>("HUD/ScoreLabel").Text = $"SCORE: {score / ScoreModifier}";
	}

	void CheckHighScore()
	{
		if (score > highScore)
		{
			highScore = score;
			GetNode<Label>("HUD/HighScoreLabel").Text = $"HIGH SCORE: {highScore / ScoreModifier}";
		}
	}

	void AdjustDifficulty()
	{
		difficulty = score / SpeedModifier;
		if (difficulty > MaxDifficulty)
			difficulty = MaxDifficulty;
	}

	void GameOver()
	{
		CheckHighScore();
		GetTree().Paused = true;
		gameRunning = false;
		GetNode<CanvasLayer>("GameOver").Show();
	}
}
