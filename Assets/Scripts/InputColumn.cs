using UnityEngine;
using System.Collections;

public class InputColumn : MonoBehaviour {

    public TileScript tile;

    [HideInInspector]
    public int columnNumber;

    [HideInInspector]
    public TileScript[] tiles = new TileScript[6];

	void Start ()
    {
        
	}

    public void Init()
    {
        int columnSize = 6;

        float posX = transform.position.x - tile.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center.x;
        float posY = transform.position.y + tile.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center.y;
        float posZ = transform.position.z;

        for (int i = 0; i < columnSize; i++)
        {
            Vector3 pos = new Vector3(posX, posY, posZ);
            tiles[i] = Instantiate(tile, pos, Quaternion.identity) as TileScript;
            tiles[i].transform.parent = transform;
            posY += tile.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.y * 2;
        }
    }

    void OnMouseDown()
    {
		if (GameManager.Instance.gameCompleted){
			return;
		}

        Debug.Log("column pressed " + columnNumber);

        int emptyTile = CheckAvailableTiles();
        Debug.Log(emptyTile);

        if (emptyTile == -1)
        {
            return;
        }

        tiles[emptyTile].ChangeSprite(GameManager.Instance.currentPlayer);
        //check for links
        bool connected = GameBoard.Instance.CheckLinks(columnNumber, emptyTile,GameManager.Instance.currentPlayer);
        if(connected)
        {
            GameManager.Instance.EndGame();
        }
        else
        {
            GameManager.Instance.ChangePlayer();
        }
        

    }

    public int CheckAvailableTiles()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].currentSprite == 0)
            {
                return i;
            }
        }
        return -1;
    }
}
