using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Sudoku
{
    public partial class MainWindow : Window
    {
       
        private Game game;
        private int[,] currentBoard;
        private DispatcherTimer t, fader;
        private GameClock gameClock;

        public MainWindow()
        {
            InitializeComponent();
            menuPopup.IsOpen = true;
            gameClock = new GameClock();
            this.SetTimer();
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            if (t != null)
                t.Stop();
            game = new Game();
            menuPopup.IsOpen = false;
            levelPopup.IsOpen = true;
        }

        private void RestartGameClick(object sender, RoutedEventArgs e)
        {
            t.Stop();
            this.DisplayGame();
            menuPopup.IsOpen = false;
            gameClock = new GameClock();
            gameClock.Start();
            t.Start();
        }

        private void CloseMenuClick(object sender, RoutedEventArgs e)
        {
            menuPopup.IsOpen = false;
            gameClock.Resume();
            t.Start();
        }

        private void QuitGameClick(object sender, RoutedEventArgs e)
        {
            t.Stop();
            menuPopup.IsOpen = false;
            this.Close();
        }

        private void OpenMenuClick(object sender, RoutedEventArgs e)
        {
            t.Stop();
            gameClock.Pause();
            menuPopup.IsOpen = true;
        }

        private void LevelClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string level = b.Content.ToString();

            if (level.Equals("HARD"))
                game.StartGame(Game.Difficulty.HARD);
            else if (level.Equals("MEDIUM"))
                game.StartGame(Game.Difficulty.MEDIUM);
            else
                game.StartGame(Game.Difficulty.EASY);

            levelPopup.IsOpen = false;
            this.DisplayGame();
            gameClock.Start();
            t.Start();
        }

        private void DisplayGame()
        {
            int[,] board = game.GetStartBoard();
            currentBoard = new int[9, 9];
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    string cellName = "cell" + r.ToString() + c.ToString();
                    TextBox txtbx = (TextBox)this.FindName(cellName);
                    if (board[r, c] == 0)
                    {
                        txtbx.Text = "";
                        txtbx.IsReadOnly = false;
                        txtbx.MaxLength = 1;
                        txtbx.MaxLines = 1;
                        txtbx.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        txtbx.Text = board[r, c].ToString();
                        txtbx.IsReadOnly = true;
                        txtbx.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    currentBoard[r, c] = board[r, c];
                    txtbx.TextChanged += new TextChangedEventHandler(TextChanged);
                }
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox cell = (TextBox)sender;
            int r = Grid.GetRow(cell);
            int c = Grid.GetColumn(cell);
            try
            {
                currentBoard[r, c] = Int32.Parse(cell.Text);
                this.DisplayBanner(game.CheckProgress(currentBoard));
            }
            catch (System.FormatException ex) { }
        }

        private void DisplayBanner(bool? hasWon)
        {
            if (hasWon != null)
            {
                fader = new DispatcherTimer();
                fader.Interval = TimeSpan.FromMilliseconds(75);

                if (hasWon == true)
                {
                    t.Stop();
                    solved.Visibility = Visibility.Visible;
                }

                if (hasWon == false)
                {
                    notSolved.Visibility = Visibility.Visible;
                }

                fader.Tick += RemoveBanner;
                fader.Start();
            } 
        }

        private void RemoveBanner(object sender, EventArgs e)
        {
            TextBlock banner;

            if (solved.Visibility == Visibility.Visible)
                banner = solved;
            else
                banner = notSolved;

            if (banner.FontSize < 45)
            {
                fader.Stop();
                banner.Visibility = Visibility.Hidden;
                banner.FontSize = 68;
                banner.Opacity = 100;
            }
            else
            {
                banner.FontSize = banner.FontSize - 0.5;
                banner.Opacity = banner.Opacity * 0.95;
            }
        }

        private void SetTimer()
        {
            t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += UpdateClock;
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            Clock.Text = gameClock.GetTime();
        }
    }
}
