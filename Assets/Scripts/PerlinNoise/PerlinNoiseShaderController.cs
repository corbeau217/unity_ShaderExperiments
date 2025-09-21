using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseShaderController : MonoBehaviour
{
    public ComputeShader perlinNoiseShader;
    
    public RenderTexture inputNoiseTexture;
    public RenderTexture outputPerlinTexture;
    
    public Vector2Int inputNoiseResolution = Vector2Int.one * 256;
    public Vector2Int perlinOutputResolution = Vector2Int.one * 256;
    
    [Tooltip("[optional] input noise result renderer")]
    public Renderer inputNoisePreviewRenderer;
    [Tooltip("[optional] perlin noise result renderer")]
    public Renderer perlinViewRenderer;

    [Tooltip("the size of our compute shader blocks for the input noise generator")]
    public Vector3Int inputNoiseComputeBlockSize = new Vector3Int(16,16,1);
    [Tooltip("the size of our compute shader blocks for the perlin noise generator")]
    public Vector3Int perlinComputeBlockSize = new Vector3Int(16,16,1);

    [Tooltip("the size in pixels of each perlin noise octave (maximum of 4 octaves)")]
    public List<Vector2Int> octaveCellSizes = new List<Vector2Int>();
    [Tooltip("where in the noise space our cells start (maximum of 4 octaves)")]
    public List<Vector2Int> octaveCellOffsets = new List<Vector2Int>();
    [Tooltip("how much the octave effects the resulting noise (maximum of 4 octaves)")]
    public List<float> octaveInfluences = new List<float>();
    // this is hard coded by what is in the shader
    private int maximumOctaveCount = 4;
    // show how many layers were noticed
    public int octaveHasDataCount = 0;
    
    
    void Start(){
        this.GenerateRenderTexture();

        this.PerformInputNoiseShader();
        this.PerformPerlinShader();
    }
    void Update(){
        SetRenderersMainTextureTo(inputNoisePreviewRenderer, inputNoiseTexture);
        SetRenderersMainTextureTo(perlinViewRenderer, outputPerlinTexture);
    }
    void OnDestroy(){
        inputNoiseTexture?.Release();
        inputNoiseTexture = null;

        outputPerlinTexture?.Release();
        outputPerlinTexture = null;
    }

    // swap out the main texture for the first material of a renderer
    private void SetRenderersMainTextureTo(Renderer rendererToChange, Texture newTextureToUse){
        // check:
        //  * the renderer exists
        //  * has a material applied
        //  * that our texture exists
        if( (rendererToChange!=null)&&(rendererToChange.material!=null)&&(newTextureToUse!=null) ){
            // swap out the texture to what we want
            rendererToChange.material.mainTexture = newTextureToUse;
        }
    }

    private void GenerateRenderTexture(){
        this.inputNoiseTexture = new RenderTexture(inputNoiseResolution.x, inputNoiseResolution.y, 0);
        this.inputNoiseTexture.filterMode = FilterMode.Point;
        this.inputNoiseTexture.enableRandomWrite = true;
        this.inputNoiseTexture.Create();

        this.outputPerlinTexture = new RenderTexture(perlinOutputResolution.x, perlinOutputResolution.y, 0);
        this.outputPerlinTexture.filterMode = FilterMode.Point;
        this.outputPerlinTexture.enableRandomWrite = true;
        this.outputPerlinTexture.Create();
    }

    public void PerformInputNoiseShader(){
        // ================================================================================================================

        int inputNoiseKernelIndex = perlinNoiseShader.FindKernel("InputNoiseKernel");

        int[] inputNoiseResolutionArray = new int[2];
        inputNoiseResolutionArray[0] = inputNoiseResolution.x;
        inputNoiseResolutionArray[1] = inputNoiseResolution.y;

        // figure out the dispatch counts
        //  we do this firs so that the dispatch line is cleaner
        //  this will be the number of blocks per axis
        Vector3Int dispatchCounts = new Vector3Int(
            inputNoiseResolution.x / inputNoiseComputeBlockSize.x,
            inputNoiseResolution.y / inputNoiseComputeBlockSize.y,
            inputNoiseComputeBlockSize.z
        );

        // ================================================================================================================

        perlinNoiseShader.SetTexture(inputNoiseKernelIndex, "inputNoiseTexture", inputNoiseTexture);
        perlinNoiseShader.SetInts("inputNoiseResolution", inputNoiseResolutionArray);

        // ================================================================================================================

        perlinNoiseShader.Dispatch(inputNoiseKernelIndex, dispatchCounts.x, dispatchCounts.y, dispatchCounts.z);

        // perlinNoiseShader.Dispatch(inputNoiseKernelIndex, inputNoiseResolution.x / inputNoiseComputeBlockSize.x, inputNoiseResolution.y / inputNoiseComputeBlockSize.y, inputNoiseComputeBlockSize.z);

        // ================================================================================================================
    }

    public void PerformPerlinShader(){
        // ================================================================================================================

        int perlinKernelIndex = perlinNoiseShader.FindKernel("PerlinNoiseKernel");

        int[] perlinOutputResolutionArray = new int[2];
        perlinOutputResolutionArray[0] = perlinOutputResolution.x;
        perlinOutputResolutionArray[1] = perlinOutputResolution.y;

        // figure out the dispatch counts
        //  we do this firs so that the dispatch line is cleaner
        //  this will be the number of blocks per axis
        Vector3Int dispatchCounts = new Vector3Int(
            perlinOutputResolution.x / perlinComputeBlockSize.x,
            perlinOutputResolution.y / perlinComputeBlockSize.y,
            perlinComputeBlockSize.z
        );
        octaveHasDataCount = Mathf.Min( maximumOctaveCount,
            Mathf.Max(
                octaveCellSizes.Count,
                Mathf.Max(
                    octaveCellOffsets.Count,
                    octaveInfluences.Count
                )
            )
        );

        int[] octaveCellSizesArray = new int[maximumOctaveCount*2];
        int[] octaveCellOffsetsArray = new int[maximumOctaveCount*2];
        float[] octaveInfluencesArray = new float[maximumOctaveCount];

        for(int i = 0; i < maximumOctaveCount; i++){
            // if(i < octaveCellSizes.Count){
                octaveCellSizesArray[i*2] = octaveCellSizes[i].x;
                octaveCellSizesArray[i*2+1] = octaveCellSizes[i].y;
            // }
            // if(i < octaveCellOffsets.Count){
                octaveCellOffsetsArray[i*2] = octaveCellOffsets[i].x;
                octaveCellOffsetsArray[i*2+1] = octaveCellOffsets[i].y;
            // }
            // if(i < octaveInfluences.Count){
                octaveInfluencesArray[i] = octaveInfluences[i];
            // }
        }

        // ================================================================================================================

        perlinNoiseShader.SetTexture(perlinKernelIndex, "inputNoiseTexture", inputNoiseTexture);
        perlinNoiseShader.SetTexture(perlinKernelIndex, "outputPerlinTexture", outputPerlinTexture);
        perlinNoiseShader.SetInts("perlinOutputResolution", perlinOutputResolutionArray);

        perlinNoiseShader.SetInts("octaveCellSizes", octaveCellSizesArray);
        perlinNoiseShader.SetInts("octaveCellOffsets", octaveCellOffsetsArray);
        perlinNoiseShader.SetFloats("octaveInfluences", octaveInfluencesArray);
        
        perlinNoiseShader.SetInt("octaveLayerUsageCount", octaveHasDataCount);

        // ================================================================================================================

        perlinNoiseShader.Dispatch(perlinKernelIndex, dispatchCounts.x, dispatchCounts.y, dispatchCounts.z);

        // perlinNoiseShader.Dispatch(perlinKernelIndex, outputResolution.x / perlinComputeBlockSize.x, outputResolution.y / perlinComputeBlockSize.y, perlinComputeBlockSize.z);

        // ================================================================================================================
    }
}
