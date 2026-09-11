using UnityEngine;

public class GameManajer : MonoBehaviour
{
    public static GameManajer Instance;

    public int totalKoin;
    private int koinTerkumpul = 0;
    [SerializeField] private int skor = 0;

    

    void Awake()
    {
        // Sistem Singleton agar bisa diakses langsung lewat GameManajer.Instance
        if (Instance == null) 
        { 
            Instance = this; 
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        // Menghitung otomatis semua objek koin di awal game
        totalKoin = GameObject.FindGameObjectsWithTag("coin").Length;
        Debug.Log("Total koin di scene saat ini: " + totalKoin);
    }

    // Fungsi ini dipanggil otomatis dari script PlayerMovement
    public void AmbilKoin()
    {
        koinTerkumpul++;
        Debug.Log("Koin terkumpul: " + koinTerkumpul + " / " + totalKoin);

        // Jika jumlah koin terkumpul sudah mencapai target total koin
        if (koinTerkumpul >= totalKoin)
        {
            Menang();
        }
    }

    void Menang()
    {
        Debug.Log("KAMU MENANG!");
        // Tempat menaruh kode UI Menang atau pindah level berikutnya di sini
    }
    void OnEnable()
    {
        Enemy.OnZombieMati += TambahSkorSaatZombieMati;
    }
    void OnDisable()
    {
        Enemy.OnZombieMati += TambahSkorSaatZombieMati;
    }
        void TambahSkorSaatZombieMati(Enemy zombieYangMati)
    {
        skor += 10;
        Debug.Log("Skor: " + skor);
    }

}
