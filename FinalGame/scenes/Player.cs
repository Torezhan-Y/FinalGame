using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public IPlayerState State { get; set; }

	private AnimatedSprite2D _animatedSprite;
	private AudioStreamPlayer _jumpSound;

	private bool _gameStarted = false;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_jumpSound = GetNodeOrNull<AudioStreamPlayer>("JumpSound");

		State = new IdleState();
		PlayAnimation("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_gameStarted)
		{
			if (Input.IsActionJustPressed("ui_accept"))
			{
				GD.Print("Pressed ui_accept");
				_gameStarted = true;
				State = new RunState();
				GD.Print("Game started. Switched to RunState.");
			}
			return; // ❗Не выполнять движение и обновления до старта
		}

		// Гравитация
		Vector2 velocity = Velocity;
		velocity.Y += 4800 * (float)delta;
		Velocity = velocity;

		State?.HandleInput(this);
		State?.Update(this, (float)delta);

		MoveAndSlide();
	}

	public void PlayAnimation(string animName)
	{
		if (_animatedSprite != null && _animatedSprite.Animation != animName)
		{
			GD.Print("PlayAnimation: " + animName);
			_animatedSprite.Play(animName);
		}
	}

	public void Jump()
	{
		Vector2 velocity = Velocity;
		velocity.Y = -1500;
		Velocity = velocity;

		_jumpSound?.Play();
	}
}
