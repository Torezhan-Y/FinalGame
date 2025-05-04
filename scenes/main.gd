extends Node

# preload obstacles
var DyingStud_scene = preload("res://scenes/DyingStud.tscn")
var CryingStud_scene = preload("res://scenes/CryingStud.tscn")
var Papers_scene = preload("res://scenes/Papers.tscn")
var obstacle_types := [Papers_scene, DyingStud_scene, CryingStud_scene]
var obstacles : Array
var papers_heights := [100, 250, 200, 160]  # Random heights for BaubekAgai (adjust as needed)

# game variables
const PLAYER_START_POS := Vector2i(150, 485)
const CAM_START_POS := Vector2i(576, 324)
var difficulty
const MAX_DIFFICULTY : int = 2
var score : int
const SCORE_MODIFIER : int = 10
var high_score : int
var speed : float
const START_SPEED : float = 8.0
const MAX_SPEED : int = 25
const SPEED_MODIFIER : int = 5000
var screen_size : Vector2i
var ground_height : int
var game_running : bool
var last_obs
var papers_spawn_timer : int = 0  # Timer to control frequency of BaubekAgai's appearance

# Called when the node enters the scene tree for the first time.
func _ready():
	screen_size = get_window().size
	ground_height = $Ground.get_node("Sprite2D").texture.get_height()
	$GameOver.get_node("Button").pressed.connect(new_game)
	new_game()

func new_game():
	# reset variables
	score = 0
	show_score()
	game_running = false
	get_tree().paused = false
	difficulty = 0
	
	# delete all obstacles
	for obs in obstacles:
		obs.queue_free()
	obstacles.clear()
	
	# reset the nodes
	$Player.position = PLAYER_START_POS  # Make sure 'Player' node exists in the scene
	$Player.velocity = Vector2i(0, 0)
	$Camera2D.position = CAM_START_POS
	$Ground.position = Vector2i(0, 0)
	
	# reset hud and game over screen
	$HUD.get_node("StartLabel").show()
	$GameOver.hide()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if game_running:
		# speed up and adjust difficulty
		speed = START_SPEED + score / float(SPEED_MODIFIER)  # convert to float to avoid integer division warning
		if speed > MAX_SPEED:
			speed = MAX_SPEED
		adjust_difficulty()
		
		# generate obstacles
		generate_obs()
		
		# move player and camera
		$Player.position.x += speed
		$Camera2D.position.x += speed
		
		# update score
		score += speed
		show_score()
		
		# update ground position
		if $Camera2D.position.x - $Ground.position.x > screen_size.x * 1.5:
			$Ground.position.x += screen_size.x
			
		# remove obstacles that have gone off screen
		for obs in obstacles:
			if obs.position.x < ($Camera2D.position.x - screen_size.x):
				remove_obs(obs)
	else:
		if Input.is_action_pressed("ui_accept"):
			game_running = true
			$HUD.get_node("StartLabel").hide()

func generate_obs():
	# generate ground obstacles
	if obstacles.is_empty() or last_obs.position.x < score + randi_range(300, 500):
		var obs_type = obstacle_types[randi() % obstacle_types.size()]
		var obs = obs_type.instantiate()  # Ensure the obstacle is instantiated
		if obs == null:
			return  # If it's null, return early
		
		var obs_x : int = screen_size.x + score + 100
		var obs_y : int = screen_size.y - ground_height + 5
		last_obs = obs
		add_obs(obs, obs_x, obs_y)
	
	# Random chance to spawn BaubekAgai with a delay (controlled spawn rate)
	if papers_spawn_timer <= 0:
		if (randi() % 2) == 0:  # Reduced spawn frequency to avoid spam
			# generate bird obstacles (BaubekAgai)
			var obs = Papers_scene.instantiate()
			if obs == null:
				return  # If it's null, return early
			var obs_x : int = screen_size.x + score + 100
			var obs_y : int = papers_heights[randi() % papers_heights.size()]  # Random height from the list
			add_obs(obs, obs_x, obs_y)
			papers_spawn_timer = 100  # Reset spawn timer
		else:
			papers_spawn_timer -= 1  # Decrease timer if no spawn happens

func add_obs(obs, x, y):
	obs.position = Vector2i(x, y)
	obs.body_entered.connect(hit_obs)
	add_child(obs)
	obstacles.append(obs)

func remove_obs(obs):
	obs.queue_free()
	obstacles.erase(obs)
	
func hit_obs(body):
	if body.name == "Player":
		game_over()

func show_score():
	$HUD.get_node("ScoreLabel").text = "SCORE: " + str(score / SCORE_MODIFIER)

func check_high_score():
	if score > high_score:
		high_score = score
		$HUD.get_node("HighScoreLabel").text = "HIGH SCORE: " + str(high_score / SCORE_MODIFIER)

func adjust_difficulty():
	difficulty = score / SPEED_MODIFIER
	if difficulty > MAX_DIFFICULTY:
		difficulty = MAX_DIFFICULTY

func game_over():
	check_high_score()
	get_tree().paused = true
	game_running = false
	$GameOver.show()
