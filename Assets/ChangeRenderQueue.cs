using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRenderQueue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Texture texture;
    void Awake()
    {
        GetComponent<Image>().material.renderQueue = -1;
        GetComponent<Image>().material.mainTexture = texture;
    }

}
