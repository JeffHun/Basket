using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Shooter shooter;
    [SerializeField] Basket basket;
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] int shootFrequency;
    [SerializeField] Transform pointCollector;
    [SerializeField] Transform pointLocation;
    [SerializeField] GameObject pointPrefab;
    float timer;
    bool isTimeOut;
    int point;

    private static GameManager instance = null;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(!isTimeOut)
            timer += Time.deltaTime;

        int second = (int)timer % 60;
        int fractions = (int)Mathf.Round((timer - second)*100);
        timerTxt.text = second + ":" + fractions;

        if((int)timer%shootFrequency == 0)
        {
            shooter.SetIsShooting(true);
        }

        if(timer >= 59.999f)
        {
            isTimeOut = true;
            basket.isGameEnd = true;
            timerTxt.text = "59:999";
        }

    }

    public void IncreasePoint()
    {
        point++;
        GameObject ptn = Instantiate(pointPrefab, pointLocation.position, Quaternion.identity);
        ptn.transform.parent = pointCollector.transform;
        if(point%5 == 0)
            pointLocation.position = new Vector3(pointLocation.position.x - .5f*4, pointLocation.position.y - .5f, pointLocation.position.z);
        else
            pointLocation.position = new Vector3(pointLocation.position.x + .5f, pointLocation.position.y, pointLocation.position.z);
    }
}
