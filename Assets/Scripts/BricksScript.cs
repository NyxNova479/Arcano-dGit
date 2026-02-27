using System.Collections;
using UnityEngine;

public class BricksScript : MonoBehaviour
{
    public BricksData.BrickType BricksType;
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
        if(state == BrickState.Hidden) BricksType.isTranslucid = true;   
    }

    private void Start()
    {
        currentHP = BricksType.hitToBreak;

        if (BricksType.isTranslucid)
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
        spriteRenderer.color = new Color(0,spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b) ;
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
            float ratio = (float)currentHP / BricksType.hitToBreak;
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, ratio);
        }
    }
    public IEnumerator RevealBrick(BricksScript brick)
    {
        yield return new WaitForSeconds(1f);
        brick.spriteRenderer.color = new Color(brick.spriteRenderer.color.r, brick.spriteRenderer.color.g, brick.spriteRenderer.color.b, 1);
        brick.BricksType.isTranslucid = false;
    }
}