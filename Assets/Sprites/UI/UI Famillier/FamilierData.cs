using UnityEngine;

[CreateAssetMenu(fileName = "NewFamillier", menuName = "Inventory/Famillier")]
public class FamillierData : ScriptableObject
{
    public string famillierName;
    public string famillierMetier;
    public Sprite PenseA;
    public Sprite icon;
    public int humeur;
    public Sprite[] humeurSprite;
    [TextArea] public string description;

    public Color colorPref;
     public RenderTexture modelRenderTexture;  
}