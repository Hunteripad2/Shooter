using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector] private Transform player;
    [HideInInspector] private Vector3 playerPos;
    [HideInInspector] private float currentDistance;

    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed = 10f;
    //[SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float optimalDistance = 5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        playerPos = player.position;
        MoveToPlayer();
        RotateToPlayer();
    }

    private void MoveToPlayer()
    {
        currentDistance = Vector3.Distance(playerPos, transform.position);

        if (currentDistance > optimalDistance)
        {
            //transform.position = Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.deltaTime);
            //characterController.Move(Vector3.forward * movementSpeed * Time.deltaTime);
            rb.MovePosition(Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.deltaTime));
        }
    }

    private void RotateToPlayer()
    {
        //playerPos -= transform.position;

        Vector3 targetDirection = (playerPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        rb.MoveRotation(targetRotation);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90f;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
