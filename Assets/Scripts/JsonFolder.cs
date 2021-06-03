using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class JsonFolder
{
    public string[] exported_object_classes;
    public ExportedObjects[] exported_objects;

    [Serializable]
    public class ExportedObjects
    {
        public string _class;
        public int segmentation_class_id;
        public List<string> fixed_model_transform;
        public float[] cuboid_dimensions;

        public ExportedObjects(GameObject g)
        {
            _class = g.name;

            if (_class == "Gear_Large(Clone)")
                segmentation_class_id = 10;
            else if (_class == "Waterproof_Female(Clone)")
                segmentation_class_id = 20;
            else if (_class == "DSUB_Female(Clone)")
                segmentation_class_id = 30;
            else if (_class == "M16_Screw(Clone)")
                segmentation_class_id = 40;
            else if (_class == "M16_Hex_Nut(Clone)")
                segmentation_class_id = 50;
            else if (_class == "RJ45_Female(Clone)")
                segmentation_class_id = 60;
            else if (_class == "RJ45_Male(Clone)")
                segmentation_class_id = 70;
            else if (_class == "USB_Male(Clone)")
                segmentation_class_id = 80;
            else if (_class == "BNC_Male(Clone)")
                segmentation_class_id = 90;
            else if (_class == "Gear_Medium(Clone)")
                segmentation_class_id = 100;
            else if (_class == "Gear_Small(Clone)")
                segmentation_class_id = 110;
            else if (_class == "Timing_Pulley_Large(Clone)")
                segmentation_class_id = 120;
            else if (_class == "Timing_Pulley_Small(Clone)")
                segmentation_class_id = 130;
            else if (_class == "Round_Pulley_Large(Clone)")
                segmentation_class_id = 140;
            else if (_class == "Round_Pulley_Small(Clone)")
                segmentation_class_id = 150;
            else if (_class == "Sprocket_Large(Clone)")
                segmentation_class_id = 160;
            else if (_class == "Sprocket_Small(Clone)")
                segmentation_class_id = 170;
            else if (_class == "Wire_Clip_Large(Clone)")
                segmentation_class_id = 180;
            else if (_class == "Wire_Clip_Small(Clone)")
                segmentation_class_id = 190;
            else if (_class == "Stereo_Plug(Clone)")
                segmentation_class_id = 200;
            else if (_class == "004_sugar_box_16k(Clone)")
                segmentation_class_id = 10;
            else if (_class == "cup(Clone)")
                segmentation_class_id = 10;
            else if (_class == "pin(Clone)")
                segmentation_class_id = 10;
            else
                segmentation_class_id = 0;
            fixed_model_transform = new List<string>();
            fixed_model_transform.Add("remove[ 0, 0, 100, 0 ]remove");
            fixed_model_transform.Add("remove[ 0, -100, 0, 0 ]remove");
            fixed_model_transform.Add("remove[ 100, 0, 0, 0 ]remove");
            fixed_model_transform.Add("remove[ 0, 0, 0, 1 ]remove");
            BoxCollider bc = g.GetComponent<BoxCollider>();
            Vector3 e = bc.size * 100.0f;                //bc.bounds.extents;
            cuboid_dimensions = new float[3] { e.x, e.y, e.z };
        }
        public void setExportedObjects(GameObject g)
        {
            _class = g.name;

            if(_class == "Gear_Large(Clone)")
                segmentation_class_id = 10;
            else if (_class == "Waterproof_Female(Clone)")
                segmentation_class_id = 20;
            else if(_class == "DSUB_Female(Clone)")
                segmentation_class_id = 30;
            else if(_class == "M16_Screw(Clone)")
                segmentation_class_id = 40;
            else if (_class == "M16_Hex_Nut(Clone)")
                segmentation_class_id = 50;
            else if (_class == "RJ45_Female(Clone)")
                segmentation_class_id = 60;
            else if (_class == "RJ45_Male(Clone)")
                segmentation_class_id = 70;
            else if (_class == "USB_Male(Clone)")
                segmentation_class_id = 80;
            else if (_class == "BNC_Male(Clone)")
                segmentation_class_id = 90;
            else if (_class == "Gear_Medium(Clone)")
                segmentation_class_id = 100;
            else if (_class == "Gear_Small(Clone)")
                segmentation_class_id = 110;
            else if (_class == "Timing_Pulley_Large(Clone)")
                segmentation_class_id = 120;
            else if (_class == "Timing_Pulley_Small(Clone)")
                segmentation_class_id = 130;
            else if (_class == "Round_Pulley_Large(Clone)")
                segmentation_class_id = 140;
            else if (_class == "Round_Pulley_Small(Clone)")
                segmentation_class_id = 150;
            else if (_class == "Sprocket_Large(Clone)")
                segmentation_class_id = 160;
            else if (_class == "Sprocket_Small(Clone)")
                segmentation_class_id = 170;
            else if (_class == "Wire_Clip_Large(Clone)")
                segmentation_class_id = 180;
            else if (_class == "Wire_Clip_Small(Clone)")
                segmentation_class_id = 190;
            else if (_class == "Stereo_Plug(Clone)")
                segmentation_class_id = 200;
            else if (_class == "004_sugar_box_16k(Clone)")
                segmentation_class_id = 10;
            else if (_class == "cup(Clone)")
                segmentation_class_id = 10;
            else if (_class == "pin(Clone)")
                segmentation_class_id = 10;
            else
                segmentation_class_id = 0;
            fixed_model_transform.Add("remove[ 0, 0, 100, 0 ]remove");
            fixed_model_transform.Add("remove[ 0, -100, 0, 0 ]remove");
            fixed_model_transform.Add("remove[ 100, 0, 0, 0 ]remove");
            fixed_model_transform.Add("remove[ 0, 0, 0, 1 ]remove");
            BoxCollider bc = g.GetComponent<BoxCollider>();
            Vector3 e = bc.size * 100.0f;                //bc.bounds.extents;
            cuboid_dimensions = new float[3] { e.x, e.y, e.z };
        }
    }
    public JsonFolder(int n, GameObject[] g)
    {
        exported_object_classes = new string[n];
        exported_objects = new ExportedObjects[n];
        for (int i = 0; i < n; i++)
        {
            exported_object_classes[i] = g[i].name;
            exported_objects[i] = new ExportedObjects(g[i]);
        }
        
    }

}

