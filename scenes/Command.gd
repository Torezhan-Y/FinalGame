extends Node

class Command:
	func execute() -> void:
		pass

class JumpCommand extends Command:
	var player: CharacterBody2D

	func _init(player: CharacterBody2D):
		self.player = player

	func execute() -> void:
		player.velocity.y = -1800
		player.play_animation("jump")

class MoveLeftCommand extends Command:
	var player: CharacterBody2D

	func _init(player: CharacterBody2D):
		self.player = player

	func execute() -> void:
		player.position.x -= 5
		player.play_animation("run")
