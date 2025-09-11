using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCSManager : MonoBehaviour
{
    public ComputeShader LayerCS;
    public Vector2Int resultDimensions = Vector2Int.one * 256;
    public Vector3Int blockSize = new Vector3Int(16,16,1);
    public RenderTexture LayerTexture;
    public Vector2 GridSize = Vector2.one*4.0f;
    public Vector2 GridMargin = Vector2.one;
    
    public Color fillColour = Color.white;
    public Color clearColour = Color.black;
    
    public GameObject LayerObject;
    private Renderer LayerRenderer; 

    private Vector4 fillColourVector = Vector4.one;
    private Vector4 clearColourVector = Vector4.one;
    


    void Start() {
        this.PrepareRenderTextures();
        this.PrepareData();
    }
    void Update(){
        this.DispatchLayerShader();
        this.LayerRenderer.material.mainTexture = this.LayerTexture;
    }


    public void PrepareRenderTextures(){
        LayerTexture = new RenderTexture(resultDimensions.x, resultDimensions.y, 0);
        LayerTexture.enableRandomWrite = true;
        LayerTexture.Create();
    }
    public void PrepareData(){
        this.fillColourVector = new Vector4(this.fillColour.r,this.fillColour.g,this.fillColour.b,this.fillColour.a);
        this.clearColourVector = new Vector4(this.clearColour.r,this.clearColour.g,this.clearColour.b,this.clearColour.a);
        this.LayerRenderer = this.LayerObject.GetComponent<Renderer>();
    }
    public void DispatchLayerShader(){
        int kernelIndex = this.LayerCS.FindKernel("GrassLayer");
        this.LayerCS.SetTexture(kernelIndex, "OutputLayer", LayerTexture);
        this.LayerCS.SetVector("CellSize", GridSize);
        this.LayerCS.SetVector("CellMargin", GridMargin);
        this.LayerCS.SetVector("FillColour", this.fillColourVector);
        this.LayerCS.SetVector("ClearColour", this.clearColourVector);
        float layerHeight = this.LayerObject.transform.position.y%1.0f;
        this.LayerCS.SetFloat("LayerHeight", layerHeight);

        this.LayerCS.Dispatch(kernelIndex, this.resultDimensions.x / this.blockSize.x, this.resultDimensions.y / this.blockSize.y, 1);
    }
}
