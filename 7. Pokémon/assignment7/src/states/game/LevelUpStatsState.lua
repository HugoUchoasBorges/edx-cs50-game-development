--[[
    GD50
    Pokemon

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

LevelUpStatsState = Class{__includes = BaseState}

function LevelUpStatsState:init(stats, onExit)
    self.levelUpMenu = Menu {
        x = VIRTUAL_WIDTH / 2 - 128,
        y = VIRTUAL_HEIGHT / 2 - 64,
        width = 256,
        height = 128,
        showCursor = false,
        items = {
            {
                text = LevelUpStatsState:statToString('HP', stats['HPIncrease'], stats['pokemon'].HP),
                onSelect = function()
                    gStateStack:pop()
                    onExit()
                end
            },
            {
                text = LevelUpStatsState:statToString('attack', stats['attackIncrease'], stats['pokemon'].attack),
                onSelect = function()
                    gStateStack:pop()
                    onExit()
                end
            },
            {
                text = LevelUpStatsState:statToString('defense', stats['defenseIncrease'], stats['pokemon'].defense),
                onSelect = function()
                    gStateStack:pop()
                    onExit()
                end
            },
            {
                text = LevelUpStatsState:statToString('speed', stats['speedIncrease'], stats['pokemon'].speed),
                onSelect = function()
                    gStateStack:pop()
                    onExit()
                end
            },
        }
    }
end

function LevelUpStatsState:statToString(statName, statValue, statIncrease)
    return statName .. ': ' .. statIncrease .. " + " .. statValue .. " = " .. statValue + statIncrease
end

function LevelUpStatsState:update(dt)
    self.levelUpMenu:update(dt)
end

function LevelUpStatsState:render()
    self.levelUpMenu:render()
end