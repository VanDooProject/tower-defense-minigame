extends GutTest

# Unit tests for GameStateManager

var game_state_manager

func before_each():
	game_state_manager = autoqfree(GameStateManager.new())
	add_child(game_state_manager)

func test_initial_state():
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Lobby,
		"Should start in Lobby phase")

func test_initialize_game():
	game_state_manager.InitializeGame(2)
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Planning,
		"Should transition to Planning phase after initialization")
	assert_eq(game_state_manager.PlayerCount, 2, "Should have 2 players")

func test_start_wave():
	game_state_manager.InitializeGame(1)
	game_state_manager.StartWave()
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Combat,
		"Should transition to Combat phase when wave starts")
	assert_eq(game_state_manager.CurrentWave, 1, "Should be on wave 1")

func test_complete_wave():
	game_state_manager.InitializeGame(1)
	game_state_manager.StartWave()
	game_state_manager.CompleteWave()
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Planning,
		"Should return to Planning phase after wave completion")

func test_difficulty_scaling():
	game_state_manager.InitializeGame(1)
	var difficulty1 = game_state_manager.GetDifficultyMultiplier()
	
	game_state_manager.InitializeGame(4)
	var difficulty4 = game_state_manager.GetDifficultyMultiplier()
	
	assert_gt(difficulty4, difficulty1, "Difficulty should scale with player count")

func test_victory_condition():
	game_state_manager.MaxWaves = 3
	game_state_manager.InitializeGame(1)
	
	for i in range(3):
		game_state_manager.StartWave()
		game_state_manager.CompleteWave()
	
	assert_eq(game_state_manager.CurrentPhase, GameStateManager.GamePhase.Victory,
		"Should transition to Victory phase after completing all waves")
