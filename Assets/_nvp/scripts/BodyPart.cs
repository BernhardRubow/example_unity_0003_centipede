using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public Vector3 dir;
    public bool Active;    
    public bool down = false;    
    public BodyPart next;
    [SerializeField] GameObject _head;
    [SerializeField] GameObject _body;
    [SerializeField] float _speed;
    private Vector3 _lastHorizontalDir;
    private Vector3 _hitPosition;
    private bool _isHead;




    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        dir = Vector3.left;
        _lastHorizontalDir = dir;
        _head.SetActive(false);
    }

    void Update()
    {
        if (Active && _isHead)
        {
            MoveHead();
            if(next != null) next.MoveBody(this);
        }
    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mushroom")
        {
            _hitPosition = this.transform.position;
            dir = Vector3.down;
            down = true;
            NormalizePosition();
        }
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    internal void SetInitialSpeed(float initialSpeed)
    {
        _speed = initialSpeed;
    }

    private void MoveHead()
    {
        this.transform.Translate(dir * _speed * Time.deltaTime, Space.World);

        if (down)
        {
            if (Math.Abs(this.transform.position.y - _hitPosition.y) > 1.0f)
            {
                NormalizePosition();
                dir = _lastHorizontalDir *= -1;
                _lastHorizontalDir = dir;
                down = false;
            }
        }
    }

    public void MoveBody(BodyPart previous)
    {
        if(!Active) return;
        this.transform.Translate(dir * _speed * Time.deltaTime, Space.World);

        if (down)
        {
            if (Math.Abs(this.transform.position.y - _hitPosition.y) > 1.0f)
            {
                NormalizePosition();
                dir = _lastHorizontalDir *= -1;
                _lastHorizontalDir = dir;
                down = false;
            }
        }
        else {
            if(!previous.down && previous.transform.position.y >= this.transform.position.y){
                var sign = Mathf.Sign(previous.transform.position.x - this.transform.position.x);

                this.transform.position = new Vector3(
                    previous.transform.position.x - sign,
                    this.transform.position.y,
                    0f);
            }
        }

        if(next != null) next.MoveBody(this);
    }

    internal void SetHead()
    {
        _isHead = true;
        _body.SetActive(false);
        _head.SetActive(true);
    }

    private void NormalizePosition()
    {
        this.transform.position = new Vector3(
            Mathf.RoundToInt(this.transform.position.x),
            Mathf.RoundToInt(this.transform.position.y),
            0
        );
    }
}
