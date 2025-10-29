using JetBrains.Annotations;
using UnityEngine;

public class Hook : MonoBehaviour
{

    GameObject fishCaught;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Fish Caught!");
            Destroy(collision.gameObject);
            fishCaught.GetComponent<ScoreSystem>().AddTenPoints();


        }

        void EndState()
        {
            if (score = )
            {
                
            }
        }
    }
}
