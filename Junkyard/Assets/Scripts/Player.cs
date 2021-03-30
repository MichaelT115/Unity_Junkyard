using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player : MonoBehaviour
{
    private new Rigidbody rigidbody;
	private BatteryComponent batteryComponent;
	private HealthComponent healthComponent;
	private WeaponHandlerComponent weaponHandlerComponent;

	[SerializeField]
	private bool isShooting = false;

	public float speed = 1;
    public Vector3 moveDirection;

	[SerializeField]
	private Transform aimPositionMarker;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		batteryComponent = GetComponent<BatteryComponent>();
		healthComponent = GetComponent<HealthComponent>();
		weaponHandlerComponent = GetComponent<WeaponHandlerComponent>();
	}

	private static readonly Quaternion inputRotation = Quaternion.AngleAxis(45, Vector3.up);

	private static Vector3 InputDirection => Vector3.ClampMagnitude(inputRotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1);

	private void Update()
	{
		if (Input.GetKey(KeyCode.R))
		{
			SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
			SceneManager.LoadScene("Level", LoadSceneMode.Additive);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Break();
		}

		moveDirection = InputDirection;

		if (batteryComponent.IsZero || healthComponent.IsZeroOrBelow)
		{
			Kill();
		}


		if (ShouldActivateWeapon)
		{
			weaponHandlerComponent.Activate();
			isShooting = true;
		}
		else if (ShouldDeactivateWeapon)
		{
			weaponHandlerComponent.Deactivate();
			isShooting = false;
		}

		aimPositionMarker.position = AimPosition;
	}

	private void Kill()
	{
		weaponHandlerComponent.Deactivate();

		aimPositionMarker.gameObject.SetActive(false);

		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.AddForce(Random.onUnitSphere);
		enabled = false;
	}

	void FixedUpdate()
	{
		rigidbody.MovePosition(rigidbody.position + moveDirection * speed * Time.fixedDeltaTime);
		rigidbody.MoveRotation(Quaternion.LookRotation(AimDirection, Vector3.up));
	}

	private Vector3 AimDirection
	{
		get
		{
			Vector3 startPosition = new Vector3(transform.position.x, weaponHandlerComponent.WeaponHeight, transform.position.z);
			return (AimPosition - startPosition).normalized;
		}
	}
	private bool ShouldActivateWeapon => Input.GetMouseButton(0) && !isShooting;
	public bool IsShooting => isShooting;
	private bool ShouldDeactivateWeapon => !Input.GetMouseButton(0) && isShooting;
	public Vector3 AimPosition =>
		WeaponPlane.Raycast(PlayerMouseRay, out float distance)
		? PlayerMouseRay.GetPoint(distance)
		: rigidbody.position;
	private Ray PlayerMouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);
	private Plane WeaponPlane => new Plane(Vector3.up, -weaponHandlerComponent.WeaponHeight);

	public HealthComponent HealthComponent => healthComponent;
	public BatteryComponent BatteryComponent => batteryComponent;
}
