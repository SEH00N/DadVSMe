using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public abstract class Item : MonoBehaviour, IInteractable
    {
        public abstract void Interact(Entity interactor);
    }
}
