using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSkinLoader : MonoBehaviour
{
    [SerializeField] private GameObject preview;
    private void Start()
    {
        GameUtils.AddHandler<GameEvent.OnSkinChange>(OnSkinChangeEvent);
    }

    private void OnDestroy()
    {
        GameUtils.RemoveHandler<GameEvent.OnSkinChange>(OnSkinChangeEvent);
    }
    private void OnSkinChangeEvent(GameEvent.OnSkinChange obj)
    {
        //ShopData _shop = GameUnityData.instance.mShopData;
        ItemData skin = ShopData.Instance.PlayerSkinSelect();
        if (skin != null)
        {
            //  this.ballSkin.TargetMat = _skinFly.mat;
            //   this.ballSkin.LoadSkin();
            GameObject view = skin.prefab;
            if (view != null)
                LoadView(view);
        }
    }
    public void LoadView(GameObject view)
    {
        // var _view = this.View.transform;
        Vector3 pos = preview.transform.localPosition;
        Vector3 scl = preview.transform.localScale;
        Quaternion rot = preview.transform.localRotation;
        GameObject viewNew = Instantiate(view, transform.position, rot);
        viewNew.transform.parent = preview.transform.parent;
        viewNew.transform.localPosition = pos;
        viewNew.transform.localScale = scl;
        viewNew.transform.localRotation = rot;
        Destroy(preview.gameObject);
        preview = viewNew;
    }
}
