using Godot;
using System;

public partial class ObstacleFactory : Node
{
	private PackedScene[] obstacleTypes;
	private int[] birdHeights = new int[] { 200, 250, 300, 350 };

	public override void _Ready()
	{
		obstacleTypes = new PackedScene[]
		{
			GD.Load<PackedScene>("res://scenes/DyingStud.tscn"),
			GD.Load<PackedScene>("res://scenes/CryingStud.tscn")
		};
	}

	public Node CreateObstacle(int score)
	{
		var rand = new Random();
		var obsType = obstacleTypes[rand.Next(obstacleTypes.Length)];
		var obs = obsType.Instantiate();

		// Получаем родительский узел, предполагаем, что он имеет свойства screen_size и ground_height
		var parent = GetParent();
		if (parent == null)
		{
			GD.PrintErr("Parent node is null");
			return obs;
		}

		// Попытка получить screen_size и ground_height из родителя
		// В C# можно попробовать кастовать родителя в нужный класс, если есть
		// Если у тебя есть конкретный тип родителя — замени Node на него

		// Здесь делаем предположение, что родитель - Control или Node с public свойствами:
		Vector2 screenSize = Vector2.Zero;
		float groundHeight = 0f;

		// Пример кастинга, если у тебя есть кастомный класс с этими свойствами, например Game or MainScene
		// var mainScene = parent as YourMainSceneClass;
		// if (mainScene != null)
		// {
		//     screenSize = mainScene.screen_size;
		//     groundHeight = mainScene.ground_height;
		// }

		// Если кастинг невозможен, можно попробовать получить viewport размер
		var viewport = GetViewport();
		if (viewport != null)
		{
			screenSize = viewport.GetVisibleRect().Size;
		}

		// groundHeight нужно задать вручную или получить из родителя, если есть
		// Здесь пример фиктивного значения, замени на нужное из твоей сцены
		groundHeight = 50f;

		float obsX = screenSize.X + score + 100;
		float obsY = screenSize.Y - groundHeight + 5;

		if (obs is Node2D obsNode2D)
		{
			obsNode2D.Position = new Vector2(obsX, obsY);
		}
		else
		{
			GD.PrintErr("Obstacle node is not a Node2D");
		}

		return obs;
	}
}
