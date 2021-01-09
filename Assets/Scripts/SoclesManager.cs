using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoclesManager : MonoBehaviour
{
    [SerializeField]List<GameObject> socles = new List<GameObject>();

    public List<GameObject> Socles { get => socles; set => socles = value; }
    // Start is called before the first frame update
}
