--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

GameObject = Class{}

function GameObject:init(def, x, y)
    -- string identifying this object type
    self.type = def.type

    self.texture = def.texture
    self.frame = def.frame or 1

    -- whether it acts as an obstacle or not
    self.solid = def.solid

    -- whether the object can be grabble
    self.grabbable = def.grabbable
    self.breaked = def.breaked

    -- Throwing variables
    self.direction = ''
    self.initial_x = 0
    self.initial_y = 0
    self.currentEntities = false

    self.defaultState = def.defaultState
    self.state = self.defaultState
    self.states = def.states

    -- dimensions
    self.x = x
    self.y = y
    self.width = def.width
    self.height = def.height

    self.dx = 0
    self.dy = 0

    -- default empty collision callback
    self.onCollide = function() end
end

function GameObject:update(dt)
    if not self.breaked and (self.dx ~= 0 or self.dy ~= 0) then 
        self.x = self.x + self.dx * dt
        self.y = self.y + self.dy * dt

        if math.abs( self.x - self.initial_x ) + math.abs( self.y - self.initial_y ) >= 4 * TILE_SIZE then 
            self.breaked=true
        end

        -- Collision with Walls

        local bottomEdge = VIRTUAL_HEIGHT - (VIRTUAL_HEIGHT - MAP_HEIGHT * TILE_SIZE) 

        if self.x <= MAP_RENDER_OFFSET_X + TILE_SIZE or 
        self.x + self.width >= VIRTUAL_WIDTH - TILE_SIZE * 2 or 
        self.y <= MAP_RENDER_OFFSET_Y + TILE_SIZE - self.height / 2 or 
        self.y + self.height >= bottomEdge then 
            self.x = MAP_RENDER_OFFSET_X + TILE_SIZE
            self.breaked = true
        end

        for k, entity in pairs(self.currentEntities) do 
            if not entity.dead and entity:collides(self) then 
                entity:damage(1)
                gSounds['hit-enemy']:play()

                self.breaked = true
            end
        end
    end

    -- TODO: Destroy object after
        -- Hit an enemy (damage)
        -- Hit an wall
        -- Slide for 4 TILESIZEs
end

function GameObject:fire(x, y, direction, dungeon)
    -- Throws the gameobject as a projectile
    self.initial_x = x
    self.initial_y = y

    dx = 0
    dy = 0
    if direction == 'left' then            
        dx = -1
    elseif direction == 'right' then
        dx = 1
    elseif direction == 'up' then
        dy = -1
    else
        dy = 1
    end

    self.breaked = false
    self.picked = false
    self.x = x
    self.y = y
    self.dx = dx * PLAYER_THROWING_SPEED
    self.dy = dy * PLAYER_THROWING_SPEED

    self.currentEntities = dungeon.currentRoom.entities
end

function GameObject:render(adjacentOffsetX, adjacentOffsetY)
    if not self.collected and not self.picked and not self.breaked then
        love.graphics.draw(gTextures[self.texture], gFrames[self.texture][self.states[self.state].frame or self.frame],
            self.x + adjacentOffsetX, self.y + adjacentOffsetY)
    end
end