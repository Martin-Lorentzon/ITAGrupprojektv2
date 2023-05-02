using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


public class Scatter : MonoBehaviour
{

    public List<GameObject> models;
    public List<GameObject> buttons;
    GameObject activeModel;
    public Camera cam;
    public float brushSize;
    bool runCorutine = true;
    public float density;
    public DecalProjector decal;
    
    public GameObject button;
    public GameObject scatterPanel;

    public Slider densitySlider;
    public Slider brushSizeSlider;

    public Material decalMat;
    public Color scatterColor;
    public Color eraseColor;

    public Button scatterPlacementButton;
    public Button singelPlacementButton;
    public Button eraseButton;
    List<GameObject> removeList;
    public GameObject defaultActiveSettingsButton;

    enum Modes { scatter, singlePlacement, erase, idle}
    int mode;
    int sceneAssetLayer;
    int groundLayer;
    int UIandGround;
    int UILayer;

    void Start()
    {
        decalMat= GetComponent<DecalProjector>().material;
        activeModel = models[0];
        brushSize = brushSizeSlider.value;
        density = densitySlider.value;  
        decal = GetComponent<DecalProjector>();

        brushSizeSlider.onValueChanged.AddListener(delegate { SetBrushSize(); });
        densitySlider.onValueChanged.AddListener(delegate { SetDensity(); });
        scatterPlacementButton.onClick.AddListener(SetScatterPlacement);
        singelPlacementButton.onClick.AddListener(SetSinglePlacement);
        eraseButton.onClick.AddListener(SetErase);
        sceneAssetLayer = LayerMask.NameToLayer("Scene Asset");
        groundLayer = LayerMask.GetMask("Ground");




        //for (int i = 0; models.Count > i; i++)
        //{
        //    GameObject buttonInstance = Instantiate(button);
        //    buttons.Add(buttonInstance);
        //    buttonInstance.transform.SetParent(scatterPanel.transform);

        //}
    }

    void SetScatterPlacement()
    {
        mode = (int)Modes.scatter;
    }

    void SetSinglePlacement()
    {
        mode = (int)Modes.singlePlacement;
    }

    void SetErase()
    {
        mode = (int)Modes.erase;
    }
    void SetIdle()
    {

        decal.enabled = false;
        mode = (int)Modes.scatter;
    }
 
        
    void SetBrushSize()
    {
        brushSize = brushSizeSlider.value;
    }

    void SetDensity() 
    {
        density = densitySlider.value;
    }

    void Update()
    {

    


        if (SceneInformation.ApplicationState == SceneInformation.AppState.Scatter)
        {
            decal.enabled = true;
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePosition);

            RaycastHit hit;
            Physics.Raycast(cam.transform.position, (mouseWorldPos - cam.transform.position).normalized, out hit, 200f, groundLayer);
            gameObject.transform.position = hit.point;
            

            switch (mode)
            {
                case (int)Modes.scatter:

                    decalMat.SetColor("_Color", scatterColor);
          
                    float opacity = Mathf.Lerp(0.05f, 0.95f, density);
                    gameObject.transform.localScale = new Vector3(brushSize, brushSize, brushSize) * 2;
                    decal.fadeFactor = opacity;

                    if (Input.GetMouseButton(0) && runCorutine == true && UILayerCheck.uiHover == false)
                    {
                        Debug.Log(hit.transform.gameObject.layer);
                        StartCoroutine(ScatterObjects(Mathf.Lerp(0.2f/brushSize, 0.0005f/brushSize, density)));
                    }
                    break;

                case (int)Modes.singlePlacement:

                    decal.fadeFactor = 0f;

                    if (Input.GetMouseButtonDown(0) && UILayerCheck.uiHover == false)
                    {
                        float pivotOffset = activeModel.GetComponent<MeshRenderer>().bounds.size.y / 2;
                        GameObject instance = Instantiate(activeModel, hit.point, activeModel.transform.rotation);
                        instance.tag = "ScatterObject";
                        instance.layer = sceneAssetLayer;
                        instance.AddComponent<DontDestroyOnLoad>();
                    }
                    break;
                   
                case (int)Modes.erase:                  
                    decal.fadeFactor = 0.95f;
                    decalMat.SetColor("_Color", eraseColor);
                    gameObject.transform.localScale = new Vector3(brushSize, brushSize, brushSize) * 2;

                    if (Input.GetMouseButton(0))
                    {


                        RaycastHit[] SphereHit = Physics.SphereCastAll(hit.point, brushSize, Vector3.up, brushSize);
                        
                            if (SphereHit.Length > 0)
                            {
                                foreach(RaycastHit obj in SphereHit)
                                {
                                    if(obj.collider.tag == "ScatterObject")
                                    {
                                    //removeList.Add(obj.collider.gameObject);
                                    Destroy(obj.collider.gameObject);
                                    }
                                }
                            }
                        
                            
                    }
                    break;
            }
                               
        }
        else
        {
            SetIdle();
        }
    }


  

    private IEnumerator ScatterObjects(float density)
    {
        runCorutine = false;
        yield return new WaitForSeconds(density);
        runCorutine = true;


        float pivotOffset = activeModel.GetComponent<MeshRenderer>().bounds.size.y / 2;
        
        //if(activeModel.tag == "BottomOrigin")
        //{
        //    pivotOffset = 0f;
        //}

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePosition);
        float randomX = UnityEngine.Random.Range(-brushSize, brushSize);
        float randomZ = UnityEngine.Random.Range(-brushSize, brushSize);
        float randomScale = UnityEngine.Random.Range(0.75f, 1.25f); 
        float randomRotation = UnityEngine.Random.Range(1,359);

        RaycastHit hit;
        Physics.Raycast(cam.transform.position, (mouseWorldPos - cam.transform.position).normalized, out hit, 200f, groundLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Plane")
            {
                GameObject instance = Instantiate(activeModel, hit.point, activeModel.transform.rotation);
                Vector3 instanceScale = instance.transform.localScale;
                instance.transform.localScale= instanceScale * randomScale;
                instance.transform.position = hit.point + new Vector3(randomX, 1, randomZ);
                Vector3 pos = instance.transform.position;
                RaycastHit downHit;
                Physics.Raycast(pos, Vector3.down, out downHit, 100f, groundLayer);
                if (downHit.collider.tag == "Plane")
                {
                    instance.transform.position = downHit.point;
                    instance.transform.eulerAngles = new Vector3(instance.transform.eulerAngles.x, randomRotation, instance.transform.eulerAngles.z);
                }

                instance.tag = "ScatterObject";
                instance.layer = sceneAssetLayer;
                instance.AddComponent<DontDestroyOnLoad>();
   
            }
        }
    }

    public void ChangeObject(int idx)
    {
        activeModel = models[idx];
    }


}
