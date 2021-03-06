--[[
    GD50
    Super Mario Bros. Remake

    -- LevelMaker Class --

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

LevelMaker = Class{}

function LevelMaker.generate(width, height)
    local tiles = {}
    local entities = {}
    local objects = {}

    local tileID = TILE_ID_GROUND
    
    -- whether we should draw our tiles with toppers
    local topper = true
    local tileset = math.random(20)
    local topperset = math.random(20)

    -- Lock and key references
    local lock_gameobject = false
    local key_gameobject = false
    
    -- whether the lock and key were already spawned
    local lock_placed = false
    local key_placed = false
    
    local pole_placed = false

    -- whether the lock and key were already picked
    local lock_picked = false
    local key_picked = false

    -- insert blank tables into tiles for later access
    for x = 1, height do
        table.insert(tiles, {})
    end

    -- column by column generation instead of row; sometimes better for platformers
    for x = 1, width do
        local tileID = TILE_ID_EMPTY
        
        -- lay out the empty space
        for y = 1, 6 do
            table.insert(tiles[y],
                Tile(x, y, tileID, nil, tileset, topperset))
        end

        function spawnPole()
            pole_placed = true
            pole_frame = math.random(6)

            for pole_part = 1, 3 do
                table.insert(objects, GameObject {
                    texture = 'poles_and_flags',
                    x = (x - 1) * TILE_SIZE,
                    y = (3 - 1 + pole_part) * TILE_SIZE,
                    width = 16,
                    height = 16,
                    
                    -- select random frame from bush_ids whitelist, then random row for variance
                    frame = pole_frame + (pole_part - 1) * 9,
                    
                    consumable = true,
                    onConsume = function(player, object)
                        -- TODO: increase width and level
                        params = {
                            ['width'] = params.width + 50,
                            ['score'] = player.score,
                        }
                        gStateMachine:change('play', params)
                    end
                })
            end

            flag_frame = math.random(4)

            table.insert(objects, GameObject {
                texture = 'poles_and_flags',
                x = (x - 0.5) * TILE_SIZE,
                y = (5 - 1.5) * TILE_SIZE,
                width = 16,
                height = 16,
                
                -- select random frame from bush_ids whitelist, then random row for variance
                frame = 7 * (flag_frame) + 2 * (flag_frame - 1)
            })
        end

        -- chance to just be emptiness
        if math.random(7) == 1 then
            for y = 7, height do
                table.insert(tiles[y],
                    Tile(x, y, tileID, nil, tileset, topperset))
            end
        else
            tileID = TILE_ID_GROUND

            local blockHeight = 4

            for y = 7, height do
                table.insert(tiles[y],
                    Tile(x, y, tileID, y == 7 and topper or nil, tileset, topperset))
            end

            -- Avoiding lock and key placed in the same spot
            local lock_or_key_placed = false

            -- chance to generate a pillar
            if math.random(8) == 1 then
                blockHeight = 2
                
                -- chance to generate bush on pillar
                if math.random(8) == 1 then
                    table.insert(objects,
                        GameObject {
                            texture = 'bushes',
                            x = (x - 1) * TILE_SIZE,
                            y = (4 - 1) * TILE_SIZE,
                            width = 16,
                            height = 16,
                            
                            -- select random frame from bush_ids whitelist, then random row for variance
                            frame = BUSH_IDS[math.random(#BUSH_IDS)] + (math.random(4) - 1) * 7
                        }
                    )
                end
                
                -- pillar tiles
                tiles[5][x] = Tile(x, 5, tileID, topper, tileset, topperset)
                tiles[6][x] = Tile(x, 6, tileID, nil, tileset, topperset)
                tiles[7][x].topper = nil
            
            -- chance to generate bushes
            elseif math.random(8) == 1 then
                table.insert(objects,
                    GameObject {
                        texture = 'bushes',
                        x = (x - 1) * TILE_SIZE,
                        y = (6 - 1) * TILE_SIZE,
                        width = 16,
                        height = 16,
                        frame = BUSH_IDS[math.random(#BUSH_IDS)] + (math.random(4) - 1) * 7,
                        collidable = false
                    }
                )
            else
                if not lock_or_key_placed and not lock_placed then
                    -- chance of spawning a lock
                    if math.random(40) == 1 or (x == (width - 1) and not lock_placed) then
                        lock_or_key_placed = true

                        lock_gameobject = GameObject {
                            texture = 'keys_and_locks',
                            x = (x - 1) * TILE_SIZE,
                            y = (blockHeight - 1) * TILE_SIZE,
                            width = 16,
                            height = 16,
                            frame = 5 + math.random(4) - 1,
                            collidable = true,
                            hit = false,
                            solid = true,

                            onCollide = function(obj)
                                if key_picked == true and not lock_picked then
                                    lock_picked = true
                                    gSounds['pickup']:play()

                                    if not pole_placed then
                                        spawnPole()
                                    end
                                end
                            end
                        }

                        table.insert(objects,
                            lock_gameobject
                        )
                        lock_placed = true
                    end
                end
                if not lock_or_key_placed and not key_placed then
                    -- chance of spawning a key
                    if math.random(45) == 1 or (x == (width) and not key_placed) then
                        lock_or_key_placed = true

                        key_gameobject = GameObject {
                            texture = 'keys_and_locks',
                            x = (x - 1) * TILE_SIZE,
                            y = (6 - 1) * TILE_SIZE,
                            width = 16,
                            height = 16,
                            frame = math.random(4),
                            collidable = true,
                            hit = false,
                            solid = false,
                            consumable = true,
                            
                            onConsume = function(player, object)
                                key_picked = true
                                gSounds['pickup']:play()

                                table.insert(objects, GameObject {
                                    texture = 'keys_and_locks',
                                    x = lock_gameobject.x,
                                    y = lock_gameobject.y,
                                    width = 16,
                                    height = 16,
                                    frame = key_gameobject.frame,
                                    collidable = false,
                                    hit = false,
                                    solid = false,
                                    consumable = false,
                                })
                            end
                        }

                        table.insert(objects,
                            key_gameobject
                        )
                        key_placed = true
                    end
                end
            end

            -- chance to spawn a block
            if not lock_or_key_placed and math.random(10) == 1 then
                table.insert(objects,

                    -- jump block
                    GameObject {
                        texture = 'jump-blocks',
                        x = (x - 1) * TILE_SIZE,
                        y = (blockHeight - 1) * TILE_SIZE,
                        width = 16,
                        height = 16,

                        -- make it a random variant
                        frame = math.random(#JUMP_BLOCKS),
                        collidable = true,
                        hit = false,
                        solid = true,

                        -- collision function takes itself
                        onCollide = function(obj)

                            -- spawn a gem if we haven't already hit the block
                            if not obj.hit then

                                -- chance to spawn gem, not guaranteed
                                if math.random(5) == 1 then

                                    -- maintain reference so we can set it to nil
                                    local gem = GameObject {
                                        texture = 'gems',
                                        x = (x - 1) * TILE_SIZE,
                                        y = (blockHeight - 1) * TILE_SIZE - 4,
                                        width = 16,
                                        height = 16,
                                        frame = math.random(#GEMS),
                                        collidable = true,
                                        consumable = true,
                                        solid = false,

                                        -- gem has its own function to add to the player's score
                                        onConsume = function(player, object)
                                            gSounds['pickup']:play()
                                            player.score = player.score + 100
                                        end
                                    }
                                    
                                    -- make the gem move up from the block and play a sound
                                    Timer.tween(0.1, {
                                        [gem] = {y = (blockHeight - 2) * TILE_SIZE}
                                    })
                                    gSounds['powerup-reveal']:play()

                                    table.insert(objects, gem)
                                end

                                obj.hit = true
                            end

                            gSounds['empty-block']:play()
                        end
                    }
                )
            end
        end
    end

    local map = TileMap(width, height)
    map.tiles = tiles
    
    return GameLevel(entities, objects, map)
end