using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerChracteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitializeXR();
    }


    // Update is called once per frame
    void Update()
    {
        if(!targetDevice.isValid)
        {
            InitializeXR();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }

  
        //if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool primaryButtonValue) && primaryButtonValue)
            //Debug.Log("Pressing Grip Button");

        //if (targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButtonValue) && menuButtonValue)
            //Debug.Log("Pressing Menu Button");

        //if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
            //Debug.Log("Trigger pressed: " + triggerValue);

        //if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
            //Debug.Log("Primary Touchpad: " + primary2DAxisValue);


    }

    void InitializeXR() //Method to populate A list of InputDevices available to us such as controllers and headsets
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerChracteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0]; //Needs fixing with names
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if(prefab)
            {
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
            else
            {
                Debug.LogError("Did not find controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);

            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0f);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue))
        {
            float gripVal = System.Convert.ToSingle(gripValue);
            handAnimator.SetFloat("Grip", gripVal);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0f);
        }
    }

}
