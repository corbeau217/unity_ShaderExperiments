using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tex3DExperiment : MonoBehaviour
{
    public ComputeShader tex3DGenerationShader;
    public ComputeShader tex2DUsageShader;
    public Vector3Int dimensions3D = Vector3Int.one * 256;
    // multiply the dimensions 3d by the squareroot of one of the dimensions
    //  since the 3rd axis ends up being each slice of the volume
    //  spread out in a square grid upon saving
    public Vector2Int dimensions3DAs2D = Vector2Int.one * 256 * 16;
    public Vector2Int dimensions2D = Vector2Int.one * 256;
    public Renderer rendererPreview;
    public string inputExperimentName = "tex3Dexperiment";
    public string outputExperimentName = "tex3DuserExperiment";
    public RenderTexture inputTexture;
    public RenderTexture outputTexture;
    public Vector3Int blockSize3D = new Vector3Int(8,8,8);
    public Vector3Int blockSize2D = new Vector3Int(16,16,1);
    [Range(0,255)]
    public int textureSliceToUse = 120;
    
    void Start(){
        if(this.rendererPreview == null) this.rendererPreview = GetComponent<Renderer>();
        this.GenerateRenderTextures();
        this.Perform3DComputeShader();
    }
    void Update(){
        this.Perform2DComputeShader();
        rendererPreview.material.mainTexture = this.outputTexture;
    }
    void OnDestroy(){
        inputTexture.Release();
        outputTexture.Release();
    }

    private void GenerateRenderTextures(){
        // width, height, depth
        this.inputTexture = new RenderTexture( dimensions3DAs2D.x, dimensions3DAs2D.y, 0 );
        this.inputTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        this.inputTexture.width = dimensions3D.x;
        this.inputTexture.height = dimensions3D.y;
        this.inputTexture.volumeDepth = dimensions3D.z;
        this.inputTexture.filterMode = FilterMode.Point;
        this.inputTexture.enableRandomWrite = true;
        this.inputTexture.Create();

        this.outputTexture = new RenderTexture(dimensions2D.x, dimensions2D.y, 0);
        this.outputTexture.enableRandomWrite = true;
        this.outputTexture.filterMode = FilterMode.Point;
        this.outputTexture.Create();
    }
    public void Perform3DComputeShader(){
        int kernel3DIndex = tex3DGenerationShader.FindKernel(inputExperimentName);
        tex3DGenerationShader.SetTexture(kernel3DIndex, "OutputTexture", this.inputTexture);
        tex3DGenerationShader.SetInt("textureWidth", this.dimensions3D.x);
        tex3DGenerationShader.SetInt("textureHeight", this.dimensions3D.y);
        tex3DGenerationShader.SetInt("textureDepth", this.dimensions3D.z);
        tex3DGenerationShader.Dispatch(kernel3DIndex, dimensions3D.x / blockSize3D.x, dimensions3D.y / blockSize3D.y, dimensions3D.z / blockSize3D.z);
    }
    public void Perform2DComputeShader(){
        int kernel2DIndex = tex2DUsageShader.FindKernel(outputExperimentName);
        tex2DUsageShader.SetTexture(kernel2DIndex, "InputTexture", this.inputTexture);
        tex2DUsageShader.SetTexture(kernel2DIndex, "OutputTexture", this.outputTexture);
        tex2DUsageShader.SetInt("textureSlice", this.textureSliceToUse);
        tex2DUsageShader.Dispatch(kernel2DIndex, dimensions2D.x / blockSize2D.x, dimensions2D.y / blockSize2D.y, blockSize2D.z);
    }

}
