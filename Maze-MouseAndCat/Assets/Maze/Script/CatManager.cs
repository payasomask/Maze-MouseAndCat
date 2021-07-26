using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//負責管理貓的移動邏輯
public class CatManager : MonoBehaviour
{
  public enum Housestate{
    stop,
    idle,
  }
  public static CatManager _CatManager = null;
  List<Cat> cats_list = new List<Cat>();
  int catid = 0;
  float cellsize;
  public class Cat{
    public int id;
    public MazeCatController controller;
    public GameObject root;
    public float mtime;
    public float mmovetime;
  }
  public class CatHouse{
    public Housestate currentstate = Housestate.idle;
    public int x, y;
    public float mtime = 0.0f;
    public float mcattime = 1.0f;
    public float cat_amount = 3;
    public GameObject root;
  }
  CatHouse[] cathouse_arr = null;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }
  private void Awake()
  {
    _CatManager = this;
  }
  // Update is called once per frame
  void Update(){
    updateCatHouse();
    updateCats();
    }

  public void init(float cellsize){
    this.cellsize = cellsize;
    cathouse_arr = new CatHouse[1];
    //cathouse_arr[0] = 
    CatHouse ch = new CatHouse();
    ch.x = 2;
    ch.y = 2;
    ch.root = instantiateObject(gameObject, "CatHouse");
    ch.root.transform.localPosition = MazeManager._MazeManager.GetMaze().GetCellPosition(ch.x, ch.y);
    //ch.root.transform.Find("icon").localScale = new Vector3(cellsize, cellsize, 1.0f);
    cathouse_arr[0] = ch;
  }

  public void InstantiateCat(CatHouse cathouse) {
    //貓的產生位置只能是貓屋?
    Cat c = new Cat();
    c.root = instantiateObject(gameObject, "Cat");
    c.root.transform.localPosition = MazeManager._MazeManager.GetMaze().GetCellPosition(cathouse.x, cathouse.y);
    c.id = catid;
    c.mmovetime = 1.0f;
    c.mtime = 0f;
    c.controller = c.root.GetComponent<MazeCatController>();
    c.controller.init(cathouse.x, cathouse.y, cellsize);

    cats_list.Add(c);
  }

  GameObject instantiateObject(GameObject parent, string name)
  {
    GameObject g = AssetbundleLoader._AssetbundleLoader.InstantiatePrefab(name);
    g.transform.SetParent(parent.transform, true);

    return g;
  }

  void updateCatHouse(){
    if (cathouse_arr == null)
      return;
    if (cathouse_arr.Length == 0)
      return;

    for(int i = 0; i < cathouse_arr.Length; i++){

      CatHouse ch = cathouse_arr[i];
      if (ch.currentstate == Housestate.stop)
        continue;

      if (ch.cat_amount <= 0)
        continue;

      ch.mtime += Time.deltaTime;
      if(ch.mtime >= ch.mcattime){
        ch.mtime = 0f;
        InstantiateCat(ch);
        ch.cat_amount--;
      }
    }
  }
  void updateCats(){
    if (cats_list == null)
      return;

    for(int i =  0; i< cats_list.Count; i++){
      Cat c = cats_list[i];

      if (c.controller.IsStop())
        continue;

      if (c.controller.IsMoving())
        continue;

      c.mtime += Time.deltaTime;
      if(c.mtime >= c.mmovetime){
        c.mtime = 0.0f;
        c.controller.moveDir();
      }
    }
  }

  public void Stop(){
    for(int i = 0; i < cathouse_arr.Length; i++){
      cathouse_arr[i].currentstate = Housestate.stop;
    }

    for (int i = 0; i < cats_list.Count; i++){
      cats_list[i].controller.Stop();
    }
  }

  public void Resume(){
    for (int i = 0; i < cathouse_arr.Length; i++){
      cathouse_arr[i].currentstate = Housestate.idle;
    }

    for (int i = 0; i < cats_list.Count; i++)
    {
      cats_list[i].controller.Resume();
    }
  }

  public bool isTouchCats(Vector2 playerposition){
    if (cats_list.Count <= 0)
      return false;
    for(int i = 0; i< cats_list.Count; i++){
      Cat c = cats_list[i];
      if (c.controller.IsStop()){
        continue;
      }

      Vector2 catposition = c.root.transform.localPosition;
      float dis = (catposition - playerposition).magnitude;
      if (dis < cellsize * 0.5f)
      {
        //判定玩家被碰到輸了
        return true;
      }
      else continue;
    }
    return false;
  }

  public List<Cat> GetCatsList(){
    return cats_list;
  }
}
