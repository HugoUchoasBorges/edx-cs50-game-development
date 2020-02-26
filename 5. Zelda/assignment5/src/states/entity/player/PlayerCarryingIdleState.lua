--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

PlayerCarryingIdleState = Class{__includes = EntityIdleState}

function PlayerCarryingIdleState:init(entity)
    self.entity = entity

    self.entity:changeAnimation('carrying-idle-' .. self.entity.direction)

    -- used for AI waiting
    self.waitDuration = 0
    self.waitTimer = 0
end

function PlayerCarryingIdleState:enter(params)
    -- render offset for spaced character sprite
    self.entity.offsetY = 5
    self.entity.offsetX = 0

    self.object = params.object
    self.object.picked = true
end

function PlayerCarryingIdleState:update(dt)
    EntityIdleState.update(self, dt)
end

function PlayerCarryingIdleState:update(dt)
    if love.keyboard.isDown('left') or love.keyboard.isDown('right') or
       love.keyboard.isDown('up') or love.keyboard.isDown('down') then
        params = {
            ['object'] = self.object
        }
        self.entity:changeState('carrying', params)
    end

    if love.keyboard.wasPressed('space') or love.keyboard.wasPressed('enter') or love.keyboard.wasPressed('return') or love.keyboard.wasPressed('f') then
        -- TODO: New State - Throwing
        self.entity:changeState('idle')
        --self.entity:changeState('player-throwing')
        self.entity.walkSpeed = PLAYER_WALK_SPEED
    end
end

function PlayerCarryingIdleState:render()
    EntityIdleState.render(self)

    if self.object then
        local adjacentOffsetX = -1
        local adjacentOffsetY = -8
        love.graphics.draw(gTextures[self.object.texture], gFrames[self.object.texture][self.object.states[self.object.state].frame or self.object.frame],
            self.entity.x + adjacentOffsetX, self.entity.y + adjacentOffsetY)
    end
end