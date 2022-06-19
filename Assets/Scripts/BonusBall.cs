using System.Collections;
using UnityEngine;

public class BonusBall : MonoBehaviour
{
    [SerializeField] float addScalePerBall=0.05f, addMassPerBall=0.05f;
    [SerializeField] float forceByBall = 1;
    [SerializeField] GameObject processParticles,pushParticles;
    private Rigidbody rb;
    private int balls = 0;
    [SerializeField] AudioSource source;
    [SerializeField] XOR.Clip pushClip;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        source.Play();
        processParticles.SetActive(true);
    }

    public void AddBall()
    {
        balls++;
        transform.localScale += Vector3.one * addScalePerBall;
        rb.mass += addMassPerBall;
    }
    IEnumerator delaySlow()
    {
        yield return new WaitForSeconds(0.5f);
        XOR.SlowMotionEffect.instance.Slow(3,1,0.4f);
    }
    internal void Push(Vector3 dir)
    {
        XOR.SoundCreator.Create(pushClip);
        source.Stop();
        StartCoroutine(delaySlow());
        XOR.CameraShaker.instance.Shake(2, 22);
        processParticles.SetActive(false);
        pushParticles.SetActive(true);
        rb.isKinematic = false;
        rb.AddForce(dir * (forceByBall+5) * balls, ForceMode.Impulse);
    }
    
}
