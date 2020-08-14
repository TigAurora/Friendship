using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class OnVolumeChangeTrans : MonoBehaviour
    {
        [Header("FadingTime")]
        public float fadeinTime = 0.5f;
        public float fadeoutTime = 0.5f;

        //Any other muted obj need to be included
        [Header("SoundSourceComp")]
        public GameObject[] others;

        public bool isFadeIn = false;
        public bool isFadeOut = false;

        [HideInInspector] public AudioSource adsource;
        [HideInInspector] public SpriteRenderer sprite;

        // Start is called before the first frame update
        void Start()
        {
            if (PlayerPrefs.GetInt("myCharacter") == 1)
            {
                transform.gameObject.GetComponent<OnVolumeChangeTrans>().enabled = false;
            }
            adsource = GetComponent<AudioSource>();
            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (adsource.isPlaying && !isFadeIn)
            {
                isFadeIn = true;
                isFadeOut = false;
                StartCoroutine(fadeIn());
            }
            if (!adsource.isPlaying && !isFadeOut)
            {
                isFadeIn = false;
                isFadeOut = true;
                StartCoroutine(fadeOut());
            }
        }


        IEnumerator fadeIn()
        {
            float a = 0;
            float[] oa = new float[99];
            a = sprite.material.color.a;
            for (int j = 0; j < others.Length; ++j)
            {
                oa[j] = others[j].GetComponent<SpriteRenderer>().material.color.a;
            }
            // loop over 1 second
            for (float i = 0; i <= fadeinTime; i += Time.deltaTime)
            {
                // set color with i as alpha
                //Color32 col = sprite.material.GetColor("_Color");
                //col.a = (byte)((sprite.color.a + ((1 - sprite.color.a) / fadeinTime) * i) * 255);
                //sprite.material.SetColor("_Color", col);
                //sprite.material.color= new Color(0, 0, 0, sprite.color.a + ((1 - sprite.color.a) / fadeinTime) * i);
                sprite.material.SetColor("_Color", new Color(1, 1, 1, a + ((1 - a) / fadeinTime + (1 - a) % fadeinTime) * i));
                if (others.Length != 0)
                {
                    for(int j = 0; j < others.Length; ++j)
                    {
                        others[j].GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 1, 1, oa[j] + ((1 - oa[j]) / fadeinTime + (1 - oa[j]) % fadeinTime) * i));
                    }
                }
                yield return null;
            }
        }

        IEnumerator fadeOut()
        {
            float a = 0;
            float[] oa = new float[99];
            // fade from opaque to transparent
            // loop over 1 second backwards
            a = sprite.material.color.a;
            for (int j = 0; j < others.Length; ++j)
            {
                oa[j] = others[j].GetComponent<SpriteRenderer>().material.color.a;
            }
            for (float i = fadeoutTime; i >= 0 && !isFadeIn; i -= Time.deltaTime)
            {
                // set color with i as alpha
                sprite.material.SetColor("_Color", new Color(1,1,1, a - (a / fadeoutTime + a % fadeoutTime) * (fadeoutTime - i)));
                if (others.Length != 0)
                {
                    foreach (GameObject other in others)
                    {
                        for (int j = 0; j < others.Length; ++j)
                        {
                            others[j].GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 1, 1, oa[j] - (oa[j] / fadeoutTime + oa[j] % fadeoutTime) * (fadeoutTime - i)));
                        }
                    }
                }
                yield return null;
            }
            if (!isFadeIn)
            {
                sprite.material.SetColor("_Color", new Color(1, 1, 1, 0));
                if (others.Length != 0)
                {
                    foreach (GameObject other in others)
                    {
                        for (int j = 0; j < others.Length; ++j)
                        {
                            others[j].GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 1, 1, 0));
                        }
                    }
                }
            }
        }
    }
}
