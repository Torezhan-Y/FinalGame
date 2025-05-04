extends Node

class_name GameManager

var score: int = 0
var game_running: bool = false
var high_score: int = 0

# Static method for getting the instance
static var instance: GameManager

func _ready():
	instance = self  # Set this instance as the global singleton
	score = 0
	game_running = false
