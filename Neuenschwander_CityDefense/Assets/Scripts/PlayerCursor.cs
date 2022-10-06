using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField]
    private GameObject goodMissile;
    private AudioSource audioSource;

    private PlayerInput plrInput;

    private InputAction vertAction;
    private InputAction horzAction;

    [SerializeField]
    private float cursorSpeed = 8f;

    private void Awake()
    {
        plrInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();

        vertAction = plrInput.actions["Vertical"];
        horzAction = plrInput.actions["Horizontal"];
        transform.position = new(0f, 20f, 65f);
        plrInput.actions["Fire"].started += PlayerFired;
    }

    private void Update()
    {
        float vertical = vertAction.ReadValue<float>();
        float horizontal = horzAction.ReadValue<float>();

        Vector3 transVector = cursorSpeed * Time.deltaTime * new Vector3(horizontal, vertical, 0f).normalized;

        transform.Translate(transVector, Space.World);
        float newX = Mathf.Clamp(transform.position.x, -84f, 84f);
        float newY = Mathf.Clamp(transform.position.y, 8f, 162f);
        transform.position = new(newX, newY, 65f);
    }
    private void PlayerFired(InputAction.CallbackContext _)
    {
        audioSource.Play();
        GameObject misObj = Instantiate(goodMissile, new Vector3(0.5f, 0.6f, 72f), Quaternion.identity);
        GoodMissile goodMis = misObj.GetComponent<GoodMissile>();
        Vector3 toPos = transform.position;
        toPos.z = 72f;
        goodMis.gotoPos = toPos;
    }
}
