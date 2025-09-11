using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderManager : MonoBehaviour
{
    public ComputeShader csToUse;
    public string shaderKernelName;
    public Vector2Int resultDimensions = Vector2Int.one * 256;
    public Vector2Int blockSize = new Vector2Int(32,16);
    public RenderTexture resultRenderTexture;
    public int randomSeed;
    public bool keepUpdating = false;
    private int lastUsedSeed = -1;

    void Start() {

        this.resultRenderTexture = new RenderTexture(this.resultDimensions.x, this.resultDimensions.y, 0);
        this.resultRenderTexture.enableRandomWrite = true;
        this.resultRenderTexture.Create();

        this.DispatchShader();
    }
    void Update(){
        if(keepUpdating && this.lastUsedSeed!=this.randomSeed) {
            this.DispatchShader();
        }
    }


    public void UseComputeShader( ComputeShader computeShader, RenderTexture renderTexture ){
        int kernelIndex = computeShader.FindKernel(this.shaderKernelName);

        // Set all the necessary buffers
        computeShader.SetInt("textureWidth", this.resultDimensions.x);
        computeShader.SetInt("textureHeight", this.resultDimensions.y);
        computeShader.SetInt("seed", this.randomSeed);
        computeShader.SetTexture(kernelIndex, "Result", renderTexture);

        // Dispatch
        computeShader.Dispatch(kernelIndex, this.resultDimensions.x / this.blockSize.x, this.resultDimensions.y / this.blockSize.y, 1);
    }
    public void DispatchShader(){
        this.lastUsedSeed = this.randomSeed;
        this.UseComputeShader(this.csToUse, this.resultRenderTexture);
    }
}
