using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart_V2 : MonoBehaviour
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++   
    public BodyPart_V2 previous;
    public BodyPart_V2 next;
    public bool active;
    public Vector3 dir;
    public System.Action action;
    public Transform visualQuad;





    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private float _speed;
    private GameObject _head;
    private GameObject _body;
    private bool _isHead;
    private float _lastHeight;
    private Vector3 _lastDir;


    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isHead) return;

        action();
    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++	  
    private void NormalizePosition()
    {
        this.transform.position = new Vector3(
            Mathf.Ceil(this.transform.position.x) + 0.5f,
            Mathf.Ceil(this.transform.position.y) + 0.5f,
            0
        );
    }
    void MoveHorizontal()
    {
        if (Map.EvaluteTurn(this.transform.position, dir))
        {


            // check if down is free
            if (!Map.EvaluteTurn(this.transform.position + Vector3.down, -dir))
            {
                _lastDir = dir;
                dir = Vector3.down;
                TurnVisual(dir);
                action = MoveVertical;
                _lastHeight = this.transform.position.y;
                if (next != null) next.action();
                return;
            }
            else
            {
                dir = -dir;
            }

        }

        this.transform.Translate(
            dir * _speed * Time.deltaTime,
            Space.World
        );
        if (next != null) next.action();

    }

    void MoveVertical()
    {
        this.transform.Translate(
            Vector3.down * _speed * Time.deltaTime,
            Space.World
        );
        if (Mathf.Abs(this.transform.position.y - _lastHeight) > 1.0f)
        {
            dir = _lastDir * -1;
            TurnVisual(dir);
            action = MoveHorizontal;
            this.transform.position = new Vector3(
                this.transform.position.x,
                _lastHeight - 1f,
                0);
        }
        if (previous != null)
        {
            if (previous.dir.y == 0 && this.dir.y == 0)
            {
                this.transform.position = previous.transform.position - dir;
            }
        }
        if (next != null) next.action();
    }

    private void TurnVisual(Vector3 dir)
    {
        if (dir == Vector3.left) visualQuad.rotation = Quaternion.identity;
        else if (dir == Vector3.right) visualQuad.eulerAngles = new Vector3(0f, 0f, 180f);
        else visualQuad.eulerAngles = new Vector3(0f, 0f, 90f);
    }

    internal void SetInitialSpeed(float initialSpeed)
    {
        _speed = initialSpeed;
    }

    void Init()
    {
        _head = transform.Find("Head").gameObject;
        _body = transform.Find("Body").gameObject;

        dir = Vector3.left;

        action = MoveHorizontal;
    }

    internal void SetHead(bool isHead)
    {
        _isHead = isHead;
        _body.SetActive(!_isHead);
        _head.SetActive(_isHead);

        if (_isHead)
            visualQuad = _head.transform;
        else
            visualQuad = _body.transform;
    }
}

public class Turn
{
    public Vector2 point;
    public Vector3 dir;
}
