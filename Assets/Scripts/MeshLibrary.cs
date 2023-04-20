using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class MeshLibrary : MonoBehaviour
{
    public GameObject meshContainer;
    public List<Mesh> meshList;
    public MeshFilter meshFilter;
    private MeshRenderer renderer;
    private int index = 0;
    private float originPointOffset;

    public List<Texture2D> thumbnails;
    public List<GameObject> buttons;
    public List<Sprite> sprites;

    public GameObject button;
    private Vector3 pos;
    public GameObject UIpanel;
    public Camera cameraObj;

    void Start()
    {
        meshFilter = meshContainer.GetComponent<MeshFilter>();
        renderer = meshContainer.GetComponent<MeshRenderer>();

        if (meshFilter == null)
        {
            meshContainer = meshContainer.transform.GetChild(0).gameObject;
            meshFilter = meshContainer.GetComponent<MeshFilter>();
            renderer = meshContainer.GetComponent<MeshRenderer>();
        }

        originPointOffset = meshFilter.mesh.bounds.size.y / 2;
        renderer.enabled = false;
        pos = (button.GetComponent<RectTransform>().position);
        SetThumbnail();

        if (cameraObj == null) cameraObj = Camera.main;
    }


    void Update()
    {

        // on click: instansiate mesh at mouse cursor position on a plane (the plane needs a plane tag).
        // offsets y position by half the mesh size, so that the object is placed on the planes surface.

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            RaycastHit hit;
            Physics.Raycast(cameraObj.transform.position, (mouseWorldPos - cameraObj.transform.position).normalized, out hit, 50f);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Plane")
                {
                    GameObject instance = Instantiate(meshContainer, hit.point + new Vector3(0, originPointOffset, 0), meshContainer.transform.rotation);
                    instance.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }

    }


    // Körs vid start, hämtar asset preview thumbnails för objekt/mesh och sätter som sprites för knapparna.
    public void SetThumbnail()
    {
        for (int i = 0; i < meshList.Count; i++)
        {
            Debug.Log("thumbnail");
            GameObject buttonInstance = Instantiate(button);
            buttons.Add(buttonInstance);
            buttonInstance.GetComponent<RectTransform>().position = pos;
            thumbnails.Add(null);
            sprites.Add(null);
            thumbnails[i] = AssetPreview.GetAssetPreview(meshList[i]);
            sprites[i] = Sprite.Create(thumbnails[i], new Rect(0f, 0f, 128f, 128f), new Vector2(0f, 0f));
            buttons[i].GetComponent<Image>().sprite = sprites[i];
            pos = pos + new Vector3(100, 0, 0);
            buttonInstance.transform.SetParent(UIpanel.transform);

        }
    }


    // byter aktivt mesh. anropas via knapparna där varje knapps specifika index sätts som argument

    public void ChangeMesh(int idx)
    {
        meshFilter.mesh = meshList[idx];
        originPointOffset = meshFilter.mesh.bounds.size.y / 2;
    }


}
