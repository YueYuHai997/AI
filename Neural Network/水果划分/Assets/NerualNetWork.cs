using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public struct DataPoint
{
    public double[] inputs;
    public double[] expectedOutPuts;
}

[Serializable]
public class NerualNetWork
{
    [SerializeField] public Layer[] _layers;


    //ctor
    public NerualNetWork(params int[] layerSizes)
    {
        _layers = new Layer[layerSizes.Length - 1];

        for (int i = 0; i < _layers.Length; i++)
        {
            _layers[i] = new Layer(layerSizes[i], layerSizes[i + 1]);
        }
    }

    //计算最终层结果
    double[] CalculatrOuputs(double[] inputs)
    {
        foreach (var item in _layers)
        {
            inputs = item.CalulateOutOutputs(inputs);
        }

        return inputs;
    }

    // 产生结果 当前区域是有毒还是无毒
    public int Classify(params double[] inputs)
    {
        double[] result = CalculatrOuputs(inputs);

        return result[0] > result[1] ? 0 : 1;
    }

    /// <summary>
    /// 成本函数
    /// </summary>
    /// <param name="dadaPoint"></param>
    /// <returns></returns>
    public double Cost(DataPoint dadaPoint)
    {
        double[] outputs = CalculatrOuputs(dadaPoint.inputs);
        Layer outputLayer = _layers[_layers.Length - 1];

        double cost = 0;
        for (int nodeOut = 0; nodeOut < outputs.Length; nodeOut++)
        {
            cost += outputLayer.NodeCost(outputs[nodeOut], dadaPoint.expectedOutPuts[nodeOut]);
        }

        return cost;
    }

    /// <summary>
    /// 代价函数
    /// </summary>
    /// <param name="dataPoint"></param>
    /// <returns></returns>
    public double Cost(DataPoint[] dataPoint)
    {
        double totalCost = 0;
        foreach (var item in dataPoint)
        {
            totalCost += Cost(item);
        }

        return totalCost / dataPoint.Length;
    }

    /// <summary>
    /// 运行一次梯度下降 （使用有限元差分法）
    /// </summary>
    /// <param name="trainingData"></param>
    /// <param name="learnRate"></param>
    public void Learn(DataPoint[] trainingData, Double learnRate)
    {
        const double h = 0.0001; //步长
        double originalCost = Cost(trainingData);
        foreach (var item in _layers)
        {
            //计算当前权重的代价梯度
            for (int nodeIn = 0; nodeIn < item.numNodesIn; nodeIn++)
            {
                for (int nodeOut = 0; nodeOut < item.numNodesOut; nodeOut++)
                {
                    item.weigths[nodeIn, nodeOut] += h; //假设权重修改为 +h
                    double deltaCost = Cost(trainingData) - originalCost; //则h权重对代价函数的影响为 deltaCost
                    item.weigths[nodeIn, nodeOut] -= h; //恢复权重值
                    item.costGradintW[nodeIn, nodeOut] = deltaCost / h; //修改梯度下降权重
                }
            }

            //计算当前偏差的代价梯度
            for (int biasIndex = 0; biasIndex < item.biases.Length; biasIndex++)
            {
                item.biases[biasIndex] += h; //假设权重修改为 +h
                double deltaCost = Cost(trainingData) - originalCost; //则h偏执对代价函数的影响为 deltaCost
                item.biases[biasIndex] -= h;
                item.costGradientB[biasIndex] = deltaCost / h; //修改梯度下降偏执
            }
        }

        ApplyAllGradients(learnRate);
    }

    /// <summary>
    /// 应用所有的
    /// </summary>
    /// <param name="learnRate"></param>
    public void ApplyAllGradients(double learnRate)
    {
        foreach (var item in _layers)
        {
            item.ApplyGradients(learnRate);
        }
    }
}