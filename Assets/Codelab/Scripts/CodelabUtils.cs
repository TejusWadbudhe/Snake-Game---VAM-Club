using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Codelab utility methods.
/// </summary>
/// <remarks>
/// This class implements methods to show an Android UI toast message and then
/// exit the app.  This is used to communicate fatal configuration errors.
/// </remarks>
public class CodelabUtils
{
    /// <summary>Coroutine to display an error then exit.</summary>
    /// A coroutine is like a function that has the ability to pause execution and return control
    /// to Unity but then to continue where it left off on the following frame.
    ///  IEnumerator is a . NET type that is used to fragment large collection or files, 
    ///  or simply to pause an iteration. 
    ///  
    public static IEnumerator ToastAndExit(string message, int seconds)
    {
        _ShowAndroidToastMessage(message);
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }
   
    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    /// <param name="length">Toast message time length.</param>
    public static void _ShowAndroidToastMessage(string message)
    { 
        AndroidJavaClass unityPlayer =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass =
                new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaObject toastObject =
                            toastClass.CallStatic<AndroidJavaObject>(
                                "makeText", unityActivity,
                                message, 0);
                        toastObject.Call("show");
                    }));
        }
    }
}
