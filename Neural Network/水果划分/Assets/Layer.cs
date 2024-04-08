using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;


public class Layer
{
    /// <summary>
    /// 当前节点的输入节点和输出节点
    /// </summary>
    public int numNodesIn, numNodesOut;
    /// <summary> 权重 </summary>
    public double[,] weigths;
    /// <summary> 权重 </summary>
    public double[] biases; 

    
    //ctor
    public Layer(int numNodesIn, int numNodesOut)
    {
        this.numNodesIn = numNodesIn;
        this.numNodesOut = numNodesOut;
        
        weigths = new double[numNodesIn, numNodesOut];
        costGradintW = new double[numNodesIn, numNodesOut];
        
        biases = new double[numNodesOut];
        costGradientB = new double[numNodesOut];

        //随机权重的值
        InitializeRandomWeights();
    }

    /// <summary>
    /// 计算当前层输出
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public double[] CalulateOutOutputs(double[] inputs)
    {
        double[] weightedInputs = new double [numNodesOut];
        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            double weightedInput = biases[nodeOut] /* *BiasScale  */;
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                weightedInput += inputs[nodeIn] * weigths[nodeIn, nodeOut];
            }

            weightedInputs[nodeOut] = ActionFunction(weightedInput);
        }
        return weightedInputs;
    }

    /// <summary>
    /// 网络权重初始化
    /// </summary>
    public void InitializeRandomWeights()
    {
        Random rng = new Random();
        for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
        {
            for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
            {
                //返回一个 -1 ~ 1的值
                double randomValue = rng.NextDouble() * 2 - 1;
                //将随机缩放到 1 / Math.Sqrt(numNodesIn)
                weigths[nodeIn, nodeOut] = randomValue / Math.Sqrt(numNodesIn);

                //注意有许多不同的方法可以尝试为随机权重选择一个好的范围，这些取决于诸如使用的激活函数以及如何缩放网络的输入等因素。
                //以s型激活函数为例，我们不希望加权输入太大，否则函数的斜率会非常接近于零，导致梯度下降算法学习非常缓慢(或者根本不学习)。
            }
        }
    }

    /// <summary>
    /// S函数
    /// </summary>
    /// <param name="weightedInput"></param>
    /// <returns></returns>
    double ActionFunction(double weightedInput)
    {
        return 1 / (1 + Math.Exp(-weightedInput));
    }
    

    /// <summary>
    /// cost函数  计算 输出结果与期望结果的差平方 
    /// </summary>
    /// <param name="OutputActivation"></param>
    /// <param name="expectedOutPut"></param>
    /// <returns></returns>
    public double NodeCost(double OutputActivation, double expectedOutPut)
    {
        double errror = OutputActivation - expectedOutPut;
        return errror * errror;
    }
    
    //梯度下降
    /// <summary> 梯度下降 权重 </summary>
    public double[,] costGradintW;  
    /// <summary> 梯度下降 偏置 </summary>
    public double[] costGradientB;  

   /// <summary>
   ///  更新权重和偏置 根据梯度下降权重和偏置 （梯度下降）
   /// </summary>
   /// <param name="learnRate"></param>
    public void ApplyGradients(double learnRate)
    {
        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            biases[nodeOut] -= costGradientB[nodeOut] * learnRate;
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                weigths[nodeIn, nodeOut] -= costGradintW[nodeIn, nodeOut] * learnRate;
            }
        }
    }
}
