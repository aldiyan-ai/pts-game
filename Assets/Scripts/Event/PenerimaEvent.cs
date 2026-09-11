using UnityEngine;

public class PenerimaEventr : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     void OnEnable()
    {
        PemancarEvent.OnTekanSpasi += Reaksi;
    }

    void OnDisable()
    {
        PemancarEvent.OnTekanSpasi -= Reaksi;
    }

    void Reaksi()
    {
        Debug.Log("Penerima: aku dengar event spasi!");
    }
}
