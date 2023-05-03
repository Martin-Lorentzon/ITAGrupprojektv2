using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// VAR FAN FÅR DEN PREFABS FÖR BILDEN. VAR ÄR BILD OBJECT KOPIAN GJORD
// SÅ JAG KAN SÄTTA PÅ DEN!
public class GetSetImage : MonoBehaviour
{
    List<GameObject> prefabs = new List<GameObject>();
    public GameObject prefab;
    public string FileName;
    public RenderTexture RT;
    public GameObject RenderCamera;
    public RawImage RI;

    Texture2D texSend;

    private void Start()
    {
        prefabs.Add(RI.gameObject);
    }

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
        GameObject newest = Instantiate(prefab);
        prefabs.Add(newest);
        newest.transform.SetParent(transform.GetChild(0));
        //newest.transform.position = prefabs[0].transform.position + Vector3.right*120f*(prefabs.Count);

        Texture2D texture2D = new Texture2D(RT.width, RT.height);
        string path = Application.persistentDataPath + "/" + FileName + ".png";
        byte[] bytes = File.ReadAllBytes(path);

        texture2D.LoadImage(bytes);
        texture2D.Apply();
        prefabs[prefabs.Count-1].GetComponent<RawImage>().texture = texture2D;
        texSend = texture2D;
    }

    void OtherRenderProcess()
    {
        RenderCamera.SetActive(true);
        GetImage();
        RI.gameObject.SetActive(true);
        SetImage();
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
        //StartCoroutine(RenderProcess());
        OtherRenderProcess();
    }
    public GameObject GetSetImage_non()
    {
        //StartCoroutine(RenderProcess());
        StartCoroutine(RenderProcess());
        return prefabs[prefabs.Count-1];
    }
}