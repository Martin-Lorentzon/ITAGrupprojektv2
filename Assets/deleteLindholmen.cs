using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteLindholmen : MonoBehaviour
{
    public GameObject lindholmel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Destroy(lindholmel);

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(Vector3.zero, 20000f, Vector3.up, 0.1f, LayerMask.GetMask("Scene Asset"));

            foreach (RaycastHit hit in hits)
            {
                Destroy(hit.transform.gameObject);
            }
        }

        
    }
}
