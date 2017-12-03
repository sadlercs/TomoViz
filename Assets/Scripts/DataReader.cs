﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataReader : MonoBehaviour {

    public struct Data
    {
        public float[,] elev;
        public int[] eledim;

        public float[,,] vel;
        public Color[,,] colors;

        public int[] dim;
    }

    private Data data;
    private bool loaded = false;
    public Texture2D colorMap;
    public Color[] gradient;
    private float min = -3f;
    private float max = 3f;

    public GameObject longitude_slider;
    public GameObject latitude_slider;
    public GameObject elevation_slider;
    public Slider elevationScale_slider;
    public Toggle snap_toggle;

    public GameObject cam;

    private int xMin = 0;
    private int yMin = 0;
    private int zMin = 0;
    private int xMax = 0;
    private int yMax = 0;
    private int zMax = 0;

    private float elevationScale = 1f;
    Vector2Int xB, yB, zB;

    private bool intClamp;
    private bool bBox;
    public bool contourEnabled;


    // Use this for initialization
    void Start () {

        // End mesh creation info
        gradient = colorMap.GetPixels();

        StartCoroutine(ParseData("cascadia.dat"));
        //StartCoroutine(ParseData("Endeavor.dat"));
    }


    

    public delegate float DelVal(ref float low, ref float high, ref float val);

    private static float Contour(ref float low, ref float high, ref float val)
    {
        float rVal = Mathf.Round(val);
        return (rVal < low) ? low : (rVal > high) ? high : rVal;
    }

    private static float Clamp(ref float low, ref float high, ref float val)
    {
        return (val < low) ? low : (val > high) ? high : val;
    }

    public int GetLongestSide()
    {
        int longest = xB.y;
        if (yB.y > longest)
        {
            longest = yB.y;
        }
        if(zB.y > longest)
        {
            longest = zB.y;
        }
        return longest;
    }

    public void ScaleElevation()
    {
        elevationScale = elevationScale_slider.value;
        StartCoroutine(RebuildMesh());
    }

    public void BoundingBox()
    {
        bBox = !bBox;

        meshObj[6].SetActive(bBox);
        meshObj[7].SetActive(bBox);
        meshObj[8].SetActive(bBox);
        meshObj[9].SetActive(bBox);
    }

    public void SnapToBB()
    {

        Vector3 newPos = (snap_toggle.isOn) ? new Vector3(-xMax * .5f, zMax * .5f, -yMax * .5f) :
            new Vector3(-(xB.y - xB.x) * .5f, (zB.y - zB.x) * .5f, -(yB.y - yB.x) * .5f);

                 
        for (int i = 0; i < 6; ++i)
        {
            meshObj[i].transform.localPosition = newPos;
        }
    }


    public IEnumerator Reshade()
    {
        DelVal Shading;
        if (contourEnabled)
        {
            Shading = Contour;
        }
        else
        {
            Shading = Clamp;
        }


        float range = 256f / (max - min);
        float maxmin1 = max - 1f;
        for (int i = 0; i < zMax; ++i)
        {
            for (int j = 0; j < yMax; ++j)
            {
                for (int k = 0; k < xMax; ++k)
                {
                    data.colors[k, j, i] = gradient[(int)((Shading(ref min, ref maxmin1, ref data.vel[k, j, i]) + (float)max) * range)];
                }
            }
        }

        yield return RebuildMesh();

    }

    public void SetContour()
    {
        contourEnabled = !contourEnabled;
        
        StartCoroutine(Reshade());
    }



    public IEnumerator ParseData(string fileName)
    {
        Debug.Log("Loading Data");


        /*
        WWW file_get = new WWW(Application.dataPath + "/Resources/" + fileName); // Resources Folder Version

        while (!file_get.isDone)
        {
            yield return null;
        }
        */
        data = new Data();

        StreamReader inp_stm = new StreamReader(Application.dataPath + "/Resources/" + fileName);


        // Get Elevation Data

        string inp_ln = inp_stm.ReadLine();

        List<string> dimens = new List<string>(inp_ln.Split(' '));


        if (dimens.Count == 2)
        {
            data.eledim = new int[dimens.Count];

            int total = 1;

            for (int i = 0; i < dimens.Count; ++i)
            {
                data.eledim[i] = Convert.ToInt32(dimens[i]);
                total *= data.eledim[i];
            }

            //Populate data matrix O(n^3)

            xMax = data.eledim[0];
            Debug.Log("xMax: " + xMax);
            yMax = data.eledim[1];
            Debug.Log("yMax: " + yMax);

            data.elev = new float[xMax, yMax];
            
            // MatLab is column major order -> z,y,x
            for (int i = 0; i < yMax; ++i)
            {
                for (int j = 0; j < xMax; ++j)
                {
                    data.elev[j, i] = Convert.ToSingle(inp_stm.ReadLine());
                }
            }
        }
        else
        {
            Debug.LogError("Elevation dimensions are not correct: " + dimens.Count);
        }



        //file_get.Dispose();

        inp_ln = inp_stm.ReadLine();

        dimens = new List<string>(inp_ln.Split(' '));


        if (dimens.Count == 3)
        {
            data.dim = new int[dimens.Count];

            int total = 1;

            for (int i = 0; i < dimens.Count; ++i)
            {
                data.dim[i] = Convert.ToInt32(dimens[i]);
                total *= data.dim[i];
            }
           
            //Populate data matrix O(n^3)

                xMax = data.dim[0];
                Debug.Log("xMax: " + xMax);
                yMax = data.dim[1];
                Debug.Log("yMax: " + yMax);
                zMax = data.dim[2];
                Debug.Log("zMax: " + zMax);
                data.vel = new float[xMax, yMax, zMax];
                data.colors = new Color[xMax, yMax, zMax];


                // start counter after dimensions
                // change later to be of more general use
                int counter = 1;
               
                // MatLab is column major order -> z,y,x
                for (int i = 0; i < zMax; ++i) { 
                    for (int j = 0; j < yMax; ++j) { 
                        for (int k = 0; k < xMax; ++k) {
                            //data.vel[k, j, i] = Convert.ToSingle(arrayString[counter++]);
                            data.vel[k, j, i] = Convert.ToSingle(inp_stm.ReadLine());
                        }
                    }
                }

                inp_stm.Close();

                xB = new Vector2Int(0, xMax);
                yB = new Vector2Int(0, yMax);
                zB = new Vector2Int(0, zMax);

                yield return Reshade();


            //Assign the slider information

            longitude_slider.GetComponent<SliderConnection>().min.maxValue = xMax;
            longitude_slider.GetComponent<SliderConnection>().max.maxValue = xMax;
            longitude_slider.GetComponent<SliderConnection>().max.value = xMax;

            latitude_slider.GetComponent<SliderConnection>().min.maxValue = yMax;
            latitude_slider.GetComponent<SliderConnection>().max.maxValue = yMax;
            latitude_slider.GetComponent<SliderConnection>().max.value = yMax;

            elevation_slider.GetComponent<SliderConnection>().min.maxValue = zMax;
            elevation_slider.GetComponent<SliderConnection>().max.maxValue = zMax;
            elevation_slider.GetComponent<SliderConnection>().max.value = zMax;


                // Create bounding box

                #region BoundingBox

                float xh = xMax * 0.5f;
                float mxh = -xh; --xh;
                float yh = zMax * 0.5f;
                float myh = (-yh)+1;
                float zh = yMax * 0.5f;
                float mzh = -zh; --zh;


                LineRenderer lr = meshObj[6].GetComponent<LineRenderer>();
                lr.positionCount = 10;
                lr.SetPositions(
                new Vector3[] {
                    new Vector3(xh, yh, mzh), new Vector3(xh, yh, zh), new Vector3(mxh, yh, zh), new Vector3(mxh, yh, mzh), new Vector3(xh, yh, mzh),
                    new Vector3(xh, myh, mzh), new Vector3(xh, myh, zh), new Vector3(mxh, myh, zh), new Vector3(mxh, myh, mzh), new Vector3(xh, myh, mzh)
                });

                lr = meshObj[7].GetComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPositions(new Vector3[] { new Vector3(mxh, myh, mzh), new Vector3(mxh, yh, mzh) });

                lr = meshObj[8].GetComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPositions(new Vector3[] { new Vector3(mxh, myh, zh), new Vector3(mxh, yh, zh) });

                lr = meshObj[9].GetComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPositions(new Vector3[] { new Vector3(xh, myh, zh), new Vector3(xh, yh, zh) });

            #endregion BoundingBox

            SnapToBB();
            // End Create bounding box


        }
        else
        {
            Debug.LogError("Dimensions are not correct: " + dimens.Count);
        }


        Debug.Log("Data Loaded");

        cam.GetComponent<DragMouseOrbit>().SetDistance(GetLongestSide() * 2);
    }




    // Mesh Creation stuff

    private Mesh[] meshes;
    private Mesh mesh;
    public GameObject[] meshObj;
    public Material meshMat;

    // This builds the Square Mesh
    // Input bounds for x, y, z axis
    IEnumerator RebuildMesh()
    {
        meshes = new Mesh[6];

        for (int i = 0; i < 6; ++i)
        {
            meshObj[i].GetComponent<MeshFilter>().mesh = GetFaceMesh(i);
        }

        yield return null;

    }

    private Mesh GetFaceMesh(int face)
    {

        //Debug.Log("hSize: " + hSize);
        //Debug.Log("wSize: " + wSize);
        

        int xMax1 = xB.y - 1;
        int yMax1 = yB.y - 1;
        int zMax1 = zB.y - 1;


        //float a, b, c, d, e, f = 0f;
        int hSize = 0;
        int wSize = 0;

        switch (face) {
            case 0: hSize = (yB.y - yB.x); wSize = (xB.y - xB.x); break;  // Top 
            case 1: hSize = (yB.y - yB.x); wSize = (xB.y - xB.x); break;  // Bottom
            case 2: hSize = (yB.y - yB.x); wSize = (zB.y - zB.x); break;  // Left 
            case 3: hSize = (yB.y - yB.x); wSize = (zB.y - zB.x); break;  // Right
            case 4: hSize = (xB.y - xB.x); wSize = (zB.y - zB.x); break;  // Front
            case 5: hSize = (xB.y - xB.x); wSize = (zB.y - zB.x); break;  // Back
        }

        hSize = Math.Abs(hSize) - 1;
        wSize = Math.Abs(wSize) - 1;
        int size = (wSize + 1) * (hSize + 1);
        Vector3[] vertices = new Vector3[size];
        Color[] colors = new Color[size];

        
        // North Side

        if (face == 0)
        {
            for (int i = 0, y = yB.x; y <= hSize; y++)
            {
                for (int x = xB.x; x <= wSize; x++, i++)
                {
                    vertices[i] = new Vector3(x, zB.x + data.elev[x,y] * elevationScale, y);
                    colors[i] = data.colors[x, y, zB.x];
                }
            }
        }
        else if (face == 1)
        {
            for (int i = 0, y = yB.x; y <= hSize; y++)
            {
                for (int x = xB.x; x <= wSize; x++, i++)
                {
                    vertices[i] = new Vector3(x, -zMax1 + data.elev[x, y] * elevationScale, y);
                    colors[i] = data.colors[x, y, zMax1];
                }
            }
        }
        else if (face == 2)
        {
            for (int i = 0, y = yB.x; y <= hSize; y++)
            {
                for (int x = zB.x; x <= wSize; x++, i++)
                {
                    vertices[i] = new Vector3(xB.x, -x + data.elev[xB.x, y] * elevationScale, y);
                    colors[i] = data.colors[xB.x, y, x];
                }
            }
        }
        else if (face == 3)
        {
            for (int i = 0, y = yB.x; y <= hSize; y++)
            {
                for (int x = zB.x; x <= wSize; x++, i++)
                {
                    vertices[i] = new Vector3(xMax1, -x + data.elev[xMax1, y] * elevationScale, y);
                    colors[i] = data.colors[xMax1, y, x];
                }
            }
            
        }
        else if (face == 4)
        {
            for (int i = 0, y = xB.x; y <= hSize; y++)
            {
                for (int x = zB.x; x <= wSize; x++, i++)
                {
                    vertices[i] = new Vector3(y, -x + data.elev[y, yB.x] * elevationScale, yB.x);
                    colors[i] = data.colors[y, yB.x, x];
                }
            }
            
        }
        else if (face == 5)
        {
            for (int i = 0, y = xB.x; y <= hSize; y++)
            {
                for (int x = zB.x; x <= wSize; x++, i++)
                {
                    vertices[i] = new Vector3(y, -x + data.elev[y, yMax1] * elevationScale, yMax1);
                    colors[i] = data.colors[y, yMax1, x];
                }
            }
        }

        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.colors = colors;

        int[] triangles = new int[size * 6];
        for (int ti = 0, vi = 0, y = 0; y < hSize; y++, vi++)
        {
            for (int x = 0; x < wSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + wSize + 1;
                triangles[ti + 5] = vi + wSize + 2;
            }
        }
        mesh.triangles = triangles;
        //Debug.Log(triangles.Length);

        //Debug.Log(xB + " " + yB + " " + zB);
        return mesh;
    }


    public void ChangeSlider(int i)
    {
        
        switch (i)
        {
            //Longitude
            case 0:
                xB = new Vector2Int((int)longitude_slider.GetComponent<SliderConnection>().min.value,(int)longitude_slider.GetComponent<SliderConnection>().max.value);
                break;

            // Latitude
            case 1:
                yB = new Vector2Int((int)latitude_slider.GetComponent<SliderConnection>().min.value, (int)latitude_slider.GetComponent<SliderConnection>().max.value);
                break;

            // Elevation
            case 2:
                zB = new Vector2Int((int)elevation_slider.GetComponent<SliderConnection>().min.value, (int)elevation_slider.GetComponent<SliderConnection>().max.value);
                break;
        }

        SnapToBB();

        //Debug.Log(xB + " " + yB + " " + zB );
        StartCoroutine(RebuildMesh());

    }


}