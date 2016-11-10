using UnityEngine;
using System.Collections;

    public class NitroScript : MonoBehaviour
    {

        private int quantityOfNitroLeft;

        public int quantityMaxOfNitro = 100;

        // Use this for initialization
        void Start()
        {
            quantityOfNitroLeft = quantityMaxOfNitro;

        }

        // Update is called once per frame
        void Update()
        {

        }

        int nitroQuantity()
        {
            return quantityOfNitroLeft;
        }

        void addSomeNitro(int quantityToAdd)
        {
            quantityOfNitroLeft += quantityOfNitroLeft;
            if (quantityOfNitroLeft > quantityMaxOfNitro)
            {
                quantityOfNitroLeft = quantityMaxOfNitro;
            }
        }

        void removeSomeNitro(int quantityToRemove)
        {
            quantityOfNitroLeft -= quantityToRemove;
            if (quantityOfNitroLeft < 0)
            {
                quantityOfNitroLeft = 0;
            }
        }
    }
