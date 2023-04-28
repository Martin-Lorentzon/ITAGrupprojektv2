using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GetSetImageBackup : MonoBehaviour
{

    public string FileName;
    public RenderTexture RT;
    public GameObject RenderCamera;
    public RawImage RI;

    void GetImage()
    {
        Texture2D texture2D = new Texture2D(RT.width, RT.height, TextureFormat.ARGB32, false);
        RenderTexture.active = RT;
        texture2D.ReadPixels(new Rect(0, 0, RT.width, RT.height), 0, 0);
        texture2D.Apply();

        string Path = Application.persistentDataPath + "/" + FileName + ".png";
        byte[] bytes = texture2D.EncodeToPNG();

        File.WriteAllBytes(Path, bytes);
    }

    void SetImage()
    {
        Texture2D texture2D = new Texture2D(RT.width, RT.height);
        string path = Application.persistentDataPath + "/" + FileName + ".png";
        byte[] bytes = File.ReadAllBytes(path);

        texture2D.LoadImage(bytes);
        texture2D.Apply();
        RI.texture = texture2D;
    }

    IEnumerator RenderProcess()
    {
        RenderCamera.SetActive(true);
        yield return new WaitForSeconds(.01f);
        GetImage();
        yield return new WaitForSeconds(.01f);
        RI.gameObject.SetActive(true);
        yield return new WaitForSeconds(.01f);
        SetImage();
        yield return new WaitForSeconds(.01f);
        RenderCamera.SetActive(false);
    }

    public void GetSetImage_btn()
    {
        StartCoroutine(RenderProcess());
    }
}
