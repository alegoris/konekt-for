using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combination
{
    public Vector2[] coord = new Vector2[4];
    public int redVal = 0;
    public int yellowVal = 0;

    public void Init()
    {
        for (int i = 0; i < 4; i++)
        {
            coord[i] = Vector2.zero;
        }
    }

    public bool ContainsCoord(int x, int y)
    {
        if(redVal == -1 || yellowVal == -1)
        {
            return false;
        }

        Vector2 testCoord = new Vector2(x, y);

        for(int i = 0; i < coord.Length; i++)
        {
            if (coord[i] == testCoord)
            {
                return true;
            }
        }

        return false;
    }

    public int InsertToken(TileSprite tileType)
    {
        if (tileType == TileSprite.SPRITE_YELLOW && redVal > 0 || 
            tileType == TileSprite.SPRITE_RED && yellowVal > 0)
        {
            redVal = yellowVal = -1;
            return -1;
        }
        else if(tileType == TileSprite.SPRITE_YELLOW )
        {
            yellowVal++;
            return yellowVal;
        }
        else
        {
            redVal++;
            return redVal;
        }
    }


}


public class GameBoard : Singleton<GameBoard>
{
    public GameObject column;
    public Vector3 boardPos;
	private bool moved = false;
	float lastHorizontal = 0;
	int selectedColumn = 0;

	private GameObject[] columns = new GameObject[7];

    private Combination[] combinations = new Combination[69]; //ha-ha.


	void Update(){
		float horizontal = Input.GetAxis ("Horizontal");
		if (horizontal < 0 && lastHorizontal >= 0) {
			MoveLeft ();
		} 
		else if (horizontal > 0 && lastHorizontal <= 0) {
			MoveRight ();
		}
		lastHorizontal = horizontal;

		if (Input.GetKeyDown (KeyCode.Space)) {
			columns [selectedColumn].GetComponent<InputColumn> ().PlaceTile();
		}
/*
		if (Input.GetKey (KeyCode.LeftArrow)) {
			//MovePlayer (Vector3.left);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			//MovePlayer (Vector3.right);
		}
		 */
	}

	private void MoveLeft () 
	{
		columns [selectedColumn].GetComponent<InputColumn> ().IsSelected (false);
		selectedColumn--;
		if (selectedColumn < 0)
			selectedColumn = columns.Length - 1;
		columns [selectedColumn].GetComponent<InputColumn> ().IsSelected (true);
		
	}

	private void MoveRight () 
	{
		columns [selectedColumn].GetComponent<InputColumn> ().IsSelected (false);
		selectedColumn++;
		if (selectedColumn > columns.Length-1)
			selectedColumn = 0;
		columns [selectedColumn].GetComponent<InputColumn> ().IsSelected (true);


	}

	// Use this for initialization
	public void Init ()
    {
		Camera c = Camera.main;
		Debug.Log (c.name);
		c.orthographicSize = GameManager.Instance.rowCount/2f;
		Debug.Log ("Camera size: " + c.orthographicSize);

        for (int i = 0; i < columns.Length; i++)
        {
            Vector3 pos = new Vector3(boardPos.x + 1 * i, boardPos.y, boardPos.z);
            GameObject columnNew = Instantiate(column, pos, Quaternion.identity) as GameObject;
            columnNew.transform.parent = transform;
            columnNew.GetComponent<InputColumn>().columnNumber = i;
            columns[i] = columnNew;
            columns[i].GetComponent<InputColumn>().Init();
        }

        IndexCombinations();

	}
	
    public bool CheckLinks(int columnIdx, int tileIdx, TileSprite tileType)
    {
        for (int i = 0; i < combinations.Length; i++)
        {
            if(!combinations[i].ContainsCoord(columnIdx,tileIdx))
            {
                continue;
            }
            int val = combinations[i].InsertToken(tileType);

            if(val == 4)
            {
                GameManager.Instance.EndGame();
            }
        }

        return false;
    }

    void CheckAvailableMoves()
    {
        List<Vector2> possibleCombinations = new List<Vector2>();

        for(int i = 0; i < 7; i++)
        {
            int tile = columns[i].GetComponent<InputColumn>().CheckAvailableTiles();
            if ( tile == -1)
            {
                continue;
            }
            possibleCombinations.Add(new Vector2(i, tile));
        }


        for (int i = 0; i < possibleCombinations.Count; i++)
        {
            int column = (int)possibleCombinations[i].x;
            int tile = (int)possibleCombinations[i].y;

            for (int n = 0; n< combinations.Length; n++)
            {
                if (!combinations[n].ContainsCoord(column, tile))
                {
                    continue;
                }
            }
            // for each tile, count all combinations
            // if there is a possible combination by the player, add a tile to that position (weighted), 
            // then choose a random tile from the max weight category
            // otherwise, choose a random highest weighted tile and add to it
        }
    }

	// Index all available combinations
	void IndexCombinations ()
    {
        for(int i = 0; i < combinations.Length; i++)
        {
            combinations[i] = new Combination();
            combinations[i].Init();
        }


        int x = 0;
        int y = 0;
        
        //horizontal combinations
        for (int i = 0; i < 24; i++)
        {
            for (int n = 0; n < 4; n++)
            {
                combinations[i].coord[n] = new Vector2(x + n, y);
            }
            x++;
            if(x >3)
            {
                x = 0;
                y++;
            }
        }

        x = y = 0;
        //vertical combinations
        for (int i = 24; i < 45; i++)
        {
            for (int n = 0; n < 4; n++)
            {
                combinations[i].coord[n] = new Vector2(x, y + n);
            }
            y++;
            if (y > 2)
            {
                y = 0;
                x++;
            }
        }

        x = 3;
        y = 0;
        int startX = 3;
        bool nextLine = false;
        //downward diagonal
        for (int i = 45; i < 57; i++)
        {
            if(x-3 < 0 || y+ 3 >5)
            {
                nextLine = true;
            }
            else
            {
                for (int n = 0; n < 4; n++)
                {
                    combinations[i].coord[n] = new Vector2(x - n, y + n);
                }
                x--;
                y++;
            }

            if (nextLine)
            {
                startX++;
                if (startX > 6)
                { break; }
                x = startX;
                y = 0;
                nextLine = false;
            }
        }

        startX = 3;
        x = 3;
        y = 0;
       
        //upward diagonal
        for (int i = 57; i < 69; i++)
        {
            if( x+3 > 6 || y+3 > 5)
            {
                nextLine = true;
            }
            else
            {
                for (int n = 0; n < 4; n++)
                {
                    combinations[i].coord[n] = new Vector2(x + n, y + n);
                }
                x++;
                y++;
            }

            if(nextLine)
            {
                startX--;
                if (startX < 0)
                { break; }
                x = startX;
                y = 0;
                nextLine = false;
            }
        }

    //    AssignCombinationsToTiles();
    }

    void AssignCombinationsToTiles()
    {
        for (int i =0; i < combinations.Length; i++)
        {
            for (int x=0; x<6;x++)
            {
                for (int y=0; y < 7; y++)
                {
                    if (!combinations[i].ContainsCoord(x,y))
                    {
                        continue;
                    }

                    InputColumn column = columns[y].GetComponent<InputColumn>();
                    TileScript tile = column.tiles[x];
                    Debug.Log("x = " + x + " y = " + y);
                    tile.AddCombination(combinations[i]);

                }
            }
        }
    }
}
