using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using TMPro;

public class LoadSDKText : MonoBehaviour
{
	//Key in which the text is placed on the JSON file
	public string key;
	public bool playTTSOnEnable = false;
	[Header("Level Options"), Tooltip("Se esse texto tiver separado por algum underscore '_' e tiver um level na frente, marque essa caixa pra ele trocar sozinho o que tiver na frente do simbolo pelo level atual, automaticamente.")]
	public bool replaceUnderscoreWithCurrentLevel;

	[HideInInspector]
	public Text myText;
	[HideInInspector]
	public TextMeshProUGUI myTextM;
	[HideInInspector]
	public TextMeshPro myTextMPro;

	//If the key is right, updates it on enable
	void OnEnable()
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (replaceUnderscoreWithCurrentLevel)
			{
				var index = key.IndexOf('_');
				var text = key.Substring(0, index);
				key = text + "_" + (Level.currentLevelId + 1);
			}

			UpdateText();
			if (playTTSOnEnable)
				PlaySpeechText();
		}
		else
		{
			Debug.LogWarning("Key variable is empty at:" + this.gameObject.name, this.gameObject);
		}
	}

	//Set key from outside (through UI events)
	public void SetKey(string key)
	{
		this.key = key;
		UpdateText();
	}

	//Updates the UI text that is shown
	public void UpdateText()
	{
		//Adds references
		if (!myTextM)
		{
			myTextM = GetComponent<TextMeshProUGUI>();
		}
		if (!myText)
		{
			myText = GetComponent<Text>();
		}
		if (!myTextMPro)
		{
			myTextMPro = GetComponent<TextMeshPro>();
		}

		//Adds an asterisk if the text coult not be loaded from the language file
		if (myTextM)
		{
			myTextM.text = SharedState.languageDefs != null ? SharedState.languageDefs[key].Value : myTextM.text + "*";
		}
		if (myText)
		{
			myText.text = SharedState.languageDefs != null ? SharedState.languageDefs[key].Value : myText.text + "*";
		}
		if (myTextMPro)
		{
			myTextMPro.text = SharedState.languageDefs != null ? SharedState.languageDefs[key].Value : myTextMPro.text + "*";
		}
	}

	//Plays the TTS tool
	public void PlaySpeechText()
	{
		try
		{
			AudioController.Singleton.PlayClick();
			LOLSDK.Instance?.SpeakText(key);
		}
		catch { }
	}
}