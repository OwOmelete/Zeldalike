using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerTD : MonoBehaviour
{
    public int energie;
    public int life;
    public TextMeshProUGUI energieText;
    public TextMeshProUGUI lifeText;
    public void UpdateEnergie(bool signe,int add)
    {
        if(signe) energie+=add;
        else energie-=add;
        energieText.text = energie.ToString();
    }
    public void UpdateLife(int add)
    {
        life-=add;
        lifeText.text = life.ToString();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        energieText.text = energie.ToString();
        lifeText.text = life.ToString();
    }
}
