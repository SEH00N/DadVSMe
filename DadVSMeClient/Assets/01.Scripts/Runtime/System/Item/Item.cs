using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public abstract class Item : MonoBehaviour
    {
        public abstract void Use(Unit user);
    }
}
