using Lurkers.Taste;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Environment.Taste
{
    public class RefillStationManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] refillStations;

        private Flavor[] refills = new Flavor[5];

        public static RefillStationManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (GameObject station in refillStations)
            {
                station.GetComponent<RefillStation>().InstantiateFlavor();
                station.GetComponent<RefillStation>().MoreThanThree();
            }
        }

        public Flavor GenerateNewFlavor(int mixTimes)
        {
            int cnt = 1;
            int index1 = UnityEngine.Random.Range(0, refills.Length - 1);
            int index2 = UnityEngine.Random.Range(0, refills.Length - 1);
            Flavor newFlavor = Formula.Combine(refills[index1], refills[index2]);
            while (cnt < mixTimes)
            {
                int newIndex = UnityEngine.Random.Range(0, refills.Length - 1);
                newFlavor = Formula.Combine(newFlavor, refills[newIndex]);
                cnt += 1;
            }
            return newFlavor;
        }
    }
}
