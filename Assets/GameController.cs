using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    List<HeroScript> heroList;
    //prefabs
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject heroPrefab;

    //Spawn Slot
    [SerializeField] private List<Transform> heroSlot;
    [SerializeField] private Transform mobSlot;

   
    //UI Element
    [SerializeField] private TextMeshProUGUI moneyUI;
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] private TextMeshProUGUI monsterHealth;
    [SerializeField] private TextMeshProUGUI heroCost;
    [SerializeField] private TextMeshProUGUI incomeUI;
    [SerializeField] private TextMeshProUGUI dpsUI;


    int heroCount = 0;
    private int heroLv = 1;
    private int damlevel = 0;
    [SerializeField] private float baseDam = 0;

    //money
    public float money = 5000;
    private float baseCost = 5000;
    private float income = 0;
    private float timeSinceLastIncrease = 0f;
    

    //mobstats
    private bool isBoss = false;
    private float monsterKilled = 0;
    private float baseMobHealth = 100;
    private float currentMobHealth=100;
    [SerializeField]private int   waveMulti=1;
    
    
    
    private GameObject currentMob;
    // Start is called before the first frame update
    void Start()
    {
        heroList = new List<HeroScript>();
        instance = this;
        //Spawn first mob
        SpawnMonster(null);
        money = 5000; 
        monsterHealth.text = currentMobHealth.ToString();
        heroCost.text = baseCost.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyUI.text =money.ToString("0");
        timeSinceLastIncrease += Time.deltaTime;
        incomeUI.text = income.ToString()+"/s";
        if (timeSinceLastIncrease >= 0.1f)
        {
            
            //Increase money and deal damage  per 0.1 sec
            money += income*0.1f;
            timeSinceLastIncrease = 0f;
            
            currentMobHealth -= baseDam*0.1f;
            monsterHealth.text= currentMobHealth.ToString();
        }
        if (currentMobHealth <= 0)
        {
            if (isBoss)
            {
                //Spawn after boss is defeted
                GameObject.Destroy(currentMob);
                SpawnMonster(false);
            
            
            }
            else
            {
                //Spawn monster and boss
                GameObject.Destroy(currentMob);
                SpawnMonster(null);

            }
        }
        

    }
    
    public void SpawnHero()
    {
        //buy new Hero
        if (money >= baseCost&& heroCount<12)
        {
            HeroScript hero = Instantiate(heroPrefab, heroSlot[heroCount].position, Quaternion.identity).GetComponent<HeroScript>();
            heroList.Add(hero);
            heroCount++;
            money -= baseCost;
            
            baseCost *= 1.55f;
            UpdateIncome();
            UpdateDamage();
            heroCost.text= baseCost.ToString();
        }
        
    }
    public void SpawnMonster(bool? defeted)
    {
        if(waveMulti % 10 == 0)
        {
            if(!isBoss)
            {
                //Spawn boss
                monsterKilled++;
                
                
                if (monsterKilled >= 10)
                {
                    waveMulti++;
                    waveUI.text = "Boss " + waveMulti.ToString();
                }
                currentMob = Instantiate(bossPrefab, mobSlot);
                baseMobHealth = waveMulti * waveMulti * Mathf.Log10(waveMulti) + 150 * Random.Range(1, 3);
                money += Mathf.Pow(baseMobHealth,0.33f)*100;
                currentMobHealth = baseMobHealth;
                isBoss = true;
            }
             
            else
            {
                //spawn normal mob
                isBoss = false;
                waveMulti=waveMulti+1;
                waveUI.text = "Wave " + waveMulti.ToString();
                currentMob = Instantiate(bossPrefab, mobSlot);
                baseMobHealth= waveMulti * waveMulti * Mathf.Log10(waveMulti) + 150;
                currentMobHealth = baseMobHealth;

            }
            
        }
        else
        {
            //spawn normal mob
            monsterKilled++;
            
            if (monsterKilled >= 10)
            {
                waveMulti++;
                waveUI.text = "Wave " + waveMulti.ToString();
                monsterKilled = 0;
            }
            currentMob= Instantiate(mobPrefab, mobSlot);
            baseMobHealth= waveMulti * waveMulti * Mathf.Log10(waveMulti) + 150;
            money += Mathf.Pow(baseMobHealth, 0.33f) * 50;
            currentMobHealth = baseMobHealth;
        }
        
    }

    public void UpgradeHero(int level)
    {
           //subtract money after hero level up
           
        
            money -= (level * level % 12f + 150 * level);
            
            UpdateIncome();
            UpdateDamage();
        
        
    }

    public void UpgradeDam()
    {
        //upgrade percentage damge
        int cost= (int)(heroLv *Mathf.Log(heroLv/10f) + 100);
        if(cost<money)
        {
            money-= cost;
            damlevel++;
        }
        
    }
    public void UpdateIncome()
    {
        // set income
        float temp = 0;
        Debug.Log(temp);
        foreach(var item in heroList) {
            Debug.Log("income"+item.income);
            temp += item.income;
        }
        income = temp;
    }
    public void UpdateDamage()
    {
        //set damage
        int temp = 0;
        foreach(var item in heroList)
        {
            temp += (int)item.damage;
            
        }
        baseDam = temp+ temp * damlevel * 0.1f;
        Debug.Log(baseDam);
        dpsUI.text = baseDam.ToString() + " /s";
    }
}
