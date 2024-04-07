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
    [Range(-100, 100)] public double bias_2_1, bias_2_2, bias_2_3;
    
    [Range(-1, 1)] public double weight2_1_1, weight2_1_2;
    [Range(-1, 1)] public double weight2_2_1, weight2_2_2;
    [Range(-1, 1)] public double weight2_3_1, weight2_3_2;

    [Range(-1, 1)] public double bias_3_1, bias_3_2;
    
    
    public Color SafeColor = Color.blue;
    public Color PoisonColor = Color.red;


    private NerualNetWork network = new NerualNetWork(2,3,2);
    
    void Start()
    {
        width = Screen.width;
        height = Screen.height;

        Texture = new Texture2D(width, height) { name = "Trail" };
        raw.texture = Texture;

        colors = new Color[width * height];
    }

    void setWeight()
    {
        
        network._layers[0].weigth[0, 0] = weight1_1_1;
        network._layers[0].weigth[0, 1] = weight1_1_2;
        network._layers[0].weigth[0, 2] = weight1_1_3;
        
        network._layers[0].weigth[1, 0] = weight1_2_1;
        network._layers[0].weigth[1, 1] = weight1_2_2;
        network._layers[0].weigth[1, 2] = weight1_2_3;

        network._layers[0].biases[0] = bias_2_1;
        network._layers[0].biases[1] = bias_2_2;
        network._layers[0].biases[2] = bias_2_3;
        
        network._layers[1].weigth[0, 0] = weight2_1_1;
        network._layers[1].weigth[0, 1] = weight2_1_2;
        
        network._layers[1].weigth[1, 0] = weight2_2_1;
        network._layers[1].weigth[1, 1] = weight2_2_2;

        network._layers[1].weigth[2, 0] = weight2_3_1;
        network._layers[1].weigth[2, 1] = weight2_3_2;
        
        network._layers[1].biases[0] = bias_3_1;
        network._layers[1].biases[1] = bias_3_2;

    }


    private int num;
    void judge()
    {
        num = 0;
        foreach (var item in Flowers)
        {
            if (item.Classfimy(network.Classify(item.X, item.Y)))
                num++;
        }

        DataPoint[] datas = Flowers.Select(X => X.data).ToArray();
        
        
        double costs = network.Cost(datas);
        
        text.text = $"{num}/{Flowers.Count}";
        
        cost.text = $"Cost:{costs}";
    }

    private int count = 0;
    // Update is called once per frame
    void Update()
    { 
        setWeight();
        

        
        count++;
        
        if (count >= 1)
        {
            count = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Visualize(i, j);
                }
            }
        
            Texture.SetPixels(colors);
             Texture.Apply();
        }
        
        judge();
        
    }

    // public int Classify(int input_1, int input_2)
    // {
    //      double output_1 = input_1 * weight_1_1 + input_2 * weight_2_1 + bias_1;
    //      double output_2 = input_2 * weight_1_2 + input_2 * weight_2_2 + bias_2;
    //      return (output_1 > output_2) ? 0 : 1;
    // }


    void Visualize(int X,int Y)
    {
        if (network.Classify(X, Y) == 0) //Classify(X, Y) == 0
        {
            SetPixelAt(X, Y, SafeColor);
        }
        else
        {
            SetPixelAt(X, Y, PoisonColor);
        }
    }

    protected void SetPixelAt(int x, int y, Color color)
    {
        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);
        colors[y * width + x] = color;
    }
}