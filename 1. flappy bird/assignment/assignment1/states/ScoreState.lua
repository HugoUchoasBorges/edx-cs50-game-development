--[[
    ScoreState Class
    Author: Colton Ogden
    cogden@cs50.harvard.edu

    A simple state used to display the player's score before they
    transition back into the play state. Transitioned to from the
    PlayState when they collide with a Pipe.
]]

ScoreState = Class{__includes = BaseState}

-- CS50 Assignment - Medal Images
local medal = {
    ['gold'] = love.graphics.newImage('gold_medal.png'),
    ['silver'] = love.graphics.newImage('silver_medal.png'),
    ['bronze'] = love.graphics.newImage('bronze_medal.png')
}

local GOLD_REQUIRED_SCORE = 5
local SILVER_REQUIRED_SCORE = 3
local BRONZE_REQUIRED_SCORE = 1

--[[
    When we enter the score state, we expect to receive the score
    from the play state so we know what to render to the State.
]]
function ScoreState:enter(params)
    self.score = params.score

    if self.score >= GOLD_REQUIRED_SCORE then
        self.medal = medal['gold']
        self.next_medal_score = 0
    elseif self.score >= SILVER_REQUIRED_SCORE then
        self.medal = medal['silver']
        self.next_medal_score = GOLD_REQUIRED_SCORE - self.score
    elseif self.score >= BRONZE_REQUIRED_SCORE then
        self.medal = medal['bronze']
        self.next_medal_score = SILVER_REQUIRED_SCORE - self.score
    else
        self.medal = nil
        self.next_medal_score = BRONZE_REQUIRED_SCORE - self.score
    end
end

function ScoreState:update(dt)
    -- go back to play if enter is pressed
    if love.keyboard.wasPressed('enter') or love.keyboard.wasPressed('return') then
        gStateMachine:change('countdown')
    end
end

function ScoreState:render()
    -- simply render the score to the middle of the screen
    love.graphics.setFont(flappyFont)
    love.graphics.printf('Oof! You lost!', 0, 30, VIRTUAL_WIDTH, 'center')

    love.graphics.setFont(mediumFont)
    love.graphics.printf('Score: ' .. tostring(self.score), 0, 60, VIRTUAL_WIDTH, 'center')


    if (self.medal) then
        love.graphics.draw(self.medal, VIRTUAL_WIDTH / 2 - 64, 90)
    end

    if (self.next_medal_score > 0) then
        love.graphics.setFont(smallFont)
        love.graphics.printf('+' .. tostring(self.next_medal_score) .. " to next Medal!!!", 0, 230, VIRTUAL_WIDTH, 'center')
    end

    love.graphics.setFont(mediumFont)
    love.graphics.printf('Press Enter to Play Again!', 0, 250, VIRTUAL_WIDTH, 'center')
end