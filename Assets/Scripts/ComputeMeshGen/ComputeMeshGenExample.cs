using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ComputeMeshGenExample : MonoBehaviour
{
    public ComputeShader computeMeshGenShader;
    public string computeMeshGenShaderName = "computeMeshGenExampleKernel";

    // needs to match compile time values used in the shader
    public Vector3Int blockSizeMeshGen = new Vector3Int(16,16,1);
    // can vary, but requires us to redo the shader, prefers multiples of block size
    public Vector3Int meshVertexCount = new Vector3Int(64,64,1);
    // determined before dispatch
    public Vector3Int dispatchCount = Vector3Int.one;

    public MeshFilter filterToDisplayMesh;
    public Mesh generatedMesh;


    public GraphicsBuffer meshTriangles;
    public GraphicsBuffer meshPositions;
    public bool preparedBuffers = false;
    
    void Start(){
        this.dispatchCount = new Vector3Int(
            meshVertexCount.x / blockSizeMeshGen.x,
            meshVertexCount.y / blockSizeMeshGen.y,
            meshVertexCount.z / blockSizeMeshGen.z
        );
        if(this.generatedMesh==null){
            this.generatedMesh = new Mesh();
            // make it internal temporary so you cant modify it
            this.generatedMesh.hideFlags = HideFlags.HideAndDontSave;
            // We want GraphicsBuffer access as Raw (ByteAddress) buffers.
            //  trying to use structured, bricked our ship, probs graphics api crying
            this.generatedMesh.indexBufferTarget |= GraphicsBuffer.Target.Raw;
            this.generatedMesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;
        }
        this.generatedMesh.name = "Generated Mesh";

        int meshGenKernelIndex = computeMeshGenShader.FindKernel(computeMeshGenShaderName);
        Debug.Log("'"+(this.computeMeshGenShaderName)+"' is supported?: "+ computeMeshGenShader.IsSupported(meshGenKernelIndex).ToString());
    }
    void Update(){
        if(!this.preparedBuffers){
            this.PrepareBuffers();
            this.VerifyShader();
            this.GenerateMeshData();
            this.preparedBuffers = true;
        }
        this.ProvideMeshData();
    }
    // call destroy when disabled
    void OnDisable() => OnDestroy();
    void OnDestroy(){
        meshTriangles?.Dispose();
        meshTriangles = null;
        meshPositions?.Dispose();
        meshPositions = null;

        if (generatedMesh != null){
            if (Application.isPlaying)
                Object.Destroy(generatedMesh);
            else
                Object.DestroyImmediate(generatedMesh);
        }

        this.generatedMesh = null;
    }

    private void PrepareBuffers(){

        // // throw away the old
        // meshTriangles?.Dispose();
        // meshPositions?.Dispose();

        // multiply our axises but throw away one row of vertices as being our first
        //  then multiply by 2 because 2 triangles per quad
        int totalTriangles = ((this.meshVertexCount.x-1)*(this.meshVertexCount.y))*2;
        //  then multiply by 3 because we want to have 3 points per triangle?
        int totalTriangleBindings = totalTriangles*3;
        int totalVertices = this.meshVertexCount.x * this.meshVertexCount.y;
        // trick, hoodwink, pull the wool over the eyes of the mesh so it makes our buffer bigger

        // fist make the descriptors
        VertexAttributeDescriptor vertPositionDescriptor = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3);
        VertexAttributeDescriptor vertNormalDescriptor = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3);
        
        // then give them to our mesh
        this.generatedMesh.SetVertexBufferParams( totalVertices, vertPositionDescriptor, vertNormalDescriptor );
        this.generatedMesh.SetIndexBufferParams( totalTriangleBindings, IndexFormat.UInt32 );

        // now the submesh stuff
        SubMeshDescriptor genSubMeshDescriptor = new SubMeshDescriptor(0, totalVertices, MeshTopology.Triangles);
        genSubMeshDescriptor.bounds = new Bounds( Vector3.one*0.5f, Vector3.one );
        genSubMeshDescriptor.firstVertex = 0;
        genSubMeshDescriptor.indexCount = totalTriangleBindings;
        // then setup the submesh stuff
        this.generatedMesh.SetSubMesh(0, genSubMeshDescriptor, MeshUpdateFlags.DontRecalculateBounds);

        // now heist our buffers for messing around in the compute shader
        meshPositions = this.generatedMesh.GetVertexBuffer(0);
        meshTriangles = this.generatedMesh.GetIndexBuffer();

        // note: remember to check "Read/Write" on the mesh asset to get access to the geometry data
        // meshTriangles = new GraphicsBuffer(GraphicsBuffer.Target.Structured, totalTriangleBindings, sizeof(int));
        // meshPositions = new GraphicsBuffer(GraphicsBuffer.Target.Structured, totalVertices, 3 * sizeof(float));
    }
    private void VerifyShader(){
        int meshGenKernelIndex = computeMeshGenShader.FindKernel(computeMeshGenShaderName);
        Debug.Log("mesh readable? "+ (this.generatedMesh.isReadable).ToString());
        Debug.Log("'"+(this.computeMeshGenShaderName)+"' is supported?: "+ computeMeshGenShader.IsSupported(meshGenKernelIndex).ToString());
        Debug.Log("meshVertices valid? "+ (this.meshPositions.IsValid()).ToString());
        Debug.Log("meshVertices stride: "+ (this.meshPositions.stride).ToString());
        Debug.Log("meshVertices count: "+ (this.meshPositions.count).ToString());
        Debug.Log("meshVertices size: "+ (this.meshPositions.stride*this.meshPositions.count).ToString());
        Debug.Log("meshTriangles valid? "+ (this.meshTriangles.IsValid()).ToString());
        Debug.Log("meshTriangles stride: "+ (this.meshTriangles.stride).ToString());
        Debug.Log("meshTriangles count: "+ (this.meshTriangles.count).ToString());
        Debug.Log("meshTriangles size: "+ (this.meshTriangles.stride*this.meshTriangles.count).ToString());
    }
    // aaa GetVertexBuffer
    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mesh.GetVertexBuffer.html
    public void GenerateMeshData(){
        int meshGenKernelIndex = computeMeshGenShader.FindKernel(computeMeshGenShaderName);
        // Debug.Log("'"+(this.computeMeshGenShaderName)+"' is supported?: "+ computeMeshGenShader.IsSupported(meshGenKernelIndex).ToString());
        computeMeshGenShader.SetBuffer(meshGenKernelIndex, "meshVertices", this.meshPositions);
        computeMeshGenShader.SetBuffer(meshGenKernelIndex, "meshTriangleBindings", this.meshTriangles);
        computeMeshGenShader.SetInt("vertexCountX", this.meshVertexCount.x);
        computeMeshGenShader.SetInt("vertexCountY", this.meshVertexCount.y);
        // computeMeshGenShader.SetInt("totalVertices", ((this.meshVertexCount.x)*(this.meshVertexCount.y)));
        computeMeshGenShader.Dispatch(meshGenKernelIndex, this.dispatchCount.x, this.dispatchCount.y, 1);

        // add in our shape
        // this.generatedMesh.SetVertices(this.meshVertices);
        // this.generatedMesh.SetTriangles(this.meshBindings,0,false);

        // // recalculate what we're lazy about
        // this.generatedMesh.RecalculateNormals();
        // this.generatedMesh.RecalculateBounds();
    }
    // https://github.com/Unity-Technologies/UnityCsReference/blob/2021.3/Runtime/Export/Graphics/Mesh.cs
    public void ProvideMeshData(){

        // Mesh filter component lazy initialization and update
        if (this.filterToDisplayMesh == null)
        {
            // this.filterToDisplayMesh = gameObject.AddComponent<MeshFilter>();

            this.filterToDisplayMesh = GetComponent<MeshFilter>();
            // this.filterToDisplayMesh.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }

        this.filterToDisplayMesh.mesh = this.generatedMesh;
    }

}
