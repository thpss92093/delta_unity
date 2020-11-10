using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class JsonFile
{
    public CameraData camera_data = new CameraData();
    public Objects[] objects;
    [Serializable]
    public class CameraData
    {
        public float[] location_worldframe;             // Vector3
        public float[] quaternion_xyzw_worldframe;      // Quaternion

        public CameraData()
        {
            location_worldframe = new float[3] { 0.0f, 0.0f, 0.0f };
            quaternion_xyzw_worldframe = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
        }
        public CameraData(Vector3 lw, Quaternion qw)
        {
            location_worldframe = new float[3] { lw.x, lw.y, lw.z };
            quaternion_xyzw_worldframe = new float[4] { qw.x, qw.y, qw.z, qw.w };
        }
    }
    [Serializable]
    public class Objects
    {
        public string _class;
        public int visibility;
        public float[] location;                        // Vector3
        public float[] quaternion_xyzw;                 // Quaternion
        // public float[,] pose_transform_permuted;        // Matrix4x4    
        public List<string> pose_transform_permuted;        // Matrix4x4         

        public float[] cuboid_centroid;                 // Vector3
        public float[] projected_cuboid_centroid;       // (x,y)
        public BoundingBox bounding_box;
        // public float[,] cuboid;                      // Vector3 []
        public List<string> cuboid;                     // Vector3 [] 

        // public float[,] projected_cuboid;            // Vector2 [] (x,y)
        public List<string> projected_cuboid;           // Vector2 [] (x,y)



        public Objects()
        {
            _class = " ";
            visibility = 1;
            location = new float[3];
            quaternion_xyzw = new float[4];
            // pose_transform_permuted = new float[4,4];
            pose_transform_permuted = new List<string>();
            cuboid_centroid = new float[3];
            projected_cuboid_centroid = new float[2];
            bounding_box = new BoundingBox();
            // cuboid = new float[8,3];
            cuboid = new List<string>();
            // projected_cuboid = new float[8,2];
            projected_cuboid = new List<string>();
        }
        List<string> get_pose_transform_permuted(Matrix4x4 ptp)
        {
            List<string> str_list = new List<string>();
            float[] temp = new float[4] { ptp.m00, ptp.m01, ptp.m02, ptp.m03 };
            str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");

            temp = new float[4] { ptp.m10, ptp.m11, ptp.m12, ptp.m13 };
            str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");

            temp = new float[4] { ptp.m20, ptp.m21, ptp.m22, ptp.m23 };
            str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");

            temp = new float[4] { ptp.m30, ptp.m31, ptp.m32, ptp.m33 };
            str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");

            return str_list;
        }
        List<string> get_projected_cuboid(Vector2[] pc)
        {
            List<string> str_list = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                float[] temp = new float[2] { pc[i].x, pc[i].y };
                str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");
            }
            return str_list;
        }
        List<string> get_cuboid(Vector3[] cub)
        {
            List<string> str_list = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                float[] temp = new float[3] { cub[i].x, cub[i].y, cub[i].z };
                str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");
            }
            return str_list;
        }
        public Objects(string cla, int vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
        {
            _class = cla;
            visibility = vis;
            location = new float[3] { l.x, l.y, l.z };
            quaternion_xyzw = new float[4] { q.x, q.y, q.z, q.w };
            pose_transform_permuted = get_pose_transform_permuted(ptp);
            cuboid_centroid = new float[3] { cc.x, cc.y, cc.z };
            projected_cuboid_centroid = new float[2] { pcc.x, pcc.y };
            bounding_box = bb;
            projected_cuboid = get_projected_cuboid(pc);
            cuboid = get_cuboid(cub);
        }
        
        public void setObjects(string cla, int vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
        {
            _class = cla;
            visibility = vis;
            location = new float[3] { l.x, l.y, l.z };
            quaternion_xyzw = new float[4] { q.x, q.y, q.z, q.w };
            pose_transform_permuted = get_pose_transform_permuted(ptp);
            cuboid_centroid = new float[3] { cc.x, cc.y, cc.z };
            projected_cuboid_centroid = new float[2] { pcc.x, pcc.y };
            bounding_box = bb;
            projected_cuboid = get_projected_cuboid(pc);
            cuboid = get_cuboid(cub);

        }
        public string get_class()
        {
            return _class;
        }
    }
    [Serializable]
    public class BoundingBox
    {
        public float[] top_left;                        // Vector2 (y,x)
        public float[] bottom_right;                    // Vector2 (y,x)

        public BoundingBox()
        {
            top_left = new float[2] { 0.0f, 0.0f };
            bottom_right = new float[2] { 0.0f, 0.0f };
        }
        public BoundingBox(Vector2 tl, Vector2 br)
        {
            top_left = new float[2] { tl.x, tl.y };
            bottom_right = new float[2] { br.x, br.y };
        }
    }

    public JsonFile(int n)
    {
        //current_json = null;

        //current_json.camera_data.location_worldframe = Vector3.zero;
        //current_json.camera_data.quaternion_xyzw_worldframe = Quaternion.identity;
    }

    public void get_cam(GameObject renderCam)
    {
        camera_data.location_worldframe = new float[3] { renderCam.transform.position.x, renderCam.transform.position.y, renderCam.transform.position.z };
        camera_data.quaternion_xyzw_worldframe = new float[4] { renderCam.transform.rotation.x, renderCam.transform.rotation.y, renderCam.transform.rotation.z, renderCam.transform.rotation.w };
    }
    public void get_obj(int n, GameObject[] renderObj, GameObject renderCam)
    {
        //Debug.Log("obj length: " + objects.Length);
        //Debug.Log("renderObj length: " + renderObj.Length);
        string cla;
        int vis = 1;
        Vector3 l;
        Quaternion q;
        Matrix4x4 ptp;
        Vector3 cc;
        Vector2 pcc;
        BoundingBox bb;
        Vector3[] cub;
        Vector2[] pc;

        objects = new Objects[n];

        for (int i = 0; i < n; i++)
        {
            objects[i] = new Objects();
            cla = renderObj[i].name;
            // =========== object to cam location ==============
            l = renderCam.transform.position - renderObj[i].transform.position;
            //Debug.Log("location1: " + l.ToString("f4"));
            l = renderCam.transform.TransformPoint(renderObj[i].transform.position);
            //Debug.Log("location2: " + l.ToString("f4"));
            cc = l;
            // =========== object to cam rotation ==============
            q = renderObj[i].transform.rotation;

            ptp = Matrix4x4.Rotate(renderObj[i].transform.rotation);

            pcc = renderCam.GetComponent<Camera>().WorldToScreenPoint(renderObj[i].transform.position);
            pcc.y = 480 - (pcc.y - 960);

            cub = getOBBox(renderObj[i]);
            pc = get2DOBBox(cub, renderCam);
            bb = get2DBox(pc);
            

            objects[i].setObjects(cla, vis, l, q, ptp, cc, pcc, bb, cub, pc);
        }
    }
    Vector3[] getOBBox(GameObject g)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector3[] pt = new Vector3[8];
        pt[0] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[1] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[2] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[3] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[4] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[5] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[6] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f);
        pt[7] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f);

        return pt;
    }
    Vector2[] get2DOBBox(Vector3[] v3, GameObject renderCam)
    {
        Vector2[] v2 = new Vector2[8];
        for (int i = 0; i < v3.Length; i++)
        {
            renderCam.GetComponent<Camera>().WorldToScreenPoint(v3[i]);
            v2[i].y = 480 - (v2[i].y - 960);
        }
        return v2;
    }

    BoundingBox get2DBox(Vector2[] box)
    {
        BoundingBox bb_temp = new BoundingBox();
        Vector2 min = box[0];
        Vector2 max = box[0];

        for (int i = 1; i < box.Length; i++)
        {
            if (min.x > box[i].x)
                min.x = box[i].x;
            if (min.y > box[i].y)
                min.y = box[i].y;
            if (max.x < box[i].x)
                max.x = box[i].x;
            if (max.y < box[i].y)
                max.y = box[i].y;
        }
        bb_temp.top_left = new float[2] { min.x, min.y };
        bb_temp.bottom_right = new float[2] { max.x, max.y };

        return bb_temp;
    }
    public void get_file()
    {

    }
    
}


// ========= array =========
/*
public class JsonFile
{
    public CameraData camera_data = new CameraData();
    public Objects[] objects;
    [Serializable]
    public class CameraData
    {
        public float[] location_worldframe;             // Vector3
        public float[] quaternion_xyzw_worldframe;      // Quaternion

        public CameraData()
        {
            location_worldframe = new float[3] { 0.0f, 0.0f, 0.0f };
            quaternion_xyzw_worldframe = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
        }
        public CameraData(Vector3 lw, Quaternion qw)
        {
            location_worldframe = new float[3] { lw.x, lw.y, lw.z };
            quaternion_xyzw_worldframe = new float[4] { qw.x, qw.y, qw.z, qw.w };
        }
    }
    [Serializable]
    public class Objects
    {
        public string _class;
        public int visibility;
        public float[] location;                        // Vector3
        public float[] quaternion_xyzw;                 // Quaternion
        public float[,] pose_transform_permuted;        // Matrix4x4         
        public float[] cuboid_centroid;                 // Vector3
        public float[] projected_cuboid_centroid;       // (x,y)
        public BoundingBox bounding_box;
        public float[,] cuboid;                         // Vector3 []
        public float[,] projected_cuboid;               // Vector2 [] (x,y)

        public Objects()
        {
            _class = " ";
            visibility = 1;
            location = new float[3];
            quaternion_xyzw = new float[4];
            pose_transform_permuted = new float[4, 4];
            cuboid_centroid = new float[3];
            projected_cuboid_centroid = new float[2];
            bounding_box = new BoundingBox();
            cuboid = new float[8, 3];
            projected_cuboid = new float[8, 2];
        }
        public Objects(string cla, int vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
        {
            _class = cla;
            visibility = vis;
            location = new float[3] { l.x, l.y, l.z };
            quaternion_xyzw = new float[4] { q.x, q.y, q.z, q.w };
            pose_transform_permuted = new float[4, 4] { { ptp.m00, ptp.m01, ptp.m02, ptp.m03 }, { ptp.m10, ptp.m11, ptp.m12, ptp.m13 }, { ptp.m20, ptp.m21, ptp.m22, ptp.m23 }, { ptp.m30, ptp.m31, ptp.m32, ptp.m33 } };

            cuboid_centroid = new float[3] { cc.x, cc.y, cc.z };
            projected_cuboid_centroid = new float[2] { pcc.x, pcc.y };
            bounding_box = bb;
            cuboid = new float[8, 3] { { cub[0].x, cub[0].y, cub[0].z }, { cub[1].x, cub[1].y, cub[1].z }, { cub[2].x, cub[2].y, cub[2].z }, { cub[3].x, cub[3].y, cub[3].z }, { cub[4].x, cub[4].y, cub[4].z }, { cub[5].x, cub[5].y, cub[5].z }, { cub[6].x, cub[6].y, cub[6].z }, { cub[7].x, cub[7].y, cub[7].z } };
            projected_cuboid = new float[8, 2] { { pc[0].x, pc[0].y }, { pc[1].x, pc[1].y }, { pc[2].x, pc[2].y }, { pc[3].x, pc[3].y }, { pc[4].x, pc[4].y }, { pc[5].x, pc[5].y }, { pc[6].x, pc[6].y }, { pc[7].x, pc[7].y } };
        }
        public void setObjects(string cla, int vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
        {
            _class = cla;
            visibility = vis;
            location = new float[3] { l.x, l.y, l.z };
            quaternion_xyzw = new float[4] { q.x, q.y, q.z, q.w };
            pose_transform_permuted = new float[4, 4] { { ptp.m00, ptp.m01, ptp.m02, ptp.m03 }, { ptp.m10, ptp.m11, ptp.m12, ptp.m13 }, { ptp.m20, ptp.m21, ptp.m22, ptp.m23 }, { ptp.m30, ptp.m31, ptp.m32, ptp.m33 } };
            Debug.Log("debug ptp: " + pose_transform_permuted.Length);
            foreach(var number in pose_transform_permuted)
            {
                Debug.Log("debug ptp value:" + number);
            }

            cuboid_centroid = new float[3] { cc.x, cc.y, cc.z };
            projected_cuboid_centroid = new float[2] { pcc.x, pcc.y };
            bounding_box = bb;
            cuboid = new float[8, 3] { { cub[0].x, cub[0].y, cub[0].z }, { cub[1].x, cub[1].y, cub[1].z }, { cub[2].x, cub[2].y, cub[2].z }, { cub[3].x, cub[3].y, cub[3].z }, { cub[4].x, cub[4].y, cub[4].z }, { cub[5].x, cub[5].y, cub[5].z }, { cub[6].x, cub[6].y, cub[6].z }, { cub[7].x, cub[7].y, cub[7].z } };
            projected_cuboid = new float[8, 2] { { pc[0].x, pc[0].y }, { pc[1].x, pc[1].y }, { pc[2].x, pc[2].y }, { pc[3].x, pc[3].y }, { pc[4].x, pc[4].y }, { pc[5].x, pc[5].y }, { pc[6].x, pc[6].y }, { pc[7].x, pc[7].y } };
            Debug.Log("debug cuboid: " + cuboid.Length);
            Debug.Log("debug projected_cuboid: " + projected_cuboid.Length);

        }
        public string get_class()
        {
            return _class;
        }
    }
    [Serializable]
    public class BoundingBox
    {
        public float[] top_left;                        // Vector2 (y,x)
        public float[] bottom_right;                    // Vector2 (y,x)

        public BoundingBox()
        {
            top_left = new float[2] { 0.0f, 0.0f };
            bottom_right = new float[2] { 0.0f, 0.0f };
        }
        public BoundingBox(Vector2 tl, Vector2 br)
        {
            top_left = new float[2] { tl.x, tl.y };
            bottom_right = new float[2] { br.x, br.y };
        }
    }

    public JsonFile(int n)
    {
        //current_json = null;

        //current_json.camera_data.location_worldframe = Vector3.zero;
        //current_json.camera_data.quaternion_xyzw_worldframe = Quaternion.identity;
    }

    public void get_cam(GameObject renderCam)
    {
        camera_data.location_worldframe = new float[3] { renderCam.transform.position.x, renderCam.transform.position.y, renderCam.transform.position.z };
        camera_data.quaternion_xyzw_worldframe = new float[4] { renderCam.transform.rotation.x, renderCam.transform.rotation.y, renderCam.transform.rotation.z, renderCam.transform.rotation.w };
    }
    public void get_obj(int n, GameObject[] renderObj, GameObject renderCam)
    {
        //Debug.Log("obj length: " + objects.Length);
        //Debug.Log("renderObj length: " + renderObj.Length);
        string cla;
        int vis = 1;
        Vector3 l;
        Quaternion q;
        Matrix4x4 ptp;
        Vector3 cc;
        Vector2 pcc;
        BoundingBox bb;
        Vector3[] cub;
        Vector2[] pc;

        objects = new Objects[n];

        for (int i = 0; i < n; i++)
        {
            objects[i] = new Objects();
            cla = renderObj[i].name;
            // =========== object to cam location ==============
            l = renderCam.transform.position - renderObj[i].transform.position;
            //Debug.Log("location1: " + l.ToString("f4"));
            l = renderCam.transform.TransformPoint(renderObj[i].transform.position);
            //Debug.Log("location2: " + l.ToString("f4"));
            cc = l;
            // =========== object to cam rotation ==============
            q = renderObj[i].transform.rotation;

            ptp = Matrix4x4.Rotate(renderObj[i].transform.rotation);

            pcc = renderCam.GetComponent<Camera>().WorldToScreenPoint(renderObj[i].transform.position);
            pcc.y = 480 - (pcc.y - 960);

            cub = getOBBox(renderObj[i]);
            pc = get2DOBBox(cub, renderCam);
            bb = get2DBox(pc);


            objects[i].setObjects(cla, vis, l, q, ptp, cc, pcc, bb, cub, pc);
        }
    }
    Vector3[] getOBBox(GameObject g)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector3[] pt = new Vector3[8];
        pt[0] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[1] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[2] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[3] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[4] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[5] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[6] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f);
        pt[7] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f);

        return pt;
    }
    Vector2[] get2DOBBox(Vector3[] v3, GameObject renderCam)
    {
        Vector2[] v2 = new Vector2[8];
        for (int i = 0; i < v3.Length; i++)
        {
            renderCam.GetComponent<Camera>().WorldToScreenPoint(v3[i]);
            v2[i].y = 480 - (v2[i].y - 960);
        }
        return v2;
    }

    BoundingBox get2DBox(Vector2[] box)
    {
        BoundingBox bb_temp = new BoundingBox();
        Vector2 min = box[0];
        Vector2 max = box[0];

        for (int i = 1; i < box.Length; i++)
        {
            if (min.x > box[i].x)
                min.x = box[i].x;
            if (min.y > box[i].y)
                min.y = box[i].y;
            if (max.x < box[i].x)
                max.x = box[i].x;
            if (max.y < box[i].y)
                max.y = box[i].y;
        }
        bb_temp.top_left = new float[2] { min.x, min.y };
        bb_temp.bottom_right = new float[2] { max.x, max.y };

        return bb_temp;
    }
    public void get_file()
    {

    }

}
*/

// ========= Vector =========
/*
public class JsonFile
{
    public CameraData camera_data = new CameraData();
    public Objects[] objects;
    [Serializable]
    public class CameraData
    {
        public Vector3 location_worldframe;
        public Quaternion quaternion_xyzw_worldframe;

        public CameraData()
        {
            location_worldframe = Vector3.zero;
            quaternion_xyzw_worldframe = Quaternion.identity;
        }
        public CameraData(Vector3 lw, Quaternion qw)
        {
            location_worldframe = lw;
            quaternion_xyzw_worldframe = qw;
        }
    }
    [Serializable]
    public class Objects
    {
        public string _class;
        public int visibility;
        public Vector3 location;
        public Quaternion quaternion_xyzw;
        public Matrix4x4 pose_transform_permuted;
        public Vector3 cuboid_centroid;
        public Vector2 projected_cuboid_centroid;       // (x,y)
        public BoundingBox bounding_box;
        public Vector3[] cuboid;
        public Vector2[] projected_cuboid;              // (x,y)

        public Objects()
        {
            _class = " ";
            visibility = 1;
            location = Vector3.zero;
            quaternion_xyzw = Quaternion.identity;
            pose_transform_permuted = Matrix4x4.identity;
            cuboid_centroid = Vector3.zero;
            projected_cuboid_centroid = Vector2.zero;
            bounding_box = new BoundingBox();
            cuboid = new Vector3[8];
            projected_cuboid = new Vector2[8];
        }
        public Objects(string cla, int vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
        {
            _class = cla;
            visibility = vis;
            location = l;
            quaternion_xyzw = q;
            pose_transform_permuted = ptp;
            cuboid_centroid = cc;
            projected_cuboid_centroid = pcc;
            bounding_box = bb;
            cuboid = cub;
            projected_cuboid = pc;
        }
        public void setObjects(string cla, int vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
        {
            _class = cla;
            visibility = vis;
            location = l;
            quaternion_xyzw = q;
            pose_transform_permuted = ptp;
            cuboid_centroid = cc;
            projected_cuboid_centroid = pcc;
            bounding_box = bb;
            cuboid = cub;
            projected_cuboid = pc;
        }
        public string get_class()
        {
            return _class;
        }
    }
    [Serializable]
    public class BoundingBox
    {
        public Vector2 top_left;                        // (y,x)
        public Vector2 bottom_right;                    // (y,x)

        public BoundingBox()
        {
            top_left = Vector2.zero;
            bottom_right = Vector2.zero;
        }
        public BoundingBox(Vector2 tl, Vector2 br)
        {
            top_left = tl;
            bottom_right = br;
        }
    }

    public JsonFile(int n)
    {
        Debug.Log("start current_json");
        //current_json = null;

        //current_json.camera_data.location_worldframe = Vector3.zero;
        //current_json.camera_data.quaternion_xyzw_worldframe = Quaternion.identity;
    }

    public void get_cam(GameObject renderCam)
    {
        camera_data.location_worldframe = renderCam.transform.position;
        camera_data.quaternion_xyzw_worldframe = renderCam.transform.rotation;
    }
    public void get_obj(int n, GameObject[] renderObj, GameObject renderCam)
    {
        //Debug.Log("obj length: " + objects.Length);
        //Debug.Log("renderObj length: " + renderObj.Length);
        string cla;
        int vis = 1;
        Vector3 l;
        Quaternion q;
        Matrix4x4 ptp;
        Vector3 cc;
        Vector2 pcc;
        BoundingBox bb;
        Vector3[] cub;
        Vector2[] pc;

        objects = new Objects[n];

        for (int i = 0; i < n; i++)
        {
            objects[i] = new Objects();
            cla = renderObj[i].name;
            // =========== object to cam location ==============
            l = renderCam.transform.position - renderObj[i].transform.position;
            Debug.Log("location1: " + l.ToString("f4"));
            l = renderCam.transform.TransformPoint(renderObj[i].transform.position);
            Debug.Log("location2: " + l.ToString("f4"));
            cc = l;
            // =========== object to cam rotation ==============
            q = renderObj[i].transform.rotation;

            ptp = Matrix4x4.Rotate(renderObj[i].transform.rotation);

            pcc = renderCam.GetComponent<Camera>().WorldToScreenPoint(renderObj[i].transform.position);
            pcc.y = 480 - (pcc.y - 960);

            cub = getOBBox(renderObj[i]);
            pc = get2DOBBox(cub, renderCam);
            bb = get2DBox(pc);

            Debug.Log("objects debug: " + objects.Length + ", " + objects.ToString());
            Debug.Log("objects debug2: " + objects[0].get_class());

            objects[i].setObjects(cla, vis, l, q, ptp, cc, pcc, bb, cub, pc);
        }
    }
    Vector3[] getOBBox(GameObject g)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector3[] pt = new Vector3[8];
        pt[0] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[1] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[2] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[3] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[4] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[5] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[6] = g.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f);
        pt[7] = g.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f);

        return pt;
    }
    Vector2[] get2DOBBox(Vector3[] v3, GameObject renderCam)
    {
        Vector2[] v2 = new Vector2[8];
        for (int i = 0; i < v3.Length; i++)
        {
            renderCam.GetComponent<Camera>().WorldToScreenPoint(v3[i]);
            v2[i].y = 480 - (v2[i].y - 960);
        }
        return v2;
    }

    BoundingBox get2DBox(Vector2[] box)
    {
        BoundingBox bb_temp = new BoundingBox();
        Vector2 min = box[0];
        Vector2 max = box[0];

        for (int i = 1; i < box.Length; i++)
        {
            if (min.x > box[i].x)
                min.x = box[i].x;
            if (min.y > box[i].y)
                min.y = box[i].y;
            if (max.x < box[i].x)
                max.x = box[i].x;
            if (max.y < box[i].y)
                max.y = box[i].y;
        }
        bb_temp.top_left = min;
        bb_temp.bottom_right = max;

        return bb_temp;
    }
    public void get_file()
    {

    }

}
 */
