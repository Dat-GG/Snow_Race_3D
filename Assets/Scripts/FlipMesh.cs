using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlipMesh : MonoBehaviour
{
    [ContextMenu("Flip")]
    public void FlipMeshContext()
    {
        Mesh mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }

}
