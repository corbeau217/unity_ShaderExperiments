using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfoDumper : MonoBehaviour
{
    void Start() {
        this.DumpSystemData();
    }
    void Update(){}

    public void DumpSystemData(){
        Debug.Log("SystemInfo.graphicsShaderLevel: "+SystemInfo.graphicsShaderLevel);
        Debug.Log(
            "SystemInfo.supportsComputeShaders: "+SystemInfo.supportsComputeShaders+"\n"+
            "SystemInfo.supportsGeometryShaders: "+SystemInfo.supportsGeometryShaders
        );
    }
}
