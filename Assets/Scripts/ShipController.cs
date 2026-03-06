using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float force = 5f; // Fuerza del movimiento
    [SerializeField] private Vector3 endPosition; // Posición final de la nave al inicio
    [SerializeField] private float duration; // Duración de la transición al inicio
    [SerializeField] int blinkNum;


    // Referencia al prefab del disparo
    [SerializeField] GameObject shootPrefab;

    // Distancia desde el centro de la nave hasta la posición donde se creará el disparo
    [SerializeField] float shootOffset = 0.5f;
    private bool active = false; // Variable para determinar si se puede realizar alguna acción

    private Rigidbody2D rb; // Referencia al componente Rigidbody

    [SerializeField] GameObject explosion;
    Vector3 initialPosition; //Posición inicial de la nave

    [SerializeField] private TrailRenderer trail;

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>(); // Inicializamos la referencia al Rigidbody
        StartCoroutine("StartPlayer");
    }

    void Update()
    {
        // Comprobar si la nave está activa y se ha pulsado la tecla de disparo (barra espaciadora)
        if (active && Input.GetKeyDown(KeyCode.Space))
        {
            // Calcular la posición donde se creará el disparo (un poco por delante de la nave)
            Vector3 shootPosition = transform.position + Vector3.up * shootOffset;

            // Crear el disparo en la posición calculada y sin rotación
            Instantiate(shootPrefab, shootPosition, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;
        if(tag == "Enemy" || tag == "BigAsteroid"){
            DestroyShip();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("BigAsteroid"))
        {
            DestroyShip();
        }
    }

    private void FixedUpdate()
    {
        if(active){
            CheckMove(); // Llamamos al método para comprobar el movimiento
        }
    }
    private void CheckMove()
    {
        // Obtenemos la dirección del movimiento en los ejes horizontal y vertical
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction.Normalize(); // Normalizamos el vector para que tenga magnitud 1

        // Aplicamos una fuerza en la dirección obtenida
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    void DestroyShip(){
        // Desactivar comportamiento
        active = false;
        // Instanciar la animación de la explosión
        Instantiate(explosion, transform.position, Quaternion.identity);
        //Desactivamos el trail para que no se muestre en el reinicio de la nave
        trail.enabled = false;
        // Resetear posición de la nave
        transform.position = initialPosition;
        //Reiniciamos el trail
        trail.Clear();
        //Reactivamos el trail
        trail.enabled = true;
        // Reiniciar la naveGameManager.GetInstance().LoseLife();

        //Restamos vidas cuando se destruye la nave
        GameManager.GetInstance().LoseLife();

        if (GameManager.GetInstance().HasLives())
        {
            transform.position = initialPosition;
            StartCoroutine("StartPlayer");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator StartPlayer()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        Color color = mat.color;

        // Desactivamos las colisiones para la nave
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Posición inicial
        Vector3 initialPosition = transform.position;
        float t = 0, t2 = 0; // Tiempo que va transcurriendo en cada uno de los distintos intervalos

        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(initialPosition, endPosition, t / duration);
            transform.position = newPosition;

            t2 += Time.deltaTime;
            float newAlpha = blinkNum * (t2 / duration);
            if (newAlpha > 1)
            {
                t2 = 0;
            }
            color.a = newAlpha;
            mat.color = color;
            yield return null;
        }

        color.a = 1;
        mat.color = color;
        // Volvemos a activar las colisiones
        collider.enabled = true;
        active = true;
    }
}
