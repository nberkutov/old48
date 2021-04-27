using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public AudioClip go_in;
    public AudioClip go_inside;
    public AudioSource audioSource;

    public GameController gameController;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D circleCollider;
    private float speed;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        
    }
    private void Update()
    {
        float hor = Input.GetAxis("horizontal");
        transform.Translate(Vector3.right * hor * Time.deltaTime);
    }
    bool ins = false;
    float res = 0;
    private void FixedUpdate()
    {
        if (ins)
        {
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.AddForce(Vector2.up * res);
            }
            
            _rigidbody.velocity = _rigidbody.velocity.y > 0 ? Vector2.zero : _rigidbody.velocity;
        }
        //Debug.Log(rigidbody.velocity);
    }


    public void AddForce(float velocity)
    {
        _rigidbody.AddForce(Vector2.down * velocity);
    }

    void foo(float resistance)
    {
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, _rigidbody.velocity / resistance, 0.3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Material")
        {
            Material material = collision.gameObject.GetComponent<Material>();
            speed -= material.resistance;
            _rigidbody.AddForce(Vector2.up * 7.5f);
            ins = true;
            res = material.resistance;
            Debug.Log(material.name);
        }

        if (collision.gameObject.tag == "Oil")
        {
            _rigidbody.velocity = Vector2.zero;
            gameController.SetGameOver();
        }
    }


}
