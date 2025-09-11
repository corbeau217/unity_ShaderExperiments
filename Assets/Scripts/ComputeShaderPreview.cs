using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderPreview : MonoBehaviour
{
    public ComputeShader computeShader;
    public Vector2Int outputDimensions = Vector2Int.one * 256;
    public Renderer rendererPreview;
    public string computeShaderKernel = "ComputeShaderTestable";
    protected RenderTexture outputTexture;
    protected Vector3Int blockSize = new Vector3Int(16,16,1);
    public bool oneShot = true;
    
    void Start(){
        this.GenerateRenderTexture();
        if(oneShot) this.PerformComputeShader();
    }
    void Update(){
        if(!oneShot) this.PerformComputeShader();
        rendererPreview.material.mainTexture = this.outputTexture;
    }
    void OnDestroy(){
        outputTexture.Release();
    }

    private void GenerateRenderTexture(){
        this.outputTexture = new RenderTexture(outputDimensions.x, outputDimensions.y, 0);
        this.outputTexture.enableRandomWrite = true;
        this.outputTexture.Create();
    }

    public virtual void AssignBuffers(int kernelIndex){
        // Set all the necessary buffers
        computeShader.SetTexture(kernelIndex, "OutputTexture", outputTexture);
    }
    public void PerformComputeShader(){
        int kernelIndex = computeShader.FindKernel(computeShaderKernel);
        this.AssignBuffers(kernelIndex);
        computeShader.Dispatch(kernelIndex, outputDimensions.x / blockSize.x, outputDimensions.y / blockSize.y, blockSize.z);
    }

    public RenderTexture GetOutputTexture(){
        return this.outputTexture;
    }

}
