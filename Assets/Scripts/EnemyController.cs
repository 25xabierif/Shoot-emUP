using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Velocidad de caída de la nave enemiga
    [SerializeField] float speed; 

    // Altura a la que se destruirá la nave enemiga
    const float DESTROY_HEIGHT = -6f;

    void Update()
    {
        // Movimiento hacia abajo
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destruir la nave enemiga cuando alcanza la altura de destrucción
        if(transform.position.y < DESTROY_HEIGHT)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
        Debug.Log("Colisión con nave enemiga"); 
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Colisión con nave enemiga"); 
    }
}
