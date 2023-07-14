using Microsoft.VisualBasic.FileIO;
using NineCell;

namespace NineCellTest;

[TestClass]
public class TestSudokuSolver
{
    [TestMethod]
    public void TestSolver()
    {
        using StreamReader reader = new StreamReader("sudoku.csv");
        using TextFieldParser parser = new TextFieldParser(reader);
        int min_clues_solved = 81;
        int max_clues_unsolved = 0;
        float min_difficulty_solved = 10;
        float max_difficulty_unsolved = 0;
        int solved = 0;
        int unsolved = 0;

        parser.SetDelimiters(",");
        parser.ReadLine();

        while (!parser.EndOfData)
        {
            string[] line = parser.ReadFields()!;
            Board board = ReadBoard(line[1]);
            Board solution = ReadBoard(line[2]);
            int clues = Convert.ToInt32(line[3]);
            float difficulty = Convert.ToSingle(line[4]);
            int untouched = 0;

            while (!board.Complete && untouched < 5)
                if (!board.UpdateNotes() || !board.UpdateValues())
                    untouched++;

            if (board.Equals(solution))
            {
                min_clues_solved = Math.Min(min_clues_solved, clues);
                min_difficulty_solved = Math.Min(min_difficulty_solved, difficulty);
                solved++;
            }
            else
            {
                max_clues_unsolved = Math.Max(max_clues_unsolved, clues);
                max_difficulty_unsolved = Math.Max(max_difficulty_unsolved, difficulty);
                unsolved++;
            }
        }

        Console.WriteLine($"Clues: {min_clues_solved} {max_clues_unsolved}");
        Console.WriteLine($"Difficulty: {min_difficulty_solved} {max_difficulty_unsolved}");
        Console.WriteLine($"{solved}/{unsolved}");

        if (unsolved > 0)
            Assert.Fail();
    }

    private static Board ReadBoard(string input)
    {
        Board board = new Board();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                int pos = y * 9 + x;

                if (input[pos] >= '1' && input[pos] <= '9')
                    board[x, y].Value = (byte)(input[pos] - '0');
            }
        }

        return board;
    }
}