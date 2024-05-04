using System.Text.Json.Serialization;

namespace Game2048.Logic;

/*      2048WPF Copyright (C) 2024 BLCK
        This program is free software: you can redistribute it and/or modify
        it under the terms of the GNU General Public License as published by
        the Free Software Foundation, either version 3 of the License, or
        (at your option) any later version.
        This program is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        GNU General Public License for more details.
        You should have received a copy of the GNU General Public License
        along with this program.  If not, see <https://www.gnu.org/licenses/>.*/

/** Tracking objects' positions and tools for generation, moving, merging */
public class GameBoard
{
    /* board map
     [0 0] [0 1] [0 2] [0 3]
     [1 0] [1 1] [1 2] [1 3]
     [2 0] [2 1] [2 2] [2 3]
     [3 0] [3 1] [3 2] [3 3]
    */
    private readonly Square[,] Board = new Square[4, 4];
    [JsonInclude]
    private string playerName;
    [JsonInclude]
    private List<Square> squareList = [];
    [JsonInclude]
    private int score;
    public int getScore()
    {
        return score;
    }

    public GameBoard(string playerName)
    {
        this.playerName = playerName;
        // [,] does not support null, therefore use empty constructor and isNull()
        // initialize with null
        for (int row = 0; row <= 3; ++row) 
        {
            for (int col = 0; col <= 3; ++col) 
            {
                Board[row, col] = new Square();
            }
        }
    }

    public string getPlayerName()
    {
        return playerName;
    }
    /** returns gameboard in the form of a list for UI display purposes */
    public int getBoardValue(int pos)
    {
        if (pos == 0)
            boardToList();
        return squareList[pos].GetValue();
    }
    /** serialisation does not support [,]
        assemble board from saved list     */
    private void boardToList()
    {
        squareList.Clear();
        for (int row = 0; row <= 3; ++row)
        {
            for (int col = 0; col <= 3; ++col)
            {
                squareList.Add(Board[row, col]);
            }
        }
    }
    /** assemble array from list after game loaded */
    public void boardFromList()
    {
        int listPos = 0;
        for (int row = 0; row <= 3; ++row)
        {
            for (int col = 0; col <= 3; ++col)
            {
                Board[row, col] = squareList[listPos];
                ++listPos;
            }
        }
    }

    public bool generateSquare()
    {
        // gather null positions and count score
        score = 0;
        List<dynamic> voidList = [];
        for (int row = 0; row <= 3; ++row)
        {
            for (int col = 0; col <= 3; ++col)
            {
                if (!Board[row, col].IsNull())
                    score += Board[row, col].GetValue();
                else
                    voidList.Add(new {Row = row, Col = col});
            }
        }
        // full board notice
        if (voidList.Count == 0)
            return true;
        // generate a square
        Random rnd = new Random();
        int rndPos = rnd.Next(0, voidList.Count);
        dynamic rndVoid = voidList[rndPos];
        if (rnd.NextDouble() > 0.1) {
            Board[rndVoid.Row, rndVoid.Col] = new Square(2);
            score += 2;
        }
        else {
            Board[rndVoid.Row, rndVoid.Col] = new Square(4);
            score += 4;
        }
        return false;
    }
    
    // column-direction universal stepper
    public void stepper(int[] startPos)
    {
        int step = startPos.Max() == 1 ? -1 : 1;
        for (int sweep = 0; sweep >= 0 && sweep <= 3; ++sweep)
        {
            int slide = startPos.Max();
            while (slide >= 0 && slide <= 3)
            {
                if (startPos[0] == 0)
                    solveLine(sweep, slide, step, true);
                else
                    solveLine(slide, sweep, step, false);
                slide -= step;
            }
        }
    }
    private void solveLine(int row, int col, int step, bool horiz)
    {
        
        if (Board[row, col].IsNull())
            return;
        
        int next = horiz ? (col + step) : (row + step);
        while (next >= 0 && next <= 3)
        {
            int nextDir1 = horiz ? row : next;
            int nextDir2 = horiz ? next : col;
            // if empty space, move there
            if (Board[nextDir1, nextDir2].IsNull())
            {
                Board[nextDir1, nextDir2] = Board[row, col];
                Board[row, col] = new Square();
                next += step;
                (horiz ? ref col : ref row) += step;
                continue;
            }
            // if same value square, merge
            if (Board[row, col].GetValue() == Board[nextDir1, nextDir2].GetValue())
            {
                Board[nextDir1, nextDir2].DoubleValue();
                Board[row, col] = new Square();
            }
            break;
        }
    }

    public bool checkGameEnd()
    {
        // check vertical
        for (int col = 0; col <= 3; ++col)
        {
            for (int row = 0; row <= 2; ++row)
            {
                int nextValue = Board[row + 1, col].GetValue();
                if (Board[row, col].GetValue() == nextValue)
                    return false;
            }
        }
        // check horizontal
        for (int row = 0; row <= 3; ++row)
        {
            for (int col = 0; col <= 2; ++col)
            {
                int nextValue = Board[row, col + 1].GetValue();
                if (Board[row, col].GetValue() == nextValue)
                    return false;
            }
        }
        // gridlock => game end
        return true;
    }
    
}