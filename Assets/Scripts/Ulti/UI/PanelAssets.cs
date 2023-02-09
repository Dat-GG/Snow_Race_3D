using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PanelAssets", menuName = "Assets/Scriptable Objects/PanelAssets")]
public class PanelAssets : ScriptableObject
{
    static PanelAssets _Instance;
    public static PanelAssets Instance
    {
        get
        {
            if(_Instance==null)
                _Instance = Resources.Load<PanelAssets>("UI/PanelAssets");
            return _Instance;
        }
    }
    public List<GameObject> Datas;
}
