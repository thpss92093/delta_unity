using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenCvSharp;
using NWH;

using Rect =  UnityEngine.Rect;

public class CameraControllerMultiple : MonoBehaviour {

    // time after taking each picture, deafault t=0
    public float timeForEachPics;

    public CameraVertices camVertices;

    // link corresponding camera and object;
    public GameObject renderCam;
    public GameObject[] renderObj;
    public GameObject renderBackground;
    ObjectController objectController;
    public Light lt;

    public IniFile ini;
    string dataPath;

    public string folderName;
    public int maximumScene;
    public int sceneCnt;
    public int maximumImage;
    public int imageCnt;
    public int objNumber;

    // Use this for initialization
    void Start() {
        dataPath = Application.dataPath;
        dataPath = dataPath.Substring(0, dataPath.Length - 6);

        StartCoroutine(startDifferentView());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator startDifferentView()
    {
        int i = 0;
        int clone_id;
        while (!camVertices.ifDone)
        {
            yield return new WaitForSeconds(timeForEachPics);
        }

        for (i = 0; i < renderObj.Length; i++)
            {
                objectController = renderObj[i].GetComponent<ObjectController>();
                objectController.Place_away();
            }

        while (sceneCnt < maximumScene)
        {
            /* Creat mutiple object
             * renderObj: the original model, stay in buffer, won't be placed for collecting dataset, won't be destroy
             * objNumber: the number of the objects
             * clone_id: choose one from all objects, and segment material
             * cloneObj: copy from the original model(renderObj) in order to generate multiple same object, will be destroy
             * cloneObj_label: segment object, follow the cloneObj, will be destroy. layer: 9
             */
            GameObject[] cloneObj;
            GameObject[] cloneObj_label;
            cloneObj = new GameObject[objNumber];
            cloneObj_label = new GameObject[objNumber];
            JsonFile json_file = new JsonFile(objNumber);

            for (i = 0; i < objNumber; i++)
            {
                clone_id = Random.Range(0, renderObj.Length);
                cloneObj[i] = (GameObject)Instantiate(renderObj[clone_id], new Vector3(0.0f, 0.2f, 0.0f), Random.rotation);
                objectController = cloneObj[i].GetComponent<ObjectController>();
                objectController.Place();
                cloneObj_label[i] = Objclone_label(cloneObj[i], clone_id);
                yield return new WaitForSeconds(1.0f);
            }

            string sceneName = "Images/" + folderName + "/Scene" + sceneCnt + "_";
            System.IO.Directory.CreateDirectory("Images/" + folderName);
            //string sceneName = "Images/Scene" + sceneCnt + "_";
            
            //ini.Load_File()
            ini = new IniFile();
            ini.Load_File(dataPath + sceneName + "info.ini");
            //Debug.Log( dataPath + sceneName + "info.ini");
            
            // save object information
            ini.Create_Section("Object Information");
            ini.Goto_Section("Object Information");

            for(i = 0; i < objNumber; i++)
            {
                ini.Set_Vector3("Position", cloneObj[i].transform.position);
                ini.Set_Vector3("Orientation(Euler)", cloneObj[i].transform.rotation.eulerAngles);
            }
                
            //ini.Create_Section("View Information");
            ini.Goto_Section("View Information");

            while (imageCnt < maximumImage)
            {
                // get camera position
                Vector3 camPos = Vector3.zero;
                Vector3 lookPos = Vector3.zero;
                for (i = 0; i < objNumber; i++)
                {
                    cloneObj[i].GetComponent<Renderer>().sharedMaterial = ObjColorRandom(10, 1);
                    drawAABBox(cloneObj[i]);
                    drawOBBox(cloneObj[i]);
                }
                //ObjPoseRandom();
                BackgroundRandom(renderBackground.GetComponent<Renderer>().sharedMaterial);
                lt.color = Color.HSVToRGB(Random.value * 0.7f, Random.value * 0.5f, 1.0f - Random.value * 0.15f, true);
                camPos = camVertices.get_random_vertice();
                lookPos = camVertices.get_random_look_vertice();

                // Debug.Log("camPos =" + camPos);
                renderCam.transform.position = camPos;
                ini.Set_Vector3(imageCnt.ToString(), camPos);
                renderCam.transform.LookAt(lookPos);   // set camera look at the object


                //json_file = new JsonFile();
                json_file.get_cam(renderCam);
                json_file.get_obj(1, cloneObj, renderCam);

                //json_file.get_file();

                //json_file.get_obj(objNumber, cloneObj, renderCam);
                string convertToJson = JsonUtility.ToJson(json_file, true);
                convertToJson = convertToJson.Replace("_class", "class");
                convertToJson = convertToJson.Replace("(Clone)", "");

                //Debug.Log(convertToJson);
                System.IO.File.WriteAllText( sceneName+ "_test.json", convertToJson);

                // capture the screenshot;
                yield return new WaitForEndOfFrame();
                yield return StartCoroutine(ScreenShot(sceneName + imageCnt));
                //yield return StartCoroutine(Drawimage2DOBBox(cloneObj[0], sceneName + imageCnt));

                imageCnt++;
                yield return new WaitForSeconds(0.2f);
                
            }
            //ini.SaveTo(dataPath + sceneName + "info.ini");
            for (i = 0; i < objNumber; i++)
            {
                Destroy(cloneObj[i]);
                Destroy(cloneObj_label[i]);
            }
            // next scene
            // Object must move certain distance from a scene to another

            sceneCnt++;
            imageCnt = 0;
        }
        yield return null;
    }
    void drawAABBox(GameObject g)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector3 center = g.transform.position;
        /*Debug.Log("game object size:" + bc.size.ToString("f4"));
        Debug.Log("game object position:" + center.ToString("f4"));
        Debug.Log("Bounds center:" + bc.bounds.center.ToString("f4"));//中点
        //Debug.Log("Bounds Extents:" + bc.bounds.extents.ToString("f4"));
        Debug.Log("Bounds Size:" + bc.bounds.size.ToString("f4"));//bound的大小，等于extents*2
        //Debug.Log("Bounds Min:" + bc.bounds.min.ToString("f4"));//bound上最小的点，总是等于center-extents
        //Debug.Log("Bounds Max:" + bc.bounds.max.ToString("f4"));//bound上最大的点，总是等于center+extents*/

        Vector3 c = bc.bounds.center;
        Vector3 e = bc.bounds.extents;

        Vector3 p0 = new Vector3(c.x + e.x, c.y - e.y, c.z + e.z);
        Vector3 p1 = new Vector3(c.x - e.x, c.y - e.y, c.z + e.z);
        Vector3 p2 = new Vector3(c.x - e.x, c.y + e.y, c.z + e.z);
        Vector3 p3 = new Vector3(c.x + e.x, c.y + e.y, c.z + e.z);
        Vector3 p4 = new Vector3(c.x + e.x, c.y - e.y, c.z - e.z);
        Vector3 p5 = new Vector3(c.x - e.x, c.y - e.y, c.z - e.z);
        Vector3 p6 = new Vector3(c.x - e.x, c.y + e.y, c.z - e.z);
        Vector3 p7 = new Vector3(c.x + e.x, c.y + e.y, c.z - e.z);

        Debug.DrawLine(p0, p1, Color.red, 1.0f);
        Debug.DrawLine(p0, p3, Color.red, 1.0f);
        Debug.DrawLine(p2, p1, Color.red, 1.0f);
        Debug.DrawLine(p2, p3, Color.red, 1.0f);

        Debug.DrawLine(p0, p4, Color.red, 1.0f);
        Debug.DrawLine(p1, p5, Color.red, 1.0f);
        Debug.DrawLine(p2, p6, Color.red, 1.0f);
        Debug.DrawLine(p3, p7, Color.red, 1.0f);
    
        Debug.DrawLine(p4, p5, Color.red, 1.0f);
        Debug.DrawLine(p5, p6, Color.red, 1.0f);
        Debug.DrawLine(p6, p7, Color.red, 1.0f);
        Debug.DrawLine(p7, p4, Color.red, 1.0f);
    }

    void drawOBBox(GameObject g)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector3 p0 = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        Vector3 p1 = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        Vector3 p2 = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        Vector3 p3 = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        Vector3 p4 = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        Vector3 p5 = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        Vector3 p6 = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f);
        Vector3 p7 = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f);

        Debug.DrawLine(p0, p1, Color.blue, 1.0f);
        Debug.DrawLine(p0, p3, Color.blue, 1.0f);
        Debug.DrawLine(p2, p1, Color.blue, 1.0f);
        Debug.DrawLine(p2, p3, Color.blue, 1.0f);

        Debug.DrawLine(p0, p4, Color.blue, 1.0f);
        Debug.DrawLine(p1, p5, Color.blue, 1.0f);
        Debug.DrawLine(p2, p6, Color.blue, 1.0f);
        Debug.DrawLine(p3, p7, Color.blue, 1.0f);

        Debug.DrawLine(p4, p5, Color.blue, 1.0f);
        Debug.DrawLine(p5, p6, Color.blue, 1.0f);
        Debug.DrawLine(p6, p7, Color.blue, 1.0f);
        Debug.DrawLine(p7, p4, Color.blue, 1.0f);
    }

    void draw2DOBBox(GameObject g)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector2 p0 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f));
        Vector2 p1 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f));
        Vector2 p2 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f));
        Vector2 p3 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f));
        Vector2 p4 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f));
        Vector2 p5 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f));
        Vector2 p6 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f));
        Vector2 p7 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f));

        Debug.DrawLine(p0, p1, Color.blue, 1.0f);
        Debug.DrawLine(p0, p3, Color.blue, 1.0f);
        Debug.DrawLine(p2, p1, Color.blue, 1.0f);
        Debug.DrawLine(p2, p3, Color.blue, 1.0f);

        Debug.DrawLine(p0, p4, Color.blue, 1.0f);
        Debug.DrawLine(p1, p5, Color.blue, 1.0f);
        Debug.DrawLine(p2, p6, Color.blue, 1.0f);
        Debug.DrawLine(p3, p7, Color.blue, 1.0f);

        Debug.DrawLine(p4, p5, Color.blue, 1.0f);
        Debug.DrawLine(p5, p6, Color.blue, 1.0f);
        Debug.DrawLine(p6, p7, Color.blue, 1.0f);
        Debug.DrawLine(p7, p4, Color.blue, 1.0f);
    }

    private Material ObjColorRandom(int c, int m)
    {
        Material obj_material = new Material(Shader.Find("Specular"));

        obj_material.SetFloat("_Metallic", Random.value);
        obj_material.SetFloat("_Glossiness", Random.value);
        obj_material.SetFloat("_SpecularHighlights", (float)Random.Range(0, 2));
        obj_material.SetFloat("_GlossyReflections", (float)Random.Range(0, 2));
        obj_material.mainTexture = null;
        if (m == 0)
        {
            int color_texture_flag = Random.Range(0, c);

            if (color_texture_flag < c - 2)
            {
                obj_material.color = Color.HSVToRGB(Random.value, Random.value, Random.value, true);
            }
            else if (color_texture_flag == c - 2)
                obj_material.mainTexture = Generate2DPerlinTexture(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(10.0f, 100.0f));
            else if (color_texture_flag == c - 1)
                obj_material.mainTexture = Generate2DPerlinTexture(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f));
        }
        else if (m == 1)
        {
            obj_material.color = Color.HSVToRGB(Random.value, Random.value * 0.7f + 0.3f, Random.value * 0.7f + 0.3f, true);
        }
        else if (m == 2)
        {
            obj_material.color = Color.HSVToRGB(Random.value * 0.15f + 0.25f, Random.value * 0.5f + 0.5f, Random.value * 0.5f + 0.5f, true);
        }

        return obj_material;

        // Debug.Log("chang obj color = " + renderObj.GetComponent<Renderer>().material.color);
    }

    private void BackgroundRandom(Material bg_material)
    {
        Texture2D tex = null;
        byte[] fileData;
        string filePath = "Assets/Resources/background/" + Random.Range(0, 2001).ToString() + ".jpg";

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        bg_material.mainTexture = tex;
    }

    private void ObjPoseRandom()
    {
        for (int i = 0; i < renderObj.Length; i++)
        {
            renderObj[i].transform.position = new Vector3(Random.Range(-0.1f, 0.1f), 0.008f, Random.Range(-0.1f, 0.1f));
            renderObj[i].transform.eulerAngles = new Vector3(0, Random.value * 360.0f, 0);
        }
        // Debug.Log("chang obj color = " + renderObj.GetComponent<Renderer>().material.color);
    }

    private GameObject Objclone_label(GameObject obj, int id)
    {
        //GameObject labelClone = (GameObject)Instantiate(obj, obj.transform.position, obj.transform.rotation);
        GameObject labelClone = (GameObject)Instantiate(obj);
        foreach (var comp in labelClone.GetComponents<Component>())
        {
            if ((comp is Rigidbody) || (comp is MeshCollider) || (comp is Collider) || (comp is BoxCollider) || (comp is SphereCollider))
            {
                DestroyImmediate(comp);
            }
        }
        FollowRenderCam labFol = labelClone.AddComponent<FollowRenderCam>() as FollowRenderCam;
        labelClone.GetComponent<FollowRenderCam>().cam = obj;
        
        string label_matname = "Materials/Seg_" + (id + 1).ToString();
        Material labelMaterial = Resources.Load<Material>(label_matname);
        labelClone.GetComponent<Renderer>().sharedMaterial = labelMaterial;
        labelClone.name = obj.name + "_label";
        labelClone.layer = 9;

        return labelClone;
    }

    private Texture2D Generate2DPerlinTexture(float xOrg, float yOrg, float scale)
    {
        int pixWidth = 50;
        int pixHeight = 500;
        // float scale = 20.0f;
        Texture2D noiseTex;
        Color[] pix;

        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];

        float y = 0;
        while (y < noiseTex.height)
        {
            float x = 0.0f;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + (float)x / noiseTex.width * scale;
                float yCoord = yOrg + (float)y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        return noiseTex;
    }

	private void texToPNG(string filename, Texture2D tex)
	{
		byte[] bytes = tex.EncodeToPNG ();
		File.WriteAllBytes (filename, bytes);
	}

    IEnumerator ScreenShot(string filename)
    {
        yield return new WaitForEndOfFrame();

        Texture2D entireScreen = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        entireScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        entireScreen.Apply();
        // get original image
        Texture2D original = new Texture2D(Screen.width, Screen.height / 3, TextureFormat.RGB24, false);
        Color[] pix = entireScreen.GetPixels(0, Screen.height / 3 * 2, Screen.width, Screen.height / 3);
        original.SetPixels(pix);
        original.Apply();
        texToPNG(filename + "_original.png", original);
        Destroy(original);

        // get depth
        Texture2D depth = new Texture2D(Screen.width, Screen.height / 3, TextureFormat.RGB24, false);
        pix = entireScreen.GetPixels(0, 0, Screen.width, Screen.height / 3);
        depth.SetPixels(pix);
        depth.Apply();
        texToPNG(filename + "_depth.png", depth);
        Destroy(depth);

        //get seg
        Texture2D seg = new Texture2D(Screen.width, Screen.height / 3, TextureFormat.RGB24, false);
        pix = entireScreen.GetPixels(0, Screen.height / 3, Screen.width, Screen.height / 3);

        for (int i = 0; i < pix.Length; i++)
        {
            if (pix[i].g != 1.0f)
            {
                pix[i] = Color.black;
            }
            else
            {
                pix[i].g = pix[i].r;
                pix[i].b = pix[i].r;
            }
        }
        seg.SetPixels(pix);
        seg.Apply();
        texToPNG(filename + "_seg.png", seg);
        Destroy(seg);

        Destroy(entireScreen);
    }

    IEnumerator Drawimage2DOBBox(GameObject g, string filename)
    {
        yield return new WaitForEndOfFrame();
        Texture2D entireScreen = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        entireScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        entireScreen.Apply();
        
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector2 p0 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f));
        Vector2 p1 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f));
        Vector2 p2 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f));
        Vector2 p3 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f));
        Vector2 p4 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f));
        Vector2 p5 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f));
        Vector2 p6 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f));
        Vector2 p7 = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f));
        Vector2 pcenter = renderCam.GetComponent<Camera>().WorldToScreenPoint(g.transform.TransformPoint(bc.center));
        Debug.Log("pcenter" + pcenter);
        Debug.Log("pcenter fix: " + pcenter.x.ToString("f4") + ", " + (pcenter.y - 960).ToString("f4"));


        Texture2D seg = new Texture2D(Screen.width, Screen.height / 3, TextureFormat.RGB24, false);
        Color[] pix = entireScreen.GetPixels(0, Screen.height / 3, Screen.width, Screen.height / 3);
        int p;

        p = (int)((int)p0.x + ((int)p0.y - 960) * 640);
        if ((int)p0.x < 640 && (int)p0.y - 960 < 480) 
            pix[p] = Color.red;
        p = (int)((int)p1.x + ((int)p1.y - 960) * 640);
        if ((int)p1.x < 640 && (int)p1.y < 480)
            pix[p] = Color.red;
        p = (int)((int)p2.x + ((int)p2.y - 960) * 640);
        if ((int)p2.x < 640 && (int)p2.y - 960 < 480)
            pix[p] = Color.red;
        p = (int)((int)p3.x + ((int)p3.y - 960) * 640);
        if ((int)p3.x < 640 && (int)p3.y - 960 < 480)
            pix[p] = Color.red;
        p = (int)((int)p4.x + ((int)p4.y - 960) * 640);
        if ((int)p4.x < 640 && (int)p4.y - 960 < 480)
            pix[p] = Color.red;
        p = (int)((int)p5.x + ((int)p5.y - 960) * 640);
        if ((int)p5.x < 640 && (int)p5.y - 960 < 480)
            pix[p] = Color.red;
        p = (int)((int)p6.x + ((int)p6.y - 960) * 640);
        if ((int)p6.x < 640 && (int)p6.y - 960 < 480)
            pix[p] = Color.red;
        p = (int)((int)p7.x + ((int)p7.y - 960) * 640);
        if ((int)p7.x < 640 && (int)p7.y - 960 < 480)
            pix[p] = Color.red;

        p = (int)((int)pcenter.x + ((int)pcenter.y - 960) * 640);
        if ((int)pcenter.x < 640 && (int)pcenter.y - 960 < 480)
            pix[p] = Color.white;
        
        seg.SetPixels(pix);
        seg.Apply();
        texToPNG(filename + "_seg2dtest.png", seg);
        Destroy(seg);

        Destroy(entireScreen);
    }

}
