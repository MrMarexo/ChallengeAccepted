using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScript : MonoBehaviour
{
    [SerializeField] Transform marexo;

    void Start()
    {
        var newX = marexo.position.x - Screen.width;
        marexo.position = new Vector3(newX, marexo.position.y, marexo.position.z);
        StartCoroutine(Promenade());
    }

    IEnumerator Promenade()
    {
        yield return new WaitForSecondsRealtime(1f);
        LeanTween.moveX(marexo.gameObject, marexo.position.x + Screen.width, 0.2f).setEaseInOutBounce();
        yield return new WaitForSecondsRealtime(1.2f);
        LeanTween.moveX(marexo.gameObject, marexo.position.x + Screen.width, 0.2f).setEaseInOutCubic().setOnComplete(() => {
            FindObjectOfType<GameManager>().NextScene();
        });

    }
    
}
