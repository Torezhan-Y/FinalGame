using Godot;

public partial class Papers : Area2D
{
	private float speed = 200f;  // скорость движения в пикселях в секунду

	public override void _Process(double delta)
	{
		// Двигаем объект влево (по оси X)
		Position -= new Vector2(speed * (float)delta, 0);

		// Если объект уходит слишком далеко влево, можно его удалить (опционально)
		if (Position.X < -100)
		{
			QueueFree();
		}
	}
}
