--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

PlayerCarryingState = Class{__includes = BaseState}

function PlayerCarryingState:init(player, dungeon)
    self.entity = player
    self.dungeon = dungeon

    self.entity.walkSpeed = PLAYER_CARRYING_SPEED

    -- render offset for spaced character sprite
    self.entity.offsetY = 5
    self.entity.offsetX = 0

    self.entity:changeAnimation('carrying-' .. self.entity.direction)
end

function PlayerCarryingState:enter(params)

    -- restart sword swing animation
    self.entity.currentAnimation:refresh()

    self.object = params.object
end

function PlayerCarryingState:update(dt)

    if love.keyboard.wasPressed('space') or love.keyboard.wasPressed('enter') or love.keyboard.wasPressed('return') or love.keyboard.wasPressed('f') then
        params = {
            ['object'] = self.object
        }
        self.entity:changeState('throwing', params)
        self.entity.walkSpeed = PLAYER_WALK_SPEED
    end

    if love.keyboard.isDown('left') then
        self.entity.direction = 'left'
        self.entity:changeAnimation('carrying-left')
    elseif love.keyboard.isDown('right') then
        self.entity.direction = 'right'
        self.entity:changeAnimation('carrying-right')
    elseif love.keyboard.isDown('up') then
        self.entity.direction = 'up'
        self.entity:changeAnimation('carrying-up')
    elseif love.keyboard.isDown('down') then
        self.entity.direction = 'down'
        self.entity:changeAnimation('carrying-down')
    else
        params = {
            ['object'] = self.object
        }
        self.entity:changeState('carrying-idle', params)
        self.entity.walkSpeed = PLAYER_WALK_SPEED
    end

    -- perform base collision detection against walls
    EntityWalkState.update(self, dt)

    if not bumped then 
        for k, object in pairs(self.dungeon.currentRoom.objects) do
            if object.solid and not object.picked and not object.breaked and self.entity:collides(object) then
                self.bumped = true
                
                if self.entity.direction == 'left' then            
                    self.entity.x = self.entity.x + PLAYER_CARRYING_SPEED * dt
                elseif self.entity.direction == 'right' then
            
                    -- temporarily adjust position
                    self.entity.x = self.entity.x - PLAYER_CARRYING_SPEED * dt
                elseif self.entity.direction == 'up' then
            
                    -- temporarily adjust position
                    self.entity.y = self.entity.y + PLAYER_CARRYING_SPEED * dt
                else
            
                    -- temporarily adjust position
                    self.entity.y = self.entity.y - PLAYER_CARRYING_SPEED * dt
                end
            end
        end
    end

    -- if we bumped something when checking collision, check any object collisions
    if self.bumped then
        if self.entity.direction == 'left' then
            
            -- temporarily adjust position
            self.entity.x = self.entity.x - PLAYER_CARRYING_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.y = doorway.y + 4
                    Event.dispatch('shift-left')
                end
            end

            -- readjust
            self.entity.x = self.entity.x + PLAYER_CARRYING_SPEED * dt
        elseif self.entity.direction == 'right' then
            
            -- temporarily adjust position
            self.entity.x = self.entity.x + PLAYER_CARRYING_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.y = doorway.y + 4
                    Event.dispatch('shift-right')
                end
            end

            -- readjust
            self.entity.x = self.entity.x - PLAYER_CARRYING_SPEED * dt
        elseif self.entity.direction == 'up' then
            
            -- temporarily adjust position
            self.entity.y = self.entity.y - PLAYER_CARRYING_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.x = doorway.x + 8
                    Event.dispatch('shift-up')
                end
            end

            -- readjust
            self.entity.y = self.entity.y + PLAYER_CARRYING_SPEED * dt
        else
            
            -- temporarily adjust position
            self.entity.y = self.entity.y + PLAYER_CARRYING_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.x = doorway.x + 8
                    Event.dispatch('shift-down')
                end
            end

            -- readjust
            self.entity.y = self.entity.y - PLAYER_CARRYING_SPEED * dt
        end
    end
end

function PlayerCarryingState:render()
    local anim = self.entity.currentAnimation
    love.graphics.draw(gTextures[anim.texture], gFrames[anim.texture][anim:getCurrentFrame()],
        math.floor(self.entity.x - self.entity.offsetX), math.floor(self.entity.y - self.entity.offsetY))

    -- debug for player and hurtbox collision rects
    -- love.graphics.setColor(255, 0, 255, 255)
    -- love.graphics.rectangle('line', self.entity.x, self.entity.y, self.entity.width, self.entity.height)
    -- love.graphics.rectangle('line', self.swordHurtbox.x, self.swordHurtbox.y,
    --     self.swordHurtbox.width, self.swordHurtbox.height)
    -- love.graphics.setColor(255, 255, 255, 255)
end

function PlayerCarryingState:render()
    EntityIdleState.render(self)

    if self.object then
        local adjacentOffsetX = -1
        local adjacentOffsetY = -8
        love.graphics.draw(gTextures[self.object.texture], gFrames[self.object.texture][self.object.states[self.object.state].frame or self.object.frame],
            self.entity.x + adjacentOffsetX, self.entity.y + adjacentOffsetY)
    end
end