using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Players;

namespace TexasBot.Games.Dealers
{
    public class PlayerRegistry
    {
        public PlayerRegistry(IPlayer player)
        {
            Player = player;
            _alive = true;
            _polled = false;
        }

        public IPlayer Player { get; set; }

        private bool _alive;

        private bool _polled;

        public bool IsAlive()
        {
            return _alive;
        }

        public bool HasBeenPolled()
        {
            return _polled;
        }

        public void NewStageReset()
        {
            _polled = false;
        }

        public void NewGameReset()
        {
            _alive = true;
            _polled = false;
        }

        public void Polled()
        {
            _polled = true;
        }
        
        public int ChipsBet { get; set; }

        public void Fold()
        {
            if (_alive == false)
            {
                throw new InvalidOperationException($"Player {Player} already folded");
            }
            _alive = false;
        }
    }
}
