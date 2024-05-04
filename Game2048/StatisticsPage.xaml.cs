using System.Windows;
using System.Windows.Controls;
using Game2048.GameData;
using Game2048.Logic;

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

namespace Game2048
{
    public partial class StatisticsPage : Page
    {
        // call this object to execute file operations
        private FileOperations FO = new();
        
        public StatisticsPage()
        {
            InitializeComponent();
        }
        
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            List<ScoreName> scoresList = FO.getStatistics();
            foreach (var record in scoresList) {
                PlayerScoresDataGrid.Items.Add( new {Score = record.getScore(), Name = record.getPlayer()});
            }
        }
        
        private void backToMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GameMenu());
        }
    }
}
