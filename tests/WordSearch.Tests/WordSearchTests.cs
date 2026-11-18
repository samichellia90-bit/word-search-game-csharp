using Xunit;

namespace WordSearch.Tests
{
    public class WordSearchTests
    {
        private static readonly Dictionnaire French = new Dictionnaire("FR");

        private static Plateau SimpleGrid() =>
            new Plateau(Path.Combine(AppContext.BaseDirectory, "CasSimple.csv"), French);

        [Fact]
        public void BinarySearchFindsExistingWordsOnly()
        {
            Assert.True(French.RechDicoRecursif("PAYE"));
            Assert.False(French.RechDicoRecursif("ZZZZZZ"));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(12)]
        public void RandomWordRespectsMaximumLength(int maxLength)
        {
            Assert.True(French.GetWord(maxLength).Length <= maxLength);
        }

        [Fact]
        public void WordIsAcceptedOnlyAtItsRealPosition()
        {
            Plateau grid = SimpleGrid();
            Assert.True(grid.Test_Plateau("VOYAGE", 1, 0, "O-E"));
            Assert.False(grid.Test_Plateau("VOYAGE", 0, 1, "O-E"));
        }

        [Fact]
        public void DirectionOutsideDifficultyLevelIsRejected()
        {
            // Level 1 only allows N-S and O-E placements.
            Assert.False(SimpleGrid().Test_Plateau("VOYAGE", 1, 5, "E-O"));
        }

        [Fact]
        public void GeneratedGridContainsEveryWordToFind()
        {
            Plateau grid = new Plateau(French, 1);
            Assert.Equal(8, grid.ToFind.Length);
            Assert.All(grid.ToFind, word => Assert.False(string.IsNullOrEmpty(word)));
        }
    }
}
