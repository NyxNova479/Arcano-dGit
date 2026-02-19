using UnityEngine;

public class BricksScript : MonoBehaviour
{

    public BricksData.BrickType BrickType;
    public int ScoreData;


    private SpriteRenderer spriteRenderer;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject outline = new GameObject("Outline");
        outline.transform.SetParent(transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localScale = Vector3.one * 1.1f;

        SpriteRenderer osr = outline.AddComponent<SpriteRenderer>();
        osr.sprite = spriteRenderer.sprite;
        osr.color = Color.black;
        osr.sortingLayerID = spriteRenderer.sortingLayerID;
        osr.sortingOrder = spriteRenderer.sortingOrder - 1;
    }







}
