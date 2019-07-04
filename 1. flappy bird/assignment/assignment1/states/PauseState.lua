--[[
    PauseState Class
    Author: Hugo Borges
    hugouchoas@outlook.com

    The PauseState class is responsible for pausing the game.
]]

PauseState = Class{__includes = BaseState}

-- takes 1 second to count down each time
COUNTDOWN_TIME = 0.75

function PauseState:init()
    love.audio.pause(sounds['music'])
    love.audio.play(sounds['pause'])

    -- The resume flag is used for start a Countdown timer before Resuming the game
    -- I chose to do this way instead of calling the "Countdown state" because I wanted to show the countdown timer
    -- in the same State as the Pause Information
    self.resume = false
    self.count = 3
    self.timer = 0
end

function PauseState:update(dt)
    -- transition to countdown when enter/return are pressed
    if love.keyboard.wasPressed('enter') or love.keyboard.wasPressed('return') or love.keyboard.wasPressed('p') then
        love.audio.play(sounds['resume'])
        self.resume = true
    end

    if self.resume then
        -- Assignment Code .. Only update the self.timer when the player 
        self.timer = self.timer + dt
    end

    if self.timer > COUNTDOWN_TIME then
        self.timer = self.timer % COUNTDOWN_TIME
        self.count = self.count - 1

        -- when 0 is reached, we should enter the PlayState
        if self.count == 0 then
            gStateMachine:change('play', self.params)
        end
    end
end

function PauseState:render()
    -- Assignment Code .. Render the game normally (but don't animate it )
    for k, pair in pairs(self.pipePairs) do
        pair:render()
    end

    love.graphics.setFont(flappyFont)
    love.graphics.print('Score: ' .. tostring(self.score), 8, 8)

    love.graphics.setFont(hugeFont)
    love.graphics.printf("PAUSED", 0, 80, VIRTUAL_WIDTH, 'center')

    self.bird:render()

    if self.resume then
        love.graphics.setFont(mediumFont)
        love.graphics.printf(tostring(self.count), 0, VIRTUAL_HEIGHT - 50, VIRTUAL_WIDTH, 'center')
    end
end

--[[
    Called when this state is transitioned to from another state.
]]
function PauseState:enter(params)
    self.params = params or {}

    -- recover the parameters PauseState need to render
    self.pipePairs = self.params.pipePairs or nil
    self.score = self.params.score or nil
    self.bird = self.params.bird or nil
end

--[[
    Called when this state changes to another state.
]]
function PauseState:exit()
    love.audio.play(sounds['music'])
end