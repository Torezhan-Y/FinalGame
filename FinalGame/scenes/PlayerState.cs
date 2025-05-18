using Godot;

public interface IPlayerState
{
	void HandleInput(Player player);
	void Update(Player player, float delta);
}

public class IdleState : IPlayerState
{
	public void HandleInput(Player player)
	{
		if (Input.IsActionJustPressed("ui_accept") && player.IsOnFloor())
		{
			player.State = new JumpState();
			player.Jump();
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			player.State = new DuckState();
		}
	}

	public void Update(Player player, float delta)
	{
		player.PlayAnimation("idle");

		Vector2 velocity = player.Velocity;
		velocity.X = 0;
		player.Velocity = velocity;
	}
}

public class RunState : IPlayerState
{
	public void HandleInput(Player player)
	{
		if (Input.IsActionJustPressed("ui_accept") && player.IsOnFloor())
		{
			player.State = new JumpState();
			player.Jump();
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			player.State = new DuckState();
		}
	}

	public void Update(Player player, float delta)
	{
		player.PlayAnimation("run");

		Vector2 velocity = player.Velocity;
		velocity.X = 300;
		player.Velocity = velocity;
	}
}

public class JumpState : IPlayerState
{
	public void HandleInput(Player player)
	{
		if (player.IsOnFloor())
		{
			player.State = new IdleState();
		}
	}

	public void Update(Player player, float delta)
	{
		player.PlayAnimation("jump");
	}
}

public class DuckState : IPlayerState
{
	public void HandleInput(Player player)
	{
		if (Input.IsActionJustPressed("ui_accept") && player.IsOnFloor())
		{
			player.State = new JumpState();
			player.Jump();
		}
		else if (!Input.IsActionPressed("ui_down"))
		{
			player.State = new IdleState();
		}
	}

	public void Update(Player player, float delta)
	{
		player.PlayAnimation("duck");
	}
}
