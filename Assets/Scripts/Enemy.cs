using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public int hp = 100;
    public float ms = 2f;
    [SerializeField] private int damagetabrakan = 20;
    protected Transform player;


     [Header("State Machine")]
    [SerializeField] private float jarakDeteksi = 6f;
    [SerializeField] private float jarakSerang = 1.2f;
    [SerializeField] private float jedaSerang = 1f;

    private statezombie state = statezombie.IDLE;
    private float waktuSerangTerakhir = 0f;
    [SerializeField] private float radiusPatrol = 3f;
    private Vector2 titikAwal;
    private Vector2 tujuanPatrol;
    public static event Action<Enemy> OnZombieMati; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        titikAwal = transform.position;
        PilihTujuanPatrolBaru();
    }

    // Update is called once per frame
    void Update()
    {
        
        PeriksaTransisi();

        switch (state)
        {
            case statezombie.IDLE: 
                PerilakuIdle(); 
                break;
            case statezombie.PATROL: 
                PerilakuPatrol(); 
                break;
            case statezombie.CHASE: 
                PerilakuChase(); 
                break;
            case statezombie.ATTACK: 
                PerilakuAttack(); 
                break;
        }
    }



    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= jarakSerang)
        {
            state = statezombie.ATTACK; 
        }
        else if (jarak <= jarakDeteksi)
        {
            state = statezombie.CHASE; 
        }
        else
        {
            state = statezombie.PATROL;
        }
    }

    void PerilakuIdle()
    {
    }

    void PerilakuPatrol()
    {
       
        transform.position = Vector2.MoveTowards(transform.position, tujuanPatrol, 0.5f * Time.deltaTime);

        
        if (Vector2.Distance(transform.position, tujuanPatrol) < 0.1f)
        {
            PilihTujuanPatrolBaru();
        }
    }

    void PilihTujuanPatrolBaru()
    {
        Vector2 acak = UnityEngine.Random.insideUnitCircle * radiusPatrol;
        tujuanPatrol = titikAwal + acak;
    }

    void PerilakuChase()
    {
        Kejar();
        Debug.Log("Dikejar Enemy!");
    }

    void PerilakuAttack()
    {
        
        if (Time.time >= waktuSerangTerakhir + jedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;
        }
    }


    public float JarakKePlayer()
    {
        if (player == null) return 999f; 
        return Vector2.Distance(transform.position, player.position);
    }

    public void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
        Debug.Log("Enemy Mengejar!!");
    } 

    public virtual void Serang()
    {
        Debug.Log("Enemy Menyerang!");
    }
    public void KenaDamage(int Damage)
    {
        hp -= Damage;
        Debug.Log("enemy Kena Damage: " + Damage + ", HP Sekarang : " + hp );

        if (hp <= 0)
        {
            Mati();
        }
    }
    protected virtual void Mati()
    {
        Debug.Log(name + "Kalah!");
        OnZombieMati?.Invoke(this);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         IDamageable playerScript = other.GetComponent<IDamageable>();
         if (playerScript != null)
         {
             playerScript.KenaDamage(damagetabrakan);
         }
    }
}
