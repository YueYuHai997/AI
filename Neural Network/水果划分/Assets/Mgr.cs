using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Mgr : MonoBehaviour
{
    public static Mgr Instance;

    public Text text;
    public Text cost;
    private void Awake()
    {
        Instance = this;
    }
    
    [HideInInspector]
    public List<Flower> Flowers = new List<Flower>();
    
    public RawImage raw;

    private Texture2D Texture;

     Color[] colors;

    private int width;
    private int height;

    [Range(-1, 1)] public double weight1_1_1, weight1_1_2, weight1_1_3;
    [Range(-1, 1)] public double weight1_2_1, weight1_2_2, weight1_2_3;
    [Range(-1, 1)] public double bias_1_1, bias_1_2, bias_1_3;
    
    [Range(-1, 1)] public double weight2_1_1, weight2_1_2;
    [Range(-1, 1)] public double weight2_2_1, weight2_2_2;
    [Range(-1, 1)] public double weight2_3_1, weight2_3_2;

    [Range(-1, 1)] public double bias_2_1, bias_2_2;
    
    
    public Color SafeColor = Color.blue;
    public Color PoisonColor = Color.red;


    private NerualNetWork network;
    
    void Start()
    {
        width = Screen.width;
        height = Screen.height;

        Texture = new Texture2D(width, height) { name = "Trail" };
        raw.texture = Texture;

        colors = new Color[width * height];
        
        network = new NerualNetWork(2, 3, 2);
    }

    void setWeight()
    {
          weight1_1_1 = network._layers[0].weigths[0, 0];
          weight1_1_2 = network._layers[0].weigths[0, 1];
          weight1_1_3 = network._layers[0].weigths[0, 2];
         
          weight1_2_1 = network._layers[0].weigths[1, 0];
          weight1_2_2 = network._layers[0].weigths[1, 1];
          weight1_2_3 = network._layers[0].weigths[1, 2];
         
          bias_1_1 = network._layers[0].biases[0];
          bias_1_2 = network._layers[0].biases[1];
          bias_1_3 = network._layers[0].biases[2];
         
          weight2_1_1 = network._layers[1].weigths[0, 0];
          weight2_1_2 = network._layers[1].weigths[0, 1];
         
          weight2_2_1 = network._layers[1].weigths[1, 0];
          weight2_2_2 = network._layers[1].weigths[1, 1];

          weight2_3_1 = network._layers[1].weigths[2, 0];
          weight2_3_2 = network._layers[1].weigths[2, 1];

          bias_2_1 = network._layers[1].biases[0];
          bias_2_2 = network._layers[1].biases[1];
          
          return;


          network._layers[0].weigths[0, 0] = weight1_1_1;
          network._layers[0].weigths[0, 1] = weight1_1_2;
          network._layers[0].weigths[0, 2] = weight1_1_3;
          network._layers[0].weigths[1, 0] = weight1_2_1;
          network._layers[0].weigths[1, 1] = weight1_2_2;
          network._layers[0].weigths[1, 2] = weight1_2_3;
          network._layers[0].biases[0] = bias_1_1;
          network._layers[0].biases[1] = bias_1_2;
          network._layers[0].biases[2] = bias_1_3;
          network._layers[1].weigths[0, 0] = weight2_1_1;
          network._layers[1].weigths[0, 1] = weight2_1_2;
          network._layers[1].weigths[1, 0] = weight2_2_1;
          network._layers[1].weigths[1, 1] = weight2_2_2;
          network._layers[1].weigths[2, 0] = weight2_3_1;
          network._layers[1].weigths[2, 1] = weight2_3_2;
          network._layers[1].biases[0] = bias_2_1;
          network._layers[1].biases[1] = bias_2_2;

    }


    private int num;
    double judge(DataPoint[] datas)
    {
        num = 0;
        foreach (var item in Flowers)
        {
            if (item.Classfimy(network.Classify(item.data.inputs[0], item.data.inputs[1])))
                num++;
        }
        
        double costs = network.Cost(datas);
        text.text = $"{num}/{datas.Length}";
        cost.text = $"Cost:{costs}";
        return costs;
    }

    private int count = 0;
    // Update is called once per frame
    void Update()
    { 
        DataPoint[] datas = Flowers.Select(X => X.data).ToArray();
        
        setWeight();

        network.Learn(datas, judge(datas));
        //judge(datas);

        
        count++;
        if (count >= 1)
        {
            // foreach (var VARIABLE in network._layers)
            // {
            //     VARIABLE.InitializeRandomWeights();
            // }
            double ww = width;
            double hh = height;
            
            
            count = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Visualize(i / ww, j / hh, i, j);
                }
            }
        
            Texture.SetPixels(colors); 
            Texture.Apply();
        }
        
    }

    // public int Classify(int input_1, int input_2)
    // {
    //     double output_1 = input_1 * weight1_1_1 + input_2 * weight1_2_1 + bias_1_1 * width;
    //     double output_2 = input_2 * weight1_2_1 + input_2 * weight1_2_2 + bias_1_2 * width;
    //     return (output_1 > output_2) ? 0 : 1;
    // }


    void Visualize(double X, double Y, int x, int y)
    {
        //
        if (network.Classify(X, Y) == 0) //Classify(X, Y) == 0
        {
            SetPixelAt(x, y, SafeColor);
        }
        else
        {
            SetPixelAt(x, y, PoisonColor);
        }
    }

    protected void SetPixelAt(int x, int y, Color color)
    {
        x = Math.Clamp(x, 0, width);
        y = Math.Clamp(y, 0, height);
        colors[y * width + x] = color;
    }
}