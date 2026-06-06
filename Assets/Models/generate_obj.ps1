# Procedural OBJ Generator for UnityMine
# This script generates low-poly 3D meshes as Wavefront .obj files.

$modelsDir = "Assets\Models"

class MeshBuilder {
    [System.Collections.Generic.List[string]] $vertices
    [System.Collections.Generic.List[string]] $uvs
    [System.Collections.Generic.List[string]] $normals
    [System.Collections.Generic.List[string]] $faces
    [int] $vCount
    [int] $vtCount
    [int] $vnCount

    MeshBuilder() {
        $this.vertices = [System.Collections.Generic.List[string]]::new()
        $this.uvs = [System.Collections.Generic.List[string]]::new()
        $this.normals = [System.Collections.Generic.List[string]]::new()
        $this.faces = [System.Collections.Generic.List[string]]::new()
        $this.vCount = 0
        $this.vtCount = 0
        $this.vnCount = 0
    }

    [void] AddVertex([double]$x, [double]$y, [double]$z) {
        $this.vertices.Add("v $x $y $z")
        $this.vCount++
    }

    [void] AddUV([double]$u, [double]$v) {
        $this.uvs.Add("vt $u $v")
        $this.vtCount++
    }

    [void] AddNormal([double]$nx, [double]$ny, [double]$nz) {
        $this.normals.Add("vn $nx $ny $nz")
        $this.vnCount++
    }

    [void] UseMaterial([string]$matName) {
        $this.faces.Add("usemtl $matName")
    }

    [void] AddFace([int[]]$vIndices, [int[]]$vtIndices, [int[]]$vnIndices) {
        $faceParts = [System.Collections.Generic.List[string]]::new()
        for ($i = 0; $i -lt $vIndices.Length; $i++) {
            $v = $vIndices[$i]
            $vt = if ($vtIndices -and $vtIndices.Length -gt $i) { $vtIndices[$i] } else { 0 }
            $vn = if ($vnIndices -and $vnIndices.Length -gt $i) { $vnIndices[$i] } else { 0 }
            
            if ($vt -gt 0 -and $vn -gt 0) {
                $faceParts.Add("$v/$vt/$vn")
            } elseif ($vt -gt 0) {
                $faceParts.Add("$v/$vt")
            } elseif ($vn -gt 0) {
                $faceParts.Add("$v//$vn")
            } else {
                $faceParts.Add("$v")
            }
        }
        $this.faces.Add("f " + ($faceParts -join " "))
    }

    [string] GetOBJString() {
        $sb = [System.Text.StringBuilder]::new()
        $sb.AppendLine("# Generated OBJ file") | Out-Null
        foreach ($v in $this.vertices) { $sb.AppendLine($v) | Out-Null }
        foreach ($vt in $this.uvs) { $sb.AppendLine($vt) | Out-Null }
        foreach ($vn in $this.normals) { $sb.AppendLine($vn) | Out-Null }
        foreach ($f in $this.faces) { $sb.AppendLine($f) | Out-Null }
        return $sb.ToString()
    }

    # Helper: Adds a Cylinder (for tree trunks)
    [void] AddCylinder([double]$radius, [double]$height, [double]$yOffset, [int]$segments, [string]$matName) {
        $this.UseMaterial($matName)
        $startV = $this.vCount + 1
        $startUV = $this.vtCount + 1
        
        # Bottom center vertex
        $this.AddVertex(0, $yOffset, 0)
        $this.AddUV(0.5, 0.5)
        # Top center vertex
        $this.AddVertex(0, $yOffset + $height, 0)
        $this.AddUV(0.5, 0.5)

        # Create vertices around the circumference
        for ($i = 0; $i -lt $segments; $i++) {
            $angle = ($i * [Math]::PI * 2) / $segments
            $x = $radius * [Math]::Cos($angle)
            $z = $radius * [Math]::Sin($angle)
            $u = $i / $segments
            
            # Bottom vertex
            $this.AddVertex($x, $yOffset, $z)
            $this.AddUV($u, 0.0)
            
            # Top vertex
            $this.AddVertex($x, $yOffset + $height, $z)
            $this.AddUV($u, 1.0)
        }

        # Faces
        for ($i = 0; $i -lt $segments; $i++) {
            $next = ($i + 1) % $segments
            
            $b_curr = $startV + 2 + ($i * 2)
            $t_curr = $startV + 2 + ($i * 2) + 1
            $b_next = $startV + 2 + ($next * 2)
            $t_next = $startV + 2 + ($next * 2) + 1
            
            $b_curr_uv = $startUV + 2 + ($i * 2)
            $t_curr_uv = $startUV + 2 + ($i * 2) + 1
            $b_next_uv = $startUV + 2 + ($next * 2)
            $t_next_uv = $startUV + 2 + ($next * 2) + 1

            # Side quad (split into 2 triangles)
            $this.AddFace(@($b_curr, $t_curr, $t_next), @($b_curr_uv, $t_curr_uv, $t_next_uv), $null)
            $this.AddFace(@($b_curr, $t_next, $b_next), @($b_curr_uv, $t_next_uv, $b_next_uv), $null)

            # Bottom cap triangle
            $this.AddFace(@($startV, $b_next, $b_curr), @($startUV, $b_next_uv, $b_curr_uv), $null)
            
            # Top cap triangle
            $this.AddFace(@(($startV + 1), $t_curr, $t_next), @(($startUV + 1), $t_curr_uv, $t_next_uv), $null)
        }
    }

    # Helper: Adds a Cone (for forest foliage)
    [void] AddCone([double]$radius, [double]$height, [double]$yOffset, [int]$segments, [string]$matName) {
        $this.UseMaterial($matName)
        $startV = $this.vCount + 1
        $startUV = $this.vtCount + 1

        # Tip vertex
        $this.AddVertex(0, $yOffset + $height, 0)
        $this.AddUV(0.5, 1.0)
        
        # Bottom center vertex
        $this.AddVertex(0, $yOffset, 0)
        $this.AddUV(0.5, 0.5)

        # Base vertices
        for ($i = 0; $i -le $segments; $i++) {
            $angle = ($i * [Math]::PI * 2) / $segments
            $x = $radius * [Math]::Cos($angle)
            $z = $radius * [Math]::Sin($angle)
            $u = $i / $segments
            
            $this.AddVertex($x, $yOffset, $z)
            $this.AddUV($u, 0.0)
        }

        # Faces
        for ($i = 0; $i -lt $segments; $i++) {
            $curr = $startV + 2 + $i
            $next = $startV + 2 + $i + 1
            
            $curr_uv = $startUV + 2 + $i
            $next_uv = $startUV + 2 + $i + 1

            # Cone side face
            $this.AddFace(@($curr, $startV, $next), @($curr_uv, $startUV, $next_uv), $null)
            # Bottom cap face
            $this.AddFace(@(($startV + 1), $next, $curr), @(($startUV + 1), $next_uv, $curr_uv), $null)
        }
    }

    # Helper: Adds an octahedron-based blob/sphere (for meadow leaves / rocks)
    [void] AddBlob([double]$radius, [double]$xCenter, [double]$yCenter, [double]$zCenter, [double]$noiseScale, [string]$matName) {
        $this.UseMaterial($matName)
        $startV = $this.vCount + 1
        $startUV = $this.vtCount + 1

        # 6 octahedron vertices
        $dirs = @(
            @(0.0, 1.0, 0.0),
            @(0.0, -1.0, 0.0),
            @(1.0, 0.0, 0.0),
            @(-1.0, 0.0, 0.0),
            @(0.0, 0.0, 1.0),
            @(0.0, 0.0, -1.0)
        )

        # Apply noise and radius
        for ($i = 0; $i -lt $dirs.Length; $i++) {
            $d = $dirs[$i]
            $jitter = (Get-Random -Minimum (-$noiseScale) -Maximum $noiseScale)
            $r = $radius + $jitter
            $px = $xCenter + $d[0] * $r
            $py = $yCenter + $d[1] * $r
            $pz = $zCenter + $d[2] * $r
            $this.AddVertex($px, $py, $pz)

            # Simple UVs mapping from direction
            $u = 0.5 + $d[0] * 0.5
            $v = 0.5 + $d[2] * 0.5
            $this.AddUV($u, $v)
        }

        # 8 faces connecting the octahedron vertices
        # Indices: 0=Up, 1=Down, 2=Right, 3=Left, 4=Front, 5=Back
        # Triangles (1-based: add $startV)
        # Up-Right-Front
        $this.AddFace(@(($startV+0), ($startV+2), ($startV+4)), @(($startUV+0), ($startUV+2), ($startUV+4)), $null)
        # Up-Front-Left
        $this.AddFace(@(($startV+0), ($startV+4), ($startV+3)), @(($startUV+0), ($startUV+4), ($startUV+3)), $null)
        # Up-Left-Back
        $this.AddFace(@(($startV+0), ($startV+3), ($startV+5)), @(($startUV+0), ($startUV+3), ($startUV+5)), $null)
        # Up-Back-Right
        $this.AddFace(@(($startV+0), ($startV+5), ($startV+2)), @(($startUV+0), ($startUV+5), ($startUV+2)), $null)
        # Down-Front-Right
        $this.AddFace(@(($startV+1), ($startV+4), ($startV+2)), @(($startUV+1), ($startUV+4), ($startUV+2)), $null)
        # Down-Left-Front
        $this.AddFace(@(($startV+1), ($startV+3), ($startV+4)), @(($startUV+1), ($startUV+3), ($startUV+4)), $null)
        # Down-Back-Left
        $this.AddFace(@(($startV+1), ($startV+5), ($startV+3)), @(($startUV+1), ($startUV+5), ($startUV+3)), $null)
        # Down-Right-Back
        $this.AddFace(@(($startV+1), ($startV+2), ($startV+5)), @(($startUV+1), ($startUV+2), ($startUV+5)), $null)
    }

    # Helper: Adds a Box (for houses, walls, etc.)
    [void] AddBox([double]$xSize, [double]$ySize, [double]$zSize, [double]$xOffset, [double]$yOffset, [double]$zOffset, [string]$matName) {
        $this.UseMaterial($matName)
        $startV = $this.vCount + 1
        $startUV = $this.vtCount + 1

        $dx = $xSize / 2
        $dy = $ySize
        $dz = $zSize / 2

        # 8 vertices
        $this.AddVertex($xOffset - $dx, $yOffset,       $zOffset - $dz) # 0
        $this.AddVertex($xOffset + $dx, $yOffset,       $zOffset - $dz) # 1
        $this.AddVertex($xOffset + $dx, $yOffset + $dy, $zOffset - $dz) # 2
        $this.AddVertex($xOffset - $dx, $yOffset + $dy, $zOffset - $dz) # 3
        $this.AddVertex($xOffset - $dx, $yOffset,       $zOffset + $dz) # 4
        $this.AddVertex($xOffset + $dx, $yOffset,       $zOffset + $dz) # 5
        $this.AddVertex($xOffset + $dx, $yOffset + $dy, $zOffset + $dz) # 6
        $this.AddVertex($xOffset - $dx, $yOffset + $dy, $zOffset + $dz) # 7

        # UV coordinates for flat mapping
        $this.AddUV(0.0, 0.0) # 0
        $this.AddUV(1.0, 0.0) # 1
        $this.AddUV(1.0, 1.0) # 2
        $this.AddUV(0.0, 1.0) # 3

        # Front Face (v0, v1, v2, v3)
        $this.AddFace(@(($startV+0), ($startV+1), ($startV+2)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        $this.AddFace(@(($startV+0), ($startV+2), ($startV+3)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
        
        # Back Face (v5, v4, v7, v6)
        $this.AddFace(@(($startV+5), ($startV+4), ($startV+7)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        $this.AddFace(@(($startV+5), ($startV+7), ($startV+6)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
        
        # Left Face (v4, v0, v3, v7)
        $this.AddFace(@(($startV+4), ($startV+0), ($startV+3)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        $this.AddFace(@(($startV+4), ($startV+3), ($startV+7)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
        
        # Right Face (v1, v5, v6, v2)
        $this.AddFace(@(($startV+1), ($startV+5), ($startV+6)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        $this.AddFace(@(($startV+1), ($startV+6), ($startV+2)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
        
        # Top Face (v3, v2, v6, v7)
        $this.AddFace(@(($startV+3), ($startV+2), ($startV+6)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        $this.AddFace(@(($startV+3), ($startV+6), ($startV+7)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
        
        # Bottom Face (v4, v5, v1, v0)
        $this.AddFace(@(($startV+4), ($startV+5), ($startV+1)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        $this.AddFace(@(($startV+4), ($startV+1), ($startV+0)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
    }

    # Helper: Adds a Prism (for house roof)
    [void] AddPrism([double]$width, [double]$height, [double]$depth, [double]$xOffset, [double]$yOffset, [double]$zOffset, [string]$matName) {
        $this.UseMaterial($matName)
        $startV = $this.vCount + 1
        $startUV = $this.vtCount + 1

        $dx = $width / 2
        $dz = $depth / 2

        # 6 vertices
        $this.AddVertex($xOffset - $dx, $yOffset,           $zOffset - $dz) # 0: Front bottom left
        $this.AddVertex($xOffset + $dx, $yOffset,           $zOffset - $dz) # 1: Front bottom right
        $this.AddVertex($xOffset,       $yOffset + $height, $zOffset - $dz) # 2: Front top peak
        $this.AddVertex($xOffset - $dx, $yOffset,           $zOffset + $dz) # 3: Back bottom left
        $this.AddVertex($xOffset + $dx, $yOffset,           $zOffset + $dz) # 4: Back bottom right
        $this.AddVertex($xOffset,       $yOffset + $height, $zOffset + $dz) # 5: Back top peak

        # UVs
        $this.AddUV(0.0, 0.0) # 0
        $this.AddUV(1.0, 0.0) # 1
        $this.AddUV(0.5, 1.0) # 2 (peak)
        $this.AddUV(1.0, 1.0) # 3

        # Front triangle face
        $this.AddFace(@(($startV+0), ($startV+2), ($startV+1)), @(($startUV+0), ($startUV+2), ($startUV+1)), $null)
        
        # Back triangle face
        $this.AddFace(@(($startV+3), ($startV+4), ($startV+5)), @(($startUV+0), ($startUV+1), ($startUV+2)), $null)
        
        # Left sloped face (v0, v3, v5, v2)
        $this.AddFace(@(($startV+0), ($startV+3), ($startV+5)), @(($startUV+0), ($startUV+3), ($startUV+2)), $null)
        $this.AddFace(@(($startV+0), ($startV+5), ($startV+2)), @(($startUV+0), ($startUV+2), ($startUV+1)), $null)
        
        # Right sloped face (v1, v2, v5, v4)
        $this.AddFace(@(($startV+1), ($startV+2), ($startV+5)), @(($startUV+0), ($startUV+2), ($startUV+3)), $null)
        $this.AddFace(@(($startV+1), ($startV+5), ($startV+4)), @(($startUV+0), ($startUV+3), ($startUV+1)), $null)
        
        # Bottom flat face
        $this.AddFace(@(($startV+0), ($startV+1), ($startV+4)), @(($startUV+0), ($startUV+1), ($startUV+3)), $null)
        $this.AddFace(@(($startV+0), ($startV+4), ($startV+3)), @(($startUV+0), ($startUV+3), ($startUV+2)), $null)
    }
}

# 1. Forest Tree: Cylinder trunk + 3 stacked cones for a pine tree
$builder = [MeshBuilder]::new()
$builder.AddCylinder(0.25, 1.5, 0.0, 6, "Mat_BarkForest")
$builder.AddCone(1.0, 1.5, 1.2, 6, "Mat_LeavesForest")
$builder.AddCone(0.8, 1.3, 2.0, 6, "Mat_LeavesForest")
$builder.AddCone(0.6, 1.0, 2.8, 6, "Mat_LeavesForest")
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_TreeForest.obj" -Encoding utf8

# 2. Meadow Tree: Cylinder trunk + 2 intersecting blobs for cherry blossom
$builder = [MeshBuilder]::new()
$builder.AddCylinder(0.25, 2.0, 0.0, 6, "Mat_BarkMeadow")
$builder.AddBlob(1.1, 0.0, 2.5, 0.0, 0.1, "Mat_LeavesMeadow")
$builder.AddBlob(0.8, 0.4, 3.2, 0.2, 0.05, "Mat_LeavesMeadow")
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_TreeMeadow.obj" -Encoding utf8

# 3. Forest Stone: Sharp low-poly rock (overlapping offset blobs)
$builder = [MeshBuilder]::new()
$builder.AddBlob(0.8, 0.0, 0.4, 0.0, 0.2, "Mat_StoneForest")
$builder.AddBlob(0.5, 0.5, 0.3, 0.2, 0.15, "Mat_StoneForest")
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_StoneForest.obj" -Encoding utf8

# 4. Meadow Stone: Smooth rounded boulder
$builder = [MeshBuilder]::new()
$builder.AddBlob(0.9, 0.0, 0.45, 0.0, 0.05, "Mat_StoneMeadow")
$builder.AddBlob(0.6, -0.3, 0.3, 0.1, 0.03, "Mat_StoneMeadow")
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_StoneMeadow.obj" -Encoding utf8

# 5. Forest Ore: Low-poly rock with mini-blobs sticking out (representing crystals)
$builder = [MeshBuilder]::new()
$builder.AddBlob(0.7, 0.0, 0.35, 0.0, 0.1, "Mat_OreForest")
$builder.AddBlob(0.3, 0.4, 0.5, 0.3, 0.02, "Mat_OreForest") # copper outcrop
$builder.AddBlob(0.25, -0.3, 0.4, -0.3, 0.02, "Mat_OreForest") # copper outcrop
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_OreForest.obj" -Encoding utf8

# 6. Meadow Ore: Light rock with gold outcrops
$builder = [MeshBuilder]::new()
$builder.AddBlob(0.8, 0.0, 0.4, 0.0, 0.05, "Mat_OreMeadow")
$builder.AddBlob(0.35, 0.5, 0.4, 0.1, 0.01, "Mat_OreMeadow") # gold outcrop
$builder.AddBlob(0.3, -0.4, 0.6, -0.1, 0.01, "Mat_OreMeadow") # gold outcrop
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_OreMeadow.obj" -Encoding utf8

# 7. Forest House: Dark wood walls box + prism roof
$builder = [MeshBuilder]::new()
$builder.AddBox(3.0, 2.2, 3.0, 0.0, 0.0, 0.0, "Mat_HouseForest")
$builder.AddPrism(3.4, 1.2, 3.4, 0.0, 2.2, 0.0, "Mat_HouseRoof")
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_HouseForest.obj" -Encoding utf8

# 8. Meadow House: White plaster walls box + prism roof + small chimney box
$builder = [MeshBuilder]::new()
$builder.AddBox(2.8, 2.0, 2.8, 0.0, 0.0, 0.0, "Mat_HouseMeadow")
$builder.AddPrism(3.2, 1.3, 3.2, 0.0, 2.0, 0.0, "Mat_HouseRoof")
$builder.AddBox(0.4, 1.0, 0.4, 0.9, 1.8, 0.9, "Mat_BarkMeadow") # Chimney box using wood texture as fallback or simple style
$builder.GetOBJString() | Set-Content -Path "$modelsDir\Mesh_HouseMeadow.obj" -Encoding utf8

Write-Host "All low-poly OBJ meshes successfully generated!"
