using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing.Common;
using ZXing;

public class QRCodeObject : MonoBehaviour
{
    [SerializeField] private List<RawImage> _qrCodeImages;
    [SerializeField] private List<GameObject> _objectList;
    private Dictionary<int, GameObject> _ObjectsToQR = new Dictionary<int, GameObject>();
    private List<Texture2D> _storedEncodedTextures = new List<Texture2D>();
    private void OnEnable()
    {
        for (int i = 0; i < _objectList.Count; i++)
        {
            _storedEncodedTextures.Add(new Texture2D(256, 256));
            _ObjectsToQR.Add(i, _objectList[i]);
        }
        for(int i = 0; i < _ObjectsToQR.Count; i++)
        {
            Color32[] _convertPixelsToTexture = EncodeObject(_ObjectsToQR[i].gameObject.ToString(), _storedEncodedTextures[i].width, _storedEncodedTextures[i].height);
            _storedEncodedTextures[i].SetPixels32(_convertPixelsToTexture);
            _storedEncodedTextures[i].Apply();
            _qrCodeImages[i].texture = _storedEncodedTextures[i];
        }
    }
    private Color32[] EncodeObject(string objectForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(objectForEncoding);
    }
}
