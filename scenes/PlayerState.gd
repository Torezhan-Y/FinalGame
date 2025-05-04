extends Node

# Base class for Player State
class PlayerState:
	func handle_input(player: CharacterBody2D) -> void:
		pass
	func update(player: CharacterBody2D, delta: float) -> void:
		pass

# Idle state
class IdleState extends PlayerState:
	func handle_input(player: CharacterBody2D) -> void:
		if Input.is_action_pressed("ui_accept"):
			player.state = JumpState.new()
		elif Input.is_action_pressed("ui_down"):
			player.state = DuckState.new()

	func update(player: CharacterBody2D, delta: float) -> void:
		player.play_animation("idle")

# Run state
class RunState extends PlayerState:
	func handle_input(player: CharacterBody2D) -> void:
		if Input.is_action_pressed("ui_accept"):
			player.state = JumpState.new()

	func update(player: CharacterBody2D, delta: float) -> void:
		player.play_animation("run")

# Jump state
class JumpState extends PlayerState:
	func handle_input(player: CharacterBody2D) -> void:
		if player.is_on_floor():
			player.state = IdleState.new()

	func update(player: CharacterBody2D, delta: float) -> void:
		player.play_animation("jump")
		# Apply gravity
		player.velocity.y += 4200 * delta

# Duck state
class DuckState extends PlayerState:
	func handle_input(player: CharacterBody2D) -> void:
		if Input.is_action_pressed("ui_accept"):
			player.state = JumpState.new()
		elif not Input.is_action_pressed("ui_down"):
			player.state = RunState.new()

	func update(player: CharacterBody2D, delta: float) -> void:
		player.play_animation("duck")
