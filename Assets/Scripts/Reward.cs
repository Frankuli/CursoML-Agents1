using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MLAgent"))
        {
            Invoke("MoveInitialPosition", 4);
        }
    }
    void MoveInitialPosition()
    {
        bool posicionEncontrada = false;
        int intentos = 100;
        Vector3 posicionPotencial = Vector3.zero;

        while (!posicionEncontrada || intentos >= 0)
        {
            intentos--;
            posicionPotencial = new Vector3(transform.parent.position.x + Random.Range(-4f,4f),
                                            0.555f,
                                            transform.parent.position.z + Random.Range(-4f, 4f));
            
            Collider[] colliders = Physics.OverlapSphere(posicionPotencial, 0.05f);
            
            if (colliders.Length == 0)
            {
                transform.position = posicionPotencial;
                posicionEncontrada = true;
            }
        }
        
    }
}
