using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Singleton;

	// If the player has already loaded this level more than once whithout leaving it
	public static bool isStuckOnThisLevel;

	public void Awake()
	{
		if (!Singleton)
		{
			Singleton = this;
		}
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.Equals("LevelSelect"))
		{
			isStuckOnThisLevel = false;
		}
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneUnloaded(Scene scene)
	{
		if (scene.name.Contains("Level") && !scene.name.Equals("LevelSelect"))
		{
			isStuckOnThisLevel = true;		
		}
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadGameLevel(int level)
	{
		Level.currentLevelId = level - 1;
		SceneManager.LoadScene("Level" + level);
	}
}
