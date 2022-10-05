using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using ZXing.Common;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] private RawImage _qrCodeImage;
    [SerializeField] private TMP_InputField _textInputField;
    private Texture2D _storedEncodedTexture;

    void Start()
    {
        _storedEncodedTexture = new Texture2D(256, 256);
    }
    public void OnClickEncoded()
    {
        EncodeTextToQR();
    }
    private void EncodeTextToQR()
    {
        string _textWrite;
        if (string.IsNullOrEmpty(_textInputField.text))
        {
            _textWrite = "You shoud write something!";
        }
        else
        {
            _textWrite = _textInputField.text;
        }
        Color32[] _convertPixelsToTexture = Encode(_textWrite, _storedEncodedTexture.width, _storedEncodedTexture.height);
        _storedEncodedTexture.SetPixels32(_convertPixelsToTexture);
        _storedEncodedTexture.Apply();
        _qrCodeImage.texture = _storedEncodedTexture;
    }
    private Color32[] Encode(string textForEncoding, int width, int height)
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
        return writer.Write(textForEncoding);
    }
}
