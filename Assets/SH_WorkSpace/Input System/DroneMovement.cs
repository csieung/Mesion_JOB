using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference droneMovement;
    [SerializeField]
    private InputActionReference droneRotate;
    [SerializeField]
    private InputActionReference toggleCanvasAction;

    public Canvas vrCanvas;

	private void OnEnable()
	{
        droneMovement.action.Enable();
        droneRotate.action.Enable();
        toggleCanvasAction.action.Enable();
	}

	private void OnDisable()
	{
		droneMovement.action.Disable();
		droneRotate.action.Disable();
        toggleCanvasAction.action.Disable();
	}
	// Start is called before the first frame update
	void Start()
    {
        if (vrCanvas != null) 
        { 
            vrCanvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        Vector3 dir = droneMovement.action.ReadValue<Vector3>();
        Vector3 new3dir = new Vector3(-dir.x, dir.z ,dir.y);
        gameObject.transform.Translate(new3dir * 5 * Time.deltaTime);

        //rotation
        float rotate = droneRotate.action.ReadValue<float>();
        gameObject.transform.Rotate(Vector3.forward, rotate * 100 * Time.deltaTime);

        //toggle
        if (toggleCanvasAction.action.triggered)
        {
            vrCanvas.gameObject.SetActive(!vrCanvas.gameObject.activeSelf);
        }
    }
}
