using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float TimeMultiply{get;set;} = 1;

    private float StartTime{get;set;} = 0;
     
    private float RoundTime{get;set;} = 180;

    public float currentTime =0;

    [field: SerializeField] private Gradient Color{get;set;}
    [field: SerializeField] private Light Luz{get;set;}


    void FixedUpdate()
    {
        StartTime += TimeMultiply * Time.fixedDeltaTime;


        currentTime = (StartTime/RoundTime) * 100;

        UpdateLight(Interpolate(currentTime, 0f, 1f));
    }
    
    private void UpdateLight(float dayPercent)
    {
        Luz.color = Color.Evaluate(dayPercent);
    }

    float Interpolate(float valor, float valorMinimo, float valorMaximo)
    {   
    // Verifica se o valor está fora do intervalo e retorna 0 ou 1, respectivamente
    if (valor <= valorMinimo)
        return 0;
    if (valor >= valorMaximo)
        return 1;

    // Calcula a porcentagem de interpolação entre o valor mínimo e máximo
    float porcentagem = (valor - valorMinimo) / (valorMaximo - valorMinimo);

    return porcentagem;
    }
    
}
