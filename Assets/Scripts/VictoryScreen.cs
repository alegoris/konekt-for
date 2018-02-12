using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour {

	public Text victoryText;
	public Image victoryPanel;
	public float panelAlpha = 0.5f;
	public Button newGameBtn;

	// Use this for initialization
	void Start () {
		victoryText.enabled = false;
		victoryPanel.enabled = false;
		newGameBtn.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	public void DisplayVictory (TileSprite winner)
	{

		victoryText.enabled = true;
		victoryPanel.enabled = true;
		newGameBtn.gameObject.SetActive (true);


		string winnerStr;
		Color victoryColor;

		if (winner == TileSprite.SPRITE_YELLOW) {

			winnerStr = "yellow";
			victoryColor = Color.yellow;

		} else {
			
			winnerStr = "red";
			victoryColor = Color.red;

		}

		victoryColor.a = panelAlpha;
		victoryPanel.color = victoryColor;

		victoryText.text = "Winner is " + winnerStr; 
	}
}
