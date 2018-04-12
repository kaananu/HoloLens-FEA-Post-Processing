﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombine : MonoBehaviour {

    public void CombineMesh()
    {

        MeshFilter myMeshFilter = GetComponent<MeshFilter>();

        Mesh mesh = myMeshFilter.sharedMesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            myMeshFilter.sharedMesh = mesh;
        }
        else
        {
            mesh.Clear();
            Debug.Log("Mesh Cleared");
        }

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>(false);
        Debug.Log(name + " is combining " + filters.Length + " meshes!");

        List<CombineInstance> combiners = new List<CombineInstance>();

        foreach (MeshFilter filter in filters)
        {
            if (filter == myMeshFilter)
                continue;

            CombineInstance ci = new CombineInstance();
            ci.mesh = filter.sharedMesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            combiners.Add(ci);
        }
        mesh.CombineMeshes(combiners.ToArray(), true);
    }

    public void CombineMeshNew()
    {
        gameObject.AddComponent<MeshFilter>();

        // All our children (and us)
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>(false);


        // All the meshes in our children (just a big list)
        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(); // <-- you can optimize this
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.transform == transform)
                continue;
            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats)
                if (!materials.Contains(localMat))
                    materials.Add(localMat);
        }

        // Each material will have a mesh for it.
        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials)
        {
            // Make a combiner for each (sub)mesh that is mapped to the right material.
            List<CombineInstance> combiners = new List<CombineInstance>();
            foreach (MeshFilter filter in filters)
            {
                if (filter.transform == transform) continue;
                // The filter doesn't know what materials are involved, get the renderer.
                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();  // <-- (Easy optimization is possible here, give it a try!)
                if (renderer == null)
                {
                    Debug.LogError(filter.name + " has no MeshRenderer");
                    continue;
                }

                // Let's see if their materials are the one we want right now.
                Material[] localMaterials = renderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
                {
                    if (localMaterials[materialIndex] != material)
                        continue;
                    // This submesh is the material we're looking for right now.
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = Matrix4x4.identity;
                    combiners.Add(ci);
                }
            }
            // Flatten into a single mesh.
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);
            submeshes.Add(mesh);
        }

        // The final mesh: combine all the material-specific meshes as independent submeshes.
        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;
        Debug.Log("Final mesh has " + submeshes.Count + " materials.");

        for (int a = 0; a < transform.childCount; a++)
        {
            Destroy(transform.GetChild(a).gameObject);
        }

        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().materials = materials.ToArray();

        gameObject.AddComponent<MeshCollider>();

        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.position = new Vector3(0, 0, 2);
    }

}
