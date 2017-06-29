using System.Collections.Generic;
using BattleshipBot.Properties;
using Battleships.Player.Interface;
using Jint;

namespace BattleshipBot
{
    public class MyBot : IBattleshipsBot
    {
        private readonly string javascript;
        private Engine engine;
        private IGridSquare lastTarget;

        public MyBot()
        {
            engine = new Engine();
            javascript = Resources.MyBot;
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            lastTarget = null; // Forget all our history when we start a new game

            return new List<IShipPosition>
            {
                GetShipPosition('A', 1, 'A', 5), // Aircraft Carrier
                GetShipPosition('C', 1, 'C', 4), // Battleship
                GetShipPosition('E', 1, 'E', 3), // Destroyer
                GetShipPosition('G', 1, 'G', 3), // Submarine
                GetShipPosition('I', 1, 'I', 2) // Patrol boat
            };
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            lastTarget = nextTarget;
            return nextTarget;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            // Ignore whether we're successful
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            // Ignore what our opponent does
        }

        public string Name => new Engine().Execute(javascript + "\nName();").GetCompletionValue().AsString();

        private static ShipPosition GetShipPosition(char startRow, int startColumn, char endRow, int endColumn)
        {
            return new ShipPosition(new GridSquare(startRow, startColumn), new GridSquare(endRow, endColumn));
        }

        private IGridSquare GetNextTarget()
        {
            if (lastTarget == null)
                return new GridSquare('A', 1);

            var row = lastTarget.Row;
            var col = lastTarget.Column + 1;
            if (lastTarget.Column != 10)
                return new GridSquare(row, col);

            row = (char) (row + 1);
            if (row > 'J')
                row = 'A';
            col = 1;
            return new GridSquare(row, col);
        }
    }
}