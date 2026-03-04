using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float force = 5f; // Fuerza del movimiento
    [SerializeField] private Vector3 endPosition; // Posición final de la nave al inicio
    [SerializeField] private float duration; // Duración de la transición al inicio
    [SerializeField] int blinkNum;
    private bool active = false; // Variable para determinar si se puede realizar alguna acción

    private Rigidbody2D rb; // Referencia al componente Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Inicializamos la referencia al Rigidbody
        StartCoroutine("StartPlayer");
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
