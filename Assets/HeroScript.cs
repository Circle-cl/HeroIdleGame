using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    public int level=1;
    public float income=0;
    public float damage=0;

    private void OnMouseDown()
    {//upgrade level
        if (GameController.instance.money > (level * level % 12f + 150 * level))
        {
            level += 1;
            Income();
            Damage();
            GameController.instance.UpdateDamage();
            GameController.instance.UpdateIncome();
            GameController.instance.UpgradeHero(level);
        }
        
        
    }
    private void Awake()
    {
        //Setup dam and income
        level= 1;
        Income();
        Damage();
        
    }
    private void Income()
    {
        //income
        income = (int)(level * (Mathf.Log10(level / 10f) + 15f));
    }
    private void Damage()
    {   //income
        Debug.Log(Mathf.Log10(10));
        damage = (((level / 2f) * Mathf.Log10(level)) + 30f);
    }
}
