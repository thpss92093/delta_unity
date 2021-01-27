using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class JsonCamera
{
    // public CameraSettings camera_settings = new CameraSettings();
    public CameraSettings[] camera_settings;
    

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
                // focal_pixel = (image_width_in_pixels * 0.5) / tan(FOV * 0.5 * PI/180)
                fx = (640.0f * 0.5f) / Mathf.Tan(30.0f * 0.5f * (float)Math.PI / 180.0f);
                fy = fx;
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
            horizontal_fov = 30;
            intrinsic_settings = new IS();
            captured_image_size = new CIS();
        }
        public void setCameraSettings()
        {
            name = "main";
            horizontal_fov = 30;
            intrinsic_settings = new IS();
            captured_image_size = new CIS();
        }
    }
    public JsonCamera()
    {
        camera_settings = new CameraSettings[1];
        camera_settings[0] = new CameraSettings();
    }
    
}

