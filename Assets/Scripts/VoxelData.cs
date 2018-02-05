using UnityEngine;
using System.Collections;

[System.Serializable]
public class VoxelData
{
   
    [SerializeField]
    float xOrigin;
    [SerializeField]
    float yOrigin;
    [SerializeField]
    float zOrigin;

    public float scale = 3f;

    public int width;
    public int height;
    public int depth;


    public byte[,,] _data;
    public byte[,,] data
    {
        get
        {
            if (_data == null)
            {
                //xOrigin = Random.value;
                //yOrigin = Random.value;
                //zOrigin = Random.value;
                _data = new byte[width, depth, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < depth; y++)
                    {
                        for (int z = 0; z < height; z++)
                        {
                            _data[x, y, z] = 1;
                        }
                    }
                }
            }
            return _data;
        }
    }

   

    public byte this[int x,int y,int z]
    {
        get
        {
            if (x < 0 
                || y < 0 
                || z < 0
                || x >= width 
                || y >= height 
                || z >= depth)
            {
                return 0;
            }

            return data[x, y, z];
        }

        set
        {
            if (x < 0 
                || y < 0 
                || z < 0 
                || x >= width 
                || y >= height 
                || z >= depth)
            {
                return;
            }

            data[x, y, z] = value;
            if (dataChanged != null)
            {
                dataChanged();
            }
        }
    }

    public byte PerlinNoise(int x, int y, int z)
    {
        float xCoord = xOrigin + (float)x / width * scale;
        float yCoord = yOrigin + (float)y / height * scale;
        float zCoord = zOrigin + (float)z / depth * scale;

        float xySample = Mathf.PerlinNoise(xCoord, yCoord);
        float xzSample= Mathf.PerlinNoise(xCoord, zCoord);
        float yzSample = Mathf.PerlinNoise(yCoord, zCoord);

        float result = (xySample + xzSample + yzSample) / 3 * 256;
        return (byte)result;
    }


    public byte LinearSample(float x, float y, float z)
    {
        float coordX = x * width;
        float coordY = y * depth;
        float coordZ = z * height;
        
        int xx = Mathf.FloorToInt(coordX);
        int yy = Mathf.FloorToInt(coordY);
        int zz = Mathf.FloorToInt(coordZ);

        float tx = coordX - xx;
        float ty = coordY - yy;
        float tz = coordZ - zz;

        float p0 = this[xx, yy, zz];
        float p1 = this[xx, yy, zz + 1];
        float p2 = this[xx, yy + 1, zz];
        float p3 = this[xx, yy + 1, zz + 1];
        float p4 = this[xx + 1, yy, zz];
        float p5 = this[xx + 1, yy, zz + 1];
        float p6 = this[xx + 1, yy + 1, zz];
        float p7 = this[xx + 1, yy + 1, zz + 1];

        float np = Mathf.Lerp(Mathf.Lerp(p0, p2, ty), Mathf.Lerp(p4, p6, ty), tx);
        float fp = Mathf.Lerp(Mathf.Lerp(p1, p3, ty), Mathf.Lerp(p5, p7, ty), tx);

        return (byte)Mathf.Lerp(np, fp, tz);
    }

    public event System.Action dataChanged;
}
