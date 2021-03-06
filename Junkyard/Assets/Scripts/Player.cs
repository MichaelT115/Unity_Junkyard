﻿using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private BatteryComponent batteryComponent;
	private HealthComponent healthComponent;

    public float speed = 1;
    public Vector3 moveDirection;

	[SerializeField]
	private Transform aimPositionMarker;
	[SerializeField]
	private Transform shootTransform;

	[SerializeField]
	private Weapon weapon;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		batteryComponent = GetComponent<BatteryComponent>();
		healthComponent = GetComponent<HealthComponent>();
	}

	private void Start()
    {
		weapon.Equip(this, shootTransform);
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

		moveDirection = InputDirection;

		if (batteryComponent.IsZero || healthComponent.IsZeroOrBelow)
		{
			Kill();
		}

		weapon.Update(Time.deltaTime);

		if (ShouldShoot)
		{
			weapon.Activate();
		} 
		else
		{
			weapon.Deactivate();
		}

		aimPositionMarker.position = AimPosition;
	}

	private static bool ShouldShoot
	{
		get
		{
			const int LEFT_MOUSE_BUTTON = 0;
			return Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON);
		}
	}

	public Vector3 AimPosition =>
		WeaponPlane.Raycast(PlayerMouseRay, out float distance)
		? PlayerMouseRay.GetPoint(distance)
		: rigidbody.position;

	private Ray PlayerMouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);
	private Plane WeaponPlane => new Plane(Vector3.up, -ShootTransform.position.y);

	private void Kill()
	{
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
			Vector3 startPosition = new Vector3(transform.position.x, ShootTransform.position.y, transform.position.z);

			return (AimPosition - startPosition).normalized;
		}
	}

	public Transform ShootTransform => shootTransform;

	public HealthComponent HealthComponent => healthComponent;
	public BatteryComponent BatteryComponent => batteryComponent;
}
