using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Skins skin;
    private ShopManager shopManager;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI skinText;
    [SerializeField] private Image background;

    private Button button;
    private bool isUnlocked;
    private bool isEquipped;

    public void Setup(Skins _skin, ShopManager manager)
    {
        skin = _skin;
        shopManager = manager;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        LoadState();
        UpdateUI();
    }

    public void LoadState()
    {
        isUnlocked = PlayerPrefs.GetInt(skin.skinID, skin.unlockedByDefault ? 1 : 0) == 1;
        string activeSkin = PlayerPrefs.GetString("ActiveSkin", "");
        isEquipped = isUnlocked && activeSkin == skin.skinID;
    }

    private void SaveState()
    {
        PlayerPrefs.SetInt(skin.skinID, isUnlocked ? 1 : 0);
    }

    public void UpdateUI()
    {
        if (!isUnlocked)
        {
            skinText.gameObject.SetActive(false);
            priceText.text = skin.price.ToString();
            priceText.gameObject.SetActive(true);
            if (background != null)
                background.color = new Color(0.1f, 0.1f, 0.3f, 1f); // sötétkék lockolt
        }
        else
        {
            skinText.text = skin.effectName.ToString();
            skinText.gameObject.SetActive(true);
            priceText.gameObject.SetActive(false);
            if (isEquipped)
            {
                if (background != null)
                {
                    Color g = skin.unlockedColor;
                    background.color = new Color(g.r, g.g, g.b, 0.4f); // alpha 60%
                }
                    
            }
            else
            {
                if (background != null)
                {
                    background.color = skin.unlockedColor;
                }
            }
        }
    }

    private void OnClick()
    {
        if (!isUnlocked)
            TryBuy();
        else
            Equip();
    }

    private void TryBuy()
    {
        if (GameManager.Instance.allCrystalsPoint >= skin.price)
        {
            GameManager.Instance.MinusCrystal(skin.price);
            UIManager.Instance.RefreshCrystals();

            isUnlocked = true;
            SaveState();
            Equip();
        }
        else
        {
            Debug.Log("No enaught crystal");
        }
    }

    private void Equip()
    {
        PlayerPrefs.SetString("ActiveSkin", skin.skinID);

        foreach (var b in shopManager.allButtons)
        {
            b.LoadState();
            b.UpdateUI();
        }

        PlayerSkinManager.Instance.ApplySkin(skin);
    }
}
