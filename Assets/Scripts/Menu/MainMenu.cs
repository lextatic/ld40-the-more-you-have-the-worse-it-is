using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject loadingPanel;

	public void LoadGame()
	{
		Beam.monsterKills = 0;
		SceneManager.LoadScene(1, LoadSceneMode.Single);
		loadingPanel.SetActive(true);
	}
}
