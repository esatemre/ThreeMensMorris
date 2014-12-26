using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapayZekaProjeWPF
{
    class Move
    {
        public int dest, start; // taş start:nerden dest:nereye
        public Enums.MoveMode mode; // put or move
        public Enums.Play player; // player or cpu

        // kurucu metodlar
        public Move(Enums.MoveMode mode, Enums.Play player, int start)
        {
            this.mode = mode;
            this.player = player;
            this.start = start;
            this.dest = -1;
        }

        public Move(Enums.MoveMode mode, Enums.Play player, int start, int dest)
        {
            this.mode = mode;
            this.player = player;
            this.start = start;
            this.dest = dest;
        }

        public Move(Move move)
        {
            this.mode = move.mode;
            this.player = move.player;
            this.start = move.start;
            this.dest = move.dest;
        }
    }
}
