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
			GD.Print("Switched to JumpState from IdleState");
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			player.State = new DuckState();
			GD.Print("Switched to DuckState from IdleState");
		}
	}

	public void Update(Player player, float delta)
	{
		GD.Print("IdleState.Update");
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
			GD.Print("Switched to JumpState from RunState");
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			player.State = new DuckState();
			GD.Print("Switched to DuckState from RunState");
		}
	}

	public void Update(Player player, float delta)
	{
		GD.Print("RunState.Update");
		player.PlayAnimation("run");

		Vector2 velocity = player.Velocity;
		velocity.X = 5; // 👈 Настрой под свою скорость
		player.Velocity = velocity;
	}
}

public class JumpState : IPlayerState
{
	public void HandleInput(Player player)
	{
		if (player.IsOnFloor())
		{
			player.State = new RunState(); // Возвращаемся к бегу
			GD.Print("Switched to RunState from JumpState");
		}
	}

	public void Update(Player player, float delta)
	{
		GD.Print("JumpState.Update");
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
			GD.Print("Switched to JumpState from DuckState");
		}
		else if (!Input.IsActionPressed("ui_down"))
		{
			player.State = new RunState(); // Возвращаемся к бегу
			GD.Print("Switched to RunState from DuckState");
		}
	}
	
	public void Update(Player player, float delta)
	{
		GD.Print("DuckState.Update");
		player.PlayAnimation("duck");

		Vector2 velocity = player.Velocity;
		velocity.X = 20;
		player.Velocity = velocity;
	}
}
