extends Node

class_name ScoreManager

var observers: Array = []
var score: int = 0

# Register observers
func add_observer(observer: Node):
	observers.append(observer)

# Notify observers
func notify():
	for observer in observers:
		observer.on_score_updated(score)

# Update the score
func update_score(points: int):
	score += points
	notify()
