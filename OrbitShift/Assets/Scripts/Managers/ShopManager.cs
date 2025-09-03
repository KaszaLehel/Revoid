using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Skins[] shopItems;
    public GameObject shopButtonPrefab;
    public Transform contentParent;

    [HideInInspector]
    public List<ShopButton> allButtons = new List<ShopButton>();

    private void Start()
    {
        foreach (var skin in shopItems)
        {
            GameObject go = Instantiate(shopButtonPrefab, contentParent);
            ShopButton button = go.GetComponent<ShopButton>();
            button.Setup(skin, this);
            allButtons.Add(button);
        }
    }
}
