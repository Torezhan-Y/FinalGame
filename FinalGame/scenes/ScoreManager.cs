using Godot;
using System.Collections.Generic;

public partial class ScoreManager : Node
{
	private List<Node> observers = new List<Node>();
	public int Score { get; private set; } = 0;

	// Добавление наблюдателя
	public void AddObserver(Node observer)
	{
		observers.Add(observer);
	}

	// Уведомление всех наблюдателей
	public void Notify()
	{
		foreach (var observer in observers)
		{
			if (observer.HasMethod("OnScoreUpdated"))
			{
				observer.Call("OnScoreUpdated", Score);
			}
		}
	}

	// Обновление счёта
	public void UpdateScore(int points)
	{
		Score += points;
		Notify();
	}
}
