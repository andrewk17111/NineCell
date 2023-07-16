using Microsoft.VisualBasic.FileIO;
using NineCell;

namespace NineCellTest;

[TestClass]
public class TestSudokuSolver
{
    [TestMethod]
    public void Test0()
        => Assert.IsTrue(RunTest("difficulty0.csv"));

    [TestMethod]
    public void Test1()
        => Assert.IsTrue(RunTest("difficulty1.csv"));

    [TestMethod]
    public void Test2()
        => Assert.IsTrue(RunTest("difficulty2.csv"));

    [TestMethod]
    public void Test3()
        => Assert.IsTrue(RunTest("difficulty3.csv"));

    [TestMethod]
    public void Test4()
        => Assert.IsTrue(RunTest("difficulty4.csv"));

    [TestMethod]
    public void Test5()
        => Assert.IsTrue(RunTest("difficulty5.csv"));

    [TestMethod]
    public void Test6()
        => Assert.IsTrue(RunTest("difficulty6.csv"));

    [TestMethod]
    public void Test7()
        => Assert.IsTrue(RunTest("difficulty7.csv"));

    [TestMethod]
    public void Test8()
        => Assert.IsTrue(RunTest("difficulty8.csv"));

    private static bool RunTest(string file)
    {
        using StreamReader reader = new StreamReader(file);
        using TextFieldParser parser = new TextFieldParser(reader);
        int solved = 0;
        List<int> unsolved = new List<int>();

        parser.SetDelimiters(",");
        parser.ReadLine();

        while (!parser.EndOfData)
        {
            string[] line = parser.ReadFields()!;
            Board board = ReadBoard(line[1]);
            int untouched = 0;

            while (!board.Complete && untouched < 5)
                if (!board.UpdateNotes() || !board.UpdateValues())
                    untouched++;

            if (board.Complete && board.Equals(ReadBoard(line[2])))
                solved++;
            else
                unsolved.Add(Convert.ToInt32(line[0]));
        }

        Console.WriteLine($"{solved}/{unsolved.Count}");
        Console.WriteLine(String.Join(", ", unsolved));

        return unsolved.Count == 0;
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