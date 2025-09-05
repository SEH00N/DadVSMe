using TMPro;
using UnityEngine;

namespace DadVSMe.UI
{
    public class IntroLoadingUI : MonoBehaviour
    {
        [SerializeField] float threshold = 0.5f;
        [SerializeField] TMP_Text loadingText = null;

        private float timer = 0f;
        private int counter = 0;
    
        private void Update()
        {
            timer += Time.deltaTime;
            if(timer >= threshold)
            {
                timer = 0f;

                counter++;
                loadingText.text = "Loading" + new string('.', counter % 4);
                if(counter >= 3)
                    counter = 0;
            }
        }
    }
}
