using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaCoin : MonoBehaviour
{

    [SerializeField]
    public bool hasCoin;

    [SerializeField]
    public GameObject coin;

    private void Start()
    {
        
        if(coin != null)
        {

            hasCoin = true;

        }
        else
        {

            hasCoin = false;

        }

    }


    public GameObject GetCoin()
    {

        if(hasCoin)
        {

            hasCoin = false;
            GetComponent<FurnitureSaver>().SaveStats(hasCoin);
            SpawnCoin();
            return coin;

        }
        else
        {

            return null;

        }

    }

    public bool HasCoin()
    {

        return hasCoin;

    }

    GameObject appearingCoin;

    void SpawnCoin()
    {

        appearingCoin = Instantiate(coin);
        appearingCoin.transform.SetParent(gameObject.transform, false);
        appearingCoin.transform.localPosition = Vector3.zero;

        appearingCoin.GetComponent<SpriteRenderer>().enabled = transform;

        Rigidbody2D coinBody = appearingCoin.AddComponent<Rigidbody2D>();
        coinBody.velocity = Vector3.up * 300;
        coinBody.gravityScale = 100;

        Invoke("DespawnCoin", 0.5f);

    }

    void DespawnCoin()
    {

        int i = 0;

        foreach(GameObject instance in GameObject.Find("Timmy").GetComponent<HoldablesScript>().heldSubItems)
        {

            if(instance == null)
            {

                GameObject.Find("Timmy").GetComponent<HoldablesScript>().heldSubItems[i] = coin;

                break;

            }

            i++;

        }

        Destroy(appearingCoin.GetComponent<Rigidbody2D>());

        appearingCoin.GetComponent<SpriteRenderer>().enabled = false;
        appearingCoin.GetComponent<Collider2D>().enabled = false;

    }

}
