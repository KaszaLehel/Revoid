using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public static PlayerSkinManager Instance { get; private set; }

    [Header("Player Sprite")]
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Path Settings")]
    [SerializeField] private GameObject path; 

    private GameObject currentEffect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll(); //TOROLNI A PLAYER PREFSEKET AZ OSSZESET

        string activeSkinID = PlayerPrefs.GetString("ActiveSkin", "");
        if (!string.IsNullOrEmpty(activeSkinID))
        {
            //var skin = FindSkinByID(activeSkinID);
            Skins skin = FindSkinByID(activeSkinID);

            if (skin != null)
                ApplySkin(skin);
        }
    }

    public void ApplySkin(Skins skin)
    {
        if (skin == null) return;

        if (skin.skinType == SkinType.Color)
        {
            playerSprite.color = skin.unlockedColor;

            if (currentEffect != null)
            {
                Destroy(currentEffect);
                currentEffect = null;
            }
        }
        else if (skin.skinType == SkinType.Effect && skin.effectPrefab != null)
        {
            playerSprite.color = skin.unlockedColor;//Color.white;

            if (currentEffect != null)
                Destroy(currentEffect);

            currentEffect = Instantiate(skin.effectPrefab, playerSprite.transform);
            currentEffect.transform.localPosition = Vector3.zero;
        }
    }

    private Skins FindSkinByID(string id)
    {
        var shopManager = FindFirstObjectByType<ShopManager>();
        if (shopManager == null) return null;

        foreach (var s in shopManager.shopItems)
        {
            if (s.skinID == id)
                return s;
        }

        return null;
    }
}
