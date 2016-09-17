using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UILoading : MonoBehaviour {
    public GameObject loadingPict;
    public float speed;
    public float limit;

    SpriteRenderer pictSR;
    Camera camera;
    bool inverse = false;
	// Use this for initialization
	void Start () {
        pictSR = loadingPict.GetComponent<SpriteRenderer>();
        camera = GetComponent<Camera>();
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneBuffer.GetBuffer().sceneName);
        yield return async ;
    }

    // Update is called once per frame
    void Update () {
        setColors();
    }

    void setColors()
    {
        Color cameraBg = camera.backgroundColor;
        Color pictColor = pictSR.color;
        float currCamValue = cameraBg.r;
        float currpictValue = pictColor.r;

        if (!inverse)
        {
            if (currCamValue < limit && !inverse)
                cameraBg = new Color(currCamValue + speed, currCamValue + speed, currCamValue + speed, 1f);
            if (currpictValue > 1 - limit*3 && !inverse)
                pictColor = new Color(currpictValue - speed * 3, currpictValue - speed * 3, currpictValue - speed * 3, 1f);
            if (currCamValue > limit && currpictValue < 1 - limit * 3 && !inverse)
                inverse = true;
        }
        if (inverse)
        {
            if (currCamValue >= 0.01 && inverse)
                cameraBg = new Color(currCamValue - speed, currCamValue - speed, currCamValue - speed, 1f);
            if (currpictValue <= 1 - 0.01 && inverse)
                pictColor = new Color(currpictValue + speed * 3, currpictValue + speed * 3, currpictValue + speed * 3, 1f);
            if (currCamValue < 0.01 && currpictValue > 1-0.01 && inverse)
                inverse = false;
        }
        

        print("set background color to: " + cameraBg +" set loading color to: " + pictColor + " "+inverse);
        camera.backgroundColor = cameraBg;
        pictSR.color = pictColor;
    }
}
