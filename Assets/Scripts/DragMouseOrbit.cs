using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Cameras;

[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class DragMouseOrbit : MonoBehaviour
{
    public Transform target;
    private float distance = 10.0f;
    private float movingSpeed = 0.025f;
    public float xSpeed = 120.0f, ySpeed = 120.0f;
    public float distanceMin = 5f, distanceMax = 100000f;
    public float smoothTime = 2f;
    private float moveGap = 0.2f;
    private float offsety = 5f;
    public bool moving;
    private float distSpeed = 1f;
    private float rotationYAxis = 0.0f, rotationXAxis = 0.0f;
    private float velocityX = 0.0f, velocityY = 0.0f;

    private static string MX = "Mouse X", MY = "Mouse Y", MS = "Mouse ScrollWheel";

    // Use this for initialization
    void Start()
    {
        //Start distance should be longest side * 2
        distance = 300f; ;
        rotationXAxis = 0f;
        rotationYAxis = 0f;
    }
    
    public void SetRotation(float xAxis, float yAxis)
    {
        rotationXAxis = xAxis;
        rotationYAxis = yAxis;
    } 

    public void SetDistance(float d)
    {
        distance = d;
        distSpeed = distance * 0.2f;

    }

    void LateUpdate()
    {

        if (target)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {

                if (Input.GetMouseButton(0))
                {
                    //Debug.Log("Button 0 clicked");
                    velocityX += xSpeed * CrossPlatformInputManager.GetAxis(MX) * distance * movingSpeed;
                    velocityY += ySpeed * CrossPlatformInputManager.GetAxis(MY) * movingSpeed;
                }
                        
                distance = Mathf.Clamp(distance - Input.GetAxis(MS) * distSpeed, distanceMin, distanceMax);
                distSpeed = distance * 0.2f;

                rotationYAxis += velocityX;
                rotationXAxis -= velocityY;
                
                Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);

                
                RaycastHit[] hits;
                hits = Physics.RaycastAll(new Ray(this.transform.position, this.transform.forward), 1000f);
                int hCnt = hits.Length;
                for(int i=0; i<hCnt; ++i)
                {
                    if(hits[i].transform == target)
                    {
                        distance -= hits[i].distance;
                        break;
                    }
                }

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance;
            
                transform.localPosition = position;
                transform.localRotation = rotation;
                velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
                velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
                
            }
        }
    }
    
    public static float ClampAngle(float angle, float min, float max)
    {

        if (angle < min)
            angle = min;
        else if (angle > max)
            angle = max;
            
        return Mathf.Clamp(angle, min, max);
    }
}