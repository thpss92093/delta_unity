using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVertices : MonoBehaviour {

    public float verticesAngle;
    public List<Vector3> verticesList;
    public bool ifDone;
    
	// Use this for initialization
	void Start () {
        verticesList = getTopHalfVertices(GetComponent<MeshFilter>().mesh);
        // Debug.Log("total vertices is " + get_total_views());
        ifDone = true;
    }
	
	// Update is called once per frame
	void Update () {
        //test.transform.position = verticesList[(int)(Random.value * verticesList.Count)];
	}

    List<Vector3> getTopHalfVertices(Mesh mesh)
    {
        List<Vector3> list = new List<Vector3>();

        foreach (Vector3 v in mesh.vertices)
        {
            // convert to spherical coordinate and see if it's upward vertices
            float rho, theta,fi;
            rho = Vector3.Magnitude(v);
            theta = Mathf.Rad2Deg * Mathf.Acos(v.z / rho);
            fi = Mathf.Rad2Deg * Mathf.Atan(v.y / v.x);
            if (theta < verticesAngle)
            {
                if (list.Contains(v))
                    continue;
                list.Add(transform.TransformPoint(v));
                //Debug.DrawLine(Vector3.zero, transform.TransformPoint(v), Color.red, 10f);
                //Debug.Log(theta + ", with V = " + v);
            }
        }
        
        return list;
    }


    public Vector3 get_random_vertice()
    {
        Vector3 v = verticesList[(int)(Random.value * verticesList.Count)]; //?????(int)(Random.value * verticesList.Count)????
       
        v.y = 0.5f + Random.value * 1.7f;
        v.x = Random.Range(-0.7f, 0.7f);
        v.z = Random.Range(-0.7f, 0.7f);


        return v;
    }

    public Vector3 get_circle_vertice(Vector3 objPos, int c, string o)
    {
        Vector3 v = verticesList[(int)(Random.value * verticesList.Count)]; //?????(int)(Random.value * verticesList.Count)????

        if ((c / 18) == 0)        //0 ~ 39   close
        {
            v.y = objPos.y + 0.5f; //1.3f;
            if (o == "crayola" || o == "andes" || o == "milo")
                v.y = objPos.y + 0.4f;
        }    
        else if ((c / 18) == 1)  //40 ~ 79
            v.y = objPos.y + 0.7f; //1.5f;
        else                      //80 ~ 119   far
            v.y = objPos.y + 1.0f; //1.8f;

        if (c % 18 >= 9)
            v.y -= 0.1f;

            //v.x = totePos.x + 0.2f * Mathf.Cos(c * (Mathf.PI/ (180 / 12)));
            //v.z = totePos.z + 0.2f * Mathf.Sin(c * (Mathf.PI / (180 / 12)));
            //-90 ~ 90
        if (c % 18 < 9)    //top
        {
            v.x = objPos.x + 0.2f * Mathf.Cos(Mathf.PI*(30 + (c % 9) * 15)/180);//v.x = objPos.x + 0.2f * Mathf.Cos((c % 20) * (Mathf.PI / (180 / 18)));
            v.z = objPos.z + 0.2f * Mathf.Sin(Mathf.PI*(30 + (c % 9) * 15)/180);//v.z = objPos.z + 0.2f * Mathf.Sin((c % 20) * (Mathf.PI / (180 / 18)));
        }
        else                //side
        {
            v.x = objPos.x + (0.4f * (1 + (c / 40) * 1)) * Mathf.Cos(Mathf.PI*(30 + (c % 9) * 15) / 180);//v.x = objPos.x + (0.4f * (1 + (c / 40) * 1)) * Mathf.Cos((c % 20) * (Mathf.PI / (180 / 18)));    //0.4  0.6  0.8
            v.z = objPos.z + (0.4f * (1 + (c / 40) * 1)) * Mathf.Sin(Mathf.PI*(30 + (c % 9) * 15) / 180);//v.z = objPos.z + (0.4f * (1 + (c / 40) * 1)) * Mathf.Sin((c % 20) * (Mathf.PI / (180 / 18)));
        }

        return v;
    }
    public Vector3 get_look_vertice(Vector3 objPos, int c) ///look at pos
    {
        Vector3 v = verticesList[(int)(Random.value * verticesList.Count)];
        if (c % 18 < 9)     //0 ~ 19  40 ~ 59  80 ~ 99
        {
            v.y = objPos.y - 0.2f + Random.value * 0.1f; //0.85
            v.x = objPos.x + Random.value * 0.05f;
            v.z = objPos.z + Random.value * 0.05f;
        }
        else                //20 ~ 39  60 ~ 79  100 ~ 119
        {
            v.y = objPos.y - 0.1f + Random.value * 0.1f;   //1.0
            v.x = objPos.x + Random.value * 0.05f;
            v.z = objPos.z + Random.value * 0.05f;
        }
        return v;
    }
    public Vector3 get_random_look_vertice() ///look at pos
    {
        Vector3 v = Vector3.zero;
        v.y = Random.Range(-0.5f, 0.0f);
        v.x = Random.Range(-0.1f, 0.1f);
        v.z = Random.Range(-0.1f, 0.1f);
        //v.y = 0;
        //v.x = 0;
        //v.z = 0;
        // v.y = 0.5f + Random.value * 0.4f;
        // v.x = 0.07f - Random.value * 0.14f;
        // v.z = 0.07f - Random.value * 0.14f;

        return v;
    }
    public int get_total_views()
    {
        return verticesList.Count;
    }

    public Vector3 getVerticeAt(int cnt)
    {
        if (cnt >= verticesList.Count)
            return Vector3.zero;
        return verticesList[cnt];
    }

}
