# Word Search Game (C#)

A console word-search game for several players: the program generates letter grids from a French or English dictionary, and players locate hidden words against the clock.
First-year algorithmics project (2022), written in C# on .NET.

Author: Sami Chellia.

## Features

- **Grid generation.** Five difficulty levels, from a 7 x 6 grid with 8 words to a 13 x 13 grid with 28 words. Each level unlocks more placement directions: 2, 4, 5, 6 then all 8 (horizontal, vertical and diagonal, forwards and backwards).
- **Word placement.** For every word and every allowed direction, the generator lists the positions where the word fits (empty cells or matching letters), then picks one at random. Remaining cells are filled with random letters.
- **Dictionary.** Words are loaded from a text file, sorted, and looked up with a recursive binary search.
- **Game loop.** Players take turns over five rounds of increasing difficulty, with a total time limit and a time limit per grid. A player enters a row, a column, a direction and a word; the answer is checked against the grid and the dictionary.
- **Persistence.** Grids can be saved to and loaded from CSV files (`CasSimple.csv`, `CasComplexe.csv`).

Scoring: one point per word found, plus the number of seconds left when a grid is completed before its time limit.

## Project structure

```
├── src/WordSearch/
│   ├── Program.cs         entry point
│   ├── Jeu.cs             game loop, turns, scoring, input validation
│   ├── Plateau.cs         grid generation, word placement, CSV import/export
│   ├── Dictionnaire.cs    dictionary loading and binary search
│   ├── Joueur.cs          player state
│   ├── MotsPossibles*.txt French and English word lists
│   └── Cas*.csv           sample grids
└── tests/WordSearch.Tests/  xUnit tests
```

Identifiers and source comments are in French.

## Quickstart

Requires the [.NET SDK](https://dotnet.microsoft.com/download) 8.0 or later.

```bash
git clone https://github.com/samichellia90-bit/word-search-game-csharp.git
cd word-search-game-csharp
dotnet test                              # 6 tests
dotnet run --project src/WordSearch      # play in the terminal
```

The game asks for the language (`FR` or `EN`), the number of players, their names, and the time limits in minutes. Words are typed in upper case. Directions are entered as `N`, `S`, `E`, `O`, `NE`, `NO`, `SE`, `SO` (`O` stands for *ouest*, west), and give the reading direction from the first letter.

## Notes

The word lists and sample grids were supplied with the course assignment.

## License

MIT. See [LICENSE](LICENSE).
