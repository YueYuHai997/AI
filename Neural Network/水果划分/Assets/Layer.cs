using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class Layer
{
    //输入节点 输出节点数量
    public int numNodesIn, numNodesOut;
    public double[,] weigth; //权重
    public double[] biases;  //偏置

    private Random rng = new Random();
    public Layer(int numNodesIn, int numNodesOut)
    {
        this.numNodesIn = numNodesIn;
        this.numNodesOut = numNodesOut;
        
        weigth = new double[numNodesIn, numNodesOut];
        costGradintW = new double[numNodesIn, numNodesOut];
        
        biases = new Double[numNodesOut];
        costGradientB = new Double[numNodesOut];


        InitializeRandomWeights();
    }

    //计算层输出
    public double[] CalulateOutOutputs(double[] inputs)
    {
        double[] weightedInputs = new double [numNodesOut];
        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            double weightedInput = biases[nodeOut];
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                weightedInput += inputs[nodeIn] * weigth[nodeIn, nodeOut];
            }
            weightedInputs[nodeOut] = ActionFunction(weightedInput);
        }
        return weightedInputs;
    }

    //网络权重初始化随机
    void InitializeRandomWeights()
    {
        for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
        {
            for (int NodeOut = 0; NodeOut < numNodesOut; NodeOut++)
            {
                //返回一个 -1 ~ 1的值
                double randomValue = rng.NextDouble() * 2 - 1;
                //将随机缩放到 1 / Math.Sqrt(numNodesIn)
                weigth[nodeIn, NodeOut] = randomValue / Math.Sqrt(numNodesIn);
                
                //注意有许多不同的方法可以尝试为随机权重选择一个好的范围，这些取决于诸如使用的激活函数以及如何缩放网络的输入等因素。
                //以s型激活函数为例，我们不希望加权输入太大，否则函数的斜率会非常接近于零，导致梯度下降算法学习非常缓慢(或者根本不学习)。
            }
        }
    }

    //S 函数
    double ActionFunction(double weightedInput)
    {
        return 1 / (1 + Math.Exp(-weightedInput));
    }

    //cost函数  输出结果 与 期望结果的差平方 
    public double NodeCost(double OutputActivation, double expectedOutPut)
    {
        double errror = OutputActivation - expectedOutPut;
        return errror * errror;
    }
    
    
    //梯度下降
    public double[,] costGradintW;  //梯度下降权重
    public double[] costGradientB;  //梯度下降偏差

    //更新 权重 和 偏置 根据 成本函数 （梯度下降）
    public void ApplyGradients(double learnRate)
    {
        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            biases[nodeOut] -= costGradientB[nodeOut] * learnRate;
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                weigth[nodeIn, nodeOut] = costGradintW[nodeIn, nodeOut] * learnRate;
            }
        }
    }
}
