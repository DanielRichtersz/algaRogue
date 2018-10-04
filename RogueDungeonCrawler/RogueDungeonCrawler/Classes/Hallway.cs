using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDungeonCrawler.Classes
{
    class Hallway
    {
        int enemy { get; set; }
        bool isCollapsed { get; set; }

        public Hallway(int enemy, bool collapsed)
        {
            this.enemy = enemy;
            this.isCollapsed = collapsed;
        }
    }
}
