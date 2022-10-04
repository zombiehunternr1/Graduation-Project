using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRScannerV1 : MonoBehaviour
{
    [SerializeField] RawImage _rawImageRef;
    private WebCamTexture _webcamTexture;
    private IBarcodeReader _barcodeReader;
    private string _qrCode;

    private void Start()
    {
        _qrCode = string.Empty;
        _barcodeReader = new BarcodeReader();
        _webcamTexture = new WebCamTexture(512, 512);
        _rawImageRef.texture = _webcamTexture;
        StartCoroutine(GetQRCode());
    }
    private IEnumerator GetQRCode()
    {
        _webcamTexture.Play();
        Texture2D snap = new Texture2D(_webcamTexture.width, _webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(_qrCode))
        {
            try
            {
                snap.SetPixels32(_webcamTexture.GetPixels32());
                Result result = _barcodeReader.Decode(snap.GetRawTextureData(), _webcamTexture.width, _webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (result != null)
                {
                    _qrCode = result.Text;
                    if (!string.IsNullOrEmpty(_qrCode))
                    {
                        Debug.Log("Decoded text from QR: " + _qrCode);
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogWarning(e.Message);
            }
            yield return null;
        }
        _webcamTexture.Stop();
    }
    private void OnGUI()
    {
        int w = Screen.width;
        int h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0, 0, 0.5f, 1);
        GUI.Label(rect, _qrCode, style);
    }
}
