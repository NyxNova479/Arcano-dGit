using UnityEngine;

public class BricksScript : MonoBehaviour
{
    public BricksData.BrickType BrickType;
    public int ScoreData;

    private SpriteRenderer spriteRenderer;

    private int currentHP;

    private BricksManager manager;
    private GameObject prefab;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject outline = new GameObject("Outline");
        outline.transform.SetParent(transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localScale = Vector3.one * 1.2f;

        SpriteRenderer osr = outline.AddComponent<SpriteRenderer>();
        osr.sprite = spriteRenderer.sprite;
        osr.color = Color.black;
        osr.sortingLayerID = spriteRenderer.sortingLayerID;
        osr.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    private void OnEnable()
    {
        currentHP = BrickType.hitToBreak;
    }

    public void Init(BricksManager manager, GameObject prefab)
    {
        this.manager = manager;
        this.prefab = prefab;

        // On copie la valeur depuis le ScriptableObject
        currentHP = BrickType.hitToBreak;
    }

    public void OnHit()
    {
        currentHP--;

        if (currentHP <= 0)
        {
            return;
        }
        else if (currentHP <= 1)
        {
            manager.ReturnEnemy(gameObject, prefab);
        }
        //else if (currentHP <= 1 && this.GetComponent<BrickPool>().GetBrick== )
        //{
        //    manager.ReturnEnemy(gameObject, prefab);
        //}
        else
        {
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        // Optionnel : changer la couleur selon la vie restante
        if (spriteRenderer != null)
        {
            float ratio = (float)currentHP / BrickType.hitToBreak;
            spriteRenderer.color = Color.Lerp(Color.black, Color.white, ratio);
        }
    }
}
