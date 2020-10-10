using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName()
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            return Helpers.StayCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject.GetDrawingPriority() > GetDrawingPriority();
        }
    }

    public class Player : ICreature
    {
        public string GetImageFileName()
        {
            return "Digger.png";
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public CreatureCommand Act(int x, int y)
        {
            var offsetX = 0;
            var offsetY = 0;
            
            switch (Game.KeyPressed)
            {
                case Keys.Up:
                    if (y - 1 >= 0)
                    {
                        offsetY = -1;
                    }

                    break;
                case Keys.Down:
                    if (y + 1 < Game.MapHeight)
                    {
                        offsetY = 1;
                    }

                    break;
                case Keys.Left:
                    if (x - 1 >= 0)
                    {
                        offsetX = -1;
                    }

                    break;
                case Keys.Right:
                    if (x + 1 < Game.MapWidth)
                    {
                        offsetX = 1;
                    }

                    break;
            }
            
            if (Game.Map[x + offsetX, y + offsetY] != null)
            {
                if (Game.Map[x + offsetX, y + offsetY] is Sack)
                    return Helpers.StayCommand();
            }

            return Helpers.GetCommand(offsetX, offsetY);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            switch (conflictedObject)
            {
                case Sack.Gold _:
                    Game.Scores += 10;
                    break;
                case Monster _:
                case Sack _:
                    return true;
            }

            return false;
        }
    }

    public class Sack : ICreature
    {
        private int counter = 0;

        public string GetImageFileName()
        {
            return "Sack.png";
        }

        public int GetDrawingPriority()
        {
            return 100;
        }

        public CreatureCommand Act(int x, int y)
        {
            if (y < Game.MapHeight - 1)
            {
                var map = Game.Map[x, y + 1];
                if (map == null || (counter > 0 && (map is Player || map is Monster)))
                {
                    counter++;
                    return Helpers.GetCommand(0, 1);
                }
            }

            if (counter > 1)
            {
                counter = 0;
                return Helpers.GetCommand(0, 0, new Gold());
            }

            counter = 0;
            return Helpers.StayCommand();
        }

        public class Gold : ICreature
        {
            public CreatureCommand Act(int x, int y)
            {
                return Helpers.StayCommand();
            }

            public bool DeadInConflict(ICreature conflictedObject)
            {
                return (conflictedObject is Player || conflictedObject is Monster);
            }

            public int GetDrawingPriority()
            {
                return 3;
            }

            public string GetImageFileName()
            {
                return "Gold.png";
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }
    }

    public class Monster : ICreature
    {
        public string GetImageFileName()
        {
            return "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            var player = GetPlayer();
            
            if (player == null)
            {
                return Helpers.StayCommand();
            }

            var (playerX, playerY) = (player.Value.Item1, player.Value.Item2);

            var offsetX = 0;
            var offsetY = 0;
            
            if (playerX == x)
            {
                if (playerY < y) offsetY = -1;
                else if (playerY > y) offsetY = 1;
            }
            else if (playerY == y)
            {
                if (playerX < x) offsetX = -1;
                else if (playerX > x) offsetX = 1;
            }
            else
            {
                if (playerX < x) offsetX = -1;
                else if (playerX > x) offsetX = 1;
            }

            if (!(x + offsetX >= 0 && x + offsetX < Game.MapWidth && y + offsetY >= 0 && y + offsetY <= Game.MapHeight))
            {
                return Helpers.StayCommand();
            }

            var creatureOnCell = Game.Map[offsetX + x, offsetY + y];

            if (creatureOnCell is Monster || creatureOnCell is Terrain || creatureOnCell is Sack)
            {
                return Helpers.StayCommand();
            }

            return Helpers.GetCommand(offsetX, offsetY);
        }

        public (int, int)? GetPlayer()
        {
            for (var i = 0; i < Game.MapWidth; i++)
            {
                for (var j = 0; j < Game.MapHeight; j++)
                {
                    if (Game.Map[i, j] is Player)
                    {
                        return (i, j);
                    }
                }
            }

            return null;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }
    }
    
    public static class Helpers {
        public static CreatureCommand GetCommand(int deltaX, int deltaY)
        {
            return new CreatureCommand
            {
                DeltaX = deltaX,
                DeltaY = deltaY
            };
        }
        
        public static CreatureCommand GetCommand(int deltaX, int deltaY, ICreature transformTo)
        {
            return new CreatureCommand
            {
                DeltaX = deltaX,
                DeltaY = deltaY,
                TransformTo = transformTo
            };
        }

        public static CreatureCommand StayCommand()
        {
            return new CreatureCommand
            {
                DeltaX = 0,
                DeltaY = 0
            };
        }
    }
}