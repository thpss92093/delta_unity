using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenCvSharp;
using NWH;

using Rect =  UnityEngine.Rect;

public class CameraController : MonoBehaviour {

    // if true, then randomly sampling the corresponding size
    // otherwise go througth all;
    public bool randomSampling;
    public int sampleSize;

    // time after taking each picture, deafault t=0
    public float timeForEachPics;

    public CameraVertices camVertices;

    // link corresponding camera and object;
    public GameObject renderCam;
    //public List<GameObject> renderCams;
    public GameObject[] renderObj;
    public GameObject renderConveyor;
    public GameObject renderBackground;
    ObjectController objectController;
    public Light lt;

    public IniFile ini;
    string dataPath;

    public string objName;
    public int maximumScene;
    public int sceneCnt;
    public int maximumImage;
    public int imageCnt;

    // Use this for initialization
    void Start() {
        dataPath = Application.dataPath;
        dataPath = dataPath.Substring(0, dataPath.Length - 6);
        //objectController = renderObj[0].GetComponent<ObjectController>();
        //objectController = renderConveyor.GetComponent<ObjectController>();
        //maximumImage = camVertices.get_total_views();

        StartCoroutine(startDifferentView());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator startDifferentView()
    {
        while (!camVertices.ifDone)
        {
            yield return new WaitForSeconds(timeForEachPics);
        }
        int i = 0;
        int setAwayFlag = 2;

        if (!randomSampling)
            sampleSize = camVertices.verticesList.Count;
        
        while (sceneCnt < maximumScene)
        {
            if (sceneCnt < 50)
            {
                for (i = 0; i < renderObj.Length; i++)
                {
                    objectController = renderObj[i].GetComponent<ObjectController>();
                    objectController.Place_away();
                }
                objectController = renderObj[sceneCnt / 10].GetComponent<ObjectController>();
                objectController.Reset();
                objectController.Push();
            }
            else if (sceneCnt < 120)
            {
                setAwayFlag = 2;
                if (sceneCnt > 80)
                    setAwayFlag = 3;

                for (i = 0; i < renderObj.Length; i++)
                {
                    objectController = renderObj[i].GetComponent<ObjectController>();
                    if (Random.Range(0, setAwayFlag) == 0)
                        objectController.Place_away();
                    else
                    {
                        objectController.Reset();
                        objectController.Push();
                    }
                }
            }
            else
            {
                for (i = 0; i < renderObj.Length; i++)
                {
                    objectController = renderObj[i].GetComponent<ObjectController>();
                    objectController.Reset();
                    objectController.Push();
                }
            }

            // string sceneName = "Images/" + objName + "/Scene" + sceneCnt + "/";
            //System.IO.Directory.CreateDirectory(sceneName);
            string sceneName = "Images/Scene" + sceneCnt + "_";
            
            //ini.Load_File()
            ini = new IniFile();
            ini.Load_File(dataPath + sceneName + "info.ini");
            //Debug.Log( dataPath + sceneName + "info.ini");
            
            // save object information
            ini.Create_Section("Object Information");
            ini.Goto_Section("Object Information");

            for(i = 0; i < renderObj.Length; i++)
            {
                ini.Set_Vector3("Position", renderObj[i].transform.position);
                ini.Set_Vector3("Orientation(Euler)", renderObj[i].transform.rotation.eulerAngles);
            }
                
            //ini.Create_Section("View Information");
            ini.Goto_Section("View Information");

            while (imageCnt < sampleSize)
            {
                // get camera position
                Vector3 camPos = Vector3.zero;
                Vector3 lookPos = Vector3.zero;
                for (i = 0; i < renderObj.Length; i++)
                {
                    ObjColorRandom(renderObj[i].GetComponent<Renderer>().sharedMaterial, 10, 1);
                }
                // ObjPoseRandom();
                ObjColorRandom(renderConveyor.GetComponent<Renderer>().sharedMaterial, 10, 0);
                BackgroundRandom(renderBackground.GetComponent<Renderer>().sharedMaterial);
                lt.color = Color.HSVToRGB(Random.value * 0.7f, Random.value * 0.5f, 1.0f - Random.value * 0.15f, true);

                if (randomSampling)
                {
                    // Debug.Log("randomSampling =" + randomSampling);
                    camPos = camVertices.get_random_vertice();
                    lookPos = camVertices.get_random_look_vertice();
                }

                else
                    camPos = camVertices.getVerticeAt(imageCnt);

                // Debug.Log("camPos =" + camPos);

                renderCam.transform.position = camPos;
                ini.Set_Vector3(imageCnt.ToString(), camPos);

                // set camera look at the object
                //renderCam.transform.LookAt(renderObj.transform);
                renderCam.transform.LookAt(lookPos);
                
                // capture the screenshot;
                //ScreenCapture.CaptureScreenshot(sceneName + imageCnt + ".png");
                //renderCam.GetComponent<ScreenShotForCamera>().CaptureCamera(sceneName + imageCnt + ".png");
                yield return new WaitForEndOfFrame();
                yield return StartCoroutine(ScreenShot(sceneName + imageCnt));
                imageCnt++;
                yield return new WaitForSeconds(1.0f);
                
                
                // lt.color = Color.HSVToRGB(Random.value, Random.value, Random.value, true);
            }
            //ini.SaveTo(dataPath + sceneName + "info.ini");
            for (i = 0; i < renderObj.Length; i++)
            {
                objectController = renderObj[i].GetComponent<ObjectController>();
                objectController.Reset();
            }
            // next scene
            // Object must move certain distance from a scene to another

            sceneCnt++;
            imageCnt = 0;

        }

        yield return null;
    }
  

    private void ObjColorRandom(Material obj_material, int c, int m)
    {
        obj_material.SetFloat("_Metallic", Random.value);
        obj_material.SetFloat("_Glossiness", Random.value);
        obj_material.SetFloat("_SpecularHighlights", (float)Random.Range(0, 2));
        obj_material.SetFloat("_GlossyReflections", (float)Random.Range(0, 2));
        if (m == 0)
        {
            int color_texture_flag = Random.Range(0, c);

            if (color_texture_flag < c - 2)
            {
                obj_material.mainTexture = null;
                obj_material.color = Color.HSVToRGB(Random.value, Random.value, Random.value, true);
            }
            else if (color_texture_flag == c - 2)
                obj_material.mainTexture = Generate2DPerlinTexture(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(10.0f, 100.0f));
            else if (color_texture_flag == c - 1)
                obj_material.mainTexture = Generate2DPerlinTexture(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f));
        }
        else if (m == 1)
        {
            obj_material.mainTexture = null;
            obj_material.color = Color.HSVToRGB(Random.value, Random.value * 0.7f + 0.3f, Random.value * 0.7f + 0.3f, true);

        }
        else if (m == 2)
        {
            obj_material.mainTexture = null;
            obj_material.color = Color.HSVToRGB(Random.value * 0.15f + 0.25f, Random.value * 0.5f + 0.5f, Random.value * 0.5f + 0.5f, true);
        }
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

}
