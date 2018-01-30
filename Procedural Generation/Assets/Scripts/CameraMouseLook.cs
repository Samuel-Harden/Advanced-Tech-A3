using UnityEngine;

public class CameraMouseLook : MonoBehaviour
{
    public float camSpeedVertical = 2.0f;
    private float pitch;



	// Use this for initialization
	void Start ()
    {
        pitch = 0.0f;
	}
	


	// Update is called once per frame
	void Update ()
    {
        pitch -= camSpeedVertical * Input.GetAxis("Mouse Y");

        transform.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
    }
}
