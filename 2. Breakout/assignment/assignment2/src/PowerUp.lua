--[[
    GD50
    Breakout Remake

    -- PowerUp Class --

    Author: Hugo Borges
    hugouchoas@outlook.com

    PowerUp class for GD50 Assignment2
]]

PowerUp = Class{}

function PowerUp:init(x, y, skin)
    self.x = x
    self.y = y
    self.skin = skin

    self.dx = 0
    self.dy = math.random(32, 128)

    self.width = 16
    self.height = 16

    self.inPlay = true
end

--[[
    Expects a paddle argument with a bounding box
    returns true if the bounding boxes of this and the argument overlap.
]]
function PowerUp:collides(target)
    if not self.inPlay then
        return
    end
    
    -- first, check to see if the left edge of either is farther to the right
    -- than the right edge of the other
    if self.x > target.x + target.width or target.x > self.x + self.width then
        return false
    end

    -- then check to see if the bottom edge of either is higher than the top
    -- edge of the other
    if self.y > target.y + target.height or target.y > self.y + self.height then
        return false
    end 

    -- if the above aren't true, they're overlapping
    return true
end

--[[
    Places the ball in the middle of the screen, with no movement.
]]
function PowerUp:reset()
    self.x = VIRTUAL_WIDTH / 2 - 2
    self.y = VIRTUAL_HEIGHT / 2 - 2
    self.dx = 0
    self.dy = 1
end

function PowerUp:update(dt)
    self.x = self.x + self.dx * dt
    self.y = self.y + self.dy * dt
end

function PowerUp:render()
    if self.inPlay then
        if self.skin < 0 then
            -- gTexture is our global texture for all blocks
            -- gBallFrames is a table of quads mapping to each individual ball skin in the texture
            love.graphics.draw(gTextures['main'], gFrames['keyPowerUp'],
                self.x, self.y)
        else
            -- gTexture is our global texture for all blocks
            -- gBallFrames is a table of quads mapping to each individual ball skin in the texture
            love.graphics.draw(gTextures['main'], gFrames['powerups'][self.skin],
                self.x, self.y)
        end
    end
end