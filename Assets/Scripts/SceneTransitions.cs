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

        public void LoadNextScene()
        {
            Debug.Log("I GOT HERE");
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
    
