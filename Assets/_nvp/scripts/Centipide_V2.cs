using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Centipide_V2 : MonoBehaviour {

	// +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
	// +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField] float _initialSpeed;




	// +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private List<BodyPart_V2> _bodyParts;
	private Vector3 _position;
    private int _index;
    private BodyPart_V2 _lastActivated;
    
    

    
    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		_bodyParts = this.GetComponentsInChildren<BodyPart_V2>().ToList();
        _position = this.transform.position;

		for(int i = 0, n = _bodyParts.Count; i < n; i++)
        {
            _bodyParts[i].SetInitialSpeed(_initialSpeed);

            
        }
    }

    // Update is called once per frame
    void Update () {
		if (_lastActivated == null)
        {
            _lastActivated = _bodyParts[_index];
            _lastActivated.transform.parent = null;
            _lastActivated.active = true;
            _lastActivated.SetHead(true);
            _index++;
        }
        else
        {
            var distance = Mathf.Abs(_lastActivated.transform.position.x - this.transform.position.x);
            if (distance > 1.0f)
            {
                var nextPart = _bodyParts[_index];
                nextPart.SetHead(false);
                _lastActivated.next = nextPart;
                nextPart.previous = _lastActivated;
                nextPart.transform.parent = null;
                nextPart.active = true;
                nextPart.transform.position = _lastActivated.transform.position + _lastActivated.dir * -1f;
                _lastActivated = nextPart;
                _index++;
            }
        }
		if(_index >= _bodyParts.Count) Destroy(this.gameObject);
	}
    
    
    
    
    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    

}
