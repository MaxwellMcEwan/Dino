using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAuth : MonoBehaviour
{
  PlaceManager placeManager;
    // Awake is called before the first frame update
    void Awake()
    {
      placeManager = GetComponentInParent<PlaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
      transform.position = mousePosition;
    }

    private void OnTriggerStay2D(Collider2D col){
      placeManager.authorized = false;
      if (IsInvoking("authorize")) {
        CancelInvoke();
      }
    }

    void OnTriggerExit2D(Collider2D col){
      Invoke("authorize", 0.1f);
    }

    void authorize(){
      GetComponentInParent<PlaceManager>().authorized = true;
    }
}
