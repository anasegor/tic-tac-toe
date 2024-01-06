//using GeneticAlgorithm;
using System;
using System.Collections.Specialized;
using System.Xml.Linq;
using static Symmetry;

void drawBoard(char [] board)
{
    Console.WriteLine(" ");
    Console.WriteLine($"{board[0]} {board[1]} {board[2]}");
    Console.WriteLine($"{board[3]} {board[4]} {board[5]}");
    Console.WriteLine($"{board[6]} {board[7]} {board[8]}");
}

bool checkField(int index,int[] board)
{
    if ((board[index] == 1) || (board[index] == -1)) return false;
    return true;
}
int networkResponse(int[] board, Perceptron work)//возвращает индекс на доске
{
    int maxIndex=0;
    double maxRes=0;
    List<int> empt = new List<int>();
    empt=checkBoard(board);
    for(int i = 0;i < empt.Count;i++)
    {
        board[empt[i]] = 1;
        //переделать массив и в строку 
        int[] boardN = conForm(board);
        string s = String.Join("", boardN);
        string basicBoard = s;
        // достать базовый и его переделать
        int MinId = uniqBoards[s];

        string symm = String.Join("", rotate90(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        symm = String.Join("", rotate180(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        symm = String.Join("", rotate270(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        symm = String.Join("", vertSymm(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        symm = String.Join("", horiSymm(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        symm = String.Join("", diag1Symm(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        symm = String.Join("", diag2Symm(boardN));
        if (uniqBoards[symm] < MinId)
        {
            MinId = uniqBoards[symm];
            basicBoard = symm;
        }
        boardN = basicBoard.ToCharArray().Select(i => int.Parse(i.ToString())).ToArray();
        boardN = conRetForm(boardN);
        work.train(boardN);
        //work.train(board);
        if (work.res > maxRes)
        {
            maxIndex = empt[i];
            maxRes = work.res;
        }
        board[empt[i]] = 0;
    }
    return maxIndex;


}

genAllBoards();
GeneticAlg a = new GeneticAlg(80);
List<Perceptron> res = a.startAlgoritm();
Perceptron result1 = new Perceptron(res[0]);
Perceptron result2 = new Perceptron(res[0]);
const char opponentPlayer0 = 'o';//-1
const char aiPlayer0 = 'x';//1
int opponentPlayer = -1;
int aiPlayer = 1;

while (true)
{
    int[] board = new int[9];
    char[] board0 = { '0', '1', '2', '3', '4', '5', '6', '7', '8' };
    Console.WriteLine("Какими будем играть?");
    Console.WriteLine("1-первыми, 2-вторыми");
    int movePlayer = Convert.ToInt32(Console.ReadLine());
    while (true)
    {
        if ((movePlayer != 1) && (movePlayer != 2))
        {
            Console.WriteLine("Введите корректный индекс! 1-первыми, 2-вторыми");
            movePlayer = Convert.ToInt32(Console.ReadLine());
        }
        else break;
    }
    List<int> empt = new List<int>();

    if (movePlayer == 2)
    {
        while (true)
        {
            int resIndex = networkResponse(board, result1);
            board[resIndex] = aiPlayer;
            board0[resIndex] = aiPlayer0;
            drawBoard(board0);
            empt = checkBoard(board);
            if (winning(board, aiPlayer))
            {
                Console.WriteLine("Проиграли!(");
                break;
            }
            if (empt.Count() == 0)
            {
                Console.WriteLine("Ничья!");
                break;
            }
            bool flag1 = true;
            int meMotion = -1;
            while (flag1)
            {
                meMotion = Convert.ToInt32(Console.ReadLine());
                if ((meMotion > 8) && (meMotion < 0))
                {
                    Console.WriteLine("Некорректный индекс!");
                    continue;
                }
                if (!checkField(meMotion, board))
                {
                    Console.WriteLine("Это поле занято!");
                    continue;
                }
                flag1 = false;
            }
            board[meMotion] = opponentPlayer;
            board0[meMotion] = opponentPlayer0;

            empt = checkBoard(board);
            if (winning(board, opponentPlayer))
            {
                Console.WriteLine("Выиграли!");
                break;
            }
            if (empt.Count() == 0)
            {
                Console.WriteLine("Ничья!");
                break;
            }

        }
    }
    else
    {
        while(true)
        {
            drawBoard(board0);
            bool flag1 = true;
            int meMotion = -1;
            while (flag1)
            {
                meMotion = Convert.ToInt32(Console.ReadLine());
                if ((meMotion > 8) && (meMotion < 0))
                {
                    Console.WriteLine("Некорректный индекс!");
                    continue;
                }
                if (!checkField(meMotion, board))
                {
                    Console.WriteLine("Это поле занято!");
                    continue;
                }
                flag1 = false;
            }
            board[meMotion] = opponentPlayer;
            board0[meMotion] = opponentPlayer0;
            empt = checkBoard(board);
            if (winning(board, opponentPlayer))
            {
                Console.WriteLine("Выиграли!");
                break;
            }
            if (empt.Count() == 0)
            {
                Console.WriteLine("Ничья!");
                break;
            }
            int resIndex = networkResponse(board, result1);
            board[resIndex] = aiPlayer;
            board0[resIndex] = aiPlayer0;
            empt = checkBoard(board);
            if (winning(board, aiPlayer))
            {
                Console.WriteLine("Проиграли!(");
                break;
            }
            if (empt.Count() == 0)
            {
                Console.WriteLine("Ничья!");
                break;
            }
        }
    }
    Console.WriteLine("Продолжаем? 1-да ");
    if (Convert.ToInt32(Console.ReadLine()) != 1) break;
}


