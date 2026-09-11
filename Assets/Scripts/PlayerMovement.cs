using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   public float kecepatan = 5f;
    private Vector2 arahGerak;
    
    public int skor = 0; 

    public GameManajer gameManajer;   

    void OnMove(InputValue value)
    {
        arahGerak = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 arah = new Vector3(arahGerak.x, arahGerak.y, 0);
        transform.position += arah * kecepatan * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coin"))
        {
            // 1. Hancurkan objek koin terlebih dahulu
            Destroy(other.gameObject);

            
            skor++; 
            Debug.Log("Skor : " + skor);

            // 3. DI HUBUNGKAN DI SINI: Beri tahu GameManajer bahwa koin bertambah
            if (GameManajer.Instance != null)
            {
                GameManajer.Instance.AmbilKoin();
            }
            else
            {
                Debug.LogWarning("GameManajer tidak ditemukan.");
            }
        }
    }
}