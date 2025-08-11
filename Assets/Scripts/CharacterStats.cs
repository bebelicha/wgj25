using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public int foco;
    public int resiliencia;
    public int analise;
    public int expressao;
    public int iniciativa;
    public int intuicao;
    public float energia;
    public float maxEnergia = 100f;
    public float fadiga;

    public void InitializeStats(int stat1, int stat2, int stat3, float startEnergia)
    {
        if (characterName == "Roxa")
        {
            foco = stat1;
            resiliencia = stat2;
            analise = stat3;
        }
        else
        {
            expressao = stat1;
            iniciativa = stat2;
            intuicao = stat3;
        }
        energia = startEnergia;
        fadiga = 0;
    }
       public void ChangeEnergy(float amount)
    {
        energia += amount;
        energia = Mathf.Clamp(energia, 0f, maxEnergia);
    }
}

