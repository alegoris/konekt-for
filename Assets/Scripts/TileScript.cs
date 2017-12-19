using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour
{
    public Sprite[] sprites;

    [HideInInspector]
    public int currentSprite;

    private List<Combination> myCombinations;

	void Start ()
    {
        if (sprites.Length <= 0 || sprites.Length > 3)
        {
            //error
        }

        ChangeSprite(TileSprite.SPRITE_EMPTY);
	}

    public void AddCombination(Combination comb)
    {
        myCombinations.Add(comb);
    }

    public void ChangeSprite(TileSprite tileSprite)
    {
        int spriteIdx = (int)tileSprite;
        if (spriteIdx < 0 || spriteIdx >= sprites.Length)
        {
            return;
        }

        currentSprite = spriteIdx;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[spriteIdx];
    }
}
