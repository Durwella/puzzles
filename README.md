# Sharp Puzzles

At Durwella we enjoy challenging problems that get us to *think*!

Being collected here are codified representations of brain teasers we have enjoyed.  
In most cases we did not invent these puzzles.  But we did write the code here that models them.

Good puzzles are likely to:

- Have a surprisingly succinct solution
- Require rephrasing the problem to use tools from a surprising domain
- Have depth, such that the right answer is easy to guess, but challenging to explain

## The Puzzles

- [Goats and Teslas](MakeADeal/Doors.cs)
- [Find your Number](FindYourNumber/NumberFinding.cs)

## Contributing

Pull requests are welcome if the following conditions are met:

- The puzzle (or family of related puzzles) should be a .NET/MONO class library in C# or F#
- Each puzzle should be executable as a unit test (preferably expressed with FluentAssertions)
- Solving the puzzle should result in a passing test
- The puzzle solution should be expressed as a single interface implementation
- A puzzle solution implementation should be provided in the solutions branch
- A link to the source file should be added to the list above
- The source code is entirely your original work and you consent to releasing it under the MIT license
- A plain description of the puzzle should appear at the top of the source file

[![Build status](https://ci.appveyor.com/api/projects/status/79svf13bm6slyhbu?svg=true)](https://ci.appveyor.com/project/jfoshee/puzzles)

