using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnDichuyen : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private float moveSpeed;
    private bool moveLeft, moveRight;
    
    private float TocDo = 0;
    private bool DuoiDat = true;
    private bool ChuyenHuong = false;
    private float VanToc = 3f;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        
        moveLeft = false;
        moveRight = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _animator.SetFloat("TocDo", TocDo);
        _animator.SetBool("DuoiDat", DuoiDat);
        _animator.SetBool("ChuyenHuong", ChuyenHuong);
        if (moveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Translate(Vector3.left *  5f * Time.deltaTime);
            
        }

        if (moveRight)
        {
            // TocDo = Mathf.Abs(VanToc);
            transform.localScale = new Vector3(1, 1, 1);
            transform.Translate(Vector3.right *  5f * Time.deltaTime);
            
        }
    }
    
    public void btnRight()
    {
        TocDo = 1;
        DuoiDat = true;
        Debug.Log("Right");
        Debug.Log("" + DuoiDat + TocDo);
        moveRight = true;

    }

    public void btnLeft()
    {
        Debug.Log("Button Left Pressed");
        moveLeft = true;
    }

    public void stopMoving()
    {
        Debug.Log("Stop");
        moveRight = false;
        moveLeft = false;
        _rigidbody2D.velocity = Vector2.zero;
        TocDo = 0;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "NenDat")
        {
            DuoiDat = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "NenDat")
        {
            DuoiDat = true;
        }
    }
    
    
}
