using System;
using System.Linq;
using UnityEngine;

public class MeshImport : MonoBehaviour
{

    [Tooltip("Node Position and property in form of text file")]
    public TextAsset NodeData;

    [Tooltip("Element information in form of text file")]
    public TextAsset ElemData;

    public Transform ModelCreationCube;

	// Use this for initialization
	public void LoadModel() {

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

        #region Node and Stress import

        string[] NodeDataLines = NodeData.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        Vector3[] Locations = new Vector3[NodeDataLines.Length];
        float[] stress = new float[NodeDataLines.Length];

        for (int i = 1; i < NodeDataLines.Length; i++)
        {
            string[] coord = NodeDataLines[i].Split(","[0]);

            float coordx = float.Parse(coord[3]);
            float coordy = float.Parse(coord[2]);
            float coordz = float.Parse(coord[1]);

            stress[i] = float.Parse(coord[4]);
            Locations[i] = new Vector3(coordx, coordy, coordz);
        }

        float maxStress = stress.Max();
        float minStress = stress.Min();
        float range = maxStress - minStress;

        float[] gradstress = new float[stress.Length];
        for (int i = 1; i < stress.Length; i++)
        {
            gradstress[i] = (stress[i] - minStress) / range;
        }

        #endregion

        GameObject scaleconfig = GameObject.Find("ColourScale");
        scaleconfig.GetComponent<ScaleConfig>().SetScaleValues(maxStress, minStress);

        GameObject model1 = new GameObject("model1");
        model1.transform.position = new Vector3(0, 0, 0);
        model1.transform.parent = GameObject.Find("ModelSpace").transform;
        model1.tag = "FEAModels";

        string[] ElemDataLines = ElemData.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // change i< once testing done

        for (int i=1; i < ElemDataLines.Length; i++)
        {
            string[] element = ElemDataLines[i].Split(","[0]);

            Vector3[] vertices = {
            Locations[int.Parse(element[8])],
            Locations[int.Parse(element[16])],
            Locations[int.Parse(element[9])],
            Locations[int.Parse(element[15])],
            Locations[int.Parse(element[17])],
            Locations[int.Parse(element[8])],
            Locations[int.Parse(element[15])],
            Locations[int.Parse(element[7])],
            Locations[int.Parse(element[14])],
            Locations[int.Parse(element[6])],
            Locations[int.Parse(element[17])],
            Locations[int.Parse(element[9])],
            Locations[int.Parse(element[20])],
            Locations[int.Parse(element[19])],
            Locations[int.Parse(element[18])],
            Locations[int.Parse(element[21])],
            Locations[int.Parse(element[4])],
            Locations[int.Parse(element[11])],
            Locations[int.Parse(element[3])],
            Locations[int.Parse(element[10])],
            Locations[int.Parse(element[2])],
            Locations[int.Parse(element[13])],
            Locations[int.Parse(element[5])],
            Locations[int.Parse(element[11])],
            Locations[int.Parse(element[13])],
            Locations[int.Parse(element[4])],
            Locations[int.Parse(element[12])],
            Locations[int.Parse(element[5])],
            Locations[int.Parse(element[20])],
            Locations[int.Parse(element[21])],
            Locations[int.Parse(element[8])],
            Locations[int.Parse(element[16])],
            Locations[int.Parse(element[9])]
        };
            int[] triangles =
            {
                0,3,1,
                2,1,4,
                3,7,8,
                4,8,9,
                4,1,3,
                4,3,8,
                5,12,6,
                6,13,7,
                13,17,18,
                12,16,17,
                12,17,6,
                17,13,6,
                7,13,8,
                8,14,9,
                13,18,19,
                19,20,14,
                13,19,14,
                13,14,8,
                9,14,10,
                10,15,11,
                20,21,14,
                21,22,15,
                21,15,10,
                21,10,14,
                18,23,19,
                19,24,20,
                23,25,26,
                24,26,27,
                19,23,24,
                23,26,24,
                25,28,26,
                26,29,27,
                28,30,31,
                31,32,29,
                28,29,26,
                28,31,29,
            };

            float[] uvus = { 0, 0.124023438f, 0.248046875f, 0.372070313f, 0.49609375f, 0.620117188f, 0.744140625f, 0.868164063f, 0.9921875f };
            float[] uvvs = { 0, 0.166015625f, 0.33203125f, 0.498046875f, 0.6640625f, 0.830078125f, 0.99609375f };
            Vector2[] uvs =
            {
            new Vector2(uvus[0], uvvs[2]),
            new Vector2(uvus[0], uvvs[3]),
            new Vector2(uvus[0], uvvs[4]),

            new Vector2(uvus[1], uvvs[2]),
            new Vector2(uvus[1], uvvs[4]),

            new Vector2(uvus[2], uvvs[0]),
            new Vector2(uvus[2], uvvs[1]),
            new Vector2(uvus[2], uvvs[2]),
            new Vector2(uvus[2], uvvs[3]),
            new Vector2(uvus[2], uvvs[4]),
            new Vector2(uvus[2], uvvs[5]),
            new Vector2(uvus[2], uvvs[6]),

            new Vector2(uvus[3], uvvs[0]),
            new Vector2(uvus[3], uvvs[2]),
            new Vector2(uvus[3], uvvs[4]),
            new Vector2(uvus[3], uvvs[6]),

            new Vector2(uvus[4], uvvs[0]),
            new Vector2(uvus[4], uvvs[1]),
            new Vector2(uvus[4], uvvs[2]),
            new Vector2(uvus[4], uvvs[3]),
            new Vector2(uvus[4], uvvs[4]),
            new Vector2(uvus[4], uvvs[5]),
            new Vector2(uvus[4], uvvs[6]),

            new Vector2(uvus[5], uvvs[2]),
            new Vector2(uvus[5], uvvs[4]),

            new Vector2(uvus[6], uvvs[2]),
            new Vector2(uvus[6], uvvs[3]),
            new Vector2(uvus[6], uvvs[4]),

            new Vector2(uvus[7], uvvs[2]),
            new Vector2(uvus[7], uvvs[4]),

            new Vector2(uvus[8], uvvs[2]),
            new Vector2(uvus[8], uvvs[3]),
            new Vector2(uvus[8], uvvs[4])

        };

            #region Texture Creation

            var texture1 = new Texture2D(512, 256, TextureFormat.ARGB32, false);

            int side1y1 = 85;
            int side1y2 = 169;
            int side1ymid = 127;
            int side1x1 = 0;
            int side1x2 = 126;
            int side1xmid = 63;

            for (int y = 85; y < 170; y++)
            {
                for (int x = 0; x < 127; x++)
                {
                    float d0 = Mathf.Sqrt(Mathf.Pow((x - side1x1), 2f) + Mathf.Pow((y - side1y1), 2f));
                    float d1 = Mathf.Sqrt(Mathf.Pow((x - side1x1), 2f) + Mathf.Pow((y - side1ymid), 2f));
                    float d2 = Mathf.Sqrt(Mathf.Pow((x - side1x1), 2f) + Mathf.Pow((y - side1y2), 2f));

                    float d3 = Mathf.Sqrt(Mathf.Pow((x - side1xmid), 2f) + Mathf.Pow((y - side1y1), 2f));
                    float d4 = Mathf.Sqrt(Mathf.Pow((x - side1xmid), 2f) + Mathf.Pow((y - side1y2), 2f));

                    float d5 = Mathf.Sqrt(Mathf.Pow((x - side1x2), 2f) + Mathf.Pow((y - side1y1), 2f));
                    float d6 = Mathf.Sqrt(Mathf.Pow((x - side1x2), 2f) + Mathf.Pow((y - side1ymid), 2f));
                    float d7 = Mathf.Sqrt(Mathf.Pow((x - side1x2), 2f) + Mathf.Pow((y - side1y2), 2f));

                    float z0 = gradstress[int.Parse(element[8])];
                    float z1 = gradstress[int.Parse(element[16])];
                    float z2 = gradstress[int.Parse(element[9])];
                    float z3 = gradstress[int.Parse(element[15])];
                    float z4 = gradstress[int.Parse(element[17])];
                    float z5 = gradstress[int.Parse(element[7])];
                    float z6 = gradstress[int.Parse(element[14])];
                    float z7 = gradstress[int.Parse(element[6])];

                    float zidi = (z0 / d0) + (z1 / d1) + (z2 / d2) + (z3 / d3) + (z4 / d4) + (z5 / d5) + (z6 / d6) + (z7 / d7);
                    float inversedi = (1 / d0) + (1 / d1) + (1 / d2) + (1 / d3) + (1 / d4) + (1 / d5) + (1 / d6) + (1 / d7);

                    float zxy = (zidi / inversedi);

                    texture1.SetPixel(x, y, grad.Evaluate(zxy));
                }
            }
            texture1.SetPixel(side1x1, side1y1, grad.Evaluate(gradstress[int.Parse(element[8])]));
            texture1.SetPixel(side1x1, side1ymid, grad.Evaluate(gradstress[int.Parse(element[16])]));
            texture1.SetPixel(side1x1, side1y2, grad.Evaluate(gradstress[int.Parse(element[9])]));

            texture1.SetPixel(side1xmid, side1y1, grad.Evaluate(gradstress[int.Parse(element[15])]));
            texture1.SetPixel(side1xmid, side1y2, grad.Evaluate(gradstress[int.Parse(element[17])]));

            texture1.SetPixel(side1x2, side1y1, grad.Evaluate(gradstress[int.Parse(element[7])]));
            texture1.SetPixel(side1x2, side1ymid, grad.Evaluate(gradstress[int.Parse(element[14])]));
            texture1.SetPixel(side1x2, side1y2, grad.Evaluate(gradstress[int.Parse(element[6])]));

            int side2y1 = 0;
            int side2y2 = 84;
            int side2ymid = 42;
            int side2x1 = 127;
            int side2x2 = 253;
            int side2xmid = 190;

            for (int y = 0; y < 85; y++)
            {
                for (int x = 127; x < 254; x++)
                {
                    float d0 = Mathf.Sqrt(Mathf.Pow((x - side2x1), 2f) + Mathf.Pow((y - side2y1), 2f));
                    float d1 = Mathf.Sqrt(Mathf.Pow((x - side2x1), 2f) + Mathf.Pow((y - side2ymid), 2f));
                    float d2 = Mathf.Sqrt(Mathf.Pow((x - side2x1), 2f) + Mathf.Pow((y - side2y2), 2f));

                    float d3 = Mathf.Sqrt(Mathf.Pow((x - side2xmid), 2f) + Mathf.Pow((y - side2y1), 2f));
                    float d4 = Mathf.Sqrt(Mathf.Pow((x - side2xmid), 2f) + Mathf.Pow((y - side2y2), 2f));

                    float d5 = Mathf.Sqrt(Mathf.Pow((x - side2x2), 2f) + Mathf.Pow((y - side2y1), 2f));
                    float d6 = Mathf.Sqrt(Mathf.Pow((x - side2x2), 2f) + Mathf.Pow((y - side2ymid), 2f));
                    float d7 = Mathf.Sqrt(Mathf.Pow((x - side2x2), 2f) + Mathf.Pow((y - side2y2), 2f));

                    float z0 = gradstress[int.Parse(element[8])];
                    float z1 = gradstress[int.Parse(element[15])];
                    float z2 = gradstress[int.Parse(element[7])];
                    float z3 = gradstress[int.Parse(element[20])];
                    float z4 = gradstress[int.Parse(element[19])];
                    float z5 = gradstress[int.Parse(element[4])];
                    float z6 = gradstress[int.Parse(element[11])];
                    float z7 = gradstress[int.Parse(element[3])];

                    float zidi = (z0 / d0) + (z1 / d1) + (z2 / d2) + (z3 / d3) + (z4 / d4) + (z5 / d5) + (z6 / d6) + (z7 / d7);
                    float inversedi = (1 / d0) + (1 / d1) + (1 / d2) + (1 / d3) + (1 / d4) + (1 / d5) + (1 / d6) + (1 / d7);

                    float zxy = (zidi / inversedi);

                    texture1.SetPixel(x, y, grad.Evaluate(zxy));
                }
            }
            texture1.SetPixel(side2x1, side2y1, grad.Evaluate(gradstress[int.Parse(element[8])]));
            texture1.SetPixel(side2x1, side2ymid, grad.Evaluate(gradstress[int.Parse(element[15])]));
            texture1.SetPixel(side2x1, side2y2, grad.Evaluate(gradstress[int.Parse(element[7])]));

            texture1.SetPixel(side2xmid, side2y1, grad.Evaluate(gradstress[int.Parse(element[20])]));
            texture1.SetPixel(side2xmid, side2y2, grad.Evaluate(gradstress[int.Parse(element[19])]));

            texture1.SetPixel(side2x2, side2y1, grad.Evaluate(gradstress[int.Parse(element[4])]));
            texture1.SetPixel(side2x2, side2ymid, grad.Evaluate(gradstress[int.Parse(element[11])]));
            texture1.SetPixel(side2x2, side2y2, grad.Evaluate(gradstress[int.Parse(element[3])]));


            int side3y1 = 85;
            int side3y2 = 169;
            int side3ymid = 127;
            int side3x1 = 127;
            int side3x2 = 253;
            int side3xmid = 190;

            for (int y = 85; y < 170; y++)
            {
                for (int x = 127; x < 254; x++)
                {
                    float d0 = Mathf.Sqrt(Mathf.Pow((x - side3x1), 2f) + Mathf.Pow((y - side3y1), 2f));
                    float d1 = Mathf.Sqrt(Mathf.Pow((x - side3x1), 2f) + Mathf.Pow((y - side3ymid), 2f));
                    float d2 = Mathf.Sqrt(Mathf.Pow((x - side3x1), 2f) + Mathf.Pow((y - side3y2), 2f));

                    float d3 = Mathf.Sqrt(Mathf.Pow((x - side3xmid), 2f) + Mathf.Pow((y - side3y1), 2f));
                    float d4 = Mathf.Sqrt(Mathf.Pow((x - side3xmid), 2f) + Mathf.Pow((y - side3y2), 2f));

                    float d5 = Mathf.Sqrt(Mathf.Pow((x - side3x2), 2f) + Mathf.Pow((y - side3y1), 2f));
                    float d6 = Mathf.Sqrt(Mathf.Pow((x - side3x2), 2f) + Mathf.Pow((y - side3ymid), 2f));
                    float d7 = Mathf.Sqrt(Mathf.Pow((x - side3x2), 2f) + Mathf.Pow((y - side3y2), 2f));

                    float z0 = gradstress[int.Parse(element[7])];
                    float z1 = gradstress[int.Parse(element[14])];
                    float z2 = gradstress[int.Parse(element[6])];
                    float z3 = gradstress[int.Parse(element[19])];
                    float z4 = gradstress[int.Parse(element[18])];
                    float z5 = gradstress[int.Parse(element[3])];
                    float z6 = gradstress[int.Parse(element[10])];
                    float z7 = gradstress[int.Parse(element[2])];

                    float zidi = (z0 / d0) + (z1 / d1) + (z2 / d2) + (z3 / d3) + (z4 / d4) + (z5 / d5) + (z6 / d6) + (z7 / d7);
                    float inversedi = (1 / d0) + (1 / d1) + (1 / d2) + (1 / d3) + (1 / d4) + (1 / d5) + (1 / d6) + (1 / d7);

                    float zxy = (zidi / inversedi);

                    texture1.SetPixel(x, y, grad.Evaluate(zxy));
                }
            }
            texture1.SetPixel(side3x1, side3y1, grad.Evaluate(gradstress[int.Parse(element[7])]));
            texture1.SetPixel(side3x1, side3ymid, grad.Evaluate(gradstress[int.Parse(element[14])]));
            texture1.SetPixel(side3x1, side3y2, grad.Evaluate(gradstress[int.Parse(element[6])]));

            texture1.SetPixel(side3xmid, side3y1, grad.Evaluate(gradstress[int.Parse(element[19])]));
            texture1.SetPixel(side3xmid, side3y2, grad.Evaluate(gradstress[int.Parse(element[18])]));

            texture1.SetPixel(side3x2, side3y1, grad.Evaluate(gradstress[int.Parse(element[3])]));
            texture1.SetPixel(side3x2, side3ymid, grad.Evaluate(gradstress[int.Parse(element[10])]));
            texture1.SetPixel(side3x2, side3y2, grad.Evaluate(gradstress[int.Parse(element[2])]));

            int side4y1 = 170;
            int side4y2 = 254;
            int side4ymid = 212;
            int side4x1 = 127;
            int side4x2 = 253;
            int side4xmid = 190;

            for (int y = 170; y < 255; y++)
            {
                for (int x = 127; x < 254; x++)
                {
                    float d0 = Mathf.Sqrt(Mathf.Pow((x - side4x1), 2f) + Mathf.Pow((y - side4y1), 2f));
                    float d1 = Mathf.Sqrt(Mathf.Pow((x - side4x1), 2f) + Mathf.Pow((y - side4ymid), 2f));
                    float d2 = Mathf.Sqrt(Mathf.Pow((x - side4x1), 2f) + Mathf.Pow((y - side4y2), 2f));

                    float d3 = Mathf.Sqrt(Mathf.Pow((x - side4xmid), 2f) + Mathf.Pow((y - side4y1), 2f));
                    float d4 = Mathf.Sqrt(Mathf.Pow((x - side4xmid), 2f) + Mathf.Pow((y - side4y2), 2f));

                    float d5 = Mathf.Sqrt(Mathf.Pow((x - side4x2), 2f) + Mathf.Pow((y - side4y1), 2f));
                    float d6 = Mathf.Sqrt(Mathf.Pow((x - side4x2), 2f) + Mathf.Pow((y - side4ymid), 2f));
                    float d7 = Mathf.Sqrt(Mathf.Pow((x - side4x2), 2f) + Mathf.Pow((y - side4y2), 2f));

                    float z0 = gradstress[int.Parse(element[6])];
                    float z1 = gradstress[int.Parse(element[17])];
                    float z2 = gradstress[int.Parse(element[9])];
                    float z3 = gradstress[int.Parse(element[18])];
                    float z4 = gradstress[int.Parse(element[21])];
                    float z5 = gradstress[int.Parse(element[2])];
                    float z6 = gradstress[int.Parse(element[13])];
                    float z7 = gradstress[int.Parse(element[5])];

                    float zidi = (z0 / d0) + (z1 / d1) + (z2 / d2) + (z3 / d3) + (z4 / d4) + (z5 / d5) + (z6 / d6) + (z7 / d7);
                    float inversedi = (1 / d0) + (1 / d1) + (1 / d2) + (1 / d3) + (1 / d4) + (1 / d5) + (1 / d6) + (1 / d7);

                    float zxy = (zidi / inversedi);

                    texture1.SetPixel(x, y, grad.Evaluate(zxy));
                }
            }
            texture1.SetPixel(side4x1, side4y1, grad.Evaluate(gradstress[int.Parse(element[6])]));
            texture1.SetPixel(side4x1, side4ymid, grad.Evaluate(gradstress[int.Parse(element[17])]));
            texture1.SetPixel(side4x1, side4y2, grad.Evaluate(gradstress[int.Parse(element[9])]));

            texture1.SetPixel(side4xmid, side4y1, grad.Evaluate(gradstress[int.Parse(element[18])]));
            texture1.SetPixel(side4xmid, side4y2, grad.Evaluate(gradstress[int.Parse(element[21])]));

            texture1.SetPixel(side4x2, side4y1, grad.Evaluate(gradstress[int.Parse(element[2])]));
            texture1.SetPixel(side4x2, side4ymid, grad.Evaluate(gradstress[int.Parse(element[13])]));
            texture1.SetPixel(side4x2, side4y2, grad.Evaluate(gradstress[int.Parse(element[5])]));


            int side5y1 = 85;
            int side5y2 = 169;
            int side5ymid = 127;
            int side5x1 = 254;
            int side5x2 = 380;
            int side5xmid = 317;

            for (int y = 85; y < 170; y++)
            {
                for (int x = 254; x < 381; x++)
                {
                    float d0 = Mathf.Sqrt(Mathf.Pow((x - side5x1), 2f) + Mathf.Pow((y - side5y1), 2f));
                    float d1 = Mathf.Sqrt(Mathf.Pow((x - side5x1), 2f) + Mathf.Pow((y - side5ymid), 2f));
                    float d2 = Mathf.Sqrt(Mathf.Pow((x - side5x1), 2f) + Mathf.Pow((y - side5y2), 2f));

                    float d3 = Mathf.Sqrt(Mathf.Pow((x - side5xmid), 2f) + Mathf.Pow((y - side5y1), 2f));
                    float d4 = Mathf.Sqrt(Mathf.Pow((x - side5xmid), 2f) + Mathf.Pow((y - side5y2), 2f));

                    float d5 = Mathf.Sqrt(Mathf.Pow((x - side5x2), 2f) + Mathf.Pow((y - side5y1), 2f));
                    float d6 = Mathf.Sqrt(Mathf.Pow((x - side5x2), 2f) + Mathf.Pow((y - side5ymid), 2f));
                    float d7 = Mathf.Sqrt(Mathf.Pow((x - side5x2), 2f) + Mathf.Pow((y - side5y2), 2f));

                    float z0 = gradstress[int.Parse(element[3])];
                    float z1 = gradstress[int.Parse(element[10])];
                    float z2 = gradstress[int.Parse(element[2])];
                    float z3 = gradstress[int.Parse(element[11])];
                    float z4 = gradstress[int.Parse(element[13])];
                    float z5 = gradstress[int.Parse(element[4])];
                    float z6 = gradstress[int.Parse(element[12])];
                    float z7 = gradstress[int.Parse(element[5])];

                    float zidi = (z0 / d0) + (z1 / d1) + (z2 / d2) + (z3 / d3) + (z4 / d4) + (z5 / d5) + (z6 / d6) + (z7 / d7);
                    float inversedi = (1 / d0) + (1 / d1) + (1 / d2) + (1 / d3) + (1 / d4) + (1 / d5) + (1 / d6) + (1 / d7);

                    float zxy = (zidi / inversedi);

                    texture1.SetPixel(x, y, grad.Evaluate(zxy));
                }
            }
            texture1.SetPixel(side5x1, side5y1, grad.Evaluate(gradstress[int.Parse(element[3])]));
            texture1.SetPixel(side5x1, side5ymid, grad.Evaluate(gradstress[int.Parse(element[10])]));
            texture1.SetPixel(side5x1, side5y2, grad.Evaluate(gradstress[int.Parse(element[2])]));

            texture1.SetPixel(side5xmid, side5y1, grad.Evaluate(gradstress[int.Parse(element[11])]));
            texture1.SetPixel(side5xmid, side5y2, grad.Evaluate(gradstress[int.Parse(element[13])]));

            texture1.SetPixel(side5x2, side5y1, grad.Evaluate(gradstress[int.Parse(element[4])]));
            texture1.SetPixel(side5x2, side5ymid, grad.Evaluate(gradstress[int.Parse(element[12])]));
            texture1.SetPixel(side5x2, side5y2, grad.Evaluate(gradstress[int.Parse(element[5])]));


            int side6y1 = 85;
            int side6y2 = 169;
            int side6ymid = 127;
            int side6x1 = 381;
            int side6x2 = 507;
            int side6xmid = 444;

            for (int y = 85; y < 170; y++)
            {
                for (int x = 381; x < 508; x++)
                {
                    float d0 = Mathf.Sqrt(Mathf.Pow((x - side6x1), 2f) + Mathf.Pow((y - side6y1), 2f));
                    float d1 = Mathf.Sqrt(Mathf.Pow((x - side6x1), 2f) + Mathf.Pow((y - side6ymid), 2f));
                    float d2 = Mathf.Sqrt(Mathf.Pow((x - side6x1), 2f) + Mathf.Pow((y - side6y2), 2f));

                    float d3 = Mathf.Sqrt(Mathf.Pow((x - side6xmid), 2f) + Mathf.Pow((y - side6y1), 2f));
                    float d4 = Mathf.Sqrt(Mathf.Pow((x - side6xmid), 2f) + Mathf.Pow((y - side6y2), 2f));

                    float d5 = Mathf.Sqrt(Mathf.Pow((x - side6x2), 2f) + Mathf.Pow((y - side6y1), 2f));
                    float d6 = Mathf.Sqrt(Mathf.Pow((x - side6x2), 2f) + Mathf.Pow((y - side6ymid), 2f));
                    float d7 = Mathf.Sqrt(Mathf.Pow((x - side6x2), 2f) + Mathf.Pow((y - side6y2), 2f));

                    float z0 = gradstress[int.Parse(element[4])];
                    float z1 = gradstress[int.Parse(element[12])];
                    float z2 = gradstress[int.Parse(element[5])];
                    float z3 = gradstress[int.Parse(element[20])];
                    float z4 = gradstress[int.Parse(element[21])];
                    float z5 = gradstress[int.Parse(element[8])];
                    float z6 = gradstress[int.Parse(element[16])];
                    float z7 = gradstress[int.Parse(element[9])];

                    float zidi = (z0 / d0) + (z1 / d1) + (z2 / d2) + (z3 / d3) + (z4 / d4) + (z5 / d5) + (z6 / d6) + (z7 / d7);
                    float inversedi = (1 / d0) + (1 / d1) + (1 / d2) + (1 / d3) + (1 / d4) + (1 / d5) + (1 / d6) + (1 / d7);

                    float zxy = (zidi / inversedi);

                    texture1.SetPixel(x, y, grad.Evaluate(zxy));
                }
            }
            texture1.SetPixel(side6x1, side6y1, grad.Evaluate(gradstress[int.Parse(element[4])]));
            texture1.SetPixel(side6x1, side6ymid, grad.Evaluate(gradstress[int.Parse(element[12])]));
            texture1.SetPixel(side6x1, side6y2, grad.Evaluate(gradstress[int.Parse(element[5])]));

            texture1.SetPixel(side6xmid, side6y1, grad.Evaluate(gradstress[int.Parse(element[20])]));
            texture1.SetPixel(side6xmid, side6y2, grad.Evaluate(gradstress[int.Parse(element[21])]));

            texture1.SetPixel(side6x2, side6y1, grad.Evaluate(gradstress[int.Parse(element[8])]));
            texture1.SetPixel(side6x2, side6ymid, grad.Evaluate(gradstress[int.Parse(element[16])]));
            texture1.SetPixel(side6x2, side6y2, grad.Evaluate(gradstress[int.Parse(element[9])]));

            texture1.Apply();

            #endregion

            GameObject elem = new GameObject("elem" + i);
            elem.AddComponent<MeshRenderer>();
            Mesh mesh = elem.AddComponent<MeshFilter>().mesh;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            elem.GetComponent<MeshRenderer>().material.mainTexture = texture1;
            elem.GetComponent<MeshRenderer>().material.mainTexture.filterMode = FilterMode.Point;
            elem.transform.parent = model1.transform;

            //var pngData = texture1.EncodeToPNG();
            //File.WriteAllBytes("F:/Programming/FYP/GitHub/HoloLens-FEA-University-of-Bath/" + i + ".png", pngData);

        }

        model1.transform.position = ModelCreationCube.position;
    }


}
