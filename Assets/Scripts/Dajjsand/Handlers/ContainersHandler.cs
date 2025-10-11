using UnityEngine;

namespace Dajjsand.Handlers
{
    public class ContainersHandler : MonoBehaviour
    {
        [field: SerializeField] public Transform CardsContainer { get; private set; }
        [field: SerializeField] public Transform StoreCardsContainer { get; private set; }
    }
}