using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassLayerManager : MonoBehaviour
{
    public GrassHeightCSManager grassHeightCSManager;
    public Renderer Layer1Renderer;
    public Renderer Layer2Renderer;
    public Renderer Layer3Renderer;
    void Start(){}
    void Update(){
        this.Layer1Renderer.material.mainTexture = grassHeightCSManager.layer1Output;
        this.Layer2Renderer.material.mainTexture = grassHeightCSManager.layer2Output;
        this.Layer3Renderer.material.mainTexture = grassHeightCSManager.layer3Output;
    }
}
