using UnityEngine;

public class EnemyChildren : Enemy
{
    public bool cone = true;

    public override void Serang()
    {
        Debug.Log("Children menyerang");    
    }   
}
