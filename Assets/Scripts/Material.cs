using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Material : MonoBehaviour
{

    public float resistance { get { return _resistance; } }

    [SerializeField]
    private string title;

    [SerializeField]
    private float _resistance;
}
