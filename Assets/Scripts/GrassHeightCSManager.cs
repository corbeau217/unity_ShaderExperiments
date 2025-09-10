using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassHeightCSManager : MonoBehaviour
{
    public ComputeShader noiseCS;
    public ComputeShader grassHeightCS;
    public Vector2Int resultDimensions = Vector2Int.one * 256;
    public Vector3Int blockSize = new Vector3Int(16,16,3);
    public RenderTexture noiseTexture;
    public RenderTexture layer1Output;
    public RenderTexture layer2Output;
    public RenderTexture layer3Output;
    public float Layer1Cutoff = 0.1f;
    public float Layer2Cutoff = 0.4f;
    public float Layer3Cutoff = 0.7f;
    
    public Color fillColour = Color.white;
    public Color clearColour = Color.black;

    private Vector4 fillColourVector = Vector4.one;
    private Vector4 clearColourVector = Vector4.one;
    
    public int noiseSeed = 0;


    void Start() {
        this.PrepareRenderTextures();
        this.PrepareData();
        this.DispatchNoiseShader();
        this.DispatchGrassShader();
    }
    void Update(){
    }


    public void PrepareRenderTextures(){
        noiseTexture = new RenderTexture(resultDimensions.x, resultDimensions.y, 0);
        layer1Output = new RenderTexture(resultDimensions.x, resultDimensions.y, 0);
        layer2Output = new RenderTexture(resultDimensions.x, resultDimensions.y, 0);
        layer3Output = new RenderTexture(resultDimensions.x, resultDimensions.y, 0);
        noiseTexture.enableRandomWrite = true;
        layer1Output.enableRandomWrite = true;
        layer2Output.enableRandomWrite = true;
        layer3Output.enableRandomWrite = true;
        noiseTexture.Create();
        layer1Output.Create();
        layer2Output.Create();
        layer3Output.Create();
    }
    public void PrepareData(){
        this.fillColourVector = new Vector4(this.fillColour.r,this.fillColour.g,this.fillColour.b,this.fillColour.a);
        this.clearColourVector = new Vector4(this.clearColour.r,this.clearColour.g,this.clearColour.b,this.clearColour.a);
    }
    public void DispatchNoiseShader(){
        int kernelIndex = this.noiseCS.FindKernel("GPUWhiteNoise");
        this.noiseCS.SetTexture(kernelIndex, "Result", noiseTexture);
        this.noiseCS.SetInt("textureWidth", resultDimensions.x);
        this.noiseCS.SetInt("textureHeight", resultDimensions.y);
        this.noiseCS.SetInt("seed", this.noiseSeed);

        this.noiseCS.Dispatch(kernelIndex, this.resultDimensions.x / this.blockSize.x, this.resultDimensions.y / this.blockSize.y, 1);
    }
    public void DispatchGrassShader(){
        int grassHeightKernelIndex = this.grassHeightCS.FindKernel("GrassHeightCS");
        

        // Set all the necessary buffers
        this.grassHeightCS.SetTexture(grassHeightKernelIndex, "InputHeightMap", noiseTexture);
        this.grassHeightCS.SetTexture(grassHeightKernelIndex, "Layer1", layer1Output);
        this.grassHeightCS.SetTexture(grassHeightKernelIndex, "Layer2", layer2Output);
        this.grassHeightCS.SetTexture(grassHeightKernelIndex, "Layer3", layer3Output);

        this.grassHeightCS.SetVector("FillColour", this.fillColourVector);
        this.grassHeightCS.SetVector("ClearColour", this.clearColourVector);

        this.grassHeightCS.SetFloat("Layer1Cutoff", this.Layer1Cutoff);
        this.grassHeightCS.SetFloat("Layer2Cutoff", this.Layer2Cutoff);
        this.grassHeightCS.SetFloat("Layer3Cutoff", this.Layer3Cutoff);

        // Dispatch
        this.grassHeightCS.Dispatch(grassHeightKernelIndex, this.resultDimensions.x / this.blockSize.x, this.resultDimensions.y / this.blockSize.y, this.blockSize.z);
    }
}
