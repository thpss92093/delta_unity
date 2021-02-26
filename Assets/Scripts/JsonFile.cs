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
        public float visibility;
        public float[] location;                        // Vector3
        public float[] quaternion_xyzw;                 // Quaternion
        public List<string> pose_transform_permuted;    // Matrix4x4         
        public float[] cuboid_centroid;                 // Vector3
        public float[] projected_cuboid_centroid;       // (x,y)
        public BoundingBox bounding_box;
        public List<string> cuboid;                     // Vector3 [] 
        public List<string> projected_cuboid;           // Vector2 [] (x,y)
        
        public Objects()
        {
            _class = " ";
            visibility = 1.0f;
            location = new float[3];
            quaternion_xyzw = new float[4];
            pose_transform_permuted = new List<string>();
            cuboid_centroid = new float[3];
            projected_cuboid_centroid = new float[2];
            bounding_box = new BoundingBox();
            cuboid = new List<string>();
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
            float[] temp;
            for (int i = 0; i < 8; i++)
            {
                temp = new float[2] { pc[i].x, pc[i].y };
                str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");
            }
            return str_list;
        }
        List<string> get_cuboid(Vector3[] cub)
        {
            List<string> str_list = new List<string>();
            float[] temp;
            for (int i = 0; i < 8; i++)
            {
                temp = new float[3] { cub[i].x, cub[i].y, cub[i].z };
                str_list.Add("remove[ " + string.Join(", ", temp) + " ]remove");
            }
            return str_list;
        }
        public Objects(string cla, float vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
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
        
        public void setObjects(string cla, float vis, Vector3 l, Quaternion q, Matrix4x4 ptp, Vector3 cc, Vector2 pcc, BoundingBox bb, Vector3[] cub, Vector2[] pc)
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
            top_left = new float[2] { tl.y, tl.x };
            bottom_right = new float[2] { br.y, br.x };
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
        camera_data.location_worldframe = new float[3] { renderCam.transform.position.x * 100.0f, renderCam.transform.position.y * 100.0f, renderCam.transform.position.z * 100.0f };
        camera_data.quaternion_xyzw_worldframe = new float[4] { renderCam.transform.rotation.x, renderCam.transform.rotation.y, renderCam.transform.rotation.z, renderCam.transform.rotation.w };
    }
    public void get_obj(int n, GameObject[] renderObj, GameObject renderCam)
    {
        string cla;             // class
        float vis = 1.0f;       // visibility, occluded 
        Vector3 l;              // object to cam location, XYZ position (in centimeters)
        Quaternion q;           // object to cam orientation
        Matrix4x4 ptp;          // 4x4 transformation, pose_transform_permuted
        Vector3 cc;             // the same with l
        Vector2 pcc;            // 2D projected cuboid centroid
        BoundingBox bb;         // 2D bounding box
        Vector3[] cub;          // 3D bounding cuboid (in centimeters)
        Vector2[] pc;           // 2D projected bounding cuboid of the above

        objects = new Objects[n];
        for (int i = 0; i < n; i++)
        {
            objects[i] = new Objects();
            cla = renderObj[i].name;

            l = getViewportlacation(renderObj[i].transform.position, renderCam);
            
            cc = l;
            pcc = renderCam.GetComponent<Camera>().WorldToScreenPoint(renderObj[i].transform.position);
            pcc.y = 480 - (pcc.y - 960);
            BoxCollider bc = renderObj[i].GetComponent<BoxCollider>();

            Quaternion q_temp =  Quaternion.Inverse(renderCam.transform.rotation) * renderObj[i].transform.rotation;
            q = Quaternion.Euler(q_temp.eulerAngles.x, q_temp.eulerAngles.y + 180, q_temp.eulerAngles.z);
            
            
            ptp = Matrix4x4.Rotate(q);
            ptp.m30 = l.x;
            ptp.m31 = l.y;
            ptp.m32 = l.z;
            
            Vector3[] cub_world = getOBBox(renderObj[i], renderObj[i]);
            cub = getOBBox_test(cub_world, renderCam);
            pc = get2DOBBox(cub_world, renderCam);
            bb = get2DBox(pc);
            
            vis = getvisibility(bb);

            objects[i].setObjects(cla, vis, l, q, ptp, cc, pcc, bb, cub, pc);
        }
    }
    float getvisibility(BoundingBox box)
    {
        float minx = box.top_left[1];
        float miny = box.top_left[0];
        float maxx = box.bottom_right[1];
        float maxy = box.bottom_right[0];

        if (minx >= 0.0f && miny >= 0.0f && maxx <= 640.0f && maxy <= 480.0f)
            return 1.0f;
        if (minx >= 640.0f || miny >= 480.0f || maxx <= 0.0f || maxy <= 0.0f)
            return 0.0f;

        float area;
        area = (maxy - miny) * (maxx - minx);

        if (minx < 0.0f)
            minx = 0.0f;
        if (miny < 0.0f)
            miny = 0.0f;
        if (maxx > 640.0f)
            maxx = 640.0f;
        if (maxy > 480.0f)
            maxy = 480.0f;
        return ((maxy - miny) * (maxx - minx) / area);
    }
    Vector3 getViewportlacation(Vector3 worldLocatoion, GameObject renderCam)
    {
        Vector3 l;
        float fov = renderCam.GetComponent<Camera>().fieldOfView;
        float tan = Mathf.Tan((fov / 2.0f) * (float)Math.PI / 180.0f);
        float fov_h = 30.0f;
        float tan_h = Mathf.Tan((fov_h / 2.0f) * (float)Math.PI / 180.0f);
        
        l = renderCam.GetComponent<Camera>().WorldToViewportPoint(worldLocatoion);
        l.z = l.z * 100.0f;
        l.x = (l.x - 0.5f) * 2.0f * l.z * tan_h;
        l.y = (0.5f - l.y) * 2.0f * l.z * tan;
        
        
        return l;
    }
    Vector3[] getOBBox_test(Vector3[] v3, GameObject renderCam)
    {
        Vector3[] pt = new Vector3[8];
        pt[0] = getViewportlacation(v3[0], renderCam);
        pt[1] = getViewportlacation(v3[1], renderCam);
        pt[2] = getViewportlacation(v3[2], renderCam);
        pt[3] = getViewportlacation(v3[3], renderCam);
        pt[4] = getViewportlacation(v3[4], renderCam);
        pt[5] = getViewportlacation(v3[5], renderCam);
        pt[6] = getViewportlacation(v3[6], renderCam);
        pt[7] = getViewportlacation(v3[7], renderCam);

        return pt;
    }

    Vector3[] getOBBox(GameObject g, GameObject renderCam)
    {
        BoxCollider bc = g.GetComponent<BoxCollider>();
        Vector3[] pt = new Vector3[8];
        pt[0] = renderCam.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[1] = renderCam.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, bc.size.z) * 0.5f);
        pt[2] = renderCam.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[3] = renderCam.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, bc.size.z) * 0.5f);
        pt[4] = renderCam.transform.TransformPoint(bc.center + new Vector3(bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[5] = renderCam.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z) * 0.5f);
        pt[6] = renderCam.transform.TransformPoint(bc.center + new Vector3(-bc.size.x, bc.size.y, -bc.size.z) * 0.5f);
        pt[7] = renderCam.transform.TransformPoint(bc.center + new Vector3(bc.size.x, bc.size.y, -bc.size.z) * 0.5f);
        
        return pt;
    }
    Vector2[] get2DOBBox(Vector3[] v3, GameObject renderCam)
    {
        Vector2[] v2 = new Vector2[8];
        for (int i = 0; i < v3.Length; i++)
        {
            v2[i] = renderCam.GetComponent<Camera>().WorldToScreenPoint(v3[i]);
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
