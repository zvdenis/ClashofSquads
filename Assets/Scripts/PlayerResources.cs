using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ResourceChanged();

public class PlayerResources
{

     /// <summary>
     /// Событие изменения золота игрока
     /// </summary>
     public event ResourceChanged GoldChanged;

     /// <summary>
     /// Текущее количество золота игрока
     /// </summary>
     protected float amountOfGold;
     /// <summary>
     /// Текущее количестов золота игрока
     /// </summary>
     internal float AmountOfGold
     {
          get
          {
               return amountOfGold;
          }
          set
          {
               amountOfGold = value;
               if(GoldChanged != null)
                    GoldChanged.Invoke();
          }
     }
    
}
