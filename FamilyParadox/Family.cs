using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace FamilyParadox
{
    /*
     * A guy at a bar says he has two kids and at least one is a girl.
     * What are the chances that both are girls?
     * He then says that one is named Julie. Now what are the chances that both are girls?
     * 
     */

    public class Child
    {
        public string Name { get; set; }
        public bool IsFemale { get; set; }
    }
    public class Family
    {
        public List<Child> Children { get; } = new List<Child>();
    }

    public class FamilyMaker
    {
        Random _random = new Random();
        const double _nameIsJulieFraction = 0.05;
        public Family MakeFamily()
        {
            var family = new Family();
            family.Children.Add(MakeChild());
            family.Children.Add(MakeChild());
            return family;
        }
        private Child MakeChild()
        {
            var child = new Child();
            child.IsFemale = _random.NextDouble() > 0.5;
            child.Name = _random.NextDouble() < _nameIsJulieFraction ? "Julie" : "NotJulie";
            return child;
        }
    }

    [TestFixture]
    public class Trials
    {
        [Test]
        public void ChanceOtherChildIsAGirl()
        {
            const int numberOfFamilies = 10000;
            var maker = new FamilyMaker();
            var families = Enumerable.Range(0, numberOfFamilies).Select(i=> maker.MakeFamily());

            var familiesWithAtLeastOneGirl = families.Where(f => f.Children.Any(c => c.IsFemale)).ToArray();

            var familiesWithTwoGirls = familiesWithAtLeastOneGirl.Where(f => f.Children.Count(c => c.IsFemale) == 2).ToArray();

            var chancesOfTwoGirls = (double)familiesWithTwoGirls.Length / familiesWithAtLeastOneGirl.Length;

            chancesOfTwoGirls.Should().BeGreaterThan(0.0);

        }

        [Test]
        public void ChanceOtherChildIsAGirlIfOneGirlIsNamedJulie()
        {
            const int numberOfFamilies = 500000;
            var maker = new FamilyMaker();
            var families = Enumerable.Range(0, numberOfFamilies).Select(i => maker.MakeFamily());

            var familiesWithAtLeastOneGirlNamedJulie = families.Where(f => f.Children.Any(c=>c.IsFemale && c.Name == "Julie")).ToArray();

            var familiesWithTwoGirls = familiesWithAtLeastOneGirlNamedJulie.Where(f => f.Children.Count(c => c.IsFemale) == 2).ToArray();

            var chancesOfTwoGirls = (double)familiesWithTwoGirls.Length / familiesWithAtLeastOneGirlNamedJulie.Length;

            chancesOfTwoGirls.Should().BeGreaterThan(0.0);

        }

    

    }
}
