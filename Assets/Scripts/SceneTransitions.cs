using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace sceneTransitions
{
    public class SceneTransitions : MonoBehaviour
    {

        public Animator transitionAnim;
        public string sceneName;
        public GameObject winScene;
        Scene scene;
        
        private void Start()
        {
            scene = SceneManager.GetActiveScene();
        }

        public void LoadNextScene()
        {
            

            if (scene.name == "LevelTwo")
            {
                Debug.Log("I GOT HERE");
                winScene.SetActive(true);
            }

            StartCoroutine(WaitAnim());
        }

        IEnumerator WaitAnim()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(sceneName);
        }

    }
}
    
