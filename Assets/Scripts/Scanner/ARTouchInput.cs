using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ARTouchInput : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("ARButton"))
                    {
                        hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                        return;
                    }
                }
            }
        }
    }
}
