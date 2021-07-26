using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���a�ϥιD��޲z
public class PlayerItemManager : MonoBehaviour
{
  public static PlayerItemManager _PlayerItemManager = null;
  private void Awake(){
    _PlayerItemManager = this;
  }

    // Update is called once per frame
    void Update(){

    }

  public void UseTorch(Vector2 position){
    float scale = MazeManager._MazeManager.getCellSize();
    TorchManager._TorchManager.PlaceTorch(ItemType.Torch, position, scale);
  }

  //public void UseOilLamp(float oillampmaskscale){
  //  float playerscale = MazeManager._MazeManager.PlayerMaskScale();
  //  float mazescale = MazeManager._MazeManager.getCellSize();
  //  MaskManager._MaskManager.SetMaskScale("player", playerscale * oillampmaskscale * mazescale);
  //}
  public void UseOilLamp(Vector2 position)
  {
    float scale = MazeManager._MazeManager.getCellSize();
    TorchManager._TorchManager.PlaceTorch(ItemType.OilLamp,position, scale);
  }
  //public void UseStaff()
  //{
  //  MaskManager._MaskManager.HideBlack("black");

  //}

}
