extends GutTest

# Integration tests for game flow

var game_controller
var game_state_manager
var wave_manager
var map_generator

func before_each():
	game_controller = autoqfree(GameController.new())
	add_child(game_controller)
	
	game_state_manager = GameStateManager.new()
	wave_manager = WaveManager.new()
	map_generator = MapGenerator.new()
	
	game_controller.add_child(game_state_manager)
	game_controller.add_child(wave_manager)
	game_controller.add_child(map_generator)
	
	# Add camera
	var camera = Camera3D.new()
	game_controller.add_child(camera)

func test_game_initialization():
	assert_not_null(game_controller, "Game controller should be created")
	assert_not_null(game_state_manager, "Game state manager should be created")

func test_map_generation():
	var seed_value = 12345
	map_generator.GenerateMap(seed_value)
	
	await wait_frames(2)
	
	assert_gt(map_generator.GatePositions.size(), 0, "Should have gates")
	assert_gt(map_generator.Paths.size(), 0, "Should have paths")
	assert_eq(map_generator.GatePositions.size(), map_generator.Paths.size(),
		"Should have equal gates and paths")

func test_wave_start_flow():
	game_state_manager.InitializeGame(1)
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Planning)
	
	game_state_manager.StartWave()
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Combat)
	assert_eq(game_state_manager.CurrentWave, 1)

func test_difficulty_scaling_with_players():
	game_state_manager.InitializeGame(1)
	var difficulty_1 = game_state_manager.GetDifficultyMultiplier()
	
	game_state_manager.InitializeGame(4)
	var difficulty_4 = game_state_manager.GetDifficultyMultiplier()
	
	assert_eq(difficulty_1, 1, "Single player should have base difficulty")
	assert_eq(difficulty_4, 4, "Four players should have 4x difficulty")
