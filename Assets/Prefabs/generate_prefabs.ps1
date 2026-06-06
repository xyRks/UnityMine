# Procedural Prefab Generator for UnityMine
$prefabsDir = "Assets\Prefabs"

$prefabs = @(
    @{
        name = "ForestTree"
        guid = "ffffffff000000000000000000000001"
        meshGuid = "dddddddd000000000000000000000001"
        materials = @("cccccccc000000000000000000000001", "cccccccc000000000000000000000003")
        isCapsule = $true
        radius = 0.5
        height = 4.3
        centerY = 2.15
    },
    @{
        name = "MeadowTree"
        guid = "ffffffff000000000000000000000005"
        meshGuid = "dddddddd000000000000000000000002"
        materials = @("cccccccc000000000000000000000002", "cccccccc000000000000000000000004")
        isCapsule = $true
        radius = 1.1
        height = 4.0
        centerY = 2.0
    },
    @{
        name = "ForestStone"
        guid = "ffffffff000000000000000000000002"
        meshGuid = "dddddddd000000000000000000000003"
        materials = @("cccccccc000000000000000000000005")
        isCapsule = $false
        sizeX = 1.6; sizeY = 1.2; sizeZ = 1.6
        centerX = 0.25; centerY = 0.6; centerZ = 0.1
    },
    @{
        name = "MeadowStone"
        guid = "ffffffff000000000000000000000006"
        meshGuid = "dddddddd000000000000000000000004"
        materials = @("cccccccc000000000000000000000006")
        isCapsule = $false
        sizeX = 1.8; sizeY = 1.5; sizeZ = 1.8
        centerX = -0.15; centerY = 0.75; centerZ = 0.05
    },
    @{
        name = "ForestOre"
        guid = "ffffffff000000000000000000000003"
        meshGuid = "dddddddd000000000000000000000005"
        materials = @("cccccccc000000000000000000000007")
        isCapsule = $false
        sizeX = 1.4; sizeY = 1.0; sizeZ = 1.4
        centerX = 0.05; centerY = 0.5; centerZ = 0.0
    },
    @{
        name = "MeadowOre"
        guid = "ffffffff000000000000000000000007"
        meshGuid = "dddddddd000000000000000000000006"
        materials = @("cccccccc000000000000000000000008")
        isCapsule = $false
        sizeX = 1.6; sizeY = 1.1; sizeZ = 1.6
        centerX = 0.05; centerY = 0.55; centerZ = 0.25
    },
    @{
        name = "ForestHouse"
        guid = "ffffffff000000000000000000000004"
        meshGuid = "dddddddd000000000000000000000007"
        materials = @("cccccccc000000000000000000000009", "cccccccc000000000000000000000011")
        isCapsule = $false
        sizeX = 3.4; sizeY = 3.4; sizeZ = 3.4
        centerX = 0.0; centerY = 1.7; centerZ = 0.0
    },
    @{
        name = "MeadowHouse"
        guid = "ffffffff000000000000000000000008"
        meshGuid = "dddddddd000000000000000000000008"
        materials = @("cccccccc000000000000000000000010", "cccccccc000000000000000000000011")
        isCapsule = $false
        sizeX = 3.2; sizeY = 3.3; sizeZ = 3.2
        centerX = 0.1; centerY = 1.65; centerZ = 0.1
    }
)

foreach ($prefab in $prefabs) {
    # Generate material items string
    $materialsList = [System.Collections.Generic.List[string]]::new()
    foreach ($matGuid in $prefab.materials) {
        $materialsList.Add("  - {fileID: 2100000, guid: $matGuid, type: 2}")
    }
    $matsString = $materialsList -join "`n"

    # Generate collider details string
    $colliderString = ""
    if ($prefab.isCapsule) {
        $colliderString = @"
--- !u!136 &6500000
CapsuleCollider:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Enabled: 1
  m_Radius: $($prefab.radius)
  m_Height: $($prefab.height)
  m_Direction: 1
  m_Center: {x: 0, y: $($prefab.centerY), z: 0}
  m_IsTrigger: 0
"@
    } else {
        $colliderString = @"
--- !u!65 &6500000
BoxCollider:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Enabled: 1
  m_Size: {x: $($prefab.sizeX), y: $($prefab.sizeY), z: $($prefab.sizeZ)}
  m_Center: {x: $($prefab.centerX), y: $($prefab.centerY), z: $($prefab.centerZ)}
  m_IsTrigger: 0
"@
    }

    # Generate prefab YAML
    $prefabYaml = @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1 &100000
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 400000}
  - component: {fileID: 3300000}
  - component: {fileID: 2300000}
  - component: {fileID: 6500000}
  m_Layer: 0
  m_Name: $($prefab.name)
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &400000
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 0, z: 0}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 0
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!33 &3300000
MeshFilter:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Mesh: {fileID: 4300000, guid: $($prefab.meshGuid), type: 3}
--- !u!23 &2300000
MeshRenderer:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 100000}
  m_Enabled: 1
  m_CastShadows: 1
  m_ReceiveShadows: 1
  m_DynamicOccludee: 1
  m_MotionVectors: 1
  m_LightProbeUsage: 1
  m_ReflectionProbeUsage: 1
  m_RenderingLayerMask: 1
  m_RendererPriority: 0
  m_Materials:
$matsString
  m_StaticBatchInfo:
    firstSubMesh: 0
    subMeshCount: 0
  m_StaticBatchRoot: {fileID: 0}
  m_ProbeAnchor: {fileID: 0}
  m_LightProbeVolumeOverride: {fileID: 0}
  m_ScaleInLightmap: 1
  m_ReceiveGI: 1
  m_PreserveUVs: 0
  m_IgnoreNormalsForChartDetection: 0
  m_ImportantGI: 0
  m_StitchLightmapSeams: 1
  m_SelectedEditorRenderState: 3
  m_MinimumChartSize: 4
  m_AutoUVMaxDistance: 0.5
  m_AutoUVMaxAngle: 89
  m_LightmapParameters: {fileID: 0}
  m_SortingLayerID: 0
  m_SortingLayer: 0
  m_SortingOrder: 0
$colliderString
"@

    # Generate prefab meta YAML
    $metaYaml = @"
fileFormatVersion: 2
guid: $($prefab.guid)
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@

    Set-Content -Path "$prefabsDir\$($prefab.name).prefab" -Value $prefabYaml -Encoding utf8
    Set-Content -Path "$prefabsDir\$($prefab.name).prefab.meta" -Value $metaYaml -Encoding utf8
}

Write-Host "All prefabs generated successfully!"
