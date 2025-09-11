using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVMappingPreview : ComputeShaderPreview
{

    override public void AssignBuffers(int kernelIndex){
        // Set all the necessary buffers
        computeShader.SetTexture(kernelIndex, "UVMappingTexture", outputTexture);
        computeShader.SetInt("textureWidth", outputDimensions.x);
        computeShader.SetInt("textureHeight", outputDimensions.y);
    }
}
