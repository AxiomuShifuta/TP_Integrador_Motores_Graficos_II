using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    Rigidbody playerRb;
    [SerializeField] private float movementSpeed;
    private Vector3 direction;


    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false; // Por alguna razón, Unity me advierte que nunca se usa esta variable.
    [SerializeField] private float dashingSpeed;
    [SerializeField] private TrailRenderer PlayerTr;
    [SerializeField] private DashBar dashbar;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] TextMeshProUGUI gameOverText;


    // Start is called before the first frame update
    void Start()
    {
       dashbar.enabled = false;
       gameOverText.enabled = false;
       playerRb = GetComponent<Rigidbody>();
       dashingSpeed = movementSpeed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        Restart();
    }

    private void FixedUpdate()
    {    
            playerRb.velocity = direction * movementSpeed;        
    }

    private void MovementInput()
    {
        if (isDashing)
        {
            return;
        }
        //Evita que el jugador cambie de dirección durante el dash. 
        
        
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        /*Si uso GetAxis en vez de GetAxisRaw, el suavizado hará que al dejar de pulsar el botón
         el objeto tarde un segundo en frenar. 
         Pensando en el uso de sticks analógicos, ¿cómo puede diseñarse un movimiento gradual,
         pero que se detenga instantáneamente al soltar el control?*/

    }


    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalSpeed = movementSpeed;
        movementSpeed = dashingSpeed;
        PlayerTr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        PlayerTr.emitting = false;
        movementSpeed = originalSpeed;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

        /*¿Cómo puedo programar el dash de otra forma que no sea guardando la velocidad original en un auxiliar?
         Estuve probando alternativas con playerRb.velocity, o alterando el playerRb.MovePosition y presentaban
        distintos inconvenientes. A veces me cuesta entender cómo se manejan los distintas formas de mover a los
        objetos en el espacio.*/

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Time.timeScale = 0f;
            gameOverText.enabled = true;
            gameOverText.text = "Game Over. Press R to restart.";
        }
    }

    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }
    }
}
