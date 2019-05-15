using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallisticProjectile : MonoBehaviour
{
    
    
    /// <summary>
    /// источник звука столкновения
    /// </summary>
    private AudioSource Audio;

    /// <summary>
    /// список звуков столкновения
    /// </summary>
    public AudioClip[] AttackSounds;
    
    /// <summary>
    /// цель броска
    /// </summary>
    public Transform target;
    
    /// <summary>
    /// Урон снаряда
    /// </summary>
    protected float damage;
    
    /// <summary>
    /// Частицы проигрываемые при столкновении 
    /// </summary>
    public GameObject particles;
    
    /// <summary>
    /// Цвет на который действует урон и столкновение
    /// </summary>
    private string enemyColor = "Blue";
    /// <summary>
    /// Радиус взрыва
    /// </summary>
    public float ExplosionRadius;

    /// <summary>
    /// Сила отталкивания от взрыва
    /// </summary>
    public const float ExplosionForce = 1f;

    /// <summary>
    /// Играет случайный звук из списка звуков
    /// </summary>
    protected void PlayRandomAttackSound()
    {
        AudioSource.PlayClipAtPoint(AttackSounds[Random.Range(0, AttackSounds.Length)], gameObject.transform.position, GameInfo.MusicVolume * GameInfo.VolumeCoefficient * 100);
  
    }
    
    /// <summary>
    /// Устанавливает текущую цель снаряда
    /// </summary>
    /// <param name="v"></param>
    public void SetTarget(Transform v)
    {
        target = v;
    }

    
    /// <summary>
    /// устанавливает урон снаряда
    /// </summary>
    /// <param name="v"></param>
    public void SetDamage(float v)
    {
        damage = v;
    }

    /// <summary>
    /// устанавливает цвет противника
    /// </summary>
    /// <param name="v"></param>
    public void SetColor(string v)
    {
        enemyColor = v;
    }

    void Start()
    {
        Audio = gameObject.GetComponent<AudioSource>();
        ThrowBallAtTargetLocation(target.position, 14f);
    }

    /// <summary>
    /// Бросает снаряд в указанную точку с заданной силой
    /// </summary>
    /// <param name="targetLocation">конечная цель</param>
    /// <param name="initialVelocity">сила броска</param>
    public void ThrowBallAtTargetLocation(Vector3 targetLocation, float initialVelocity)
    {
        Vector3 direction = (targetLocation - transform.position).normalized; 
        float distance = Vector3.Distance(targetLocation, transform.position); 

        float firingElevationAngle = FiringElevationAngle(Physics.gravity.magnitude, distance, initialVelocity);
        Vector3 elevation = Quaternion.AngleAxis(firingElevationAngle, transform.right) * transform.up;
        float directionAngle = AngleBetweenAboutAxis(transform.forward, direction, transform.up);
        Vector3 velocity = Quaternion.AngleAxis(directionAngle, transform.up) * elevation * initialVelocity;
        gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
        gameObject.GetComponent<Rigidbody>().AddTorque(1000, 0, 0);
    }

   /// <summary>
   /// Угол между осями
   /// </summary>
   /// <param name="v1"></param>
   /// <param name="v2"></param>
   /// <param name="n"></param>
   /// <returns></returns>
    public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
                   Vector3.Dot(n, Vector3.Cross(v1, v2)),
                   Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

   /// <summary>
   /// вычисялет угол для броска в цель
   /// </summary>
   /// <param name="gravity">гравитация</param>
   /// <param name="distance">расстояние</param>
   /// <param name="initialVelocity">начальная скорость</param>
   /// <returns></returns>
    private float FiringElevationAngle(float gravity, float distance, float initialVelocity)
    {
        float angle = 0.5f * Mathf.Asin((gravity * distance) / (initialVelocity * initialVelocity)) * Mathf.Rad2Deg;
        return angle;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Chicken met"+other.ToString());
        if (other.gameObject.tag == "Terrain")
        {
            GameObject tmp = Instantiate(particles, transform.position, transform.rotation);
            Destroy(tmp, 4);

            Explode();

            Destroy(gameObject);
        }

        if (other.gameObject.tag.Contains("Body") && other.gameObject.tag.Contains(enemyColor))
        {
            GameObject tmp = Instantiate(particles, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(tmp, 4);

            Explode();

            Destroy(gameObject);

            //other.transform.parent.SendMessage("GetDamage", -damage);
        }
    }

    
    /// <summary>
    /// Взрывает снаряд нанося урон и отталкивая враждебных юнитов
    /// </summary>
    private void Explode()
    {
        PlayRandomAttackSound();
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
        foreach (Collider hit in colliders)
        {
            


            if (hit.gameObject.tag.Contains("Body") && hit.gameObject.tag.Contains(enemyColor))
            {
                if ((hit.transform.position - explosionPos).magnitude  - ((SphereCollider)hit).radius * hit.transform.lossyScale.x> ExplosionRadius)
                    continue;

                Rigidbody rb = hit.GetComponent<Rigidbody>();
                
                
                rb = hit.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(ExplosionForce, explosionPos, ExplosionRadius, 5.0F, ForceMode.Impulse);

                rb.transform.SendMessage("GetDamage", -damage);
            }
        }
    }
}