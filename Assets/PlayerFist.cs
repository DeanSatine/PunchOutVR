using UnityEngine;
using System.Collections;

public class PlayerFist : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] bool isRightHand;
    public bool IsRightHand => isRightHand;

    public bool isBlocking;

    const float enemyPartGraceDistance = 0.15f; // used to for punch graces, where you hit in an area with multiple enemy part colliders near eachother.
    public Transform gloveCanvas; // canvas for UI attached to glove.
    const float rotationDuration = 0.125f; // duration for golveCanvas rotation.
    const float upwardsTolerance = 0.15f;  // threshhold for what glove considers upwards facing.
    Coroutine rotationCoroutine;
    bool isFacingUpwards = false;

    #endregion


    private void Update()
    {
        if (Player.instance.isRightHanded != IsRightHand) // if this hand is NOT the main hand.
        {
            bool isCurrentlyUpwards = IsFacingUpwards();
            if (isFacingUpwards != isCurrentlyUpwards) // check if state has changed.
            {
                isFacingUpwards = isCurrentlyUpwards;
                RotateGloveCanvas(isCurrentlyUpwards);
            }
        }
    }

    void RotateGloveCanvas(bool isUpwards)
    {
        if(rotationCoroutine!= null)StopCoroutine(rotationCoroutine);
        if (isUpwards)
        {
            rotationCoroutine = StartCoroutine(SmoothRotateGloveCanvas(180));
        }
        else
        {
            rotationCoroutine = StartCoroutine(SmoothRotateGloveCanvas(90));
        }


        IEnumerator SmoothRotateGloveCanvas(float targetZRotation)
        {
            float startZ = gloveCanvas.localRotation.eulerAngles.z;
            if (startZ == targetZRotation) yield break;
            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / rotationDuration);
                float currentZ = Mathf.Lerp(startZ, targetZRotation, t);

                // set Z rotation while keeping X and Y unchanged
                gloveCanvas.localRotation= Quaternion.Euler(gloveCanvas.localRotation.eulerAngles.x, gloveCanvas.localRotation.eulerAngles.y, currentZ);

                yield return null;
            }

            gloveCanvas.localRotation = Quaternion.Euler(gloveCanvas.localRotation.eulerAngles.x, gloveCanvas.localRotation.eulerAngles.y, targetZRotation);
        }
    }

    bool IsFacingUpwards()
    {
        // calculate dot product between objects up and world up 
        float dotProduct = Vector3.Dot(transform.up, Vector3.down);

        //  if dot product is 1, the rotations are the same. also gives tolerance.
        return dotProduct >= 1f - upwardsTolerance;
    }
    public void OnCollisionEnter(Collision collision)
    {
        // check if collided with enemy body part.
        if (collision.gameObject.TryGetComponent(out Enemy_BodyPart enemyPart))
        {

            Enemy_BodyPart targetBodyPart = GetHighestDamagePartInRange( // scan in a radius around nearest contact point for other potentially higher damage body parts.
                GetClosestContactToEnemyPart(enemyPart, collision) // get contact point from fist that is closest to the enemy body part we have collided with.
                );


            // if somehow the check for nearby body parts returns null, this catches that.
            if (targetBodyPart != null)
            {


                targetBodyPart.TakeDamage(GetVelocityModifiedDamage());
            }
            else enemyPart.TakeDamage(GetVelocityModifiedDamage());
        }
    }

    /// <summary>
    /// Returns the <see cref="Enemy_BodyPart"/> with the highest <see cref="Enemy_BodyPart.bodyPartDamageMultiplier"/> value that is within <see cref="enemyPartGraceDistance"/> units of <paramref name="position"/>
    /// </summary>
    /// <param name="position"> position to check for nearby enemy body parts.</param>
    public Enemy_BodyPart GetHighestDamagePartInRange(Vector3 position)
    {
        Collider[] hitColliders = new Collider[20]; //If there are more than 20 colliders within the grace distance, something has gone very very wrong.
        int colliderCount = Physics.OverlapSphereNonAlloc(position, enemyPartGraceDistance, hitColliders);

        float maxMultiplier = float.MinValue;
        Enemy_BodyPart highestDamagePart = null;

        for (int i = 0; i < colliderCount; i++)
        {
            if (hitColliders[i].TryGetComponent(out Enemy_BodyPart part) &&
                part.BodyPartDamageMultiplier > maxMultiplier)
            {
                maxMultiplier = part.BodyPartDamageMultiplier;
                highestDamagePart = part;
            }
        }

        return highestDamagePart;
    }
    /// <summary>
    /// Gets the position of the closest contact point in <paramref name="collision"/> to the <paramref name="enemyPart"/>.
    /// </summary>
    public Vector3 GetClosestContactToEnemyPart(Enemy_BodyPart enemyPart, Collision collision)
    {
        Vector3 enemyPartPosition = enemyPart.transform.position;
        float minDistance = float.MaxValue; // makes sure first value is set to the minimum.
        Vector3 closestContactPoint = Vector3.zero;

        foreach (ContactPoint contact in collision.contacts) // for each contact point, compare the distance to the current min distance.
        {
            float distance = (contact.point - enemyPartPosition).sqrMagnitude; // sqrMagnitude because it is faster.
            if (distance < minDistance)
            {
                minDistance = distance;
                closestContactPoint = contact.point;
            }
        }

        return closestContactPoint;
    }

    public float GetVelocityModifiedDamage()
    {
        return Player.instance.baseDamage * GetDamageMultiplier();

    }

    float GetDamageMultiplier()
    {
        var handVelocities = Player.instance.HandVelocities; // get hand velocities.
        Vector3 targetHand = IsRightHand ? handVelocities.right : handVelocities.left; // determine which we need.

        // take speed of hand and normalize it to range of numbers.
        // this range of numbers becomes a multiplier for damage dealt, based on speed
        return targetHand.magnitude.NormalizeToRange(
            0.75f, // minimum damage multiplier 
            2.25f,  // maximum damage multiplier
            0,     // minimum velocity threshhold ( velocity for minimum damage )
            5);    // maximum velocity threshhold ( velocity for maximum damage )
    }


}
