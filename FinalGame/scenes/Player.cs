using Godot;

public partial class Player : CharacterBody2D
{
	public IPlayerState State { get; set; }

	private AnimatedSprite2D _animatedSprite;
	private AudioStreamPlayer _jumpSound;

	private bool _gameStarted = false;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_jumpSound = GetNode<AudioStreamPlayer>("JumpSound");

		State = new IdleState();
		PlayAnimation("idle");
	}

	public void PlayAnimation(string animName)
	{
		// Принудительно воспроизводим анимацию
		_animatedSprite?.Play(animName);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_gameStarted)
		{
			if (Input.IsActionJustPressed("ui_accept"))
			{
				_gameStarted = true;
				State = new RunState();
			}
		}

		Vector2 velocity = Velocity;
		velocity.Y += 4200 * (float)delta;
		Velocity = velocity;

		State?.HandleInput(this);
		State?.Update(this, (float)delta);

		MoveAndSlide();
	}

	public void Jump()
	{
		Vector2 velocity = Velocity;
		velocity.Y = -1500;
		Velocity = velocity;

		_jumpSound?.Play();
	}
}
