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

    private float X;
    private float Y;

    [SerializeField]
    public DataPoint data;

    private void Start()
    {
        Mgr.Instance.Flowers.Add(this);
        X = this.GetComponent<RectTransform>().position.x / Screen.width;
        Y = this.GetComponent<RectTransform>().position.y / Screen.height;

        data.inputs = new double[] { X, Y };
        data.expectedOutPuts = type == Type.Blue ? new double[] { 1, 0 } : new double[] { 0, 1 };
    }


    public bool Classfimy(int num)
    {
        return (num == 0 && type == Type.Blue) || (num == 1 && type == Type.Red);
    }
}