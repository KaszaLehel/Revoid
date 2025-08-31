using UnityEngine;

public class Spike : MonoBehaviour
{
    private Animator animator;
    private bool isDespawning = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAppear()
    {
        animator.SetTrigger("Start");
    }

    public void Despawn()
    {
        if (isDespawning) return;

        isDespawning = true;

        animator.SetTrigger("End");

        Destroy(gameObject);
    }
}
