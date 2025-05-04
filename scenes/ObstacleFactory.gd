extends Node

class_name ObstacleFactory

var obstacle_types := []
var bird_heights := [200, 250, 300, 350]

func _ready():
	# Preload obstacle scenes
	obstacle_types = [
		preload("res://scenes/DyingStud.tscn"),
		preload("res://scenes/CryingStud.tscn")
	]
	
# Create a random obstacle
func create_obstacle(score: int) -> Node:
	var obs_type = obstacle_types[randi() % obstacle_types.size()]
	var obs = obs_type.instantiate()
	var obs_x = get_parent().screen_size.x + score + 100
	var obs_y = get_parent().screen_size.y - get_parent().ground_height + 5
	obs.position = Vector2(obs_x, obs_y)
	return obs
