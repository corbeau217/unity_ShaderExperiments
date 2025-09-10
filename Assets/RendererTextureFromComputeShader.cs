using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererTextureFromComputeShader : MonoBehaviour
{
    public Renderer rendererToSupply;
    public ComputeShaderManager csManager;
    
    void Start() {}
    void Update() {
        this.rendererToSupply.material.mainTexture = csManager.resultRenderTexture;
    }
}
