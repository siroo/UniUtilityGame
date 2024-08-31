using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace Game
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CombineMeshes : MonoBehaviour
    {
        [ContextMenu("MeshCombine")]
        private void MeshCombine()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.CombineMeshes(combine);
            Unwrapping.GenerateSecondaryUVSet(mesh);
            transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            transform.gameObject.SetActive(true);
        }

        [ContextMenu("MeshCombineSubmesh")]
        private void MeshCombineSubmesh()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);
            List<CombineInstance> combine = new List<CombineInstance>();

            int i = 0;
            while (i < meshFilters.Length)
            {
                if (meshFilters[i].sharedMesh == null)
                {
                    i++;
                    continue;
                }

                for (int n = 0; n < meshFilters[i].sharedMesh.subMeshCount; n++)
                {
                    var con = new CombineInstance();
                    con.mesh = meshFilters[i].sharedMesh;
                    con.subMeshIndex = n;
                    con.transform = meshFilters[i].transform.localToWorldMatrix;
                    combine.Add(con);
                }

                //con.mesh = meshFilters[i].sharedMesh;
                //con.transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }

            Debug.Log("CombineMeshes");
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            var _combines = combine.ToArray();
            mesh.CombineMeshes(_combines, false);
            Unwrapping.GenerateSecondaryUVSet(mesh);
            transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            transform.gameObject.SetActive(true);
        }
    }
}
#endif
