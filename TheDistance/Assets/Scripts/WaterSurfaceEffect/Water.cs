using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    public static float k = 0.025f;
    public static float d = 0.025f;

    [System.Serializable]
    public class Spring
    {
        public float height;
        public float velocity;

        public Spring()
        {
            height = 0;
            velocity = 0;
        }
    };

    public float waveHeight = 1;


    public List<Spring> springs = new List<Spring>();

    float[] height = new float[256];
    float[] leftDeltas = new float[256];
    float[] rightDeltas = new float[256];
    int l = 256;

    MeshFilter mr;

    public Vector3 leftButtom; public Vector3 leftUp;
    public Vector3 rightButtom; public Vector3 rightUp;

    void UpdateBound()
    {
        MeshFilter mf = GetComponentInChildren<MeshFilter>();
        float sx = mr.mesh.bounds.size.x * mr.transform.localScale.x;
        float sy = mr.mesh.bounds.size.z * mr.transform.localScale.z;
        leftButtom = transform.position + new Vector3(-sx, -sy) / 2.0f;
        rightButtom = transform.position + new Vector3(sx, -sy) / 2.0f;
        leftUp = transform.position + new Vector3(-sx, sy) / 2.0f;
        rightUp = transform.position + new Vector3(sx, sy) / 2.0f;

        Debug.DrawLine(leftUp, leftUp + Vector3.up * 100, Color.red);
        Debug.DrawLine(rightUp, rightUp + Vector3.up * 100, Color.red);
        Debug.DrawLine(leftButtom, leftButtom + Vector3.down * 100, Color.red);
        Debug.DrawLine(rightButtom, rightButtom + Vector3.down * 100, Color.red);
   }

    private void Start()
    {
        for (int i = 0; i < l; i++)
        {
            springs.Add(new Spring());
        }
        mr = GetComponentInChildren<MeshFilter>();
        GetComponent<MeshRenderer>().sortingLayerName = "Platform";
        GetComponent<MeshRenderer>().sortingOrder = -1;
    }

    public float Intersect(Box box)
    {
        UpdateBound();
        /*
        Debug.DrawLine(leftUp, Vector3.up, Color.red);
        Debug.DrawLine(rightUp, Vector3.up, Color.red);
        Debug.DrawLine(leftButtom, Vector3.down, Color.red);
        Debug.DrawLine(rightButtom, Vector3.down, Color.red);
         */

        if (box.leftButtom.y > leftUp.y)
            return 0;

        int leftIdx = GetIndex(box.leftButtom.x);
        int rightIdx = GetIndex(box.rightButtom.x);
        float delta = (rightButtom.x - leftButtom.x) / springs.Count;

        float percent = 0.0f;
        if (box.leftUp.y < leftUp.y)
            percent = 1.0f;
        else
        {
#if true
            percent = (leftUp.y - box.leftButtom.y) / (box.leftUp.y - box.leftButtom.y);
#else
            for (int i = leftIdx; i <= rightIdx; i++)
            {
                percent += (leftUp.y + springs[i].height - box.leftButtom.y) * delta;
            }
            percent /= ((box.leftUp.y - box.leftButtom.y) * (box.rightUp.x - box.leftUp.x));
#endif
        }

        print("left: " + leftIdx);
        print("right: " + rightIdx);

        for (int i = leftIdx; i <= rightIdx; i++)
        {
            springs[i].velocity = -box.move.y * percent / 5.0f;
            //springs[i].height = -leftUp.y + box.leftButtom.y;
        }

        return percent;
    }

    int GetIndex(float x)
    {
        UpdateBound();
        float dist = x - leftUp.x;
        float tmp = dist / (rightUp.x - leftUp.x) * springs.Count;
        return (Mathf.RoundToInt(tmp));
    }

    /*
    float FACTOR = 4.0f;
    float DAMP_FACTOR = 0.9985f;
    float BASE_FACTOR = 0.2f;
     */ 
    float FACTOR = 4.0f;
    float DAMP_FACTOR = 0.9985f;
    float BASE_FACTOR = 0.2f;

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            print("mouse pos: " + pos);
            pos.z = -200;
            int idx = GetIndex(Camera.main.ScreenToWorldPoint(pos).x);
            print("water surface index is : " + idx);
        }
    }

    void AddSplash(int idx)
    {
        for(int i = idx - 10; i <= idx + 10; i++)
        {
            springs[i].velocity += (10 - Mathf.Abs(i - idx)) / 30.0f;
        }
    }

    public void AddSplash(float left, float right)
    {
        int idx_l = GetIndex(left);
        int idx_r = GetIndex(right);
        print("left: " + idx_l);
        print("right : " + idx_r);
        AddSplash(idx_l);
        AddSplash(idx_r);
        int mid = (idx_r + idx_l) / 2;
        int m = mid - idx_l;
        /*
        for(int idx = idx_l; idx <= idx_r; idx++)
        {
            springs[idx].velocity += (m - Mathf.Abs(mid - idx)) / 30.0f;
        }
         */ 
    }

    int ITERATION = 10;

    private void Update()
    {

        //HandleInput();

        UpdateBound();
  

        for (int j = 0; j < ITERATION; j++)
        {

            for (int i = 0; i < springs.Count; i++)
            {
                float fromLeft = 0, fromRight = 0, fromBase = 0;
                if (i > 0)
                {
                    leftDeltas[i] = -springs[i].height + springs[i - 1].height;
                    fromLeft = leftDeltas[i] * FACTOR;
                }
                if (i < springs.Count - 1)
                {
                    rightDeltas[i] = -springs[i].height + springs[i + 1].height;
                    fromRight = rightDeltas[i] * FACTOR;
                }

                float dy = 0 - springs[i].height;
                fromBase = dy * BASE_FACTOR;

                float force = 0;
                force += fromLeft;
                force += fromRight;
                force += fromBase;

                float a = force * Time.deltaTime;
                springs[i].velocity *= DAMP_FACTOR;
                springs[i].velocity += a;
                springs[i].height += springs[i].velocity * Time.deltaTime;
            }
        }

        // export this to the shader
        for (int i = 0; i < springs.Count; i++)
        {
            height[i] = springs[i].height;
        }


        GetComponentInChildren<MeshRenderer>().material.SetFloatArray("_height", height);
    }

    public void Splash(int index, float speed)
    {
    }
}
