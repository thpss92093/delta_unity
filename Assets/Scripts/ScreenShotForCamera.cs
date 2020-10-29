
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotForCamera:MonoBehaviour{

	public void CaptureScreen(string _path = null)
	{
		if (_path == null)
			_path = "Screenshot.png";

		ScreenCapture.CaptureScreenshot(_path, 0);
	}

	public Texture2D CaptureScreen(Rect rect, bool _isCreatePhoto = false, string _path = null)
	{
		// 先创建一个的空纹理，大小可根据实现需要来设置  
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

		// 读取屏幕像素信息并存储为纹理数据，  
		screenShot.ReadPixels(rect, 0, 0);
		screenShot.Apply();

		// 然后将这些纹理数据，成一个png图片文件  
		if (_isCreatePhoto)
		{
			if(_path == null)
				_path = Application.dataPath + "/Screenshot.png";

			byte[] bytes = screenShot.EncodeToPNG();
			string filename = _path;
			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("截屏了一张图片: {0}", filename));
		}

		// 最后，我返回这个Texture2d对象，这样我们直接，所这个截图图示在游戏中，当然这个根据自己的需求的。  
		return screenShot;
	}

	//
	public Texture2D CaptureCamera(ref Camera _camera, Rect _rect, int _destX, int _destY, bool _isCreatePhoto = false, string _path = null)
	{
		RenderTexture renderTexture = new RenderTexture((int)_rect.width, (int)_rect.height, 24, RenderTextureFormat.ARGB32);
		_camera.targetTexture = renderTexture;
		_camera.Render();

		// 激活这个renderTexture, 并从中中读取像素
		RenderTexture.active = _camera.targetTexture;
		Texture2D screenShot = new Texture2D((int)_rect.width, (int)_rect.height, TextureFormat.ARGB32, false);
		screenShot.ReadPixels(_rect, _destX, _destY);   //从（_destX,_destY）坐标开始读取_rect大小的图片
		screenShot.Apply();

		//重置参数
		//_camera.targetTexture = null;
		RenderTexture.active = null;
		//GameObject.Destroy(renderTexture);

		//生成PNG图片
		if (_isCreatePhoto)
		{
			if (_path == null)
				_path = Application.dataPath + "/Screenshot.png";

			byte[] bytes = screenShot.EncodeToPNG();
			string filename = _path;
			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("截屏了一张照片: {0}", filename));
		}



		return screenShot;
	}
}