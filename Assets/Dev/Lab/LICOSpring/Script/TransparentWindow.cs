using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    // Define function signatures to import from Windows APIs

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    // Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    IntPtr Handle;
    void Start()
    {
#if !UNITY_EDITOR
        var margins = new MARGINS() { cxLeftWidth = -1 };
 
        // Get a handle to the window
        var hwnd = GetActiveWindow();
 
        // Set properties of the window
        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
         
        // Extend the window into the client area
        See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa969512%28v=vs.85%29.aspx 
        DwmExtendFrameIntoClientArea(hwnd, ref margins);

        Handle = GetForegroundWindow();

#endif
    }
    private bool mouseDown = false;
    void Update()
    {
#if !UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0))
        { 
            ReleaseCapture();
            SendMessage(Handle, 0xA1, 0x02, 0);
            SendMessage(Handle, 0x0202, 0, 0);
        }
#endif
    }
    //new instead of post-process 
        // Pass the output of the camera to the custom material
        // for chroma replacement
        //void OnRenderImage(RenderTexture from, RenderTexture to)
        //{
        //    Graphics.Blit(from, to, m_Material);
        //}
}