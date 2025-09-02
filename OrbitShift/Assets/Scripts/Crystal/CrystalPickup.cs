using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    [SerializeField] private AudioClip[] clings;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            //Debug.Log("+1");
            SoundEfectsManager.Instance.PlayRandomSoundFX(clings, transform, 1f);

            GameManager.Instance.crystalsPoint++;

            Destroy(gameObject);
        }
    }

}
