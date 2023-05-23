using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class LYCOTest : MonoBehaviour
{
    public Transform trans;
    public Vector3 defaultPos = Vector3.one * -9999.0f;
    public Vector3 originPos = Vector3.one * -9999.0f;
    public Vector3 default_a = new Vector3(0, 0, 0);
    public Vector3 default_pos = new Vector3(3, 4, 0);

    public Vector3 inertia = new Vector3(0.08f, 0.08f, 0);
    public Vector3 factor = new Vector3(1.2f, 2.0f, 0);
    public Vector3 damper = new Vector3(0.99f, 0.988f, 0);
    public float stop_threshold = 0.05f;
    private void Start()
    {
        if (originPos == defaultPos)
            originPos = trans.position;
    }
    [SerializeField]
    private float max = 0;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [SerializeField]
    private Vector3 a = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 pos = new Vector3(3, 4, 0);
    // Update is called once per frame
    void Update()
    {
        if (!m_running)
            return;

        a -= pos * 2.0f;
        pos += Vector3.Scale(Vector3.Scale(a, inertia), factor);
         if (Mathf.Abs(pos.x) > max)
            max = Mathf.Abs(pos.x);
        a = Vector3.Scale(a, damper);
        offset = pos;offset.z = 0;
        var rad = Mathf.Clamp(offset.x / default_pos.x, -1,1)  * Mathf.PI;
        offset.y += Mathf.Cos(rad) - 1;
        trans.position = originPos + offset;
        trans.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * rad * 0.5f);

        if (a.magnitude < stop_threshold || pos.magnitude < stop_threshold)
        { 
            running = false;
            reset();
        }
    }
    [SerializeField]
    private bool m_running = false;
    public bool running{
        get {
            return m_running;
        }
        set {
            m_running = value;
        }
    }
    public void reset() {
        if (originPos != defaultPos)
            trans.position = originPos;
        else
            originPos = trans.position;
        trans.eulerAngles = Vector3.zero;
        a = default_a;
        pos = default_pos;
        max = 0;
    }
    public void OnDrag()
    {
        running = false;
        //Debug.Log("OnDrag");
    }
    public void OnDrop(Vector3 offset)
    {
        pos = offset;
        running = true;

        //Debug.Log("OnDrop");
    }
    public void OnMove(Vector3 offset)
    {
        running = false;
        pos = offset;
        //Debug.Log(pos);
        var rad = Mathf.Clamp(offset.x / default_pos.x, -1, 1) * Mathf.PI;
        offset.y += Mathf.Cos(rad) - 1;
        trans.position = originPos + offset;
        trans.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * rad * 0.5f);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(LYCOTest))]
public class LYCOTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var instance = target as LYCOTest;
        if (target == null)
            return;
        if (GUILayout.Button("Test"))
        {
            if (!EditorApplication.isPlaying)
                return;
            if (instance.trans == null)
                return;
            instance.reset();
            instance.running = true;
        }
        if (GUILayout.Button("stop"))
        {
            if (instance.trans == null)
                return;
            instance.running = false;
        }
        if (GUILayout.Button("Test2"))
        {
            Debug.Log(Screen.currentResolution.width);
            Debug.Log(Screen.currentResolution.height);
            Debug.Log(Screen.resolutions);
        }
    }
}
#endif