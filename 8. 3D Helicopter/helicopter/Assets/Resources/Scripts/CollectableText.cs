using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class CollectableText : MonoBehaviour {

	public GameObject helicopter;
	private Text text;
	private int collectableCount;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if (helicopter != null) {
			collectableCount = helicopter.GetComponent<HeliController>().collectableTotal;
		}
		text.text = "Coins: " + collectableCount;
	}
}
