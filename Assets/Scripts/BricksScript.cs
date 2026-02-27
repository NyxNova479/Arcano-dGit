using System.Collections;
using UnityEngine;

public class BricksScript : MonoBehaviour
{
    public BricksData.BrickType BrickType;
    public int ScoreData;

    public SpriteRenderer spriteRenderer;

    private int currentHP;

    private BricksManager manager;
    private GameObject prefab;

    public enum BrickState
    {
        Hidden,
        Active
    }

    public BrickState state;

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

    private void Start()
    {
        currentHP = BrickType.hitToBreak;

        if (BrickType.isTranslucid)
            InitTranslucid();
        else
            InitNormal();
    }

    public void Init(BricksManager manager, GameObject prefab)
    {
        this.manager = manager;
        this.prefab = prefab;
    }

    // ===== INIT =====

    private void InitNormal()
    {
        state = BrickState.Active;
        spriteRenderer.enabled = true;

    }

    private void InitTranslucid()
    {
        state = BrickState.Hidden;
        spriteRenderer.enabled = true;
        Color trans = spriteRenderer.color;
        trans.a = 0f;
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0.1f) ;
    }





    // ===== HIT =====
    public void OnHit()
    {

        currentHP --;

        if (currentHP <= 0) return;

        else if (currentHP <= 1)
        {
            manager.ReturnEnemy(gameObject, prefab);
        }
        else
        {
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            float ratio = (float)currentHP / BrickType.hitToBreak;
            spriteRenderer.color = Color.Lerp(Color.black, Color.white, ratio);
        }
    }
    public IEnumerator RevealBrick()
    {
        yield return new WaitForSeconds(1f);
        BrickType.isTranslucid = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }
}