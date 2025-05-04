extends CharacterBody2D

const GRAVITY : int = 4200
const JUMP_SPEED : int = -1800

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	velocity.y += GRAVITY * delta
	
	if is_on_floor():  # Check if the player is on the ground
		if Input.is_action_pressed("ui_accept"):  # If the jump button is pressed
			velocity.y = JUMP_SPEED
			$JumpSound.play()  # Play the jump sound when the player jumps
			$AnimatedSprite2D.play("jump")  # Optional: Switch to the jump animation
		elif Input.is_action_pressed("ui_down"):  # If the duck button is pressed
			$AnimatedSprite2D.play("duck")  # Play the duck animation
		else:
			$AnimatedSprite2D.play("run")  # Play the run animation
	else:
		$AnimatedSprite2D.play("jump")  # Play jump animation if in air
		
	move_and_slide()
