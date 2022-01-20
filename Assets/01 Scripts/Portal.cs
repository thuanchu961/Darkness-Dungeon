using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Colliable
{
    public string[] sceneNames;
    // Start is called before the first frame update
    protected override void OnCollide(Collider2D coll)
    {
        if(coll.name == "Player")
        {
            //teleport the player
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        }
    }
}
