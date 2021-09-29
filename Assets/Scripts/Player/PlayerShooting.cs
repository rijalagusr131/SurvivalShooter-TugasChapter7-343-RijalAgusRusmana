﻿using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.15f;        
    public float range = 100f;                      

    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                             
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;                                 
    float effectsDisplayTime = 0.2f;

    void Awake()
    {
        //GetMask
        shootableMask = LayerMask.GetMask("Shootable");

        //Mendapatkan Reference component
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }    


    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        //Disable Line Render
        gunLine.enabled = false;
        //Disable Cahaya
        gunLight.enabled = false;
    }

    public void Shoot()
    {
        timer = 0f;

        //Mainkan Audio
        gunAudio.Play();

        //Mengaktifkan Cahaya
        gunLight.enabled = true;

        //Play Gun Particle
        gunParticles.Stop();
        gunParticles.Play();

        //Mengaktifkan Line Renderer dan Set First Position
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        //Set Posisi Ray Shoot dan Direction
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        //Lakukan raycast jika mendeteksi id nemy hit apapun
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //Lakukan raycast hit hace component Enemyhealth
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                //Lakukan Take Damage
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            //Set line end position ke hit position
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            //set line end position ke range freom barrel
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}