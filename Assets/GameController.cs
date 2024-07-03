using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    List<Hero> heroList;
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


    int heroCount = 0;
    [SerializeField] private float baseDam = 0;

    //money
    public float money = 100;
    private float baseCost = 100;
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
        //Spawn first mob
        SpawnMonster(null);
        monsterHealth.text = currentMobHealth.ToString();
        heroCost.text = baseCost.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        moneyUI.text =money.ToString();
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
            Instantiate(heroPrefab, heroSlot[heroCount].position,Quaternion.identity);
            heroCount++;
            money -= baseCost;
            income += 100;
            baseCost *= 5;
            baseDam += 20;
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
                money += baseMobHealth * 10 * waveMulti;
                if (monsterKilled >= 10)
                {
                    waveMulti++;
                    waveUI.text = "Boss " + waveMulti.ToString();
                }
                currentMob = Instantiate(bossPrefab, mobSlot);
                currentMobHealth = (baseMobHealth * 10) * waveMulti;
                isBoss = true;
            }
             
            else
            {
                //spawn normal mob
                isBoss = false;
                waveMulti=waveMulti+1;
                waveUI.text = "Wave " + waveMulti.ToString();
                currentMob = Instantiate(bossPrefab, mobSlot);
                currentMobHealth = (baseMobHealth + Random.Range(10, 100)) * waveMulti;

            }
            
        }
        else
        {
            //spawn normal mob
            monsterKilled++;
            money += baseMobHealth * waveMulti;
            if (monsterKilled >= 10)
            {
                waveMulti++;
                waveUI.text = "Wave " + waveMulti.ToString();
                monsterKilled = 0;
            }
            currentMob= Instantiate(mobPrefab, mobSlot);
            currentMobHealth = (baseMobHealth + Random.Range(10, 100)) * waveMulti;
        }
        
    }
    
}
