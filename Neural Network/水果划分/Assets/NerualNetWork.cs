using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct DataPoint
{
    public double[] inputs;
    public double[] expectedOutPuts;
}

[Serializable]
public class NerualNetWork
{
    [SerializeField]
    public Layer[] _layers;
    

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
        
        //Debug.Log(result.Length);
        return result[0] > result[1] ? 0 : 1;
    }

    public double Cost(DataPoint dadaPoint)
    {
        double[] outputs = CalculatrOuputs(dadaPoint.inputs);
        Layer outputLayer = _layers[^1];

        double cost = 0;
        for (var nodeOut = 0; nodeOut < outputs.Length; nodeOut++)
        {
            cost += outputLayer.NodeCost(outputs[nodeOut], dadaPoint.expectedOutPuts[nodeOut]);
        }

        return cost;
    }

    public double Cost(DataPoint[] dataPoint)
    {
        double totalCost = 0;
        foreach (var item in dataPoint)
        {
            totalCost += Cost(item);
        }
        return totalCost / dataPoint.Length;
    }

    //运行一次梯度下降 （使用有限元差分法）
    public void Learn(DataPoint[] trainingData, Double learnRate)
    {
        --https://www.youtube.com/watch?v=hfMk-kjRv4c&ab_channel=SebastianLague
          -- 20:26
    }

}