using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MarioControler : MonoBehaviour
{
    private float VanToc = 3f;
    private float VanTocToiDa = 6f; //Van toc toi da khi giu phim z
    private float TocDo = 0;
    private bool DuoiDat;
    private bool ChuyenHuong = false;
    private bool isRight = true;
    public float Nhaycao; // lay toc do nhay cua Mario
    public float NhayThap;
    public float RoiXuong;

    private float KTGiuPhim = 0.2f;
    private float TGGiuPhim = 0;

    private Rigidbody2D _rigidbody2D;
    private bool isGrounded = false;

    private Animator _animator;
    private AudioSource Amthanh;
    
    
    
    
    //Hiển thị cấp độ và độ lớn của Mario
    public int CapDo = 0;
    public bool BienHinh = false;
    private Vector2 ViTriChet;

    private float moveSpeed = 3;
    private bool moveLeft, moveRight;
    private float horizontalMove;
    private bool ischeck;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Amthanh = GetComponent<AudioSource>();
        
        moveLeft = false;
        moveRight = false;

        // currentAmmo = maxAmmo; // Khởi tạo đạn ban đầu

    }
    

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("TocDo", TocDo);
        _animator.SetBool("DuoiDat", DuoiDat);
        _animator.SetBool("ChuyenHuong", ChuyenHuong);
            Nhaylen();

            MoveMario();
        

        // if (Input.GetKeyDown(KeyCode.F) && currentAmmo > 0 )
        // {
        //     Instantiate(skill, transform.position, Quaternion.identity);
        //     DanController dan = this.skill.GetComponent<DanController>();
        //     dan.direction = Vector3.up;
        //     currentAmmo--;
        // }
        // if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        // {
        //     // Nạp đạn
        //     currentAmmo = maxAmmo; // Nạp lại toàn bộ đạn
        // }
        
        BanDanvaTangToc();
        
        if (BienHinh)
        {
            switch (CapDo)
            {
                case 0:
                {
                    StartCoroutine(MarioThuNho());
                    TaoAmThanh("MarioNhoDi");
                    BienHinh = false;
                    break;
                }
                case 1:
                {
                    StartCoroutine(MarioAnNam());
                    TaoAmThanh("MarioLonLen");
                    BienHinh = false;
                    break;
                }
                default:
                    BienHinh = false; break;
            }
        }

        if (gameObject.transform.position.y < -5.5f)
        {
            Destroy(gameObject);
        }
    }

    private void MoveMario()
    {
        if (moveLeft)
        {
            horizontalMove = -moveSpeed;
        }else if (moveRight)
        {
            horizontalMove = moveSpeed;
        }
        else
        {
            horizontalMove = 0;
        }
    }
    

    private void FixedUpdate()
    {
        Dichuyen();

        if (ischeck)
        {
            _rigidbody2D.velocity = new Vector2(horizontalMove, _rigidbody2D.velocity.y);
            TocDo = Mathf.Abs(horizontalMove);
        }
    }

    void Dichuyen()
    {
        float isLeft = Input.GetAxis("Horizontal");
        _rigidbody2D.velocity = new Vector2(VanToc * isLeft, _rigidbody2D.velocity.y);
        TocDo = Mathf.Abs(VanToc * isLeft);
        if (isLeft > 0 && !isRight)
        {
            HuongMatMario();
        }

        if (isLeft < 0 && isRight)
        {
            HuongMatMario();
        }
        
    }
    
    void HuongMatMario()
    {
        // Nếu Mario không quay thì 
        isRight = !isRight;
        Vector2 HuongQuay = transform.localScale;
        HuongQuay.x *= -1;
        
        transform.localScale = HuongQuay;

        if (TocDo > 0)
        {
            StartCoroutine(MarioChuyenHuon());
        }
    }

    void Nhaylen()
    {
        if (Input.GetKeyDown(KeyCode.W) && DuoiDat)
        {
            _rigidbody2D.AddForce((Vector2.up)*Nhaycao);
            TaoAmThanh("Nhay");
            DuoiDat = false;
        }

        {
            if (_rigidbody2D.velocity.y < 0)
            {
                _rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (RoiXuong - 1)*Time.deltaTime;
            }else if (_rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.W))
            {
                _rigidbody2D.velocity += Vector2.up *Physics2D.gravity.y * (NhayThap - 1) * Time.deltaTime;
            }
        }
    }

    public void btnNhay()
    {
        
        if (DuoiDat)
        {
            _rigidbody2D.AddForce((Vector2.up)*Nhaycao);
            TaoAmThanh("Nhay");
            DuoiDat = false; 
        }
    }

    public void btnRight()
    {
        ischeck = true;
        TocDo = 1;
        if (!isRight)
        {
            HuongMatMario();
        }

        
        moveRight = true;

    }

    public void btnLeft()
    {
        ischeck = true;
        TocDo = 1;
        if ( isRight)
        {
            HuongMatMario();
        }
        moveLeft = true;
    }

    public void stopMoving()
    {
        ischeck = false;
        moveRight = false;
        moveLeft = false;
        
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

    IEnumerator MarioChuyenHuon()
    {
        ChuyenHuong = true;
        yield return new WaitForSeconds(0.2f);
        ChuyenHuong = false;
    }

    void BanDanvaTangToc()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            TGGiuPhim += Time.deltaTime;
            if (TGGiuPhim < KTGiuPhim)
            {
                //Ban 
            }
            else
            {
                VanToc = VanToc * 1.01f;
                if (VanToc > VanTocToiDa)
                {
                    VanToc = VanTocToiDa;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            VanToc = 4f;
            TGGiuPhim = 0;
        }
    }

    public void TaoAmThanh(string FileAmThanh)
    {
        Amthanh.PlayOneShot(Resources.Load<AudioClip>("Audio/" + FileAmThanh));
    }
    
    //Thay đổi độ lớn của Mario
    IEnumerator MarioAnNam()
    {
        float DoTre = 0.1f;
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),0);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),1);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),1);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),0);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),0);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),1);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),1);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),0);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),0);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),1);
        yield return new WaitForSeconds(DoTre);
        
    }
    
    
    IEnumerator MarioThuNho()
    {
        float DoTre = 0.1f;
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),1);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),0);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),0);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),1);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),1);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),0);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),0);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),1);
        yield return new WaitForSeconds(DoTre);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioNho"),1);
        _animator.SetLayerWeight(_animator.GetLayerIndex("MarioTo"),0);
        yield return new WaitForSeconds(DoTre);
        
    }

    public void MarioChet()
    {
        ViTriChet = transform.localPosition;
        GameObject MarioChett = (GameObject)Instantiate(Resources.Load("Prefabs/MarioChet"));
        MarioChett.transform.localPosition = ViTriChet;
        Destroy(gameObject);
        
    }
    
    
    
    

    
}
