using System;
using System.Linq;
using UnityEngine;

public class AbaqusImporter : MonoBehaviour
{

    public TextAsset ElemConnectivity;
    public TextAsset SpatialDisplacement;
    public TextAsset ElemStress;

    public Material C1;
    public Material C2;
    public Material C3;
    public Material C4;
    public Material C5;
    public Material C6;
    public Material C7;
    public Material C8;
    public Material C9;
    public Material C10;
    public Material C11;
    public Material C12;
    public Material C13;

    public void LoadModel()
    {

        #region Gradient Creation

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

        #endregion

        #region Stress import

        string[] StressDataLines = ElemStress.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        float[] StressAll = new float[StressDataLines.Length - 1];
        float[] StressFront = new float[StressDataLines.Length / 2];
        float[] StressBack = new float[StressDataLines.Length / 2];

        int i1 = 0;
        int i2 = 0;

        for (int i = 1; i < (StressDataLines.Length); i++)
        {
            string[] StressData = StressDataLines[i].Split(","[0]);

            string[] StressData2 = StressData[11].Split(")"[0]);

            string StressSide = StressData2[0];
            string FrontStress = " (fraction = 1.0";
            string BackStress = " (fraction = -1.0";

            StressAll[i - 1] = float.Parse(StressData[12]);

            if (StressSide.Equals(FrontStress))
            {
                StressFront[i1] = float.Parse(StressData[12]);
                i1 = i1 + 1;
            }
            else if (StressSide.Equals(BackStress))
            {
                StressBack[i2] = float.Parse(StressData[12]);
                i2 = i2 + 1;
            }
            else
            {
                Debug.Log("Failure in comparison");
            }
        }

        float maxStress = StressAll.Max();
        float minStress = StressAll.Min();
        float range = maxStress - minStress;

        float[] GradStressFront = new float[StressFront.Length];
        float[] GradStressBack = new float[StressBack.Length];

        for (int i = 0; i < StressFront.Length; i++)
        {
            GradStressFront[i] = (StressFront[i] - minStress) / range;
            GradStressBack[i] = (StressBack[i] - minStress) / range;

        }

        #endregion

        #region Node Pos Import

        string[] NodeDataLines = SpatialDisplacement.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        Vector3[] Locations = new Vector3[NodeDataLines.Length];
        Vector3[] DeltaLocations = new Vector3[NodeDataLines.Length];
        Vector3[] NewLocations = new Vector3[NodeDataLines.Length];

        for (int i = 1; i < NodeDataLines.Length; i++)
        {
            string[] coord = NodeDataLines[i].Split(","[0]);

            float coordx = float.Parse(coord[5]);
            float coordy = float.Parse(coord[6]);
            float coordz = float.Parse(coord[7]);

            float deltax = float.Parse(coord[12]);
            float deltay = float.Parse(coord[13]);
            float deltaz = float.Parse(coord[14]);

            float newx = coordx + deltax;
            float newy = coordy + deltay;
            float newz = coordz + deltaz;

            Locations[i] = new Vector3(coordx, coordy, coordz);
            DeltaLocations[i] = new Vector3(deltax, deltay, deltaz);
            NewLocations[i] = new Vector3(newx, newy, newz);
        }

        #endregion


        GameObject model1undeformed = new GameObject("Model 1 Undeformed");
        model1undeformed.transform.position = new Vector3(0, 0, 0);
        model1undeformed.transform.parent = GameObject.Find("Model Space").transform;

        GameObject model1deformed = new GameObject("Model 1 Deformed");
        model1deformed.transform.position = new Vector3(0, 0, 0);
        model1deformed.transform.parent = GameObject.Find("Model Space").transform;

        model1deformed.AddComponent<MeshCombine>();

        model1undeformed.SetActive(false);

        #region Element Mesh Creation

        string[] ElemDataLines = ElemConnectivity.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < ElemDataLines.Length; i++)
        {
            string[] element = ElemDataLines[i].Split(","[0]);

            Vector3[] VerticesUndeformed =
            {
                Locations[int.Parse(element[3])],
                Locations[int.Parse(element[4])],
                Locations[int.Parse(element[5])],
                Locations[int.Parse(element[6])]
            };

            Vector3[] VerticesDeformed =
{
                NewLocations[int.Parse(element[3])],
                NewLocations[int.Parse(element[4])],
                NewLocations[int.Parse(element[5])],
                NewLocations[int.Parse(element[6])]
            };

            int[] TrianglesFront =
            {
                0,1,2,
                0,2,3
            };

            int[] TrianglesBack =
            {
                0,2,1,
                0,3,2
            };

            #region Deformed Mesh

            GameObject elemfrontdeformed = new GameObject("Element " + i + " Front" + " Deformed");
            elemfrontdeformed.AddComponent<MeshRenderer>();
            Mesh meshfrontdeformed = elemfrontdeformed.AddComponent<MeshFilter>().mesh;
            meshfrontdeformed.vertices = VerticesDeformed;
            meshfrontdeformed.triangles = TrianglesFront;
            elemfrontdeformed.transform.parent = model1deformed.transform;
            //elemfrontdeformed.GetComponent<MeshRenderer>().material.color = grad.Evaluate(GradStressFront[i - 1]);

            #region Material Assignment

            if (GradStressFront[i - 1] < 0.0769 && GradStressFront[i - 1] >= 0)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C13;
            }
            else if (GradStressFront[i - 1] < 0.1538 && GradStressFront[i - 1] >= 0.0769)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C12;
            }
            else if (GradStressFront[i - 1] < 0.2307 && GradStressFront[i - 1] >= 0.1538)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C11;
            }
            else if (GradStressFront[i - 1] < 0.3077 && GradStressFront[i - 1] >= 0.2307)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C10;
            }
            else if (GradStressFront[i - 1] < 0.3846 && GradStressFront[i - 1] >= 0.3077)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C9;
            }
            else if (GradStressFront[i - 1] < 0.4615 && GradStressFront[i - 1] >= 0.3846)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C8;
            }
            else if (GradStressFront[i - 1] < 0.5385 && GradStressFront[i - 1] >= 0.4615)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C7;
            }
            else if (GradStressFront[i - 1] < 0.6154 && GradStressFront[i - 1] >= 0.5385)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C6;
            }
            else if (GradStressFront[i - 1] < 0.6923 && GradStressFront[i - 1] >= 0.6154)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C5;
            }
            else if (GradStressFront[i - 1] < 0.7692 && GradStressFront[i - 1] >= 0.6923)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C4;
            }
            else if (GradStressFront[i - 1] < 0.8462 && GradStressFront[i - 1] >= 0.7692)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C3;
            }
            else if (GradStressFront[i - 1] < 0.9231 && GradStressFront[i - 1] >= 0.8462)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C2;
            }
            else if (GradStressFront[i - 1] <= 1 && GradStressFront[i - 1] >= 0.9231)
            {
                elemfrontdeformed.GetComponent<MeshRenderer>().material = C1;
            }

            #endregion

            GameObject elembackdeformed = new GameObject("Element " + i + " Back" + " Deformed");
            elembackdeformed.AddComponent<MeshRenderer>();
            Mesh meshbackdeformed = elembackdeformed.AddComponent<MeshFilter>().mesh;
            meshbackdeformed.vertices = VerticesDeformed;
            meshbackdeformed.triangles = TrianglesBack;
            elembackdeformed.transform.parent = model1deformed.transform;
            //elembackdeformed.GetComponent<MeshRenderer>().material.color = grad.Evaluate(GradStressBack[i - 1]);

            #region Material Assignment

            if (GradStressBack[i - 1] < 0.0769 && GradStressBack[i - 1] >= 0)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C13;
            }
            else if (GradStressBack[i - 1] < 0.1538 && GradStressBack[i - 1] >= 0.0769)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C12;
            }
            else if (GradStressBack[i - 1] < 0.2307 && GradStressBack[i - 1] >= 0.1538)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C11;
            }
            else if (GradStressBack[i - 1] < 0.3077 && GradStressBack[i - 1] >= 0.2307)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C10;
            }
            else if (GradStressBack[i - 1] < 0.3846 && GradStressBack[i - 1] >= 0.3077)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C9;
            }
            else if (GradStressBack[i - 1] < 0.4615 && GradStressBack[i - 1] >= 0.3846)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C8;
            }
            else if (GradStressBack[i - 1] < 0.5385 && GradStressBack[i - 1] >= 0.4615)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C7;
            }
            else if (GradStressBack[i - 1] < 0.6154 && GradStressBack[i - 1] >= 0.5385)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C6;
            }
            else if (GradStressBack[i - 1] < 0.6923 && GradStressBack[i - 1] >= 0.6154)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C5;
            }
            else if (GradStressBack[i - 1] < 0.7692 && GradStressBack[i - 1] >= 0.6923)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C4;
            }
            else if (GradStressBack[i - 1] < 0.8462 && GradStressBack[i - 1] >= 0.7692)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C3;
            }
            else if (GradStressBack[i - 1] < 0.9231 && GradStressBack[i - 1] >= 0.8462)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C2;
            }
            else if (GradStressBack[i - 1] <= 1 && GradStressBack[i - 1] >= 0.9231)
            {
                elembackdeformed.GetComponent<MeshRenderer>().material = C1;
            }

            #endregion

            #endregion

        }

        #endregion

        GameObject ModelSpace = GameObject.Find("Model Space");

        model1deformed.SendMessage("CombineMeshNew");

    }


}
