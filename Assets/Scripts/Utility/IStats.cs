using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStat
{ 
    GameObject gameObject { get; }
    public float Value{get;}
    float Ameliorateur { get; set; }
}
