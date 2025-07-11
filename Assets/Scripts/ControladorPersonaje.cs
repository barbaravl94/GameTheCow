using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

public class ControladorPersonaje : MonoBehaviour
{
    // Controlador del personaje principal del juego
    public Rigidbody2D rigidBody;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    bool isJumping = false;
    [Range(1, 500)] public float potenciaSalto;
    public LayerMask sueloLayer;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Loop principal del juego
    private void Update()
    {
        //Movimiento horizontal
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        //Velocidad horizontal
        var horizontalSpeed = horizontalInput * 5f;
        //Mover el personaje según coordenadas del jugador
        rigidBody.linearVelocity = new Vector2(horizontalSpeed, rigidBody.linearVelocity.y);
        //Sentido del sprite
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false; // Mirar a la derecha
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true; // Mirar a la izquierda
        }
        //Salto
        if (Input.GetButtonDown("Jump") && estaEnSuelo())
        {
            //Reiniciar velocidad vertical
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0f);
            rigidBody.AddForce(Vector2.up * potenciaSalto);
        }
    }

    //Verificar si el personaje está en el suelo
    private bool estaEnSuelo()
    {
        return boxCollider.IsTouchingLayers(sueloLayer);
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Jump") && !isJumping)
        {
            //Aplicar fuerza de salto
            rigidBody.AddForce(Vector2.up * potenciaSalto);
            //Marcar que el personaje está saltando
            isJumping = true; 
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Si colisiona con el suelo, permitir salto
        if (other.gameObject.CompareTag("Suelo"))
            // Cambiar el estado de salto a falso
            isJumping = false;
            // Asegurarse de que la velocidad vertical sea cero al aterrizar
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0f);
    }
}
