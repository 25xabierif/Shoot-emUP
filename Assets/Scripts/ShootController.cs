using UnityEngine;

public class ShootController : MonoBehaviour
{
    // Velocidad de los disparos
    [SerializeField] float speed;

    // Tiempo que duran los disparos antes de autodestruirse
    [SerializeField] float lifetime;

    void Start()
    {
        // Destruir el disparo después de un cierto tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover el disparo hacia arriba
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
        Debug.Log("Colisión con disparo"); 
    }

    // Método para destruir el disparo cuando sale de la pantalla
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}