using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Module : MonoBehaviour
{
        [SerializeField]
        List<Module> _Top = new List<Module>();
        public List<Module> Top { get => _Top; private set => _Top = value; }
        [SerializeField]
        List<Module> _Down = new List<Module>();

        public List<Module> Down { get => _Down; private set => _Down = value; }
        [SerializeField]
        List<Module> _Right = new List<Module>();

        public List<Module> Right { get => _Right; private set => _Right = value; }
        [SerializeField]
        List<Module> _Left = new List<Module>();

        public List<Module> Left { get => _Left; private set => _Left = value; }
        [SerializeField]
        List<Module> _Forward = new List<Module>();

        public List<Module> Forward { get => _Forward; private set => _Forward = value; }
        [SerializeField]
        List<Module> _Back = new List<Module>();

        public List<Module> Back { get => _Back; private set => _Back = value; }
        [SerializeField]

        public List<Module> AvailableModules = new List<Module>();
}