using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class ImportManager : MonoBehaviour {

    public TextAsset NodeData;
    private Vector3[] Locations;
    public GameObject Node;

    void Start()
    {
        ImportNodeData(NodeData);
    }

    void ImportNodeData(TextAsset NodeData)
    {
        string[] NodeDataLines = NodeData.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        CreateFEAModel(NodeDataLines);
    }

    void CreateFEAModel(String[] NodeDataLines)
    {

        float[] stress = new float[NodeDataLines.Length];
        float[] nodeno = new float[NodeDataLines.Length];
        for (int i = 0; i < NodeDataLines.Length; i++)
        {
            string[] props = NodeDataLines[i].Split(","[0]);

            stress[i] = float.Parse(props[4]);
            nodeno[i] = float.Parse(props[0]);

        }

        float maxStress = stress.Max();
        float minStress = stress.Min();
        float range = maxStress - minStress;

        GameObject scaleconfig = GameObject.Find("ColourScale");
        scaleconfig.GetComponent<ScaleConfig>().SetScaleValues(maxStress, minStress);

        float[] gradstress = new float[stress.Length];
        for (int i = 0; i < stress.Length; i++)
        {
            gradstress[i] = (stress[i]-minStress) / range;
        }



        Gradient grad;
        GradientColorKey[] gck;
        GradientAlphaKey[] gak;
        grad = new Gradient();
        gck = new GradientColorKey[5];
        gck[0].color = Color.red;
        gck[0].time = 1.0F;
        gck[1].color = Color.yellow;
        gck[1].time = 0.75F;
        gck[2].color = Color.green;
        gck[2].time = 0.5F;
        gck[3].color = Color.cyan;
        gck[3].time = 0.25F;
        gck[4].color = Color.blue;
        gck[4].time = 0.0F;
        gak = new GradientAlphaKey[2];
        gak[0].alpha = 1.0F;
        gak[0].time = 0.0F;
        gak[1].alpha = 1.0F;
        gak[1].time = 1.0F;
        grad.SetKeys(gck, gak);


        GameObject model1 = new GameObject("model1");
        model1.transform.position = new Vector3(0, 0, 2);
        model1.transform.parent = GameObject.Find("ModelSpace").transform;
        model1.tag = "FEAModels";

        Locations = new Vector3[NodeDataLines.Length];

        for (int i = 0; i < NodeDataLines.Length; i++)
        {
            string[] coord = NodeDataLines[i].Split(","[0]);
                       
            float nodenumber = float.Parse(coord[0]);
            float coordx = float.Parse(coord[1]);
            float coordy = float.Parse(coord[2]);
            float coordz = float.Parse(coord[3]);

            Locations[i] = new Vector3(coordx, coordy, coordz+1f);

            GameObject node = Instantiate(Node);
            node.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            node.transform.parent = model1.transform;
            node.transform.position = Locations[i];
            node.tag = "Nodes";
            node.GetComponent<Renderer>().material.color = grad.Evaluate(gradstress[i]);
            node.name = "node" + nodenumber;
        }
    }

}
