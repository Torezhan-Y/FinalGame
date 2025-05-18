using Godot;

public partial class Command : Node
{
	public virtual void Execute() { }
}

public partial class JumpCommand : Command
{
	private CharacterBody2D player;

	public JumpCommand(CharacterBody2D player)
	{
		this.player = player;
	}

	public override void Execute()
	{
		var velocity = player.Velocity;
		velocity.Y = -1800;
		player.Velocity = velocity;
		// Если у игрока есть метод PlayAnimation:
		// player.PlayAnimation("jump");
	}
}

public partial class MoveLeftCommand : Command
{
	private CharacterBody2D player;

	public MoveLeftCommand(CharacterBody2D player)
	{
		this.player = player;
	}

	public override void Execute()
	{
		player.Position = new Vector2(player.Position.X - 5, player.Position.Y);
		// player.PlayAnimation("run");
	}
}
