using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonLoader : MonoBehaviour
{

  float[] time_map_arr = new float[] {
30,
30,
40,
40,
50,
60,
70
  };

  public static JsonLoader _JsonLoader = null;
  Dictionary<int, MazeConfig> mazeConfig_dic = new Dictionary<int, MazeConfig>();

  private void Awake(){
    _JsonLoader = this;
  }

  public void Init(){
    //start init


    int column_init = 6;//��lcolumn�ƶq
    int row_init = 8;//��lrow�ƶq
    int row_interval = 4;//�C�@�Ӱ϶�row�������Y��
    int column_interval =3;//�C�@�Ӱ϶�columns�������Y��
    int level_interval_count = 0;//���d�϶��ƶq

    int first_level_interval_max_level = 10;//�Ĥ@�Ӱ϶����̤j���d
    int level_interval = 10;//���d�C�Ӱ϶��ƶq
    int max_level = 70;//�̤j���d�϶����̤j���d

    //level init
    for (int i = first_level_interval_max_level; i <= max_level; i += level_interval){
      MazeConfig tmp = new MazeConfig();
      tmp.maxlevel = i;
      tmp.Rows = row_init + row_interval * level_interval_count;
      tmp.Columns = column_init + column_interval * level_interval_count;
      float level_time;
      try
      {
        level_time = time_map_arr[level_interval_count];
      }
      catch(ArgumentOutOfRangeException e)
      {
        level_time = time_map_arr[time_map_arr.Length-1];
      }
      tmp.LimitTime = level_time;
      tmp.boxADReward = new ADReward(ItemType.OilLamp, 1, ItemType.Torch,1);
      tmp.CompletedReward = new ADReward(ItemType.OilLamp, 1, ItemType.Torch,1);
      tmp.GameOverReward = new ADReward(ItemType.OilLamp, 1, ItemType.Torch, 1);
      tmp.DownUIReward = new ADReward(ItemType.OilLamp, 1, ItemType.Torch,0);
      level_interval_count++;
      mazeConfig_dic.Add(i, tmp);
    }

  }


  public MazeConfig GetMazeConfig(int level){

    if (mazeConfig_dic.Count == 0){
      Debug.Log("645 - Init JsonLoader first..");
      return null;
    }

    MazeConfig lastconfig = null;
    foreach(var v in mazeConfig_dic){
      lastconfig = v.Value;
      if (level <= v.Value.maxlevel)
        return v.Value;
    }

    return lastconfig;
  }
}

public enum ItemType{
  Torch,
  OilLamp
}

public enum GameType{
  LIGHT,
  NIGHT
}

public enum MazeType{
  Prims=0,
  HuntKill,
  SZ
}

public class ADReward{
  public ADReward(ItemType Type, int Num, ItemType skipType, int SkipNum)
  {
    this.Type = Type;
    this.Num = Num;
    this.SkipType = skipType;
    this.SkipNum = SkipNum;
  }
  //�ݼs�i
  public ItemType Type;
  public int Num;
  //���ݼs�i���y
  public ItemType SkipType;
  public int SkipNum;
}

public class MazeConfig{
  public int maxlevel;//���d�϶�����
  public int Rows, Columns;
  public float LimitTime;
  public ADReward boxADReward;
  public ADReward CompletedReward;//
  public ADReward GameOverReward;//
  public ADReward DownUIReward;//�U��s�i���y
}
