using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
    public partial class GameBoardPage : Page
    {
        // call this object to execute file operations
        private FileOperations FO = new();
        
        public GameBoardPage()
        {
            InitializeComponent();
            backButton.Focus();
        }
        
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            reloadBoardValues();
        }

        private void receiveGameEnd()
        {
            endDiag.Visibility = Visibility.Visible;
            grid.Opacity = 0.4;
        }
        
        private void backToMenu(object sender, RoutedEventArgs e)
        {   
           
            if (!Main.isGameEnd)
                FO.saveGameState(Main.getGameBoard());
            else
            {
                FO.completedGame();
                Main.isGameEnd = false;
            }

            NavigationService.Navigate(new GameMenu());
        }
        // arrow listeners
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Main.isGameEnd)
            {
                receiveGameEnd();
                return;
            }
            
            switch (e.Key)
            {
                case Key.Left:
                    Main.step([0, 1]);
                    break;
                case Key.Right:
                    Main.step([0, 2]);
                    break;
                case Key.Up:
                    Main.step([1, 0]);
                    break;
                case Key.Down:
                    Main.step([2, 0]);
                    break;
            }
            reloadBoardValues();
        }

        private void reloadBoardValues()
        {   
            for(int j = 0; j < 16; ++j)
            {
                // set score
                scoreDisp.Text = "Score: " + Main.getGameBoard().getScore();
                // update all values
                TextBlock textB = (TextBlock)FindName("a" + j);
                // colors
                Border square = (Border)grid.Children[j];
               
                int val = Main.getGameBoard().getBoardValue(j);

                textB.Text = val > 0 ? val.ToString() : "";
                
                switch (val)
                {
                    case -1:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cdc1b4"));
                        break;
                    case 2:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eee4da"));
                        textB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#776e65"));
                        break;
                    case 4:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ede0c8"));
                        textB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#776e65"));
                        break;
                    case 8:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f2b179"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 16:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f59563"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 32:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f67c5f"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 64:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f65e3b"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 128:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ead075"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 256:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ecca66"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 512:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ebc75a"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 1024:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#edc53f"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case 2048:
                        square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#edc12e"));
                        textB.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    default:
                        square.Background = new SolidColorBrush(Colors.Aquamarine);
                        break;
                }
            }
        }
    }
}
