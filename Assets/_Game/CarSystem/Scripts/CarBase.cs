using System;
using Assets._Game.CarController;
using Assets._Game.CarSystem;
using _Game.CarSystem.Scripts;
using _Game.Interacts.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CarBase : MonoBehaviour
{
    [SerializeField] protected CarDataSo m_carDataSo;
    [SerializeField] protected Rigidbody m_rigidbody;

    protected CarBaseData CarBaseData;
    protected Transform _transform;
    protected Quaternion _direction = Quaternion.identity;
    
    private bool _moveEnable;
    private float _accTime;
    private float _speed;
    private float _curve;
    private Tween _tween;

    private void Start()
    {
        _transform = transform;
    }

    public  void InitCar()
    {
        _transform = transform;
        CarBaseData = new CarBaseData
        {
            StartPosition = _transform.position,
            StartRotation = _transform.rotation,
        };
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_moveEnable) return;
        if (!CarBaseData.IsPlayer) return;
        
        if (other.transform.CompareTag("Interact"))
        {
            CarManager.Instance.OnFail();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_moveEnable) return;
        if (!CarBaseData.IsPlayer) return;
        
        if (!other.transform.CompareTag("Target")) return;
        var order = other.transform.GetComponent<CarTargetController>().GetOrder();
        OnTarget(order);
    }


    #region Public

    public virtual void ResetCar()
    {
        StopCar();
        gameObject.SetActive(false);
    }

    protected void StopCar(bool isSmooth = false)
    {
        _moveEnable = false;
        if (isSmooth)
        {
            _tween =DOTween.To(x =>
            {
                _speed = x;
                m_rigidbody.velocity = _transform.forward * _speed;
            }, _speed, 0, 1f).SetEase(m_carDataSo.accelerationCurve).OnComplete(Stop);
            return;
        }
        
        Stop();
        return;

        void Stop()
        {
            _tween?.Kill(true);
            m_rigidbody.isKinematic = true;
            m_rigidbody.velocity = Vector3.zero;
        
            _accTime = 0;
            _speed = 0;
        }
    }

    public virtual void OnRetry()
    {
        StopCar();
        _transform.position = CarBaseData.StartPosition;
        _transform.rotation = CarBaseData.StartRotation;
    }

    [Button]
    public void OnGameStart()
    {
        m_rigidbody.isKinematic = false;
        SetEnable(true);
    }

    #endregion

    #region Private

    protected virtual void OnFixedUpdate()
    {
        if (!_moveEnable)
        {
            return;
        }
        Move();
    }

    protected virtual void Move()
    {
        if (_speed < m_carDataSo.moveSpeed)
        {
            _accTime += Time.deltaTime;
            _curve = m_carDataSo.accelerationCurve.Evaluate(_accTime / m_carDataSo.accelerationTime);
            _speed = Mathf.Lerp(0, m_carDataSo.moveSpeed, _curve);
        }
        
        m_rigidbody.rotation = _direction;
        m_rigidbody.velocity = _transform.forward * _speed;
    }
    
    private void OnTarget(int order)
    {
        if(order == CarBaseData.Order)
        {
            _moveEnable = false;
            _tween = DOTween.To(x =>
            {
                _speed = x;
                m_rigidbody.velocity = _transform.forward * _speed;
            }, _speed, 0, 0.3f).SetEase(m_carDataSo.accelerationCurve).OnComplete((() =>
            {
                CarManager.Instance.OnCarFinish(CarBaseData.IsPlayer, GetSavedData());
            }));
            return;
        }

        CarManager.Instance.OnFail();
    }

    protected virtual (Quaternion , Vector3)[] GetSavedData()
    {
        return null;
    }

    private void SetEnable(bool enable)
    {
        _moveEnable = enable;
    }
    #endregion
}
