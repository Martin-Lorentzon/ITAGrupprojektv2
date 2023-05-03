using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class MeshLibrary : MonoBehaviour
{

    //public GameObject meshContainer;
    //public List<Mesh> meshList;
    //public MeshFilter meshFilter;
    //public List<Texture2D> thumbnails;
    public List<GameObject> buttons;
    //public List<Sprite> sprites;
    //public GameObject button;
    ////private Vector3 pos;
    //public GameObject UIcontentContainer;
    public Camera cameraObj;
    public List<GameObject> prefabs;
    GameObject prefab;
    public Material presetMat;
    GameObject instance;
    public Material glassMat;
    int sceneAssetLayer;
    int groundLayer;
    LayerMask noHitLayer;
    void Start()
    {

        prefab = prefabs[0];
        //meshFilter = prefabs[0].GetComponent<MeshFilter>();

        //pos = (button.GetComponent<RectTransform>().position);
        //SetThumbnail();
        sceneAssetLayer = LayerMask.NameToLayer("Scene Asset");
        groundLayer = LayerMask.GetMask("Ground");
        noHitLayer = LayerMask.GetMask("UI" , "Ground");


    }


    void Update()
    {

        // on click: instansiate object at mouse cursor position on a plane (the plane needs a plane tag).
        // offsets y position by half the mesh size, so that the object is placed on the planes surface.
        // sets instance to be on scene asset layer



            //if (Input.GetMouseButtonDown(0))
            if (Input.GetKeyDown(KeyCode.Z))
            {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);



            RaycastHit hit;
            Physics.Raycast(cameraObj.transform.position, (mouseWorldPos - cameraObj.transform.position).normalized, out hit, 500f);
            //if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), 10000f, ~noHitLayer))

            //    Debug.Log("hit non-dirt");
            //return;

           
            if (hit.collider != null)
            {

                Debug.Log(hit.point);
                if (hit.collider.tag == "Plane" && UILayerCheck.uiHover == false)
                {

                    instance = Instantiate(prefab, hit.point, prefab.transform.rotation);
                    SetMaterial();
                    instance.layer = sceneAssetLayer;
                    prefab.AddComponent<DontDestroyOnLoad>();

                }
            }
        }

    }


    // Körs vid start, hämtar asset preview thumbnails för objekt och sätter som sprites för knapparna.
    //public void SetThumbnail()
    //{
    //    for (int i = 0; i < prefabs.Count; i++)
    //    {
    //        GameObject buttonInstance = Instantiate(button);
    //        buttons.Add(buttonInstance);
    //        buttonInstance.GetComponent<RectTransform>().position = pos;

    //        thumbnails.Add(null);
    //        sprites.Add(null);
    //        thumbnails[i] = AssetPreview.GetAssetPreview(prefabs[i]);
    //        sprites[i] = Sprite.Create(thumbnails[i], new Rect(0f, 0f, 50f, 50f), new Vector2(0f, 0f));
    //        buttons[i].GetComponent<Image>().sprite = sprites[i];

    //        pos = pos + new Vector3(70, 0, 0);
    //        buttonInstance.transform.SetParent(UIcontentContainer.transform);

    //    }
    //    Destroy(button);
    //}


    // sätter material vid instansiering. Sätter material med namnet glass till ett eget material
    public void SetMaterial()
    {

        Material[] materialArr = instance.GetComponent<Renderer>().materials;

        for (int i = 0; i < materialArr.Length; i++)
        {
            string matName = materialArr[i].name;

            switch (matName)
            {
                case "glass (Instance)":
                    materialArr[i] = glassMat;
                    break;

                case "Glass (Instance)":
                    materialArr[i] = glassMat;
                    break;

                case "GLASS (Instance)":
                    materialArr[i] = glassMat;
                    break;

                default:
                    materialArr[i] = presetMat;
                    break;
            }

        }
        instance.GetComponent<Renderer>().materials = materialArr;
    }


    // byter aktivt prefab. anropas via knapparna där varje knapps specifika index sätts som argument

    public void ChangeMesh(int idx)
    {
        prefab = prefabs[idx];
        SceneInformation.selectedObjects.Clear();
        FindAnyObjectByType<ObjectHandler>().ClearSelected();
        //originPointOffset = prefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2;

    }


}

