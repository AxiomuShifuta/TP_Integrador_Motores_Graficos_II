using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    Rigidbody playerRb;
    [SerializeField] private float movementSpeed;
    private Vector3 direction;


    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false; // Por alguna raz�n, Unity me advierte que nunca se usa esta variable.
    [SerializeField] private float dashingSpeed = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    // Start is called before the first frame update
    void Start()
    {
       playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();

        //float moveX = Input.GetAxisRaw("Horizontal");
        //float moveZ = Input.GetAxisRaw("Vertical");
        //direction = new Vector3(moveX, 0, moveZ).normalized;

        /*Si uso GetAxis en vez de GetAxisRaw, el suavizado har� que al dejar de pulsar el bot�n
         el objeto tarde un segundo en frenar. 
         Pensando en el uso de sticks anal�gicos, �c�mo puede dise�arse un movimiento gradual,
         pero que se detenga instant�neamente al soltar el control?*/


        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
      
            playerRb.MovePosition(playerRb.position + direction * movementSpeed * Time.fixedDeltaTime);
        
        /*la descripci�n de MovePosition dice que trabaja con el RigidBody Kinematic. Sin embargo, el
         objeto player no tiene activada esa opci�n. �Puede traer alg�n problema a futuro?*/
    }

    private void MovementInput()
    {
        if (isDashing)
        {
            return;
        }//Evita que el jugador cambie de direcci�n durante el dash. 
        
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

    }


    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalSpeed = movementSpeed;
        movementSpeed = dashingSpeed;
        yield return new WaitForSeconds(dashingTime);
        movementSpeed = originalSpeed;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

        /*�C�mo puedo programar el dash de otra forma que no sea guardando la velocidad original en un auxiliar?
         Estuve probando alternativas con playerRb.velocity, o alterando el playerRb.MovePosition y presentaban
        distintos inconvenientes. Me cuesta mucho entender c�mo se manejan los distintas formas de mover a los
        objetos en el espacio.*/

    }
}
