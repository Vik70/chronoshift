using UnityEngine;
using Cinemachine;

public class CharacterSwitchManager : MonoBehaviour
{
    public Transform player;
    public Transform clone;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera cloneCamera;

    private bool controllingPlayer = true;

    void Start()
    {
        if (!player || !playerCamera || !cloneCamera)
        {
            Debug.LogError("CharacterSwitchManager: Missing references.");
            return;
        }

        playerCamera.Priority = 10;
        cloneCamera.Priority = 0;

        playerCamera.Follow = player;
        Debug.Log("Camera initially following Player.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (clone == null || !clone.gameObject.activeInHierarchy)
            {
                Debug.Log("Clone is not active. Cannot switch.");
                return;
            }

            controllingPlayer = !controllingPlayer;
            if (controllingPlayer)
            {
                playerCamera.Priority = 10;
                cloneCamera.Priority = 0;
                Debug.Log("Switched to Player.");
            }
            else
            {
                playerCamera.Priority = 0;
                cloneCamera.Priority = 10;
                Debug.Log("Switched to Clone.");
            }
        }
    }

    public void SetClone(GameObject newClone, CinemachineVirtualCamera newCam)
    {
        clone = newClone.transform;
        cloneCamera = newCam;

        if (cloneCamera != null)
        {
            cloneCamera.Follow = clone;
            cloneCamera.Priority = 0;
        }

        Debug.Log("Clone and camera registered.");
    }

    public void ClearClone()
    {
        clone = null;
        if (cloneCamera != null)
        {
            cloneCamera.Priority = 0;
            cloneCamera.Follow = null;
        }

        playerCamera.Priority = 10;
        Debug.Log("Clone cleared. Reverted to player camera.");
    }
}
