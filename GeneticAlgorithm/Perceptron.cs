using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class Perceptron
{
    private int numNeuronLin;
    private int [] numNeuronL;
    private int numNeuronLout;
    public double[,] w1;
    public double[] b1;
    public double[,] w2;
    public double[] b2;
    public double[,] wout;
    public double[] bout;
    private double[] innerLayer1;
    private double[] innerLayer2;
    public double res;//выход сети


    public Perceptron(int numInput, int[] numHidden, int numOutput)
    {
        this.numNeuronLin = numInput;
        this.numNeuronL = new int [2];//входные слои
        for (int i = 0; i < 2; i++)
            this.numNeuronL[i] = numHidden[i];
        this.numNeuronLout = numOutput;

        this.w1 = new double[numInput, numHidden[0]];
        this.b1 = new double[numHidden[0]];
        this.w2 = new double[numHidden[0], numHidden[1]];
        this.b2= new double[numHidden[1]];
        this.wout = new double[numHidden[1], numOutput];
        this.bout = new double[numOutput];
        this.innerLayer1 = new double[numHidden[0]];
        this.innerLayer2 = new double[numHidden[1]];
        Init();
    }
    public Perceptron (Perceptron other )
    {
        this.numNeuronLin = other.numNeuronLin;
        this.numNeuronLout = other.numNeuronLout;
        this.numNeuronL = new int[2];
        for(int i=0; i<2;i++)
            this.numNeuronL[i]= other.numNeuronL[i]; 
        this.w1 = new double[numNeuronLin, numNeuronL[0]];
        this.b1 = new double[numNeuronL[0]];
        this.w2 = new double[numNeuronL[0], numNeuronL[1]];
        this.b2 = new double[numNeuronL[1]];
        this.wout = new double[numNeuronL[1], numNeuronLout];
        this.bout = new double[numNeuronLout];
        this.innerLayer1 = new double[numNeuronL[0]];
        this.innerLayer2 = new double[numNeuronL[1]];

        for (int i = 0; i < numNeuronLin; i++)
            for (int j = 0; j < numNeuronL[0]; j++)
            {
                this.w1[i, j] = other.w1[i, j];
                this.b1[j] = other.b1[j];
            }
        for (int i = 0; i < numNeuronL[0]; i++)
            for (int j = 0; j < numNeuronL[1]; j++)
            {
                this.w2[i, j] = other.w2[i, j];
                this.b2[j] = other.b2[j];
            }
        for (int i = 0; i < numNeuronL[1]; i++)
            for (int j = 0; j < numNeuronLout; j++)
            {
                this.wout[i, j] = other.wout[i, j];
                this.bout[j] = other.bout[j];
            }

    }

    private void Init()
    {
        Random rnd = new Random();
        int znak;
        for (int i = 0; i < numNeuronLin; i++)
            for (int j = 0; j < numNeuronL[0]; j++)
            {
                znak = rnd.Next(0, 2);
                if (znak == 0) znak = 1;
                else znak = -1;
                w1[i, j] = znak*rnd.NextDouble();
            }
        for (int j = 0; j < numNeuronL[0]; j++)
        {
            znak = rnd.Next(0, 2);
            if (znak == 0) znak = 1;
            else znak = -1;
            b1[j] = znak*rnd.NextDouble();
        }
        for (int i = 0; i < numNeuronL[0]; i++)
            for (int j = 0; j < numNeuronL[1]; j++)
            {
                znak = rnd.Next(0, 2);
                if (znak == 0) znak = 1;
                else znak = -1;
                w2[i, j] = znak * rnd.NextDouble();
            }
        for (int j = 0; j < numNeuronL[1]; j++)
        {
            znak = rnd.Next(0, 2);
            if (znak == 0) znak = 1;
            else znak = -1;
            b2[j] = znak * rnd.NextDouble();
        }
        for (int i = 0; i < numNeuronL[1]; i++)
            for (int j = 0; j < numNeuronLout; j++)
            {
                znak = rnd.Next(0, 2);
                if (znak == 0) znak = 1;
                else znak = -1;
                wout[i, j] =znak*rnd.NextDouble();
            }
                
        for (int j = 0; j < numNeuronLout; j++)
        {
            znak = rnd.Next(0, 2);
            if (znak == 0) znak = 1;
            else znak = -1;
            bout[j] = znak*rnd.NextDouble();
        }
            


    }
    public void train(int[] data)
    {
        test(data, ref res);
    }
    public void test(int[] input, ref double output)//здесь прямое распространение
    {
        for (int j = 0; j < numNeuronL[0]; j++)
        {
            innerLayer1[j] = 0;
            for (int i = 0; i < numNeuronLin; i++)
                innerLayer1[j] += input[i] * w1[i, j];
            innerLayer1[j] -= b1[j];
            innerLayer1[j] = Sigmoid(innerLayer1[j]);//активация 
        }
        for (int j = 0; j < numNeuronL[1]; j++)
        {
            innerLayer2[j] = 0;
            for (int i = 0; i < numNeuronL[0]; i++)
                innerLayer2[j] += innerLayer1[i] * w2[i, j];
            innerLayer2[j] -= b2[j];
            innerLayer2[j] = Sigmoid(innerLayer2[j]);//активация 
        }
        for (int j = 0; j < numNeuronLout; j++)
        {
            output= 0;
            for (int i = 0; i < numNeuronL[1]; i++)
                output += innerLayer2[i] * wout[i, j];
            output-= bout[j];
            output= Sigmoid(output);//активация 
        }

    }
    private double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-7 * x));
    }
    
}

