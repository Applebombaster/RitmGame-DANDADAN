using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANIM : MonoBehaviour
{
    
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * 10);
    }
}
