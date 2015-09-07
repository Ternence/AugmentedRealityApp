﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CanvasConstants : MonoBehaviour
{
    public Canvas canvas;

    public static float ReferenceWidth = 360;
    public static float ReferenceHeight = 640;

    public static event EventHandler resizeHandler;
    public static string appName = "ActivateAR";
    public static string screenShotPath;

    [HideInInspector]
    public static float canvasWidth;
    [HideInInspector]
    public static float canvasHeight;

    private static RectTransform canvasRect;
	[HideInInspector]
	public static string serverURL = @"http://studio.activatear.com/services/whatsnew.php";

    // Use this for initialization
    void Start()
    {
        if (canvas != null)
        {
            canvasRect = canvas.GetComponent<RectTransform>();
        }
        screenShotPath = Application.persistentDataPath;
        Debug.Log(screenShotPath);
    }

    void Update()
    {
        var newCanvasWidth = canvasRect != null ? canvasRect.rect.width : Screen.width;
        var newCanvasHeight = canvasRect != null ? canvasRect.rect.height : Screen.height;
        if (canvasWidth != newCanvasWidth || canvasHeight != newCanvasHeight)
        {
            canvasWidth = newCanvasWidth;
            canvasHeight = newCanvasHeight;
            if (resizeHandler != null) resizeHandler(this, new EventArgs());
            Debug.Log("Change in canvas size detected : " + canvasWidth + " , " + canvasHeight);
        }
    }


	public static IEnumerator LoadRemoteImage(RawImage image, string url)
	{
		WWW www = new WWW(url);

		yield return www;

		Texture2D texture = new Texture2D (1, 1);
			
		texture.LoadImage (www.bytes);
		image.texture = texture;
		www.Dispose();
		www = null;
	}

	public static IEnumerator GetResponse(string url)
	{
		WWW www = new WWW(url);
		
		yield return www;

	
	}

	public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
	{
		// Unix timestamp is seconds past epoch
		DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
		return dtDateTime;
	}

    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}