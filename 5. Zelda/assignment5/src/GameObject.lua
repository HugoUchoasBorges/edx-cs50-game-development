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
    if self.dx then 
        self.x = self.x + self.dx * dt
    end

    if self.dy then 
        self.y = self.y + self.dy * dt
    end

    -- TODO: Destroy object after
        -- Hit an enemy (damage)
        -- Hit an wall
        -- Slide for 4 TILESIZEs
end

function GameObject:fire(x, y, dx, dy)
    -- Throws the gameobject as a projectile
    self.picked = false
    self.x = x
    self.y = y
    self.dx = dx
    self.dy = dy
end

function GameObject:render(adjacentOffsetX, adjacentOffsetY)
    if not self.collected and not self.picked then
        love.graphics.draw(gTextures[self.texture], gFrames[self.texture][self.states[self.state].frame or self.frame],
            self.x + adjacentOffsetX, self.y + adjacentOffsetY)
    end
end