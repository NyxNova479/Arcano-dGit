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

    private bool isTranslucidRuntime;


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

    private bool isTranslucid;

    private void Start()
    {
      
    }

    public void Init(BricksManager manager, GameObject prefab)
    {
        this.manager = manager;
        this.prefab = prefab;

        currentHP = BricksType.hitToBreak;

        // IMPORTANT : on copie la valeur du type
        isTranslucidRuntime = BricksType.isTranslucid;

        if (isTranslucidRuntime)
            InitTranslucid();
        else
            InitNormal();
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
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, 0.1f) ;
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
    public IEnumerator RevealBrick()
    {
        yield return new WaitForSeconds(1f);

        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            1f
        );

        isTranslucidRuntime = false;
        state = BrickState.Active;
    }
}