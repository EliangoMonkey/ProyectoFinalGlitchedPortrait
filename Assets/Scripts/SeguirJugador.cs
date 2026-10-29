using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = jugador.position + offset;
    }
}
