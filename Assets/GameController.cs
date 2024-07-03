using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    List<Hero> heroList;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private List<Transform> heroSlot;
    [SerializeField] private Transform mobSlot;

    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private GameObject bossPrefab;

    [SerializeField] private TextMeshProUGUI moneyUI;
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] private TextMeshProUGUI monsterHealth;
    [SerializeField] private TextMeshProUGUI heroCost;
    [SerializeField] private TextMeshProUGUI incomeUI;
    int heroCount = 0;
    public float money = 100;
    private float baseCost = 100;
    private float income = 0;
    private float timeSinceLastIncrease = 0f;
    [SerializeField]private float baseDam = 0;
    private float baseMobHealth = 100;
    private float currentMobHealth=100;
    [SerializeField]private int   waveMulti=1;
    private float monsterKilled = 0;
    private bool isBoss = false;
    
    private GameObject currentMob;
    // Start is called before the first frame update
    void Start()
    {
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
            money += income*0.1f;
            timeSinceLastIncrease = 0f;
            
            currentMobHealth -= baseDam*0.1f;
            monsterHealth.text= currentMobHealth.ToString();
        }
        if (currentMobHealth <= 0)
        {
            if (isBoss)
            {
            
                GameObject.Destroy(currentMob);
                SpawnMonster(false);
            
            
            }
            else
            {
            
                GameObject.Destroy(currentMob);
                SpawnMonster(null);

            }
        }
        

    }
    
    public void SpawnHero()
    {
        if (money >= baseCost)
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

                isBoss = false;
                waveMulti=waveMulti+1;
                waveUI.text = "Wave " + waveMulti.ToString();
                currentMob = Instantiate(bossPrefab, mobSlot);
                currentMobHealth = (baseMobHealth + Random.Range(10, 100)) * waveMulti;

            }
            
        }
        else
        {
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
