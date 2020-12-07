using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnTrans;
    public Button createBtn;
    public GameObject pot;//锅的Collider
    
    // Start is called before the first frame update
    void Start()
    {
        createBtn.onClick.AddListener(OnCreateClick);
    }

    private void OnCreateClick()
    {
        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(ballPrefab,spawnTrans);
            go.transform.position += Random.Range(-1f, 1f) * Vector3.left;
            go.GetComponent<SpriteRenderer>().color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }
        InvokeRepeating("Test",0,1);
    }
    
    private void Test()
    {
        Debug.Log($"=======test");
    }
    
}