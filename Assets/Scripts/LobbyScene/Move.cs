using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float move_speed;
    [SerializeField]
    private SpriteRenderer sprite;

    private bool isRunning;
    private void Start() {
        TileGenerator.ReGenerate = true;
        PlayerControl.PlayerHP = PlayerControl.MaxHP;
    }
    private void move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isRunning", true);
            sprite.flipX = true;
            transform.Translate(Vector2.left * move_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isRunning", true);
            sprite.flipX = false;
            transform.Translate(Vector2.right * move_speed * Time.deltaTime);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("isRunning", false);
        }
    }
    void Update()
    {
        move();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("portal1")) {
            SceneSwitcher.ChangeScene();
        }
    }
}
