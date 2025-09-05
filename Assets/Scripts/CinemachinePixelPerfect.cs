using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.U2D;

[ExecuteAlways]
[RequireComponent(typeof(CinemachineCamera))]
public class CinemachinePixelPerfect : MonoBehaviour
{
    private PixelPerfectCamera pixelPerfectCamera;
    private CinemachineCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
    }

    void LateUpdate()
    {
        if (pixelPerfectCamera == null || vcam == null) return;

        // Độ rộng của 1 pixel trong Unity Unit
        float unitsPerPixel = 1f / pixelPerfectCamera.assetsPPU;

        // Lấy vị trí camera
        Transform camTransform = vcam.transform;
        Vector3 pos = camTransform.position;

        // Snap theo pixel grid
        pos.x = Mathf.Round(pos.x / unitsPerPixel) * unitsPerPixel;
        pos.y = Mathf.Round(pos.y / unitsPerPixel) * unitsPerPixel;

        camTransform.position = pos;
    }
}
