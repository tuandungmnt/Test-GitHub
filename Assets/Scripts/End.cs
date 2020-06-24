using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour {
    public void EndGame() {
        SceneManager.LoadScene(2);
    }

}
