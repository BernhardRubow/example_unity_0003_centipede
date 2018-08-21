using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Centipide : MonoBehaviour
{
	[SerializeField] float _initialSpeed;
    List<BodyPart> _bodyParts;
    BodyPart _lastActivated;
    
    Vector3 _position;
    int _index = 0;

    // Use this for initialization
    void Start()
    {
        _bodyParts = this.GetComponentsInChildren<BodyPart>().ToList();
        _position = this.transform.position;

        BodyPart previous = null;
		for(int i = 0, n = _bodyParts.Count; i < n; i++)
        {
			_bodyParts[i].SetInitialSpeed(_initialSpeed);
            if (i > 0){
                _bodyParts[i-1].next = _bodyParts[i];
            }
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastActivated == null)
        {
            _lastActivated = _bodyParts[_index];
            _lastActivated.transform.parent = null;
            _lastActivated.Active = true;
            _lastActivated.SetHead();
            _index++;
        }
        else
        {
            var distance = Mathf.Abs(_lastActivated.transform.position.x - this.transform.position.x);
            if (distance > 1.0f)
            {
                var nextPart = _bodyParts[_index];
                nextPart.transform.parent = null;
                nextPart.Active = true;
                nextPart.transform.position = _lastActivated.transform.position + _lastActivated.dir * -1f;
                _lastActivated = nextPart;
                _index++;
            }
        }
		if(_index >= _bodyParts.Count) Destroy(this.gameObject);
    }
}
