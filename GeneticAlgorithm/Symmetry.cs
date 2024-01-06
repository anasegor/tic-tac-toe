using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
public static class Symmetry
{
    //public Symmetry() { }
    public static Dictionary <string, int> uniqBoards=new Dictionary<string, int>();
    //public static Dictionary<string, int> uniqBazeBoards = new Dictionary<string, int>();
    private static int id = 0;
    public static bool winning(int[] board, int player)
    {
        if (
            (board[0] == player && board[1] == player && board[2] == player) ||
            (board[3] == player && board[4] == player && board[5] == player) ||
            (board[6] == player && board[7] == player && board[8] == player) ||
            (board[0] == player && board[3] == player && board[6] == player) ||
            (board[1] == player && board[4] == player && board[7] == player) ||
            (board[2] == player && board[5] == player && board[8] == player) ||
            (board[0] == player && board[4] == player && board[8] == player) ||
            (board[2] == player && board[4] == player && board[6] == player)
            )
        {
            return true;
        }
        else
            return false;
    }
    public static List<int> checkBoard(int[] board)
    {
        List<int> ints = new List<int>();
        for (int i = 0; i < 9; i++)
            if (board[i] == 0) ints.Add(i);
        return ints;
    }
    public static int[] rotate90(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[6]; newBoard[1] = board[3]; newBoard[2] = board[0];
        newBoard[3] = board[7]; newBoard[4] = board[4]; newBoard[5] = board[1];
        newBoard[6] = board[8]; newBoard[7] = board[5]; newBoard[8] = board[2];
        return newBoard;
    }
    public static int[] rotate180(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[8]; newBoard[1] = board[7]; newBoard[2] = board[6];
        newBoard[3] = board[5]; newBoard[4] = board[4]; newBoard[5] = board[3];
        newBoard[6] = board[2]; newBoard[7] = board[1]; newBoard[8] = board[0];
        return newBoard;
    }
    public static int[] rotate270(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[2]; newBoard[1] = board[5]; newBoard[2] = board[8];
        newBoard[3] = board[1]; newBoard[4] = board[4]; newBoard[5] = board[7];
        newBoard[6] = board[0]; newBoard[7] = board[3]; newBoard[8] = board[6];
        return newBoard;
    }
    public static int[] vertSymm(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[2]; newBoard[1] = board[1]; newBoard[2] = board[0];
        newBoard[3] = board[5]; newBoard[4] = board[4]; newBoard[5] = board[3];
        newBoard[6] = board[8]; newBoard[7] = board[7]; newBoard[8] = board[6];
        return newBoard;
    }
    public static int[] horiSymm(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[6]; newBoard[1] = board[7]; newBoard[2] = board[8];
        newBoard[3] = board[3]; newBoard[4] = board[4]; newBoard[5] = board[5];
        newBoard[6] = board[0]; newBoard[7] = board[1]; newBoard[8] = board[2];
        return newBoard;
    }
    public static int[] diag1Symm(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[0]; newBoard[1] = board[3]; newBoard[2] = board[6];
        newBoard[3] = board[1]; newBoard[4] = board[4]; newBoard[5] = board[7];
        newBoard[6] = board[2]; newBoard[7] = board[5]; newBoard[8] = board[8];
        return newBoard;
    }
    public static int[] diag2Symm(int[] board)
    {
        int[] newBoard = new int[9];
        newBoard[0] = board[8]; newBoard[1] = board[5]; newBoard[2] = board[2];
        newBoard[3] = board[7]; newBoard[4] = board[4]; newBoard[5] = board[1];
        newBoard[6] = board[6]; newBoard[7] = board[3]; newBoard[8] = board[0];
        return newBoard;
    }
    public static int[] conForm(int[] arr)// -1 на 2
    {
        int[] newBoard = new int[9];
        for (int i = 0; i < 9; i++)
        {
            if (arr[i] == -1) newBoard[i] = 2;
            else newBoard[i] = arr[i];
        }
        return newBoard;
    }
    public static int[] conRetForm(int[] arr)// 2 на 1
    {
        int[] newBoard = new int[9];
        for (int i = 0; i < 9; i++)
        {
            if (arr[i] == 2) newBoard[i] = -1;
            else newBoard[i] = arr[i];
        }
        return newBoard;
    }
    public static void genAllBoards()
    {
        int[] board = new int[9];
        genUniqBoards(board, 1);
        board = new int[9];
        genUniqBoards(board, -1);
        return;
    }
    private static void genUniqBoards(int[] board, int player)
    {
        id++;
        int player1 = -1;
        int player2 = 1;
        string s = String.Join("", conForm(board));
        uniqBoards[s] = id;
        List <int> ints = checkBoard(board);
        if (winning(board, player1))
            return;
        else if (winning(board, player2))
            return;
        if (ints.Count() == 0)
            return;
        for (int i = 0; i < ints.Count; i++)
        {
            board[ints[i]] = player;
            genUniqBoards(board, -player);
            board[ints[i]] = 0;
        }
        return;

    }


}

