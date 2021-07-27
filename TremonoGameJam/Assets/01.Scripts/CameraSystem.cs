using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachine = null;
    private PlayerMove playerMove = null;

    private void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        playerMove = FindObjectOfType<PlayerMove>();

        cinemachine.Follow = playerMove.transform;
    }
}
