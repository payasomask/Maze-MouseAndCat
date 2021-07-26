using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour
{

  public static MaskManager _MaskManager = null;

  private Sprite Circle;
  private Sprite Square;

  int maskid = 0;
  int blackid = 0;
  Dictionary<int, MaskData> mask_dic = new Dictionary<int, MaskData>();
  Dictionary<int, MaskData> black_dic = new Dictionary<int, MaskData>();


  public class MaskData {
    public int id;
    public string name;
    public Transform t;
  }

  public void Awake()
  {
    _MaskManager = this;
  }

  public void Init(){
    Square = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("common", "white_pt");
    Circle = AssetbundleLoader._AssetbundleLoader.InstantiateSprite("common", "8x8");
  }

  //��ܦb�g�c��
  public void ShowMask(string name){
    foreach(var v in mask_dic){
      if(v.Value.name == name){
        v.Value.t.GetComponent<SpriteMask>().enabled = true;
      }
    }
  }

  //���æb�g�c��
  public void HideMask(string name){
    foreach (var v in mask_dic)
    {
      if (v.Value.name == name)
      {
        v.Value.t.GetComponent<SpriteMask>().enabled = false;
      }
    }
  }

  //��ܶ·t
  public void ShowBlack(string name)
  {
    foreach (var v in black_dic)
    {
      if (v.Value.name == name)
      {
        v.Value.t.GetComponent<SpriteRenderer>().enabled = true;
      }
    }
  }

  //�����·t
  public void HideBlack(string name)
  {
    foreach (var v in black_dic)
    {
      if (v.Value.name == name)
      {
        v.Value.t.GetComponent<SpriteRenderer>().enabled = false;
      }
    }
  }

  public void SetMaskScale(string name, float scale){
    float spirtescale = Circle.bounds.size.x;//�ھڭ�Ϥj�p�٭���

    foreach (var v in mask_dic)
    {
      if (v.Value.name == name)
      {
        v.Value.t.localScale = new Vector3(scale / spirtescale, scale/ spirtescale, 1.0f);
        SineScale ss = v.Value.t.GetComponent<SineScale>();
        if(ss != null){
          ss.setScale(scale / spirtescale);
        }
      }
    }
  }

  public int AddMask(Transform parent ,string name,float scale, bool sinScale = false){
    int tmpid = maskid;
    MaskData tmp = new MaskData();
    GameObject go = new GameObject(name + "_" + tmpid);
    SpriteMask sr = go.AddComponent<SpriteMask>();
    float spirtescale = Circle.bounds.size.x;//�ھڭ�Ϥj�p�٭���
    sr.sprite = Circle;
    go.transform.SetParent(parent);
    go.transform.localPosition = Vector3.zero;
    tmp.t = go.transform;
    tmp.t.localScale = new Vector3(scale/ spirtescale, scale/ spirtescale, 1.0f);
    tmp.id = tmpid;
    tmp.name = name;

    if (sinScale)
      go.AddComponent<SineScale>();

    mask_dic.Add(tmpid, tmp);
    maskid++;
    return tmpid;
  }

 

  public void AddBlack(string name, Vector2 scale)
  {
    MaskData tmp = new MaskData();
    GameObject go = new GameObject(name + "_" + blackid);
    go.transform.SetParent(gameObject.transform);
    float depth = -5;
    //�]���g�c�����վ�black�������I
    float offset_pivot = MazeManager._MazeManager.GetMaze_Pivot();
    go.transform.localPosition = new Vector3(0.0f, offset_pivot, depth);

    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
    sr.sprite = Square;
    sr.color = Color.black;
    sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    tmp.t = go.transform;
    tmp.t.localScale = new Vector3(scale.x, scale.y, 1.0f);
    tmp.id = blackid;
    tmp.name = name;
    black_dic.Add(blackid, tmp);
    blackid++;
  }

  // Update is called once per frame
  void Update()
    {
        
    }

  public void ClearAllMask(){
    foreach(var v in mask_dic){
      if(v.Value.t !=null)
      Destroy(v.Value.t.gameObject);
    }
    mask_dic = new Dictionary<int, MaskData>();
    foreach (var v in black_dic)
    {
      if (v.Value.t != null)
        Destroy(v.Value.t.gameObject);
    }
    black_dic = new Dictionary<int, MaskData>();
  }
}
