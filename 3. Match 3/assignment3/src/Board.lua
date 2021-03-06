--[[
    GD50
    Match-3 Remake

    -- Board Class --

    Author: Colton Ogden
    cogden@cs50.harvard.edu

    The Board is our arrangement of Tiles with which we must try to find matching
    sets of three horizontally or vertically.
]]

Board = Class{}

local function has_value (tab, val)
    for index, value in ipairs(tab) do
        if value == val then
            return true
        end
    end

    return false
end

function Board:init(x, y, level)
    self.x = x
    self.y = y
    self.matches = {}

    -- Tile Diversity
    self.maxTileVariety = math.random(1, math.min(level, 6))

    -- Tile Colors
    local minTileColors = math.max(1, 4 - self.maxTileVariety)
    self.maxTileColors = math.max(minTileColors, math.random(minTileColors, 8 - self.maxTileVariety))
    self.tileColors = {}

    self.level = level

    local randomVal = 0
    for i = 1, self.maxTileColors + 1 do
        
        repeat 
            randomVal = math.random(1, 18)
        until not has_value(self.tileColors, randomVal)

        table.insert(self.tileColors, randomVal)
    end

    self:initializeTiles()
end

function Board:initializeTiles()
    self.tiles = {}

    for tileY = 1, 8 do
        
        -- empty table that will serve as a new row
        table.insert(self.tiles, {})

        for tileX = 1, 8 do
            
            -- create a new tile at X,Y with a random color and variety
            table.insert(self.tiles[tileY], Tile(tileX, tileY, 
            self.tileColors[math.random(#self.tileColors-1)], 
            math.random(1, self.maxTileVariety)))
        end
    end

    while self:calculateMatches() do
        
        -- recursively initialize if matches were returned so we always have
        -- a matchless board on start
        self:initializeTiles()
    end
end

--[[
    Calculates whether any matches are possible on the board
]]
function Board:calculatePotentialMatches()
    for y = 1, 8 do
        for x = 1, 8 do
            local tile = self.tiles[y][x]
            if not tile then
                return true
            end
            
            local neighbors = {
                ['up'] = {y+1, x},
                ['down'] = {y+1, x},
                ['left'] = {y, x-1},
                ['right'] = {y, x+1}
            }

            for k, coord in pairs(neighbors) do
                local y2 = coord[1]
                local x2 = coord[2]
                if y2 > 0 and y2 < 9 and x2 > 0 and x2 < 9 then
                    local newTile = self.tiles[y2][x2]

                    if not newTile then
                        return true
                    end

                    -- swap grid positions of tiles
                    local tempX = tile.gridX
                    local tempY = tile.gridY

                    tile.gridX = newTile.gridX
                    tile.gridY = newTile.gridY
                    newTile.gridX = tempX
                    newTile.gridY = tempY

                    -- swap tiles in the tiles table
                    self.tiles[tile.gridY][tile.gridX] = tile
                    self.tiles[newTile.gridY][newTile.gridX] = newTile

                    local matches = self:calculateMatches()

                    tempX = tile.gridX
                    tempY = tile.gridY

                    tile.gridX = newTile.gridX
                    tile.gridY = newTile.gridY
                    newTile.gridX = tempX
                    newTile.gridY = tempY

                    -- swap tiles in the tiles table
                    self.tiles[tile.gridY][tile.gridX] = tile
                    self.tiles[newTile.gridY][newTile.gridX] = newTile

                    if matches then
                        return true
                    end
                end
            end
        end
    end
    
    return false
end

--[[
    Goes left to right, top to bottom in the board, calculating matches by counting consecutive
    tiles of the same color. Doesn't need to check the last tile in every row or column if the 
    last two haven't been a match.
]]
function Board:calculateMatches()
    local matches = {}

    -- how many of the same color blocks in a row we've found
    local matchNum = 1

    -- horizontal matches first
    for y = 1, 8 do
        local colorToMatch = self.tiles[y][1].color
        local varietyToMatch = self.tiles[y][1].variety

        matchNum = 1
        
        -- every horizontal tile
        for x = 2, 8 do
            
            -- if this is the same color as the one we're trying to match...
            if self.tiles[y][x].color == colorToMatch and self.tiles[y][x].variety == varietyToMatch then
                matchNum = matchNum + 1
            else
                
                -- set this as the new color we want to watch for
                colorToMatch = self.tiles[y][x].color
                varietyToMatch = self.tiles[y][x].variety

                -- if we have a match of 3 or more up to now, add it to our matches table
                if matchNum >= 3 then
                    local match = {}

                    -- go backwards from here by matchNum
                    for x2 = x - 1, x - matchNum, -1 do
                        
                        -- add each tile to the match that's in that match
                        table.insert(match, self.tiles[y][x2])
                    end

                    -- add this match to our total matches table
                    table.insert(matches, match)
                end

                matchNum = 1

                -- don't need to check last two if they won't be in a match
                if x >= 7 then
                    break
                end
            end
        end

        -- account for the last row ending with a match
        if matchNum >= 3 then
            local match = {}
            
            -- go backwards from end of last row by matchNum
            for x = 8, 8 - matchNum + 1, -1 do
                table.insert(match, self.tiles[y][x])
            end

            table.insert(matches, match)
        end
    end

    -- vertical matches
    for x = 1, 8 do
        local colorToMatch = self.tiles[1][x].color
        local varietyToMatch = self.tiles[1][x].variety

        matchNum = 1

        -- every vertical tile
        for y = 2, 8 do
            if self.tiles[y][x].color == colorToMatch and self.tiles[y][x].variety == varietyToMatch then
                matchNum = matchNum + 1
            else
                colorToMatch = self.tiles[y][x].color
                varietyToMatch = self.tiles[y][x].variety

                if matchNum >= 3 then
                    local match = {}

                    for y2 = y - 1, y - matchNum, -1 do
                        table.insert(match, self.tiles[y2][x])
                    end

                    table.insert(matches, match)
                end

                matchNum = 1

                -- don't need to check last two if they won't be in a match
                if y >= 7 then
                    break
                end
            end
        end

        -- account for the last column ending with a match
        if matchNum >= 3 then
            local match = {}
            
            -- go backwards from end of last row by matchNum
            for y = 8, 8 - matchNum + 1, -1 do
                table.insert(match, self.tiles[y][x])
            end

            table.insert(matches, match)
        end
    end

    -- store matches for later reference
    self.matches = matches

    -- detect shiny tiles and add their entire rows to the matches
    local matchRows = {}
    for k, match in pairs(self.matches) do
        for k2, tile in pairs(match) do
            if tile.shiny then
                table.insert(matchRows, tile.gridY)
            end
        end
    end

    for k, row in pairs(matchRows) do
        local match = {}
        for x = 1, 8 do
            table.insert(match, self.tiles[row][x])
        end
        table.insert(matches, match)
    end

    -- return matches table if > 0, else just return false
    return #self.matches > 0 and self.matches or false
end

--[[
    Remove the matches from the Board by just setting the Tile slots within
    them to nil, then setting self.matches to nil.
]]
function Board:removeMatches()
    for k, match in pairs(self.matches) do
        for k, tile in pairs(match) do
            self.tiles[tile.gridY][tile.gridX] = nil
        end
    end

    self.matches = nil
end

--[[
    Shifts down all of the tiles that now have spaces below them, then returns a table that
    contains tweening information for these new tiles.
]]
function Board:getFallingTiles()
    -- tween table, with tiles as keys and their x and y as the to values
    local tweens = {}

    -- for each column, go up tile by tile till we hit a space
    for x = 1, 8 do
        local space = false
        local spaceY = 0

        local y = 8
        while y >= 1 do
            
            -- if our last tile was a space...
            local tile = self.tiles[y][x]
            
            if space then
                
                -- if the current tile is *not* a space, bring this down to the lowest space
                if tile then
                    
                    -- put the tile in the correct spot in the board and fix its grid positions
                    self.tiles[spaceY][x] = tile
                    tile.gridY = spaceY

                    -- set its prior position to nil
                    self.tiles[y][x] = nil

                    -- tween the Y position to 32 x its grid position
                    tweens[tile] = {
                        y = (tile.gridY - 1) * 32
                    }

                    -- set Y to spaceY so we start back from here again
                    space = false
                    y = spaceY

                    -- set this back to 0 so we know we don't have an active space
                    spaceY = 0
                end
            elseif tile == nil then
                space = true
                
                -- if we haven't assigned a space yet, set this to it
                if spaceY == 0 then
                    spaceY = y
                end
            end

            y = y - 1
        end
    end

    -- create replacement tiles at the top of the screen
    for x = 1, 8 do
        for y = 8, 1, -1 do
            local tile = self.tiles[y][x]

            -- if the tile is nil, we need to add a new one
            if not tile then

                -- new tile with random color and variety
                local tile = Tile(x, y, 
                self.tileColors[math.random(#self.tileColors-1)], 
                math.random(1, self.maxTileVariety))
                tile.y = -32
                self.tiles[y][x] = tile

                -- create a new tween to return for this tile to fall down
                tweens[tile] = {
                    y = (tile.gridY - 1) * 32
                }
            end
        end
    end

    return tweens
end

function Board:render()
    for y = 1, #self.tiles do
        for x = 1, #self.tiles[1] do
            self.tiles[y][x]:render(self.x, self.y)
        end
    end
end