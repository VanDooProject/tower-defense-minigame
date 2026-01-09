extends GutTest

# Unit tests for MapGenerator

var map_generator

func before_each():
	map_generator = autoqfree(MapGenerator.new())
	add_child(map_generator)

func test_map_generation_with_seed():
	var seed1 = 12345
	map_generator.GenerateMap(seed1)
	var gates1 = map_generator.GatePositions.duplicate()
	
	# Clear and regenerate with same seed
	map_generator.GenerateMap(seed1)
	var gates2 = map_generator.GatePositions
	
	assert_eq(gates1.size(), gates2.size(), "Should generate same number of gates")
	
	# Check if gates are in approximately same positions (due to seeded RNG)
	for i in range(gates1.size()):
		assert_almost_eq(gates1[i].x, gates2[i].x, 0.1,
			"Gate X positions should match with same seed")
		assert_almost_eq(gates1[i].z, gates2[i].z, 0.1,
			"Gate Z positions should match with same seed")

func test_different_seeds_produce_different_maps():
	map_generator.GenerateMap(12345)
	var gates1 = map_generator.GatePositions.duplicate()
	
	map_generator.GenerateMap(67890)
	var gates2 = map_generator.GatePositions
	
	var different = false
	for i in range(min(gates1.size(), gates2.size())):
		if gates1[i].distance_to(gates2[i]) > 1.0:
			different = true
			break
	
	assert_true(different, "Different seeds should produce different maps")

func test_paths_generated_for_each_gate():
	map_generator.GenerateMap(12345)
	assert_eq(map_generator.GatePositions.size(), map_generator.Paths.size(),
		"Should have one path per gate")

func test_paths_lead_to_castle():
	map_generator.GenerateMap(12345)
	var castle_pos = map_generator.CastlePosition
	
	for path in map_generator.Paths:
		var last_waypoint = path[path.size() - 1]
		assert_almost_eq(last_waypoint.distance_to(castle_pos), 0, 1.0,
			"Path should end near castle")
