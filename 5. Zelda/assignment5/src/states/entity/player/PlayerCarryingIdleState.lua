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
end

function PlayerCarryingIdleState:update(dt)
    EntityIdleState.update(self, dt)
end

function PlayerCarryingIdleState:update(dt)
    if love.keyboard.isDown('left') or love.keyboard.isDown('right') or
       love.keyboard.isDown('up') or love.keyboard.isDown('down') then
        self.entity:changeState('carrying')
    end

    if love.keyboard.wasPressed('space') then
        -- TODO: New State - Throwing
        self.entity:changeState('idle')
        gSounds['door']:play()
        --self.entity:changeState('throwing')
    end
end