using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTPreview : ComputeShaderPreview
{
    public Texture2D inputTexture;
    public ComputeShaderPreview uvShaderManager;
    public ComputeShaderPreview maskShaderManager;
    override public void AssignBuffers(int kernelIndex){
        // Set all the necessary buffers
        computeShader.SetTexture(kernelIndex, "InputTexture", inputTexture);
        computeShader.SetTexture(kernelIndex, "ChannelMaskingTexture", maskShaderManager.GetOutputTexture());
        computeShader.SetTexture(kernelIndex, "UVMappingTexture", uvShaderManager.GetOutputTexture());
        computeShader.SetTexture(kernelIndex, "OutputTexture", outputTexture);
        computeShader.SetInt("textureWidth", outputDimensions.x);
        computeShader.SetInt("textureHeight", outputDimensions.y);
    }
}
