--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

PlayerThrowingState = Class{__includes = EntityIdleState}

function PlayerThrowingState:init(entity, dungeon)
    self.entity = entity
    self.dungeon = dungeon

    self.entity:changeAnimation('idle-' .. self.entity.direction)
end

function PlayerThrowingState:enter(params)
    -- render offset for spaced character sprite
    gSounds['sword']:play()
    self.entity.offsetY = 5
    self.entity.offsetX = 0

    self.object = params.object
    local dx = 0
    local dy = 0
    
    self.object:fire(self.entity.x + self.entity.offsetX, self.entity.y + self.entity.offsetY, self.entity.direction, self.dungeon)
end

function PlayerThrowingState:update(dt)
    EntityIdleState.update(self, dt)
end

function PlayerThrowingState:update(dt)
    self.entity.currentAnimation.timesPlayed = 0
    self.entity:changeState('idle')
end

function PlayerThrowingState:render()
    EntityIdleState.render(self)

    if self.object then
        local adjacentOffsetX = -1
        local adjacentOffsetY = -8
        love.graphics.draw(gTextures[self.object.texture], gFrames[self.object.texture][self.object.states[self.object.state].frame or self.object.frame],
            self.entity.x + adjacentOffsetX, self.entity.y + adjacentOffsetY)
    end
end