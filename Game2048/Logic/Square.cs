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

/** Square object on the 4x4 board */
public class Square
{
    [JsonInclude]
    private int value = -1;

    public Square(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Square value can't be <= 0");
        this.value = value;
    }
    
    public Square() {/*empty constructor*/}

    public void DoubleValue()
    {
        value *= 2;
    }

    public bool IsNull()
    {
        return value == -1;
    }

    public int GetValue()
    {
        return value;
    }

    public override string ToString()
    {
        return value.ToString();
    }

}