using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;
public class Scanner : MonoBehaviour
{
    private WebCamTexture camTexture;
    [SerializeField] private GameObject BackgroundQuad;
    private Quaternion baseRotation;
    bool shouldEncodeNow;
    string LastResult = "https://graphicmismatch.com";
    private int cameraNo;
    public TMP_Text t;
    void Start()
    {
        cameraNo = 0;
        EnableCamera();
    }

    public void changeCamera() {
        cameraNo++;
        if (cameraNo > WebCamTexture.devices.Length - 1) {
            cameraNo = 0;
        }
        EnableCamera();
    }
    private void EnableCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera");

            return;
        }
        camTexture = new WebCamTexture(devices[cameraNo].name, Screen.height, Screen.width);

        if (camTexture == null)
        {
            Debug.Log("Unable to find cam");

            return;
        }
        baseRotation = transform.rotation;
        BackgroundQuad.GetComponent<Renderer>().material.mainTexture = camTexture;
        camTexture.Play();
        CalculateBackgroundQuad();
    }
    void CalculateBackgroundQuad()
    {
        Camera cam = Camera.main;
        float ScreenRatio = (float)Screen.width / (float)Screen.height;

        float videoRotationAngle = camTexture.videoRotationAngle;

        BackgroundQuad.transform.localRotation = baseRotation * Quaternion.AngleAxis(camTexture.videoRotationAngle, Vector3.forward);

        float fixedHeight = cam.orthographicSize * 2;

        Vector3 QuadScale = new Vector3(1f, fixedHeight, 1f);
        float TextureRatio;

        //adjust the scaling for portrait Mode & Landscape Mode
        if (videoRotationAngle == 0 || videoRotationAngle == 180)
        {
            //landscape mode
            TextureRatio = (float)(camTexture.width) / (float)(camTexture.height);
            if (ScreenRatio > TextureRatio)
            {
                float SH = ScreenRatio / TextureRatio;
                float TW = TextureRatio * fixedHeight * SH;
                float TH = fixedHeight * (camTexture.videoVerticallyMirrored ? -1 : 1) * SH;
                QuadScale = new Vector3(TW, TH, 1f);
            }
            else
            {
                float TW = TextureRatio * fixedHeight;
                QuadScale = new Vector3(TW, fixedHeight * (camTexture.videoVerticallyMirrored ? -1 : 1), 1f);
            }
        }
        else
        {
            //portrait mode
            TextureRatio = (float)(camTexture.height) / (float)(camTexture.width);
            if (ScreenRatio > TextureRatio)
            {
                float SH = ScreenRatio / TextureRatio;
                float TW = fixedHeight * -1f * SH;
                float TH = TW * (camTexture.videoVerticallyMirrored ? 1 : -1) * SH;
                QuadScale = new Vector3(TW, TH, 1f);
            }
            else
            {
                float TW = TextureRatio * fixedHeight;
                QuadScale = new Vector3(fixedHeight * -1f, TW * (camTexture.videoVerticallyMirrored ? 1 : -1), 1f);
            }
        }
        BackgroundQuad.transform.localScale = QuadScale;

    }
    public void ReadCode() {
        DecodeQR(camTexture.GetPixels32(),camTexture.width,camTexture.height);
    }
    void DecodeQR(Color32[] c, int w, int h)
    {
        // create a reader with a custom luminance source
        var barcodeReader = new BarcodeReader { AutoRotate = true, Options = new ZXing.Common.DecodingOptions { TryHarder = true } };

        try
        {
            // decode the current frame
            var result = barcodeReader.Decode(c, w, h);
            if (result != null)
            {
                LastResult = result.Text;
                shouldEncodeNow = true;
                print(result.Text);
            }

        }
        catch
        {
        }

    }
    private void Update()
    {
        t.text = LastResult;
    }
}
