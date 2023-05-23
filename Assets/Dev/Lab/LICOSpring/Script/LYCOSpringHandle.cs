using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYCOSpringHandle : MonoBehaviour
{
    public LayerMask lycoLayer = -1;
    public Material defaultMat;
    public Texture TexQISATO;
    public Texture TexTAKINA;
    public MeshRenderer MRdrAbove;
    public MeshRenderer MRdrBelow;
    private int _MainTex_ID = Shader.PropertyToID("_MainTex");
    // Start is called before the first frame update
    void Start()
    {
        MRdrAbove.material = new Material(defaultMat);
        MRdrAbove.material.SetTexture(_MainTex_ID, TexTAKINA);
        MRdrBelow.material = new Material(defaultMat);
        MRdrBelow.material.SetTexture(_MainTex_ID, TexQISATO);
    }

    public void SwapTex()
    {
        Texture temp = MRdrAbove.material.GetTexture(_MainTex_ID);
        MRdrAbove.material.SetTexture(_MainTex_ID, MRdrBelow.material.GetTexture(_MainTex_ID));
        MRdrBelow.material.SetTexture(_MainTex_ID, temp);
    }

    private
        Vector3 MousePosWhenClick = Vector3.zero;
    private
        Vector3 LYCOPosWhenClick = Vector3.zero;
    private
        Transform LYCO = null;
    private
        bool dragOn = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            //RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            foreach (var hit in Physics.RaycastAll(ray,999, lycoLayer))
            {
                if (hit.transform && hit.transform.gameObject != null)
                {
                    MousePosWhenClick = Input.mousePosition;
                    LYCO = hit.transform;
                    LYCOPosWhenClick = LYCO.position;
                    LYCO.gameObject.SendMessage("OnDrag");
                    dragOn = true;
                    if (hit.transform.gameObject.name == MRdrBelow.gameObject.name)
                        SwapTex();
                    break;
                }
            }
        }

        if (dragOn)
        {
            Vector3 offset = Input.mousePosition - MousePosWhenClick;
            offset.z = 0;
            offsetRecord = offset / Screen.width * 4f;
            LYCO.position = LYCOPosWhenClick + offsetRecord;
            LYCO.gameObject.SendMessage("OnMove", offsetRecord);
        }

        if (Input.GetMouseButtonUp(0))
        {
            MousePosWhenClick = Vector3.zero;
            LYCO.gameObject.SendMessage("OnDrop", offsetRecord);
            dragOn = false;
        }
    }
    public Vector3 offsetRecord;
}
