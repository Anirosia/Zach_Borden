using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniToggle : MonoBehaviour
{
    public Collider swordCollider;

    public void EnableSword()
    {
        swordCollider.enabled = true;
    }

    public void DisableSword()
    {
        swordCollider.enabled = false;
    }
}
