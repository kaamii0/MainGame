using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{   
    private float maxHP = 100;
    private float minHP = 0;
    private float currentHP;
    [SerializeField] private Slider slider;
    PlayerManager playerManager;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxHP;
        slider.minValue = minHP;

        currentHP = maxHP;
        slider.value = currentHP;
    }

    private void TakeDamage()
    {
         
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
 