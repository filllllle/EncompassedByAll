using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField]
    private string taskName;

    [SerializeField]
    private Sprite interactSprite;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;

    public string TaskName { get => taskName; }

    bool resolved;

    public bool IsResolved { get => resolved; }

    protected abstract void OnStart();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        OnStart();
    }

    public void HighlightTask()
    {
        if (interactSprite != null)
            spriteRenderer.sprite = interactSprite;
    }

    public void UnhighlightTask()
    {
        spriteRenderer.sprite = originalSprite;
    }

    protected void SetAsResolved()
    {
        resolved = true;
    }

    public abstract void OnInteract();

    public void Interact()
    {
        OnInteract();
    }
}
