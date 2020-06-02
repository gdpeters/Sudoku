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
        private const int EASY = 30;
        private const int MED = 40;
        private const int HARD = 50;
        private Game game;
        private int[,] currentBoard;
        private DispatcherTimer t, fader;
        private GameTimer gameTimer;

        public MainWindow()
        {
            InitializeComponent();
            menuPopup.IsOpen = true;
            gameTimer = new GameTimer();
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

        private void EasyClick(object sender, RoutedEventArgs e)
        {
            this.ChooseLevel(EASY);
        }
        private void MediumClick(object sender, RoutedEventArgs e)
        {
            this.ChooseLevel(MED);
        }
        private void HardClick(object sender, RoutedEventArgs e)
        {
            this.ChooseLevel(HARD);
        }
        private void ChooseLevel(int level)
        {
            game.StartGame(level);
            this.WriteCells();
            levelPopup.IsOpen = false;
            gameTimer.Start();
            t.Start();
        }

        private void SetTimer()
        {
            t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += UpdateClock;
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            Clock.Text = gameTimer.GetTime();
        }


        private void WriteCells()
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
                    txtbx.TextChanged += new TextChangedEventHandler(Text_Changed);
                }
            }
        }

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            t.Stop();
            gameTimer.Pause();
            menuPopup.IsOpen = true;
        }

        private void RestartClick(object sender, RoutedEventArgs e)
        {
            t.Stop();
            this.WriteCells();
            menuPopup.IsOpen = false;
            gameTimer = new GameTimer();
            gameTimer.Start();
            t.Start();
        }

        private void QuitClick(object sender, RoutedEventArgs e)
        {
            t.Stop();
            menuPopup.IsOpen = false;
            this.Close();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            menuPopup.IsOpen = false;
            gameTimer.Resume();
            t.Start();
        }

        private void Text_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox txtbx = (TextBox)sender;
            string txtbxName = txtbx.Name;
            int r = Int32.Parse(txtbxName.ToCharArray()[4].ToString());
            int c = Int32.Parse(txtbxName.ToCharArray()[5].ToString());
            try
            {
                currentBoard[r, c] = Int32.Parse(txtbx.Text);
                this.CheckSolution();
            }
            catch (System.FormatException ex) { }
        }

        private void CheckSolution()
        {
            bool isReady = true;
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (currentBoard[r,c] == 0)
                    {
                        isReady = false;
                    }
                }
            }
            
            if (isReady)
            {
                t.Stop();
                fader = new DispatcherTimer();
                fader.Interval = TimeSpan.FromMilliseconds(75);

                if (game.IsWinner(currentBoard))
                {
                    solved.Visibility = Visibility.Visible;                    
                }
                else
                {
                    notSolved.Visibility = Visibility.Visible;
                    t.Start();
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
    }
}
