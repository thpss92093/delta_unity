﻿using System;
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
        objects = new Objects[n];
        Debug.Log("obj length: " + objects.Length);
        Debug.Log("renderObj length: " + renderObj.Length);

        for (int i = 0; i < n; i++)
        {
            objects[i]._class = renderObj[i].name;
            objects[i].visibility = 1;
            // =========== object to cam location ==============
            objects[i].location = renderCam.transform.position - renderObj[i].transform.position;
            Debug.Log("location1: " + objects[i].location);
            objects[i].location = renderCam.transform.TransformPoint(renderObj[i].transform.position);
            Debug.Log("location2: " + objects[i].location);
            objects[i].cuboid_centroid = objects[i].location;
            // =========== object to cam rotation ==============
            objects[i].quaternion_xyzw = renderObj[i].transform.rotation;

            objects[i].pose_transform_permuted = Matrix4x4.Rotate(renderObj[i].transform.rotation);

            objects[i].projected_cuboid_centroid = renderCam.GetComponent<Camera>().WorldToScreenPoint(renderObj[i].transform.position);
            objects[i].projected_cuboid_centroid.y = 480 - (objects[i].projected_cuboid_centroid.y - 960);

            objects[i].cuboid = getOBBox(renderObj[i]);
            objects[i].projected_cuboid = get2DOBBox(objects[i].cuboid, renderCam);
            objects[i].bounding_box = get2DBox(objects[i].projected_cuboid);
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
