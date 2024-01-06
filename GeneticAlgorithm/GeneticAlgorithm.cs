using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Symmetry;
class PercepScore
{
    public Perceptron pers;
    public double score;
    public PercepScore(Perceptron newPers, double score)
    { 
        this.pers = newPers;
        this.score = score;
    }
}
class GeneticAlg
{
    private int percepNumNeuronLin=9;
    private int[] percepNumNeuronL= { 30, 10 };
    private int percepNumNeuronLout=1;
    private int sizeP;
    List<PercepScore> population1 = new List<PercepScore>();
    List<PercepScore> population2 = new List<PercepScore>();
    double averageP1 = 0;
    double averageP2 = 0;
    public Random rnd = new Random();
    List<PercepScore> populationChild = new List<PercepScore>();
    int iter = 0;


    public GeneticAlg(int sP)//инициализация
    {
        this.sizeP = sP;

        for (int i=0;i< sP; i++)
            population1.Add(new PercepScore(new Perceptron(percepNumNeuronLin, percepNumNeuronL, percepNumNeuronLout),0));
        for (int i = 0; i < sP; i++)
            population2.Add(new PercepScore(new Perceptron(percepNumNeuronLin, percepNumNeuronL, percepNumNeuronLout),0));
    }
    public List<Perceptron> startAlgoritm()//вернуть 2х?
    {
        int iter = 0;
        int maxInd1 = 0;
        int maxInd2 = 0;
        do
        {
            iter++;
            playAllPercep();
            averageP1 = 0;
            averageP2 = 0;
            for (int i = 0; i < sizeP; i++)
            {
                averageP1 += population1[i].score;
                averageP2 += population2[i].score;
            }
            averageP1 /= sizeP*sizeP;
            averageP2 /= sizeP * sizeP;

           
            if((averageP1 <= 9.1)&& (averageP1 >= 8.9)&& (averageP2 <= 9.1)&&(averageP2 >= 8.9))
            {
                for (int i = 0; i < sizeP; i++)
                    if (population1[i].score > population1[maxInd1].score)
                        maxInd1 = i;
                for (int i = 0; i < sizeP; i++)
                    if (population2[i].score > population2[maxInd2].score)
                        maxInd2 = i;

                break;
            }

            crossoverTournament(population1);
            crossoverTournament(population2);
            mutation(population1);
            mutation(population2);
            if(iter%10==0)
                Console.WriteLine("ошибка1:{0}, ошибка2:{1}  ", averageP1, averageP2);

        } while (true);
        Console.WriteLine("Количество итераций:{0} ", iter);
        Console.WriteLine("ошибка:{0} ", averageP1);
        Console.WriteLine("ошибка 2х:{0} ", averageP2);
        List<Perceptron> populationArr = new List<Perceptron>();
        populationArr.Add(population1[maxInd1].pers);
        populationArr.Add(population2[maxInd2].pers);
        return populationArr;
    }
    private void crossoverTournament(List<PercepScore> population)
    {
        int p1 = 0;
        int p2 = 0;
        int sizeChild = (int)(sizeP / 2);
        populationChild.Clear();

        crossoverTournamentParents(sizeChild, population);

        for (int k = 0; k < sizeChild; k++)
        {
            p1 = rnd.Next(0, sizeChild - 1);

            p2 = rnd.Next(0, sizeChild - 1);

            populationChild.Add(new PercepScore(new Perceptron(percepNumNeuronLin, percepNumNeuronL, percepNumNeuronLout), 0));

            double same = rnd.NextDouble();

            for (int i = 0; i < percepNumNeuronLin; i++)
                for (int j = 0; j < percepNumNeuronL[0]; j++)
                    populationChild[k].pers.w1[i, j] = same * population[p1].pers.w1[i, j] + (1 - same) * population[p2].pers.w1[i, j];

            for (int j = 0; j < percepNumNeuronL[0]; j++)
                populationChild[k].pers.b1[j] = same * population[p1].pers.b1[j] + (1 - same) * population[p2].pers.b1[j];

            for (int i = 0; i < percepNumNeuronL[0]; i++)
                for (int j = 0; j < percepNumNeuronL[1]; j++)
                    populationChild[k].pers.w2[i, j] = same * population[p1].pers.w2[i, j] + (1 - same) * population[p2].pers.w2[i, j];

            for (int j = 0; j < percepNumNeuronL[1]; j++)
                populationChild[k].pers.b2[j] = same * population[p1].pers.b2[j] + (1 - same) * population[p2].pers.b2[j];

            for (int i = 0; i < percepNumNeuronL[1]; i++)
                for (int j = 0; j < percepNumNeuronLout; j++)
                    populationChild[k].pers.wout[i, j] = same * population[p1].pers.wout[i, j] + (1 - same) * population[p2].pers.wout[i, j];

            for (int j = 0; j < percepNumNeuronLout; j++)
                populationChild[k].pers.bout[j] = same * population[p1].pers.bout[j] + (1 - same) * population[p2].pers.bout[j];

        }

        population.AddRange(populationChild);
        sizeP += sizeChild;

    }
    public void crossoverTournamentParents(int sizeChild, List<PercepScore> population)
    {
        for (int i = population.Count - 1; i >= 1; i--)//перемешать
        {
            int j = rnd.Next(i + 1);
            var temp = population[j];
            population[j] = population[i];
            population[i] = temp;
        }
        for (int i = 0; i < sizeChild; i++)
        {
            if (population[i].score > population[i + 1].score)
                population.RemoveAt(i + 1);
            else
                population.RemoveAt(i);
            sizeP--;
        }
    }
    private void playAllPercep()
    {
        for (int i = 0; i < sizeP; i++)
        {
            population1[i].score = 0;
            population2[i].score = 0;
        }

        for (int i = 0; i < sizeP; i++)
            for (int j = 0; j < sizeP; j++)
                playPercep(population1[i], population2[j]);
    }
    private void playPercep(PercepScore percepScore1,  PercepScore percepScore2)// 0-пусто  1 -1
    {
        List<int> ints = new List<int>();
        int[] board = new int[9];
        double res;
        int p1 = 1;
        int p2 = -1;
        int h=0;
        while(true)
        {
            ints=checkBoard(board);
            double max=0;
            int max_int=0;
            h++;
            int cout1 = 0;
            int cout2 = 0;
            for (int i=0; i<ints.Count; i++)
            {
                board[ints[i]] = p1;
                int[] boardN = conForm(board);
                string s = String.Join("", boardN);
                string basicBoard = s;

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

                percepScore1.pers.train(boardN);
                res=percepScore1.pers.res;
                if (res > max)
                {
                    max = res;
                    max_int = ints[i];
                }
                board[ints[i]] = 0;
            }
            board[max_int] = p1;
            cout1++;
            if(winning(board,p1))
            {
                percepScore1.score += 10-cout1;
                percepScore2.score -= (10-cout2);
                break;
            }
            if(ints.Count==0)
            {
                percepScore1.score += 9;
                percepScore2.score +=9;
                break;
            }

            max = 0;
            max_int = 0;
            ints=checkBoard(board);
            for (int i = 0; i < ints.Count; i++)
            {
                board[ints[i]] = p2;
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

                percepScore2.pers.train(boardN);
                res = percepScore2.pers.res;
                if (res > max)
                {
                    max = res;
                    max_int = ints[i];
                }
                board[ints[i]] = 0;
            }
            board[max_int] = p2;
            cout2++;
            if (winning(board, p2))
            {
                percepScore1.score -= (10-cout1);
                percepScore2.score += 10-cout2;
                break;
            }
            if (ints.Count == 0)
            {
                percepScore1.score += 9;
                percepScore2.score += 9;
                break;
            }
        }
    }
    public void mutation(List<PercepScore> population)
    {
        int sizeMutation = (int)(sizeP * 0.05);
        double n;
        int znak;
        for (int k = 0; k < sizeMutation; k++)
        {
            for (int i = 0; i < percepNumNeuronLin; i++)
                for (int j = 0; j < percepNumNeuronL[0]; j++)
                {
                    n = rnd.NextDouble() / 10;
                    znak = rnd.Next(0, 2);
                    if (znak == 0) n = 1 - n;
                    else n = 1 + n;
                    population[k].pers.w1[i, j] *= n;

                }
            for (int j = 0; j < percepNumNeuronL[0]; j++)
            {
                n = rnd.NextDouble() / 10;
                znak = rnd.Next(0, 2);
                if (znak == 0) n = 1 - n;
                else n = 1 + n;
                population[k].pers.b1[j] *= n;
            }
            for (int i = 0; i < percepNumNeuronL[0]; i++)
                for (int j = 0; j < percepNumNeuronL[1]; j++)
                {
                    n = rnd.NextDouble() / 10;
                    znak = rnd.Next(0, 2);
                    if (znak == 0) n = 1 - n;
                    else n = 1 + n;
                    population[k].pers.w2[i, j] *= n;
                }
            for (int j = 0; j < percepNumNeuronL[1]; j++)
            {
                n = rnd.NextDouble() / 10;
                znak = rnd.Next(0, 2);
                if (znak == 0) n = 1 - n;
                else n = 1 + n; ;
                population[k].pers.b2[j] *= n;
            }
            for (int i = 0; i < percepNumNeuronL[1]; i++)
                for (int j = 0; j < percepNumNeuronLout; j++)
                {
                    n = rnd.NextDouble() / 10;
                    znak = rnd.Next(0, 2);
                    if (znak == 0) n = 1 - n;
                    else n = 1 + n;
                    population[k].pers.wout[i, j] *= n;
                }
            for (int j = 0; j < percepNumNeuronLout; j++)
            {
                n = rnd.NextDouble() / 10;
                znak = rnd.Next(0, 2);
                if (znak == 0) n = 1 - n;
                else n = 1 + n; ;
                population[k].pers.bout[j] *= n;
            }
        }

    }

}

