using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace YapayZekaProjeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int oldPoint; // oyuncunun 2. tiki icin eski tiki tutma
        Board board; // mevcut tahta
        byte computerStone, playerStone; // oyuncu ve cpu taşı : max 3 olmalı her zaman
        int counter; //hamle sayaci : 100 olursa beraber
        DispatcherTimer timerTime;

        public MainWindow()
        {
            InitializeComponent();
            oldPoint = -1;
            computerStone = 0;
            playerStone = 0;
            board = new Board();
            board.player = Enums.Play.Player;
            counter = 0;
        }

        /// <summary>
        /// Oyuncu Tıklayınca bu fonksiyona düşer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (timerTime != null)
                timerTime.Stop();
            int byteParse = Int32.Parse(((Ellipse)sender).Name.Substring(1, 1));
            if (playerStone < 3)
            {
                if (board.Move(new Move(Enums.MoveMode.Put, Enums.Play.Player, byteParse)))
                {
                    Ellipse ell = (Ellipse)this.FindName("b" + byteParse);
                    ell.Fill = new SolidColorBrush(Colors.White);
                    playerStone++;
                    AfterMove();
                }
            }
            else
            {
                if (oldPoint != -1)
                {
                    if (Helper.isNear(oldPoint, byteParse))
                    {
                        if (board.Move(new Move(Enums.MoveMode.Move, Enums.Play.Player, oldPoint, byteParse)))
                        {
                            Ellipse ell = (Ellipse)this.FindName("b" + oldPoint);
                            ell.Opacity = 1;
                            ell.Fill = new SolidColorBrush(Color.FromRgb(0xD6, 0xD6, 0xEA));
                            ell = (Ellipse)this.FindName("b" + byteParse);
                            ell.Fill = new SolidColorBrush(Colors.White);
                            AfterMove();
                        }
                    }
                    else
                    {
                        Ellipse ell = (Ellipse)this.FindName("b" + oldPoint);
                        ell.Opacity = 1;
                        oldPoint = -1;
                    }
                }
                else
                {
                    if (board.getBoard()[byteParse - 1] == (int)Enums.Play.Player)
                    {
                        oldPoint = byteParse;
                        Ellipse ell = (Ellipse)this.FindName("b" + byteParse);
                        ell.Opacity = 0.3;
                    }
                }
            }
        }

        /// <summary>
        /// Hamle sonra kontroller
        /// </summary>
        private void AfterMove()
        {
            if (board.isWin())
            {
                lblBottom.Content = "You Won !!!";
            }
            else
            {
                oldPoint = -1;
                counter++;
                PlayCPU();
                Timerinit();
            }

        }

        /// <summary>
        /// Bilgisayar oynasın birazda
        /// </summary>
        private void PlayCPU()
        {
            board.player = Enums.Play.Computer; // sira cpuda
            //AI ile en mantıklı davranış aranıyor
            if (computerStone < 3) // Placement davranışı
            {
                List<Board> boardList = Helper.possiblePut(board);
                Board bestBoard = Helper.getBestBoard(boardList);
                Move bestMove = Helper.getBestPut(board, bestBoard);
                if (board.Move(bestMove))
                {
                    Ellipse ell = (Ellipse)this.FindName("b" + bestMove.start);
                    ell.Fill = new SolidColorBrush(Colors.Black);
                    computerStone++;
                }
            }
            else // Oynatma davranışı
            {
                List<Board> boardList = Helper.possibleMoves(board);
                Board bestBoard = Helper.getBestBoard(boardList);
                Move bestMove = Helper.getBestMove(board, bestBoard);
                if (board.Move(bestMove))
                {
                    Ellipse ell = (Ellipse)this.FindName("b" + bestMove.start);
                    ell.Fill = new SolidColorBrush(Color.FromRgb(0xD6, 0xD6, 0xEA));
                    ell = (Ellipse)this.FindName("b" + bestMove.dest);
                    ell.Fill = new SolidColorBrush(Colors.Black);
                }
            }
            //AI bitti
            if (board.isWin())
            {
                lblBottom.Content = "You Lost !!!";
            }
            lblCount.Content = counter.ToString();
            sliderCount.Value = counter;
            if (counter > 100)
            {
                MessageBox.Show("Draw!!!!");
            }
            board.player = Enums.Play.Player; // sira oyuncuda
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            oldPoint = -1;
            computerStone = 0;
            playerStone = 0;
            board = new Board();
            board.player = Enums.Play.Player;
            counter = 0;
            lblCount.Content = "0";
            sliderCount.Value = 0;
            lblBottom.Content = "";
            for (int i = 1; i < 10; i++)
            {
                Ellipse ell = (Ellipse)this.FindName("b" + i);
                ell.Fill = new SolidColorBrush(Color.FromRgb(0xD6, 0xD6, 0xEA));
                ell.Opacity = 1;
            }
            if ((bool)rbCom.IsChecked)
            {
                PlayCPU();
            }
        }

        private void Timerinit()
        {
            sliderCount.Value = 0;
            timerTime = new DispatcherTimer();
            timerTime.Interval = TimeSpan.FromSeconds(0.5);
            timerTime.Tick += timerTime_Tick;
            timerTime.Start();
        }

        void timerTime_Tick(object sender, EventArgs e)
        {
            sliderCount.Value += 0.5;
            if (sliderCount.Value == 10)
            {
                lblBottom.Content = "Time is over!!!";
                timerTime.Stop();
            }
        }
    }
}
