using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float camSpeedHorizontal = 2.0f;
    private float yaw;



    // Use this for initialization
    void Start ()
    {
        yaw = 0.0f;

        // Hide Cursor
        Cursor.visible = false;
    }



    // Update is called once per frame
    void Update()
    {
        // update player position
        updateInput();

        // update camera horizontal Rotation
        updateCamera();
    }



    void updateInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moveForward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveBack();
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveRight();
        }

        // Not movement related - used for Debugging
        if (Input.GetKey(KeyCode.Alpha0))
        {
            Cursor.visible = true;
        }
    }



    void updateCamera()
    {
        yaw += camSpeedHorizontal * Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }



    void moveForward()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }



    void moveBack()
    {
        GetComponent<Rigidbody>().position += -transform.forward * Time.deltaTime * movementSpeed;
    }



    void moveLeft()
    {
        GetComponent<Rigidbody>().position += -transform.right * Time.deltaTime * movementSpeed;
    }



    void moveRight()
    {
        GetComponent<Rigidbody>().position += transform.right * Time.deltaTime * movementSpeed;
    }

}
