using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitStreetview : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            RaycastHit[] hits = Physics.SphereCastAll(Vector3.zero, 20000f, Vector3.up, 0.1f, LayerMask.GetMask("Scene Asset"));
            foreach(RaycastHit hit in hits)
            {
                Destroy(hit.transform.gameObject);
            }
            SceneManager.LoadScene("MainScene");
        }
    }
}
