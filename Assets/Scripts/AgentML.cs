using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//importante
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AgentML : Agent //cambia
{
    [SerializeField] private float _fuerzaMovimiento = 200;
    [SerializeField] private Transform _target;
    private Rigidbody _rb; 

    public bool _training = true;

    public override void Initialize()//parecido al star de monoB
    {
        _rb = GetComponent<Rigidbody>();
        if (!_training) MaxStep = 0;
    }

    public override void OnEpisodeBegin()//despues der unos intento se llama
    {
        //para la esfera
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        MoveInitialPosition();
    }

    /// <summary>
    /// sirve para contruir un vector de desplazamiento [0]: X, [1]: Y
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)//valores pasados por el machine
    {
        Vector3 movimiento = new Vector3(vectorAction[0], 0f, vectorAction[1]);
        _rb.AddForce(movimiento * _fuerzaMovimiento * Time.deltaTime);
    }


    public override void CollectObservations(VectorSensor sensor)//observaciones inportantes para toma de deciciones
    {
        //calcular distancia
        Vector3 alObjetivo = _target.position - transform.position;

        //un vector ocupa 3 observaciones.
        sensor.AddObservation(alObjetivo.normalized);
    }

    public override void Heuristic(float[] actionsOut)//nosotros movemos la bola
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        //asi el ML llama a OnActionRecived
        actionsOut[0] = movement.x;
        actionsOut[1] = movement.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            AddReward(1f);//premio
        }
    }    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            AddReward(0.5f);//premio
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            if (_training)
            {
                AddReward(-0.1f);//castigo
            }
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
            posicionPotencial = new Vector3(transform.parent.position.x + Random.Range(-4f, 4f),
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
