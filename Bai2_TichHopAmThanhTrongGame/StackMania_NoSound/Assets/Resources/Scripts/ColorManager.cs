using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private List<MatConfig> listColorConfig;
    [SerializeField] private Material hideMat;
    public static ColorManager INSTANCE;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Material getTargetMat(int c)
    {
        return listColorConfig[c].targetMaterial;
    }
    public Material getShooterMat(int c)
    {
        return listColorConfig[c].shooterMaterial;
    }
    //public Material getMat(int c)
    //{
    //    return listColorConfig[c].targetMaterial;
    //}
    public Material getHideMat()
    {
        return hideMat;
    }
}
[System.Serializable]
public class MatConfig
{
    public Material targetMaterial;
    public Material shooterMaterial;
}
