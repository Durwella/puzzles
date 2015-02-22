using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace FindYourNumber
{
    /*
     * A room has 100 closed boxes. Each box has a distinct number inside between 1 and 100.
     * The boxes are in no particular order.
     * Outside the room is a group of 100 people. Each person is assigned a distict number between 1 and 100.
     * The goal of the group is to maximize the number of people who find their number in a box in the room.
     * Each person may enter the room once and open as many as 50 boxes.
     * However, after they leave the room, the boxes are closed and they may not communicate with the group.
     * What strategy should the people use to maximize the odds that *everyone* in the group will find their number in a box.
     */

    interface IBoxRoom
    {
        // Returns the number on the inside of the box
        int OpenBox(int index);
    }

    interface IPerson
    {
        int Number { get; set; }
        IBoxRoom BoxRoom { get; set; }
        void PickBox();
    }

    class Person : IPerson
    {
        public int Number { get; set; }
        public IBoxRoom BoxRoom { get; set; }

        public void PickBox()
        {
            BoxRoom.OpenBox(0);
        }
    }

    class BoxRoom : IBoxRoom
    {
        public int[] Boxes { get; set; }
        public int OpenCount { get; private set; }
        public int NumberInLastOpenedBox { get; private set; }

        public int OpenBox(int index)
        {
            OpenCount++;
            if (OpenCount > 50)
                throw new AccessViolationException("Not allowed to open more than 50 boxes!");
            return NumberInLastOpenedBox = Boxes[index];
        }

        public void CloseAllBoxes()
        {
            NumberInLastOpenedBox = -1;
            OpenCount = 0;
        }
    }

    class Game
    {
        public BoxRoom BoxRoom { get; set; }
        public IPerson[] People { get; set; }

        public Game()
        {
            BoxRoom = new BoxRoom { Boxes = Enumerable.Range(1, 100).OrderBy(i => _random.Next()).ToArray() };
            People = Enumerable.Range(1, 100).Select(n => new Person { Number = n }).ToArray();
        }

        public bool IsWinner(IPerson person)
        {
            BoxRoom.CloseAllBoxes();
            person.BoxRoom = BoxRoom;
            while(BoxRoom.NumberInLastOpenedBox != person.Number && BoxRoom.OpenCount < 50)
                person.PickBox();
            return person.Number == BoxRoom.NumberInLastOpenedBox;
        }

        public int CountWinners()
        {
            return People.Count(IsWinner);
        }

        static Random _random = new Random();
    }


    [TestFixture]
    public class NumberFinding
    {
        static bool PlayGame(int requiredWinnerCount = 100)
        {
            var game = new Game();
            var winners = game.CountWinners();
            return winners >= requiredWinnerCount;
        }

        [Test]
        public void ShouldMaximizeCountOfPeopleFindingTheirNumbers()
        {
            const int attempts = 10000;
            int wins = Enumerable.Range(0, attempts).Count(i => PlayGame());
            var ratio = wins / (float)attempts;
            ratio.Should().BeGreaterOrEqualTo( 0.59f );
        }
    }
}
