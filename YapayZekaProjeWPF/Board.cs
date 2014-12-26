using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapayZekaProjeWPF
{
    class Board
    {
        private int[] board;
        public Enums.Play player;
        public List<Board> boardList;

        // kurucu metodlar
        public Board()
        {
            this.board = new Int32[9];
        }

        public Board(Board b)
        {
            this.board = new Int32[9];
            b.board.CopyTo(this.board, 0);
            this.player = b.player;
        }

        /// <summary>
        /// mevcut boardu dondur
        /// </summary>
        /// <returns></returns>
        public int[] getBoard()
        {
            return board;
        }

        /// <summary>
        /// board'da hareket et
        /// </summary>
        /// <param name="mov"></param>
        /// <returns></returns>
        public bool Move(Move mov)
        {
            if (mov.mode == Enums.MoveMode.Put)
            {
                if (this.board[mov.start - 1] == 0)
                {
                    this.board[mov.start - 1] = (int)mov.player;
                    return true;
                }
            }
            else if (mov.mode == Enums.MoveMode.Move)
            {
                if (this.board[mov.start - 1] == (int)mov.player)
                {
                    if (this.board[mov.dest - 1] == 0)
                    {
                        this.board[mov.start - 1] = 0;
                        this.board[mov.dest - 1] = (int)mov.player;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Hamle sonunda oyun kazanıldı mı?
        /// </summary>
        /// <param name="playerId">Oyuncu No</param>
        /// <returns></returns>
        public bool isWin()
        {
            int playerId = (int)player;
            if ((board[0] == playerId && board[1] == playerId && board[2] == playerId)
                || (board[3] == playerId && board[4] == playerId && board[5] == playerId)
                || (board[6] == playerId && board[7] == playerId && board[8] == playerId)
                || (board[0] == playerId && board[3] == playerId && board[6] == playerId)
                || (board[1] == playerId && board[4] == playerId && board[7] == playerId)
                || (board[2] == playerId && board[5] == playerId && board[8] == playerId))
            {
                return true;
            }
            else
                return false;
        }
    }
}
