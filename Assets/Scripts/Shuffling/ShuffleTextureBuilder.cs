using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleTextureBuilder : MonoBehaviour
{
    public Renderer previewRenderer;

    public ComputeShader shuffleComputeShader;

    public RenderTexture PreviewSliceTexture;
    public RenderTexture SwapDataTexture;

    public Vector3Int volumeDimensions = new Vector3Int(256,256,4);
    public Vector2Int flattenedVolumeDimensions = Vector2Int.one * 512;
    
    // less than half the width, but more than 1 or we get weird results
    public Vector2Int minimumCycleSize = Vector2Int.one * 8;

    public Vector2Int cycleRectanglePosition = new Vector2Int(0,0);
    public Vector2Int cycleRectangleSize = new Vector2Int(10,10);
    public Vector2Int cycleAxisOffsets = Vector2Int.one * 2;
    public int currentSliceIndex = 0;

    public bool keepShuffling = true;
    public bool shuffleOnce = false;

    private int[] volumeDimensionsArray = new int[3];
    private int[] cycleRectangleArray = new int[4];
    private int[] cycleAxisOffsetsArray = new int[2];

    void Start() {
        this.PrepareTextures();
        this.DispatchSetupShader();
    }
    void Update() {
        if(keepShuffling||shuffleOnce){
            this.RollNewCycleRectangle();
            this.DispatchShuffleShader();
            // cycle our slice index
            this.currentSliceIndex = (this.currentSliceIndex + 1) % this.volumeDimensions.z;
            shuffleOnce = false;
        }
        // when have renderer for preview, update it to have the preview texture
        if(this.previewRenderer!=null){
            this.previewRenderer.material.mainTexture = PreviewSliceTexture;
        }
    }
    void OnDisable() => OnDestroy();
    void OnDestroy() {
        this.PreviewSliceTexture?.Release();
        this.PreviewSliceTexture = null;
        this.SwapDataTexture?.Release();
        this.SwapDataTexture = null;
    }
    private void PrepareTextures(){
        this.PreviewSliceTexture = new RenderTexture(volumeDimensions.x, volumeDimensions.y, 0);
        this.PreviewSliceTexture.filterMode = FilterMode.Point;
        this.PreviewSliceTexture.enableRandomWrite = true;
        this.PreviewSliceTexture.Create();

        this.SwapDataTexture = new RenderTexture(flattenedVolumeDimensions.x, flattenedVolumeDimensions.y, 0);
        this.SwapDataTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        this.SwapDataTexture.width = volumeDimensions.x;
        this.SwapDataTexture.height = volumeDimensions.y;
        this.SwapDataTexture.volumeDepth = volumeDimensions.z;
        this.SwapDataTexture.filterMode = FilterMode.Point;
        this.SwapDataTexture.enableRandomWrite = true;
        this.SwapDataTexture.Create();
    }

    private void DispatchSetupShader(){
        int blockWidth = 16;
        int blockHeight = 16;

        volumeDimensionsArray[0] = volumeDimensions.x;
        volumeDimensionsArray[1] = volumeDimensions.y;
        volumeDimensionsArray[2] = volumeDimensions.z;

        int fillUVKernel = shuffleComputeShader.FindKernel("FillWithUVColoursKernel");

        shuffleComputeShader.SetTexture(fillUVKernel, "PreviewSliceTexture", PreviewSliceTexture);
        shuffleComputeShader.SetTexture(fillUVKernel, "SwapDataTexture", SwapDataTexture);

        shuffleComputeShader.SetInts("volumeDimensions", volumeDimensionsArray);

        // Dispatch
        for (int i = 0; i < volumeDimensions.z; i++)
        {
            shuffleComputeShader.SetInt("currentSliceIndex", i);
            shuffleComputeShader.Dispatch(fillUVKernel, volumeDimensions.x / blockWidth, volumeDimensions.y / blockHeight, 1);
        }
    }
    private void DispatchShuffleShader(){
        int blockWidth = 16;
        int blockHeight = 16;

        cycleRectangleArray[0] = cycleRectanglePosition.x;
        cycleRectangleArray[1] = cycleRectanglePosition.y;
        cycleRectangleArray[2] = cycleRectangleSize.x;
        cycleRectangleArray[3] = cycleRectangleSize.y;

        cycleAxisOffsetsArray[0] = cycleAxisOffsets.x;
        cycleAxisOffsetsArray[1] = cycleAxisOffsets.y;

        int kernelIndex = shuffleComputeShader.FindKernel("CyclicalShuffleKernel");

        shuffleComputeShader.SetTexture(kernelIndex, "PreviewSliceTexture", PreviewSliceTexture);
        shuffleComputeShader.SetTexture(kernelIndex, "SwapDataTexture", SwapDataTexture);

        shuffleComputeShader.SetInts("volumeDimensions", volumeDimensionsArray);
        shuffleComputeShader.SetInts("cycleRectangle", cycleRectangleArray);
        shuffleComputeShader.SetInts("cycleAxisOffsets", cycleAxisOffsetsArray);

        shuffleComputeShader.SetInt("currentSliceIndex", currentSliceIndex);

        // Dispatch
        shuffleComputeShader.Dispatch(kernelIndex, volumeDimensions.x / blockWidth, volumeDimensions.y / blockHeight, 1);
    }

    // randomise the cycle rectangle location and size
    private void RollNewCycleRectangle(){
        // x
        cycleRectanglePosition.x = Random.Range(0,volumeDimensions.x-8);
        // y
        cycleRectanglePosition.y = Random.Range(0,volumeDimensions.y-8);
        // width
        cycleRectangleSize.x = 8;
        // height
        cycleRectangleSize.y = 8;

        cycleAxisOffsets.x = Random.Range(0,7);
        cycleAxisOffsets.y = Random.Range(0,7);
    }
}
