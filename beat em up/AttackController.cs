﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    public GameObject shield;

    public Transform reflectionParent;

    public float pushBackForce;

    public Entity playa;

    public string hitDetection = "";

    public int hitsBeforeFall;

    public    HitEntity enemyRef;

    public List<Collider> cols;
    
    bool dontBlock;

    float dmg;

    /*  private void OnTriggerEnter(Collider c)
      {
          if(c.gameObject.tag==hitDetection)
          {
              HandleBlocks(c.gameObject);
          }
      }*/

    public void resetHits()
    {
        playa.hitStack = 0;
        playa.anim.SetInteger("hitStack", 0);
    }

    public IEnumerator resetHitStack()
    {
        timesincehit = 0;

        if (playa.floored)
            yield break;

        while (playa.playerHit)
        {
            yield return null;
        }
        if(!playa.floored)
        {
            while (timesincehit<1f)
            {
                if (playa.playerHit || playa.floored)
                {
                    timesincehit = 0;
                }

                timesincehit += Time.deltaTime;

                yield return null;
            }
        }
            if (playa.hitStack < hitsBeforeFall)
        {
            playa.hitStack = 0;
            playa.anim.SetInteger("hitStack", 0);
        }
    }

    float timesincehit;

    public virtual void knockedOver(GameObject obj)
    {
        faceEnemy(obj.transform);
        if (playa.rb.isKinematic)
            playa.rb.isKinematic = false;
        if (enemyRef.attackStatus == EnemyAttackState.HeavyAttack || playa.health<=0)
        {
            playa.rb.AddForce(-(new Vector3(obj.transform.root.position.x, transform.root.position.y, transform.root.position.z) - transform.root.position).normalized * pushBackForce, ForceMode.Impulse);

        }
        else
        {
            playa.rb.AddForce(- (  ( new Vector3(obj.transform.position.x, transform.root.position.y, obj.transform.position.z) - transform.root.position).normalized * pushBackForce) / 2f, ForceMode.Impulse);
        }
    }

    public virtual void setStateGotHit()
    {

    }


    GameObject heffect;

    float dist;

    int spot;

    Vector3 closestSpotToPos()
    {
        dist = 100f;

        for (int i = 0; i < cols.Count; i++)
        {
            if(Vector3.Distance(cols[i].ClosestPoint(enemyRef.transform.position + (enemyRef.entity.collisionParent.localScale.x * enemyRef.returnHitEffectPosition())), enemyRef.transform.position + (enemyRef.entity.collisionParent.localScale.x * enemyRef.returnHitEffectPosition())) < dist)
            {
                dist = Vector3.Distance(cols[i].ClosestPoint(enemyRef.transform.position + (enemyRef.entity.collisionParent.localScale.x * enemyRef.returnHitEffectPosition())), enemyRef.transform.position + (enemyRef.entity.collisionParent.localScale.x * enemyRef.returnHitEffectPosition()));
                spot = i;
            }
        }

        return cols[spot].ClosestPoint(enemyRef.transform.position + (enemyRef.entity.collisionParent.localScale.x * enemyRef.returnHitEffectPosition()));
    }

    public void gotHit(Transform obj)
    {


        //disable our block temporarily if not already disabled
        if (playa.hitStack < hitsBeforeFall)
        {
        playa.cc.resetStates();
            playa.anim.SetBool("rotate", false);
            //fetch one of the hit effects
            heffect = enemyRef.hitEffect(enemyRef.hitEffects, enemyRef.hiteffectPrefab);
 //           heffect.SetActive(true);
            heffect.transform.position = closestSpotToPos();
            heffect.transform.parent = null;

            playa.playerHit = true;
            playa.anim.SetBool("playerhit", true);
            playa.anim.SetBool("Jump", false);


            //playa.rb.velocity = Vector3.zero;
            resetrb();



           // playa.cc.resetStates();
            playa.attacking = false;
            playa.anim.SetBool("Attacking", false);

            if (playa.hitStack == 0)
             {

                //think need a better way like checking if were hit again recently
                StartCoroutine(resetHitStack());
             }

            switch (enemyRef.attackStatus)
            {
                case EnemyAttackState.LightAttack:
                    playa.hitStack++;
                    break;
                case EnemyAttackState.HeavyAttack:
                    playa.hitStack = hitsBeforeFall;
                    break;
            }

            if (playa.hitStack < hitsBeforeFall)
                setStateGotHit();

                if (shield.activeInHierarchy)
                        shield.SetActive(false);
                    playa.rb.velocity = Vector2.zero;
                    playa.anim.SetInteger("hitStack", playa.hitStack);
                    if (obj.root.position.x > transform.root.position.x)
                    {
                        if (playa.hitStack < hitsBeforeFall)
                        {

                          if (playa.facingRight)
                            {
                            playa.anim.SetBool("HitFront", true);
                            }
                            else
                            {
                            playa.anim.SetBool("HitBehind", true);
                            }
                        }
                        else
                        {
                            //fall over but make sure we fall away from them
                        }
                    }
                    else
                    {
                        if (playa.hitStack < hitsBeforeFall)
                        {
                              if (playa.facingRight)
                              playa.anim.SetBool("HitBehind", true);
                              else
                              playa.anim.SetBool("HitFront", true);
                        }
                        else
                        {
                            //fall over but make sure we fall away from them
                        }
                    }

            if (playa.rolling)
            {
                playa.stopRoll();
            }
            playa.health -= dmg;
            if (playa.health <= 0)
            {
                playa.die();
                knockedOver(obj.gameObject);
                return;
            }
            if (playa.hitStack == hitsBeforeFall)
                {
                //disable our damage collider and face player towards enemy
                knockedOver(obj.gameObject);

                }

        }
    
    }

     public virtual void resetrb()
    {

    }

    public void faceEnemy(Transform target)
    {
        if (target.transform.root.position.x > transform.root.position.x)
        {
            if(!playa.facingRight)
            {
                playa.Flip();
            }
        }
        else
        {
            if (playa.facingRight)
            {
                playa.Flip();
            }
        }
    }

    public virtual void HandleBlocks( float damage)
    {
        dontBlock = false;
        dmg = damage;
                                                                                              //was ==2
        if (playa.grounded && playa.crouch && playa.es.myHE.attackPos != 1 && ((enemyRef.attackPos == 2 && !enemyRef.entity.crouch ) || enemyRef.entity.crouch && enemyRef.attackPos == 1  ))
        {
            return;
        }

        //check if shield is active

        if (shield.activeInHierarchy)
        {
            //check what way the enemy is facing

            if (enemyRef.transform.root.position.x > transform.root.position.x)
            {
                //check what way we are facing

                if (playa.facingRight)
                {
                    //check if they are hitting from low or high
                   if(returnBlockPosition(enemyRef.gameObject))
                    {
                        blocks();
                        Debug.Log("Blocked");
                        //block
                    }
                   else
                    {
                        gotHit(enemyRef.transform);
                        Debug.Log("Hit");
                        //damage
                    }
                }
                else
                {
                    gotHit(enemyRef.transform);
                    Debug.Log("hit");
                }
            }
            else
            {
                if (!playa.facingRight)
                {
                    if (returnBlockPosition(enemyRef.gameObject))
                    {
                        blocks();
                        Debug.Log("Blocked");
                        //block
                    }
                    else
                    {
                        gotHit(enemyRef.transform);
                        Debug.Log("Hit");
                        //damage
                    }
                }
                else
                {
                    gotHit(enemyRef.transform);
                    Debug.Log("hit");
                }
            }

        }
        else
        {
            gotHit(enemyRef.transform);
          //  Debug.Log("Hit");
            //else damage us
        }
    }

    public virtual void blocks()
    {
        if (!dontBlock)
        {
            if (playa.anim.GetBool("Blocked") == false)
            {
                playa.anim.SetBool("Blocked", true);
                playa.blocked = true;
            }
            else
            {
                playa.anim.SetBool("Blocked", true);
                playa.anim.SetBool("blockagain", true);
                playa.blocked = true;
            }
        }
    }

    public bool returnBlockPosition(GameObject obj)
    {
        if (!playa.crouch)
        {
            if (enemyRef.entity.crouch)
            {
                if (enemyRef.attackPos <2 || enemyRef.attackPos> 4)
                {
                    //block
                    return true;
                }
                else
                {
                    //hit
                    return false;
                }
            }
            else
            {

                if (enemyRef.attackPos >= 3 && enemyRef.attackPos<5)
                {
                    //hit
                    return false;
                }
                else
                {
                    //block
                    return true;
                }
            }
        }
        else
        {
            if (enemyRef.entity.crouch)
            {
                if(enemyRef.attackPos>2 && enemyRef.attackPos < 5 )
                {
                    //hit
                    return false;
                }
                else
                {
                    //block
                    return true;
                }
            }
            else
            {
                //check if its up so we dont bother block ? :)
                if (enemyRef.attackPos == 2 )
                    dontBlock = true;


                //block
                return true;


            }
        }
    }

/*    public bool returnBlockPosition(GameObject obj)
    {
        //check if they are hitting from low or high
        if (obj.transform.position.y > 1.6f)
        {
            //check if we are blocking low or high
            if (shield.transform.position.y > 1.6f)
            {
                //block
                return true;
            }
            else
            {
                return false;
                //damage us
            }
        }
        else if(obj.transform.position.y < 1.6f && obj.transform.position.y > 1.2f)
        {
            if (shield.transform.position.y < 1.6f)
            {
                return true;
                //block
            }
            else
            {
                return false;
                //damage us
            }
        }
        else
        {
            return false;
        }
    }*/
}
