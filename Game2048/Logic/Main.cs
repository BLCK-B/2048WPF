
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

/** Entry point of backend, controls game flow and manages GameBoard instance */
public static class Main
{
    private static GameBoard? board;
    public static bool isGameEnd = false;

    public static GameBoard getGameBoard()
    {
        return board;
    }
    public static void newGame(string name)
    {
        board = new GameBoard(name);
        board.generateSquare();
        board.generateSquare();
    }
    public static void oldGame(GameBoard oldBoard)
    {
        board = oldBoard;
        board.boardFromList();
    }
   
    public static void step(int[] pos)
    {
        if (board == null)
            return;
        
        board.stepper(pos);
        
        /* if board full, check for gridlock
           gridlock => end game, record score */
        if (board.generateSquare())
        {
            isGameEnd = board.checkGameEnd();
        }
    }
    
}