using System.IO;
using System.Text.Json;
using Game2048.GameData;

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

/** file operations: saving game state and score */
public class FileOperations
{
    private readonly JsonSerializerOptions options = new JsonSerializerOptions {IncludeFields = true};
    private const string gameDataPath = @"GameData/";
    private List<ScoreName> scoreList = [];

    /** ensure files exist */
    public void filesExist()
    {
        if (!Directory.Exists(gameDataPath))
            Directory.CreateDirectory(gameDataPath);
        
        if (!File.Exists(gameDataPath + "gameSave.txt"))
            File.Create(gameDataPath + "gameSave.txt").Dispose();
        
        if (!File.Exists(gameDataPath + "topScores.txt"))
            File.Create(gameDataPath + "topScores.txt").Dispose();
    }
    /** enable/disable continue button */
    public bool getSaveExists()
    {
        return !string.IsNullOrEmpty(File.ReadAllText(gameDataPath + "gameSave.txt"));
    }
    /** method called by menu button when game is not finished */
    public void saveGameState(GameBoard board)
    {
        Main.getGameBoard().boardFromList();
        File.WriteAllText(gameDataPath + "gameSave.txt", string.Empty);
        using FileStream stream = new FileStream(gameDataPath + "gameSave.txt", FileMode.OpenOrCreate);
        JsonSerializer.Serialize(stream, board, options);
    }
    /** method called by menu button after game ended */
    public void completedGame()
    {
        saveScore(Main.getGameBoard().getScore());
        File.WriteAllText(gameDataPath + "gameSave.txt", string.Empty);
    }
    /** method called by continue button */
    public void loadGameState()
    {
        using FileStream stream = new FileStream(gameDataPath + "gameSave.txt", FileMode.OpenOrCreate, FileAccess.Read);
        GameBoard board = JsonSerializer.Deserialize<GameBoard>(stream, options) ?? throw new InvalidOperationException();
        Main.oldGame(board);
    }
    /** method called by statistics button */
    public List<ScoreName> getStatistics()
    {
        loadScores();
        return scoreList.OrderByDescending(scoreName => scoreName.getScore()).ToList();
    }
    
    private void loadScores()
    {
        if (!string.IsNullOrEmpty(File.ReadAllText(gameDataPath + "topScores.txt"))) {
            using FileStream stream = new FileStream(gameDataPath + "topScores.txt", FileMode.OpenOrCreate, FileAccess.Read);
            scoreList = JsonSerializer.Deserialize<List<ScoreName>>(stream, options) ?? throw new InvalidOperationException();
            scoreList = scoreList.OrderByDescending(scoreName => scoreName.getScore()).ToList();
        }
    }
        
    private void saveScore(int score)
    {
        loadScores();
        string player = Main.getGameBoard().getPlayerName();
        
        if (scoreList!.Count < 10)
            scoreList.Add(new ScoreName(score, player));
        else if (scoreList[9].getScore() < score)
            scoreList[9] = new ScoreName(score, player);
      
        File.WriteAllText(gameDataPath + "topScores.txt", string.Empty);
        using FileStream stream = new FileStream(gameDataPath + "topScores.txt", FileMode.OpenOrCreate);
        JsonSerializer.Serialize(stream, scoreList, options);
    }
}