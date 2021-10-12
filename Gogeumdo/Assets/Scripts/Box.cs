using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float spawnTime = 0.01f;
    private WaitForSeconds spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        spawnDelay = new WaitForSeconds(spawnTime);
        StartCoroutine(BoxMove());
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(this.gameObject.transform.position.x, -2.5f, 2.5f), Mathf.Clamp(this.gameObject.transform.position.y, -4.75f, 4.75f));
        
    }

    public IEnumerator BoxMove() 
    {
        while (true)
        {

            this.gameObject.transform.position += new Vector3(0,0.01f,0);
            yield return spawnDelay;
        }

    }
}
