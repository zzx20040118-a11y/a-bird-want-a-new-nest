

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneExit : MonoBehaviour
{
    [Header("Scene Transition")]
    public string nextSceneName = "ConstructionScene2";

    private bool isLoading = false;

    public void GoToNextScene()
    {
        if (isLoading)
            return;

        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError("SceneExit: No destination scene set!", this);
            return;
        }

        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        isLoading = true;

        string currentScene = SceneManager.GetActiveScene().name;

        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError("SceneExit: Cannot load '" + nextSceneName +
                           "'. Check that it is added to Build Settings.", this);
            isLoading = false;
            yield break;
        }

        if (!SceneManager.GetSceneByName(nextSceneName).isLoaded)
        {
            yield return SceneManager.LoadSceneAsync(
                nextSceneName, LoadSceneMode.Additive);
        }

        Scene destination = SceneManager.GetSceneByName(nextSceneName);

        if (destination.IsValid() && destination.isLoaded)
            SceneManager.SetActiveScene(destination);

        Debug.Log("Loaded " + nextSceneName +
                  " additively. Previous scene: " + currentScene);

        isLoading = false;
    }
}
