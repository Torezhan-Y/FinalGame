using Godot;
using System;

// Interface for movement strategies
public interface IMoveStrategy
{
  void Move(Node2D node, float delta);
}

// Linear movement to the left
public class LinearMoveStrategy : IMoveStrategy
{
  private float speed;

  public LinearMoveStrategy(float speed)
  {
	this.speed = speed;
  }

  public void Move(Node2D node, float delta)
  {
	Vector2 position = node.Position;
	position.X -= speed * delta;
	node.Position = position;
  }
}

// Main Paper class
public partial class Papers : Area2D
{
  private IMoveStrategy moveStrategy;

  public override void _Ready()
  {
	// Set linear movement strategy with speed 200
	moveStrategy = new LinearMoveStrategy(200f);
  }

  public override void _Process(double delta)
  {
	if (moveStrategy != null)
	{
	  moveStrategy.Move(this, (float)delta);
	}

	if (Position.X < -100)
	{
	  QueueFree();
	}
  }
}
