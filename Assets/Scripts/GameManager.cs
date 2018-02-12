using UnityEngine;
using System.Collections;

public enum TileSprite
{
    SPRITE_EMPTY = 0,
    SPRITE_YELLOW,
    SPRITE_RED
};

public class GameManager : Singleton<GameManager>
{
	public VictoryScreen victoryScreen;

	[HideInInspector]
    public TileSprite currentPlayer;

	[HideInInspector]
	public bool gameCompleted = false;

    private int redTilesNo = 20;
    private int yellowTilesNo = 20;

	void Start ()
    {
        currentPlayer = TileSprite.SPRITE_YELLOW; // Yellow first
	}
	
    public void ChangePlayer()
    {
        if ( currentPlayer == TileSprite.SPRITE_YELLOW && yellowTilesNo <=0 ||
             currentPlayer == TileSprite.SPRITE_RED && redTilesNo <= 0)
        {
            EndGame(true);
            return;
        }

        if (currentPlayer == TileSprite.SPRITE_YELLOW)
        {
            yellowTilesNo--;
            currentPlayer = TileSprite.SPRITE_RED;
            return;
        }

        redTilesNo--;
        currentPlayer = TileSprite.SPRITE_YELLOW;
    }

    public void EndGame(bool tie = false)
    {
		gameCompleted = true;

        if(tie)
        {
            Debug.Log("Tie! Reloading...");
        }
        else
        {
            Debug.Log("Game over, winner is " + currentPlayer);
        }
        
		victoryScreen.DisplayVictory (currentPlayer);

        //Invoke("ReloadLevel", 5.0f);
    }

   public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
