using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    /// <summary>
    /// https://discussions.unity.com/t/884839
    /// </summary>
    [ExecuteAlways]
    public class ExtractTreeCollidersFromTerrain : MonoBehaviour
    {
        [SerializeField] NavMeshSurface _surface;
        [SerializeField] Terrain _terrain;
        [SerializeField] List<GameObject> _excludeList = new(); //èúäO
        [SerializeField] List<GameObject> _navVolList = new();

        private readonly List<GameObject> _createdObjects = new List<GameObject>();

        private Terrain Terrain => GetTerrain();

        private Terrain GetTerrain()
        {
            if (_terrain != null)
                return _terrain;

            _terrain = GetComponent<Terrain>();
            return _terrain;
        }

        [ContextMenu("Extract, Bake, and Delete")]
        public void ExtractBakeDelete()
        {
            var count = ExtractTreesFromTerrain();
            CreateNavVol();
            SetExclude();
            if (_surface.navMeshData == null)
            {
                _surface.BuildNavMesh();
            }
            else
            {
                _surface.UpdateNavMesh(_surface.navMeshData);
            }
            EndExclude();
            DestroyCachedObjects();
            Debug.Log($"<color=#99ff99>Successfully created {count} colliders and baked the NavMesh!</color>");
            UnityEditor.AssetDatabase.Refresh();
        }

        [ContextMenu("Delete Collider Objects")]
        private void DestroyCachedObjects()
        {
            foreach (var obj in _createdObjects)
                DestroyImmediate(obj);

            _createdObjects.Clear();
        }

        [ContextMenu("ExtractBase")]
        private void ExtractBase()
        {
            var count = ExtractTreesFromTerrain();
            CreateNavVol();
            SetExclude();
            if (_surface.navMeshData == null)
            {
                _surface.BuildNavMesh();
            }
            else
            {
                _surface.UpdateNavMesh(_surface.navMeshData);
            }
            //_surface.BuildNavMesh();
            EndExclude();
            UnityEditor.AssetDatabase.Refresh();
        }

        [ContextMenu("Extract Only")]
        public int ExtractTreesFromTerrain()
        {
            for (var prototypeIndex = 0; prototypeIndex < Terrain.terrainData.treePrototypes.Length; prototypeIndex++)
                ExtractInstancesFromTreePrototype(prototypeIndex);

            return _createdObjects.Count;
        }

        private void ExtractInstancesFromTreePrototype(int prototypeIndex)
        {
            var tree = Terrain.terrainData.treePrototypes[prototypeIndex];
            var instances = Terrain.terrainData.treeInstances.Where(x => x.prototypeIndex == prototypeIndex).ToArray();

            for (var instanceIndex = 0; instanceIndex < instances.Length; instanceIndex++)
            {
                UpdateInstancePosition(instances, instanceIndex);
                CreateNavMeshObstacle(tree, prototypeIndex, instances, instanceIndex);
            }
        }

        private void CreateNavVol()
        {
            foreach (var vol in _navVolList)
            {
                int index = 0;
                var boxs = vol.GetComponentsInChildren<BoxCollider>();
                foreach (var box in boxs)
                {
                    CreateGameObjectForModifierVolume(box.gameObject, index);
                    index++;
                }
            }
        }

        private void CreateGameObjectForModifierVolume(GameObject pre, int index)
        {
            var col = pre.GetComponent<BoxCollider>();
            //col.bounds

            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = pre.name + index;
            obj.transform.localScale = pre.transform.lossyScale;
            //obj.transform.localScale = new Vector3(instances[instanceIndex].widthScale, instances[instanceIndex].heightScale, instances[instanceIndex].widthScale);
            obj.transform.rotation = pre.transform.rotation;
            obj.transform.position = pre.transform.position;
            obj.transform.parent = Terrain.transform;
            obj.isStatic = true;

            var nav = obj.AddComponent<NavMeshModifierVolume>();
            nav.center = col.center;
            nav.size = col.size;
            nav.area = NavMesh.GetAreaFromName("Not Walkable");

            _createdObjects.Add(obj);
        }

        private void CreateNavMeshObstacle(TreePrototype tree, int prototypeIndex, TreeInstance[] instances, int instanceIndex)
        {
            var navMeshObstacle = tree.prefab.GetComponent<NavMeshObstacle>();
            if (!navMeshObstacle) return;

            var primitiveScale = CalculatePrimitiveScale(navMeshObstacle);

            var obj = CreateGameObjectForNavMeshObstacle(tree, instances, instanceIndex, primitiveScale);
            _createdObjects.Add(obj);
        }

        private GameObject CreateGameObjectForNavMeshObstacle(TreePrototype tree, TreeInstance[] instances, int instanceIndex, Vector3 primitiveScale)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.name = tree.prefab.name + instanceIndex;
            obj.layer = Terrain.preserveTreePrototypeLayers ? tree.prefab.layer : Terrain.gameObject.layer;
            var scale = tree.prefab.transform.localScale;
            primitiveScale.Scale(new Vector3(instances[instanceIndex].widthScale, instances[instanceIndex].heightScale, instances[instanceIndex].widthScale));
            primitiveScale.Scale(scale);
            obj.transform.localScale = primitiveScale;
            //obj.transform.localScale = new Vector3(instances[instanceIndex].widthScale, instances[instanceIndex].heightScale, instances[instanceIndex].widthScale);
            obj.transform.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * instances[instanceIndex].rotation, 0);
            obj.transform.position = instances[instanceIndex].position;
            obj.transform.parent = Terrain.transform;
            obj.isStatic = true;

            /*
            var nav = obj.AddComponent<NavMeshModifier>();
            nav.applyToChildren = false;
            nav.overrideArea = true;
            nav.area = NavMesh.GetAreaFromName("Not Walkable");
            */
            /*
            var col = obj.GetComponent<CapsuleCollider>();
            var nav = obj.AddComponent<NavMeshModifierVolume>();
            nav.center = col.center;
            var size = col.bounds.size;
            size.x -= 0.1f;
            size.y -= 0.1f;
            size.z -= 0.1f;
            nav.size = size;
            nav.area = NavMesh.GetAreaFromName("Not Walkable");
            */


            return obj;
        }

        private Vector3 CalculatePrimitiveScale(NavMeshObstacle navMeshObstacle)
        {
            if (navMeshObstacle.shape == NavMeshObstacleShape.Capsule)
                return navMeshObstacle.radius * Vector3.one;

            return navMeshObstacle.size;
        }

        private void UpdateInstancePosition(TreeInstance[] instances, int instanceIndex)
        {
            instances[instanceIndex].position = Vector3.Scale(instances[instanceIndex].position, Terrain.terrainData.size);
            instances[instanceIndex].position += Terrain.GetPosition();
        }

        private void SetExclude()
        {
            foreach (var item in _excludeList)
            {
                item.SetActive(false);
            }
        }

        private void EndExclude()
        {
            foreach (var item in _excludeList)
            {
                item.SetActive(true);
            }
        }
    }
}