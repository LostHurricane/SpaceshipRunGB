using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputNameField : MonoBehaviour
{
    [SerializeField] private TMP_InputField _InputField;

    public TMP_InputField InputField { get => _InputField; }
}
