using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoclesManager : MonoBehaviour
{
    [SerializeField] GameObject[] socles;

    public GameObject[] Socles { get => socles; set => socles = value; }
}