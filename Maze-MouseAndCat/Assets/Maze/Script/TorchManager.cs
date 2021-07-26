using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour
{
  public static TorchManager _TorchManager = null;

  int torchid = 0;
  class Torch{
    public ItemType itemtype;
    public GameObject gameobj = null;
    public float time = 0.0f;
    public int id;
    public bool die;
  }

  List<Torch> torch_list = new List<Torch>();
  
  private void Awake(){
    _TorchManager = this;
  }


  //基礎照亮範圍
  float basic_Light_Radius = 3.0f;

  public void PlaceTorch(ItemType type, Vector2 position, float scale){

    Torch tmpT = new Torch();

    GameObject tmp = instantiateObject(gameObject, type.ToString());
    GameObject icon_go = tmp.transform.Find("Icon").gameObject;
    Sprite icon = icon_go.GetComponent<SpriteRenderer>().sprite;
    tmpT.time = type == ItemType.Torch ? 3.0f : 10.0f;
    if (icon != null)
    {
      float iconscale = scale / icon.bounds.size.x;//根據圖資重新計算scale大小
      icon_go.transform.localScale = new Vector3(iconscale, iconscale, 0.0f);
    }

    tmp.transform.localPosition = position;

    GameObject mask_go = tmp.transform.Find("mask").gameObject;
    Sprite mask = mask_go.GetComponent<SpriteMask>().sprite;

    if (mask != null){
      float iconscale = scale / mask.bounds.size.x;//根據圖資重新計算scale大小
      tmp.transform.Find("mask").localScale = new Vector3(basic_Light_Radius * iconscale, basic_Light_Radius * iconscale, 1.0f);
    }

    SineScale ss = tmp.transform.Find("mask").gameObject.AddComponent<SineScale>();
    tmpT.gameobj = tmp;
    tmpT.id = torchid;
    torchid++;
    torch_list.Add(tmpT);

  }

  public void ClearAllTorch(){
    foreach(var v in torch_list){
      Destroy(v.gameobj);
    }
    torch_list = new List<Torch>();
  }

  GameObject instantiateObject(GameObject parent, string name)
  {
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  public void CheckTouch(List<CatManager.Cat> cats_list){
    float cellsize = MazeManager._MazeManager.getCellSize();
    for (int i = 0; i < cats_list.Count; i++)
    {
      CatManager.Cat c = cats_list[i];
      Vector2 catposition = c.root.transform.localPosition;
      if (torch_list.Count == 0)
        c.controller.Resume();
      for(int j = 0; j< torch_list.Count; j++){
        Torch t = torch_list[j];
        Vector2 torchposition = t.gameobj.transform.localPosition;
        if ((torchposition - catposition).magnitude < cellsize * 0.5f){
          c.controller.Stop();
          //每一隻都可以減少食物的時間，如果不要食物的時間要透過狀態切換另外計算剩餘秒速
          t.time -= Time.deltaTime;
          if (t.time <= 0.0f){
            GameObject.Destroy(t.gameobj);
            torch_list.Remove(t);
            j--;
            continue;
          }
        }
        else{
          c.controller.Resume();
        }
      }

    }



  }
}
