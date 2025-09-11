using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderUserTest : MonoBehaviour
{
    public ComputeShader usingComputeShader;
    public Vector2Int computeShaderResultDimensions = Vector2Int.one * 256;
    public RenderTexture resultRenderTexture;
    // Start is called before the first frame update
    void Start()
    {

        this.resultRenderTexture = new RenderTexture(computeShaderResultDimensions.x, computeShaderResultDimensions.y, 0);
        this.resultRenderTexture.enableRandomWrite = true;
        this.resultRenderTexture.Create();
        // ...
        this.UseComputeShader(this.usingComputeShader, this.resultRenderTexture);
    }

    // Update is called once per frame
    void Update()
    {
        // ...
    }
    public void UseComputeShader(ComputeShader computeShader, RenderTexture renderTexture){
        // ...
        int kernelIndex = computeShader.FindKernel("ComputeShaderTestable");
        int blockWidth = 32;
        int blockHeight = 16;

        // Set all the necessary buffers
        computeShader.SetInt("textureWidth", computeShaderResultDimensions.x);
        computeShader.SetInt("textureHeight", computeShaderResultDimensions.y);
        computeShader.SetTexture(kernelIndex, "Result", renderTexture);

        // Dispatch
        computeShader.Dispatch(kernelIndex, computeShaderResultDimensions.x / blockWidth, computeShaderResultDimensions.y / blockHeight, 1);
    }
}
