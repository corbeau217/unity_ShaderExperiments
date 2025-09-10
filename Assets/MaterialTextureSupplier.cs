using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTextureSupplier : MonoBehaviour
{
    public Renderer renderMaterialHolder;
    public ComputeShaderUserTest csUserTest;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.renderMaterialHolder.material.mainTexture = csUserTest.resultRenderTexture;
    }
}
