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
            else if (_class == "004_sugar_box_16k(Clone)")
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

            if(_class == "Gear_Large")
                segmentation_class_id = 10;
            else if (_class == "Waterproof_Female")
                segmentation_class_id = 20;
            else if(_class == "DSUB_Female")
                segmentation_class_id = 30;
            else if(_class == "M16_Screw")
                segmentation_class_id = 40;
            else if (_class == "M16_Hex_Nut")
                segmentation_class_id = 50;
            else if (_class == "004_sugar_box_16k(Clone)")
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

