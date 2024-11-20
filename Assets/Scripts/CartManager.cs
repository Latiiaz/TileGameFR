using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    private List<Transform> _carts;
    [SerializeField] private Transform _cartPrefab;

    private int _currentCarts = 0;
    [SerializeField] private int _maxCarts = 2;

    // COPIED FROM LEVELMANAGER TO TEST (USE "T" KEY) 
    public float RestartTime = 1f;
    bool rKeyDown = false;
    float timeRKeyDown = 0f;

    private TractorMovement _tractorMovement;

    // Start is called before the first frame update
    void Start()
    {
        _currentCarts = 0;

        _carts = new List<Transform>();
        _carts.Add(this.transform);


    }

    // Update is called once per frame
    void Update()
    {
        RestartScene();
        //for (int i = _carts.Count - 1; i > 0; i--)
        //{
        //    _carts[i].position = _carts[i - 1].position;
        //}

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y), 0);
    }

    void InstantiateCart()
    {
        if (_currentCarts < _maxCarts)
        {
            _currentCarts++;
            Debug.Log("Spawning One Cart");
            Transform cart = Instantiate(this._cartPrefab);
            cart.position = _carts[_carts.Count - 1].position;

            _carts.Add(cart);

        }

    }

    public void RestartScene() // Copied to test spawning of carts
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (!rKeyDown)
            {
                rKeyDown = true;
                timeRKeyDown = 0f;
            }
            if (rKeyDown)
            {
                timeRKeyDown += Time.deltaTime;

                if (timeRKeyDown >= RestartTime)
                {
                    InstantiateCart();
                }
            }
            if (rKeyDown)
            {
                timeRKeyDown += Time.deltaTime;

                if (timeRKeyDown >= RestartTime)
                {
                    InstantiateCart();
                }
            }
            else // restart counter if doesnt reach req  time
            {
                rKeyDown = false;
                timeRKeyDown = 0f;
            }
        }
    }


}
