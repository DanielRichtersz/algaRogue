using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    class Player
    {
        private int X;
        private int Y;

        public Player(int startingX, int startingY)
        {
            this.X = startingX;
            this.Y = startingY;
        }

        #region Getters & Setters
        public int GetX()
        {
            return this.X;
        }

        public int GetY()
        {
            return this.Y;
        }

        public void SetX(int X)
        {
            this.X = X;
        }

        public void SetY(int Y)
        {
            this.Y = Y;
        }
        #endregion

    }
}
