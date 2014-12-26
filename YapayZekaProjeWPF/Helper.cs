using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapayZekaProjeWPF
{
    class Helper
    {
        /// <summary>
        /// possible moves için ekstra metod; hem player hem cpu için kullanılsın diye
        /// </summary>
        /// <param name="board">tahta</param>
        /// <param name="player">oyuncu</param>
        /// <returns></returns>
        private static List<Board> possibleMovesExtra(Board board, Enums.Play player)
        {
            List<Board> boardList = new List<Board>();
            for (int i = 0; i < 9; i++)
            {
                if (board.getBoard()[i] == (int)player)
                {
                    #region PossibleMoves
                    int aa, bb, cc, dd;
                    aa = i - 1; bb = i + 3; cc = i + 1; dd = i - 3;
                    if (cc % 3 != 0)
                    {
                        Board newBoard = new Board(board);
                        if (newBoard.Move(new Move(Enums.MoveMode.Move, player, i + 1, cc + 1)))
                        {
                            boardList.Add(newBoard);
                        }
                    }
                    if (bb < 9)
                    {
                        Board newBoard = new Board(board);
                        if (newBoard.Move(new Move(Enums.MoveMode.Move, player, i + 1, bb + 1)))
                        {
                            boardList.Add(newBoard);
                        }
                    }
                    if (aa != -1 && aa % 3 != 2)
                    {
                        Board newBoard = new Board(board);
                        if (newBoard.Move(new Move(Enums.MoveMode.Move, player, i + 1, aa + 1)))
                        {
                            boardList.Add(newBoard);
                        }
                    }
                    if (dd > -1)
                    {
                        Board newBoard = new Board(board);
                        if (newBoard.Move(new Move(Enums.MoveMode.Move, player, i + 1, dd + 1)))
                        {
                            boardList.Add(newBoard);
                        }
                    }
                    #endregion
                }
            }
            return boardList;
        }
        /// <summary>
        /// tahta için mümkün olan adımları tahta seklinde döndürür
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static List<Board> possibleMoves(Board board)
        {
            List<Board> boardList = Helper.possibleMovesExtra(board, Enums.Play.Computer);
            foreach (Board item in boardList)
            {
                item.boardList = Helper.possibleMovesExtra(item, Enums.Play.Player);
            }
            return boardList;
        }
        /// <summary>
        /// mümkün olan yerleştirmeleri tahta şeklinde döndürür.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static List<Board> possiblePut(Board board)
        {
            List<Board> boardList = Helper.possiblePutExtra(board, Enums.Play.Computer);
            if (board.getBoard().Count(p => p == (int)Enums.Play.Player) > 2)
            {
                foreach (Board item in boardList)
                    item.boardList = Helper.possibleMovesExtra(item, Enums.Play.Player);
            }
            else
            {
                foreach (Board item in boardList)
                    item.boardList = Helper.possiblePutExtra(item, Enums.Play.Player);
            }
            return boardList;
        }
        /// <summary>
        /// possible put için extra metod:hem player hem cpu için kullanılsın diye
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private static List<Board> possiblePutExtra(Board board, Enums.Play player)
        {
            List<Board> boardList = new List<Board>();
            for (int i = 0; i < 9; i++)
            {
                if (board.getBoard()[i] == 0)
                {
                    Board newBoard = new Board(board);
                    if (newBoard.Move(new Move(Enums.MoveMode.Put, player, i + 1)))
                    {
                        boardList.Add(newBoard);
                    }
                }
            }
            return boardList;
        }
        /// <summary>
        /// tahta listesi ardından en iyi tahtayı döndür.
        /// </summary>
        /// <param name="boardList"></param>
        /// <returns></returns>
        public static Board getBestBoard(List<Board> boardList)
        {
            Board board = new Board();
            Dictionary<Board, int> points = new Dictionary<Board, int>();
            foreach (Board b in boardList)
            {
                if (b.isWin())
                {
                    return b;
                }
                else
                {
                    int p = 0;
                    foreach (Board bb in b.boardList)
                    {
                        bb.player = Enums.Play.Player;
                        int tmp = Helper.getBoardPoint(bb);
                        if (tmp > p)
                            p = tmp;
                    }
                    points.Add(b, p);
                }
            }
            return points.Where(p => p.Value == points.Values.Min()).First().Key;
        }
        /// <summary>
        /// eski tahta ile yeni tahta arasındaki fark ile hareketi bulur
        /// </summary>
        /// <param name="oldBoard"></param>
        /// <param name="newBoard"></param>
        /// <returns></returns>
        public static Move getBestMove(Board oldBoard, Board newBoard)
        {
            int start = 0, dest = 0;
            for (int i = 0; i < 9; i++)
            {
                if (oldBoard.getBoard()[i] == 0 && newBoard.getBoard()[i] == (int)oldBoard.player)
                {
                    dest = i;
                }
                else if (oldBoard.getBoard()[i] == (int)oldBoard.player && newBoard.getBoard()[i] == 0)
                {
                    start = i;
                }
            }
            return new Move(Enums.MoveMode.Move, oldBoard.player, start + 1, dest + 1);
        }
        /// <summary>
        /// eski tahta ile yeni tahta arasındaki fark ile yerleştirmeyi bulur.
        /// </summary>
        /// <param name="oldBoard"></param>
        /// <param name="newBoard"></param>
        /// <returns></returns>
        public static Move getBestPut(Board oldBoard, Board newBoard)
        {
            int start = 0;
            for (int i = 0; i < 9; i++)
            {
                if (oldBoard.getBoard()[i] == 0 && newBoard.getBoard()[i] == (int)oldBoard.player)
                {
                    start = i;
                }
            }
            return new Move(Enums.MoveMode.Put, oldBoard.player, start + 1);
        }
        /// <summary>
        /// tahta puanını döndür
        /// </summary>
        /// <param name="board">tahta</param>
        /// <returns></returns>
        private static int getBoardPoint(Board board)
        {
            int point = 0;
            if (board.isWin())
                point = 99;
            else
            {
                point += Helper.getPoint(board.getBoard()[0], board.getBoard()[1], board.getBoard()[2]);
                point += Helper.getPoint(board.getBoard()[3], board.getBoard()[4], board.getBoard()[5]);
                point += Helper.getPoint(board.getBoard()[6], board.getBoard()[7], board.getBoard()[8]);
                point += Helper.getPoint(board.getBoard()[0], board.getBoard()[3], board.getBoard()[6]);
                point += Helper.getPoint(board.getBoard()[1], board.getBoard()[4], board.getBoard()[7]);
                point += Helper.getPoint(board.getBoard()[2], board.getBoard()[5], board.getBoard()[8]);
            }
            return point;
        }
        /// <summary>
        /// puan dondurur
        /// </summary>
        /// <param name="points">noktalar</param>
        /// <returns></returns>
        private static int getPoint(params int[] points)
        {
            int point = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == (int)Enums.Play.Player)
                {
                    point++;
                }
                else if (points[i] == (int)Enums.Play.Computer)
                {
                    point--;
                }
            }
            if (point < 0)
                point = 0;
            return point;
        }
        /// <summary>
        /// oyuncu için mümkün mü o adım?
        /// </summary>
        /// <param name="old">eski yer</param>
        /// <param name="news">yeni yer</param>
        /// <returns></returns>
        public static bool isNear(int old, int news)
        {
            int aa = Math.Abs(((news - 1) % 3) - ((old - 1) % 3));
            int bb = Math.Abs(((int)((news - 1) / 3)) - ((int)((old - 1) / 3)));
            if (aa + bb == 1)
            {
                return true;
            }
            return false;
        }
    }
}
