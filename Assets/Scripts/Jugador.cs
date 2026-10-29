using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public GameObject objetoModelo;

    Animator componenteAnimator;
    CharacterController characterController;

    Vector3 velocidad;
    Vector2 inputMovimiento;

    PlayerInput playerInput;

    Transform camara;
    Camera componenteCamara;
    GameObject auxiliarCamara;

    public GameObject prefabBala;
    public Transform lugarDisparo;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        componenteAnimator = objetoModelo.GetComponent<Animator>();

        velocidad = Vector3.zero;
        inputMovimiento = Vector2.zero;

        playerInput = GetComponent<PlayerInput>();

        camara = Camera.main.transform;
        componenteCamara = Camera.main;

        auxiliarCamara = new GameObject();
    }

    void Update()
    {
        // Leer movimiento
        inputMovimiento = playerInput.actions["Move"].ReadValue<Vector2>();

        // Leer Sprint (botón)
        bool sprint = playerInput.actions["Sprint"].IsPressed();

        bool sprinting = sprint && inputMovimiento.magnitude > 0.1f;

        // Animaciones
        componenteAnimator.SetBool("Sprint", sprinting);

        // Velocidad según sprint
        float velocidadMovimiento = sprint ? 22f : 15f;

        velocidad.x = inputMovimiento.x * velocidadMovimiento;
        velocidad.z = inputMovimiento.y * velocidadMovimiento;



        // Movimiento relativo a la cámara
        auxiliarCamara.transform.rotation = Quaternion.Euler(0, camara.transform.rotation.eulerAngles.y, 0);

        characterController.Move(auxiliarCamara.transform.TransformDirection(velocidad) * Time.deltaTime);

        Vector3 movimientoMundo = auxiliarCamara.transform.TransformDirection(new Vector3(inputMovimiento.x, 0, inputMovimiento.y));
        Vector3 movimientoLocal = transform.InverseTransformDirection(movimientoMundo);

        componenteAnimator.SetFloat(
            "Horizontal",
            movimientoLocal.x,
            0.1f,
            Time.deltaTime
        );

        componenteAnimator.SetFloat(
            "Vertical",
            movimientoLocal.z,
            0.1f,
            Time.deltaTime
        );
        // Apuntar hacia el mouse
        Ray ray =componenteCamara.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            Debug.Log(raycastHit.point);

            Vector3 mira = raycastHit.point;
            mira.y = transform.position.y;

            transform.LookAt(mira);
        }

        // Ataque
        if (playerInput.actions["Attack"].WasPressedThisFrame())
        {
            componenteAnimator.SetTrigger("Melee");

            Instantiate(prefabBala, lugarDisparo.position, transform.rotation);
        }
    }
}