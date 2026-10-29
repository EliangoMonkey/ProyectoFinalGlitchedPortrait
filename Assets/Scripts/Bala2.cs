using UnityEngine;

public class Bala2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * 32);
        Debug.DrawRay(this.transform.position, this.transform.TransformDirection(Vector3.forward), Color.red);

        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), 1))
        {
            Destroy(this.gameObject);
        }

    }
}
