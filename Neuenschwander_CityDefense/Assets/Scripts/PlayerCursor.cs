using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    private PlayerInput plrInput;

    private InputAction vertAction;
    private InputAction horzAction;

    [SerializeField]
    private float cursorSpeed = 5f;

    private void Awake()
    {
        plrInput = GetComponent<PlayerInput>();
        vertAction = plrInput.actions["Vertical"];
        horzAction = plrInput.actions["Horizontal"];
        transform.position = new(0f, 20f, 65f);
    }

    private void Update()
    {
        float vertical = vertAction.ReadValue<float>();
        float horizontal = horzAction.ReadValue<float>();

        Vector3 transVector = new Vector3(horizontal, vertical, 0f).normalized * Time.deltaTime * cursorSpeed;

        transform.Translate(transVector, Space.World);
        float newX = Mathf.Clamp(transform.position.x, -84f, 84f);
        float newY = Mathf.Clamp(transform.position.y, 8f, 162f);
        transform.position = new(newX, newY, 65f);
    }
}
