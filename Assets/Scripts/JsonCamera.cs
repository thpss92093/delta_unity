﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class JsonCamera
{
    public CameraSettings camera_settings = new CameraSettings();
   
    [Serializable]
    public class CameraSettings
    {
        public string name;
        public int horizontal_fov;
        public IS intrinsic_settings;
        public CIS captured_image_size;

        [Serializable]
        public class IS
        {
            public float fx;
            public float fy;
            public int cx;
            public int cy;
            public int s;
            
            public IS()
            {
                fx = 68.05538f;
                fy = 68.05538f;
                cx = 320;
                cy = 240;
                s = 0;
            }
        }
        [Serializable]
        public class CIS
        {
            public int width;
            public int height;

            public CIS()
            {
                width = 640;
                height = 480;
            }
        }

        public CameraSettings()
        {
            name = "main";
            horizontal_fov = 20;
            intrinsic_settings = new IS();
            captured_image_size = new CIS();
        }
        public void setCameraSettings()
        {
            name = "main";
            horizontal_fov = 20;
            intrinsic_settings = new IS();
            captured_image_size = new CIS();
        }
    }
    
}

