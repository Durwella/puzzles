using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MakeADeal
{
    /*
     * There are 3 large doors.  
     * There is a Tesla P85D behind one of the doors.
     * There is a goat behind each of the other two doors.
     * First, the player must pick a door.  That door will not be opened yet.
     * Second, the host will pick a *different* door (not the player's door) revealing one of the goats.
     * Finally, the player must pick a door to be opened. That can be the same door, or a different door.
     * The player's final choice will be revealed. 
     * If the player picked the door with the P85D we can probably agree that the player won.
     */

    enum Prize
    {
        Unknown,
        Goat,
        TeslaP85D
    }

    interface IGameState
    {
        Prize[] Doors { get; }
    }

    interface IPlayer
    {
        IGameState Game { get; set; }
        int PickDoor();
    }

    class GameState : IGameState
    {
        public Prize[] Doors { get; private set; }

        public GameState(params Prize[] doors)
        {
            Doors = doors;
        }
    }

    class GameShowHost
    {
        public GameState RevealedGameState { get; set; }
        private GameState TrueGameState { get; set; }
        public int PlayersDoor { get; set; }

        public static GameShowHost SetupNewGame()
        {
            int winningDoor = _random.Next(3);
            var gameState = new GameState(Prize.Goat, Prize.Goat, Prize.Goat);
            gameState.Doors[winningDoor] = Prize.TeslaP85D;
            return new GameShowHost(gameState);
        }


        private GameShowHost(GameState trueGameState)
        {
            TrueGameState = trueGameState;
            RevealedGameState = new GameState(Prize.Unknown, Prize.Unknown, Prize.Unknown);
        }

        public void OpenDoor(int doorIndex)
        {
            RevealedGameState.Doors[doorIndex] = TrueGameState.Doors[doorIndex];
        }

        public int PickDoor()
        {
            // Reveal a goat door other than the player's door
            var options = new List<int> { 0, 1, 2 };
            options.Remove(PlayersDoor);
            options.Remove(Array.IndexOf(TrueGameState.Doors, Prize.TeslaP85D));
            return options[ _random.Next(options.Count) ];
        }

        private static Random _random = new Random();
    }

    class Player : IPlayer
    {
        public IGameState Game { get; set; }

        public int PickDoor()
        {
            if (Game.Doors.All(d => d == Prize.Unknown))
                return 0;
            else
                // Always switch the door
                return Array.LastIndexOf(Game.Doors, Prize.Unknown);
        }
    }

    [TestFixture]
    public class Doors
    {
        static bool PlayGame()
        {
            try
            {
                var host = GameShowHost.SetupNewGame();
                var player = new Player {
                    Game = host.RevealedGameState
                };
                var firstDoor = player.PickDoor();
                host.PlayersDoor = firstDoor;
                var secondDoor = host.PickDoor();
                host.OpenDoor(secondDoor);
                var thirdDoor = player.PickDoor();
                host.OpenDoor(thirdDoor);
                return host.RevealedGameState.Doors[thirdDoor] == Prize.TeslaP85D;
            }
            catch(Exception)
            {
                // If the player violates the rules or something catastrophic happens there will be no prizes awarded
                return false;
            }
        }

        [Test]
        public void ShouldOptimizeChancesOfWinning()
        {
            const int attempts = 10000;
            int wins = Enumerable.Range(0, attempts).Count(i => PlayGame());
            var ratio = wins / (float)attempts;
            ratio.Should().BeGreaterOrEqualTo( .65f );
        }
    }
}
