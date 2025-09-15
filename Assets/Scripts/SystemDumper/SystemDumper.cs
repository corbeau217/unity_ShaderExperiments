using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemDumper : MonoBehaviour
{
    public Text leftColumn;
    public Text rightColumn;
    public List<Tuple<string,string>> dumpLines;
    public Color valueColor = Color.white;
    public Color trueColour = Color.green;
    public Color falseColour = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        this.dumpLines = this.GetSystemInfoDumpLines();
        this.WriteLinesToTextBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WriteLinesToTextBox(){
        this.leftColumn.text = "";
        this.rightColumn.text = "";
        foreach(Tuple<string,string> currLine in this.dumpLines){
            this.leftColumn.text = this.leftColumn.text + currLine.Item1 + "\n";

            string rightColumnFormattedText = "";
            string colourCode = ColorUtility.ToHtmlStringRGB(this.valueColor);
            // if(currLine.Item2.Contains("true")){
            if(currLine.Item2 == "True"){
                colourCode = ColorUtility.ToHtmlStringRGB(this.trueColour);
            }
            // else if(currLine.Item2.Contains("false")){
            else if(currLine.Item2 == "False"){
                colourCode = ColorUtility.ToHtmlStringRGB(this.falseColour);
            }
            rightColumnFormattedText = "<color=\"#"+colourCode+"\">"+currLine.Item2+"</color>";
            this.rightColumn.text = this.rightColumn.text + rightColumnFormattedText + "\n";
        }
    }

    // https://docs.unity3d.com/2021.3/Documentation/ScriptReference/SystemInfo.html 
    public List<Tuple<string,string>> GetSystemInfoDumpLines(){
        List<Tuple<string,string>> systemInfoDump = new List<Tuple<string,string>>();
        systemInfoDump.Add( new Tuple<string,string>( "batteryLevel", (SystemInfo.batteryLevel).ToString() ) ); // The current battery level (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "batteryStatus", (SystemInfo.batteryStatus).ToString() ) ); // Returns the current status of the device's battery (Read Only).
        // systemInfoDump.Add( new Tuple<string,string>( "computeSubGroupSize", (SystemInfo.computeSubGroupSize).ToString() ) ); // Size of the compute thread group that supports efficient memory sharing on the GPU (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "constantBufferOffsetAlignment", (SystemInfo.constantBufferOffsetAlignment).ToString() ) ); // Minimum buffer offset (in bytes) when binding a constant buffer using Shader.SetConstantBuffer or Material.SetConstantBuffer.
        systemInfoDump.Add( new Tuple<string,string>( "copyTextureSupport", (SystemInfo.copyTextureSupport).ToString() ) ); // Support for various Graphics.CopyTexture cases (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "deviceModel", (SystemInfo.deviceModel).ToString() ) ); // The model of the device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "deviceName", (SystemInfo.deviceName).ToString() ) ); // The user defined name of the device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "deviceType", (SystemInfo.deviceType).ToString() ) ); // Returns the kind of device the application is running on (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "deviceUniqueIdentifier", (SystemInfo.deviceUniqueIdentifier).ToString() ) ); // A unique device identifier. It's guaranteed to be unique for every device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsDeviceID", (SystemInfo.graphicsDeviceID).ToString() ) ); // The identifier code of the graphics device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsDeviceName", (SystemInfo.graphicsDeviceName).ToString() ) ); // The name of the graphics device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsDeviceType", (SystemInfo.graphicsDeviceType).ToString() ) ); // The graphics API type used by the graphics device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsDeviceVendor", (SystemInfo.graphicsDeviceVendor).ToString() ) ); // The vendor of the graphics device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsDeviceVendorID", (SystemInfo.graphicsDeviceVendorID).ToString() ) ); // The identifier code of the graphics device vendor (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsDeviceVersion", (SystemInfo.graphicsDeviceVersion).ToString() ) ); // The graphics API type and driver version used by the graphics device (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsMemorySize", (SystemInfo.graphicsMemorySize).ToString() ) ); // Amount of video memory present (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsMultiThreaded", (SystemInfo.graphicsMultiThreaded).ToString() ) ); // Is graphics device using multi-threaded rendering (Read Only)?
        systemInfoDump.Add( new Tuple<string,string>( "graphicsShaderLevel", (SystemInfo.graphicsShaderLevel).ToString() ) ); // Graphics device shader capability level (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "graphicsUVStartsAtTop", (SystemInfo.graphicsUVStartsAtTop).ToString() ) ); // Returns true if the texture UV coordinate convention for this platform has Y starting at the top of the image.
        systemInfoDump.Add( new Tuple<string,string>( "hasDynamicUniformArrayIndexingInFragmentShaders", (SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders).ToString() ) ); // Returns true when the GPU has native support for indexing uniform arrays in fragment shaders without restrictions.
        systemInfoDump.Add( new Tuple<string,string>( "hasHiddenSurfaceRemovalOnGPU", (SystemInfo.hasHiddenSurfaceRemovalOnGPU).ToString() ) ); // True if the GPU supports hidden surface removal.
        systemInfoDump.Add( new Tuple<string,string>( "hasMipMaxLevel", (SystemInfo.hasMipMaxLevel).ToString() ) ); // Returns true if the GPU supports partial mipmap chains (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "hdrDisplaySupportFlags", (SystemInfo.hdrDisplaySupportFlags).ToString() ) ); // Returns a bitwise combination of HDRDisplaySupportFlags describing the support for HDR displays on the system.
        // systemInfoDump.Add( new Tuple<string,string>( "maxAnisotropyLevel", (SystemInfo.maxAnisotropyLevel).ToString() ) ); // Returns the maximum anisotropic level for anisotropic filtering that is supported on the device.
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeBufferInputsCompute", (SystemInfo.maxComputeBufferInputsCompute).ToString() ) ); // Determines how many compute buffers Unity supports simultaneously in a compute shader for reading. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeBufferInputsDomain", (SystemInfo.maxComputeBufferInputsDomain).ToString() ) ); // Determines how many compute buffers Unity supports simultaneously in a domain shader for reading. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeBufferInputsFragment", (SystemInfo.maxComputeBufferInputsFragment).ToString() ) ); // Determines how many compute buffers Unity supports simultaneously in a fragment shader for reading. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeBufferInputsGeometry", (SystemInfo.maxComputeBufferInputsGeometry).ToString() ) ); // Determines how many compute buffers Unity supports simultaneously in a geometry shader for reading. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeBufferInputsHull", (SystemInfo.maxComputeBufferInputsHull).ToString() ) ); // Determines how many compute buffers Unity supports simultaneously in a hull shader for reading. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeBufferInputsVertex", (SystemInfo.maxComputeBufferInputsVertex).ToString() ) ); // Determines how many compute buffers Unity supports simultaneously in a vertex shader for reading. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeWorkGroupSize", (SystemInfo.maxComputeWorkGroupSize).ToString() ) ); // The largest total number of invocations in a single local work group that can be dispatched to a compute shader (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeWorkGroupSizeX", (SystemInfo.maxComputeWorkGroupSizeX).ToString() ) ); // The maximum number of work groups that a compute shader can use in X dimension (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeWorkGroupSizeY", (SystemInfo.maxComputeWorkGroupSizeY).ToString() ) ); // The maximum number of work groups that a compute shader can use in Y dimension (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "maxComputeWorkGroupSizeZ", (SystemInfo.maxComputeWorkGroupSizeZ).ToString() ) ); // The maximum number of work groups that a compute shader can use in Z dimension (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "maxCubemapSize", (SystemInfo.maxCubemapSize).ToString() ) ); // Maximum cubemap texture size in pixels (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "maxGraphicsBufferSize", (SystemInfo.maxGraphicsBufferSize).ToString() ) ); // The maximum size of a graphics buffer (GraphicsBuffer, ComputeBuffer, vertex/index buffer, etc.) in bytes (Read Only).
        // systemInfoDump.Add( new Tuple<string,string>( "maxTexture3DSize", (SystemInfo.maxTexture3DSize).ToString() ) ); // Maximum 3D texture size in pixels (Read Only).
        // systemInfoDump.Add( new Tuple<string,string>( "maxTextureArraySlices", (SystemInfo.maxTextureArraySlices).ToString() ) ); // Maximum number of slices in a Texture array (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "maxTextureSize", (SystemInfo.maxTextureSize).ToString() ) ); // Maximum texture size in pixels (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "npotSupport", (SystemInfo.npotSupport).ToString() ) ); // What NPOT (non-power of two size) texture support does the GPU provide? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "operatingSystem", (SystemInfo.operatingSystem).ToString() ) ); // Operating system name with version (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "operatingSystemFamily", (SystemInfo.operatingSystemFamily).ToString() ) ); // Returns the operating system family the game is running on (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "processorCount", (SystemInfo.processorCount).ToString() ) ); // Number of processors present (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "processorFrequency", (SystemInfo.processorFrequency).ToString() ) ); // The processor frequency of the device in MHz (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "processorType", (SystemInfo.processorType).ToString() ) ); // Processor name (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "renderingThreadingMode", (SystemInfo.renderingThreadingMode).ToString() ) ); // Application's actual rendering threading mode (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "supportedRandomWriteTargetCount", (SystemInfo.supportedRandomWriteTargetCount).ToString() ) ); // The maximum number of random write targets (UAV) that Unity supports simultaneously. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportedRenderTargetCount", (SystemInfo.supportedRenderTargetCount).ToString() ) ); // How many simultaneous render targets (MRTs) are supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supports2DArrayTextures", (SystemInfo.supports2DArrayTextures).ToString() ) ); // Are 2D Array textures supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supports32bitsIndexBuffer", (SystemInfo.supports32bitsIndexBuffer).ToString() ) ); // Are 32-bit index buffers supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supports3DRenderTextures", (SystemInfo.supports3DRenderTextures).ToString() ) ); // Are 3D (volume) RenderTextures supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supports3DTextures", (SystemInfo.supports3DTextures).ToString() ) ); // Are 3D (volume) textures supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsAccelerometer", (SystemInfo.supportsAccelerometer).ToString() ) ); // Is an accelerometer available on the device?
        // systemInfoDump.Add( new Tuple<string,string>( "supportsAnisotropicFilter", (SystemInfo.supportsAnisotropicFilter).ToString() ) ); // Returns true when anisotropic filtering is supported on the device.
        systemInfoDump.Add( new Tuple<string,string>( "supportsAsyncCompute", (SystemInfo.supportsAsyncCompute).ToString() ) ); // Returns true when the platform supports asynchronous compute queues and false if otherwise.
        systemInfoDump.Add( new Tuple<string,string>( "supportsAsyncGPUReadback", (SystemInfo.supportsAsyncGPUReadback).ToString() ) ); // Returns true if asynchronous readback of GPU data is available for this device and false otherwise.
        systemInfoDump.Add( new Tuple<string,string>( "supportsAudio", (SystemInfo.supportsAudio).ToString() ) ); // Is there an Audio device available for playback? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsCompressed3DTextures", (SystemInfo.supportsCompressed3DTextures).ToString() ) ); // Are compressed formats for 3D (volume) textures supported? (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "supportsComputeShaders", (SystemInfo.supportsComputeShaders).ToString() ) ); // Are compute shaders supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsConservativeRaster", (SystemInfo.supportsConservativeRaster).ToString() ) ); // Is conservative rasterization supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsCubemapArrayTextures", (SystemInfo.supportsCubemapArrayTextures).ToString() ) ); // Are Cubemap Array textures supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsGeometryShaders", (SystemInfo.supportsGeometryShaders).ToString() ) ); // Are geometry shaders supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsGpuRecorder", (SystemInfo.supportsGpuRecorder).ToString() ) ); // Specifies whether the current platform supports the GPU Recorder or not. (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "supportsGraphicsFence", (SystemInfo.supportsGraphicsFence).ToString() ) ); // true if the platform supports GraphicsFences, otherwise false.
        systemInfoDump.Add( new Tuple<string,string>( "supportsGyroscope", (SystemInfo.supportsGyroscope).ToString() ) ); // Is a gyroscope available on the device?
        systemInfoDump.Add( new Tuple<string,string>( "supportsHardwareQuadTopology", (SystemInfo.supportsHardwareQuadTopology).ToString() ) ); // Does the hardware support quad topology? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsInstancing", (SystemInfo.supportsInstancing).ToString() ) ); // Is GPU draw call instancing supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsLocationService", (SystemInfo.supportsLocationService).ToString() ) ); // Is the device capable of reporting its location?
        systemInfoDump.Add( new Tuple<string,string>( "supportsMipStreaming", (SystemInfo.supportsMipStreaming).ToString() ) ); // Is streaming of texture mip maps supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsMotionVectors", (SystemInfo.supportsMotionVectors).ToString() ) ); // Whether motion vectors are supported on this platform.
        systemInfoDump.Add( new Tuple<string,string>( "supportsMultisampleAutoResolve", (SystemInfo.supportsMultisampleAutoResolve).ToString() ) ); // Returns true if multisampled textures are resolved automatically
        systemInfoDump.Add( new Tuple<string,string>( "supportsMultisampled2DArrayTextures", (SystemInfo.supportsMultisampled2DArrayTextures).ToString() ) ); // Boolean that indicates whether multisampled texture arrays are supported (true if supported, false if not supported).
        systemInfoDump.Add( new Tuple<string,string>( "supportsMultisampledTextures", (SystemInfo.supportsMultisampledTextures).ToString() ) ); // Are multisampled textures supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsMultisampleResolveDepth", (SystemInfo.supportsMultisampleResolveDepth).ToString() ) ); // Returns true if the platform supports multisample resolve of depth textures.
        systemInfoDump.Add( new Tuple<string,string>( "supportsMultiview", (SystemInfo.supportsMultiview).ToString() ) ); // Boolean that indicates whether Multiview is supported (true if supported, false if not supported). (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsRawShadowDepthSampling", (SystemInfo.supportsRawShadowDepthSampling).ToString() ) ); // Is sampling raw depth from shadowmaps supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsRayTracing", (SystemInfo.supportsRayTracing).ToString() ) ); // Checks if ray tracing is supported by the current configuration.
        systemInfoDump.Add( new Tuple<string,string>( "supportsRenderTargetArrayIndexFromVertexShader", (SystemInfo.supportsRenderTargetArrayIndexFromVertexShader).ToString() ) ); // Boolean that indicates if SV_RenderTargetArrayIndex can be used in a vertex shader (true if it can be used, false if not).
        systemInfoDump.Add( new Tuple<string,string>( "supportsSeparatedRenderTargetsBlend", (SystemInfo.supportsSeparatedRenderTargetsBlend).ToString() ) ); // Returns true when the platform supports different blend modes when rendering to multiple render targets, or false otherwise.
        systemInfoDump.Add( new Tuple<string,string>( "supportsSetConstantBuffer", (SystemInfo.supportsSetConstantBuffer).ToString() ) ); // Does the current renderer support binding constant buffers directly? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsShadows", (SystemInfo.supportsShadows).ToString() ) ); // Are built-in shadows supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsSparseTextures", (SystemInfo.supportsSparseTextures).ToString() ) ); // Are sparse textures supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsStoreAndResolveAction", (SystemInfo.supportsStoreAndResolveAction).ToString() ) ); // This property is true if the graphics API of the target build platform takes RenderBufferStoreAction.StoreAndResolve into account, false if otherwise.
        systemInfoDump.Add( new Tuple<string,string>( "supportsTessellationShaders", (SystemInfo.supportsTessellationShaders).ToString() ) ); // Are tessellation shaders supported? (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsTextureWrapMirrorOnce", (SystemInfo.supportsTextureWrapMirrorOnce).ToString() ) ); // Returns true if the 'Mirror Once' texture wrap mode is supported. (Read Only)
        systemInfoDump.Add( new Tuple<string,string>( "supportsVibration", (SystemInfo.supportsVibration).ToString() ) ); // Is the device capable of providing the user haptic feedback by vibration?
        systemInfoDump.Add( new Tuple<string,string>( "systemMemorySize", (SystemInfo.systemMemorySize).ToString() ) ); // Amount of system memory present (Read Only).
        systemInfoDump.Add( new Tuple<string,string>( "unsupportedIdentifier", (SystemInfo.unsupportedIdentifier).ToString() ) ); // Value returned by SystemInfo string properties which are not supported on the current platform.
        systemInfoDump.Add( new Tuple<string,string>( "usesLoadStoreActions", (SystemInfo.usesLoadStoreActions).ToString() ) ); // True if the Graphics API takes RenderBufferLoadAction and RenderBufferStoreAction into account, false if otherwise.
        systemInfoDump.Add( new Tuple<string,string>( "usesReversedZBuffer", (SystemInfo.usesReversedZBuffer).ToString() ) ); // This property is true if the current platform uses a reversed depth buffer (where values range from 1 at the near plane and 0 at far plane), and false if the depth buffer is normal (0 is near, 1 is far). (Read Only)

        // ...
        systemInfoDump.Add( new Tuple<string,string>( "RenderTextureFormat.Depth", (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth).ToString()) ) );
        systemInfoDump.Add( new Tuple<string,string>( "RenderTextureFormat.Shadowmap", (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Shadowmap).ToString()) ) );
        systemInfoDump.Add( new Tuple<string,string>( "Float32, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float32,1)).ToString() ) ); // 32-bit float number.
        systemInfoDump.Add( new Tuple<string,string>( "Float32, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float32,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "Float32, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float32,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "Float32, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float32,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "Float16, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float16,1)).ToString() ) ); // 16-bit float number.
        systemInfoDump.Add( new Tuple<string,string>( "Float16, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float16,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "Float16, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float16,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "Float16, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.Float16,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UNorm8, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm8,1)).ToString() ) ); // 8-bit unsigned normalized number.
        systemInfoDump.Add( new Tuple<string,string>( "UNorm8, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm8,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UNorm8, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm8,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UNorm8, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm8,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SNorm8, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm8,1)).ToString() ) ); // 8-bit signed normalized number.
        systemInfoDump.Add( new Tuple<string,string>( "SNorm8, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm8,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SNorm8, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm8,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SNorm8, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm8,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UNorm16, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm16,1)).ToString() ) ); // 16-bit unsigned normalized number.
        systemInfoDump.Add( new Tuple<string,string>( "UNorm16, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm16,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UNorm16, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm16,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UNorm16, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UNorm16,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SNorm16, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm16,1)).ToString() ) ); // 16-bit signed normalized number.
        systemInfoDump.Add( new Tuple<string,string>( "SNorm16, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm16,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SNorm16, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm16,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SNorm16, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SNorm16,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt8, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt8,1)).ToString() ) ); // 8-bit unsigned integer.
        systemInfoDump.Add( new Tuple<string,string>( "UInt8, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt8,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt8, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt8,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt8, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt8,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt8, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt8,1)).ToString() ) ); // 8-bit signed integer.
        systemInfoDump.Add( new Tuple<string,string>( "SInt8, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt8,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt8, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt8,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt8, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt8,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt16, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt16,1)).ToString() ) ); // 16-bit unsigned integer.
        systemInfoDump.Add( new Tuple<string,string>( "UInt16, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt16,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt16, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt16,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt16, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt16,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt16, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt16,1)).ToString() ) ); // 16-bit signed integer.
        systemInfoDump.Add( new Tuple<string,string>( "SInt16, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt16,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt16, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt16,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt16, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt16,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt32, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt32,1)).ToString() ) ); // 32-bit unsigned integer.
        systemInfoDump.Add( new Tuple<string,string>( "UInt32, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt32,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt32, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt32,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "UInt32, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.UInt32,4)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt32, 1", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt32,1)).ToString() ) ); // 32-bit signed integer.
        systemInfoDump.Add( new Tuple<string,string>( "SInt32, 2", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt32,2)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt32, 3", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt32,3)).ToString() ) );
        systemInfoDump.Add( new Tuple<string,string>( "SInt32, 4", (SystemInfo.SupportsVertexAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat.SInt32,4)).ToString() ) );
        // RenderTextureFormat.Depth


        return systemInfoDump;
    }
}
