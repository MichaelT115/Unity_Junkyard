using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private BatteryComponent batteryComponent;

    public float speed = 1;
    public Vector3 input;

	void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        batteryComponent = GetComponent<BatteryComponent>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        input = new Vector3
        {
            x = Input.GetAxis("Horizontal"),
            z = Input.GetAxis("Vertical")
        };

        input = Quaternion.AngleAxis(45, Vector3.up) * input;
        input.Normalize();

        if (batteryComponent.IsZero)
		{
			Kill();
		}
	}

	private void Kill()
	{
		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.AddForce(Random.onUnitSphere);
		enabled = false;
	}

	void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + input * speed * Time.fixedDeltaTime);
    }
}
