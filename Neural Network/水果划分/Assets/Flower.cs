using System;
using UnityEngine;



public enum Type
{
    Blue,
    Red
}

public class Flower : MonoBehaviour
{
    public Type type;

    public float X;
    public float Y;

    public DataPoint data;

    private void Start()
    {
        Mgr.Instance.Flowers.Add(this);
        X = this.GetComponent<RectTransform>().position.x;
        Y = this.GetComponent<RectTransform>().position.y;

        data.inputs = new double[] { X, Y };
        if (type == Type.Blue)
        {
            data.expectedOutPuts = new double[] { 1, 0 };
        }
        else
        {
            data.expectedOutPuts = new double[] { 0, 1 };
        }
    }


    public bool Classfimy(int num)
    {
        if ((num == 0 && type == Type.Blue) || (num == 1 && type == Type.Red))
            return true;
        return false;
    }
}