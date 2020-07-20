using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTW.Combat
{
    public class ChannelTimer : MonoBehaviour
    {
        public Channel channel;
        Image image;

        void Start()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            float thing = ((channel.channel / channel.maxChannel) - 1f) * -1f;
            image.fillAmount = thing;
        }
    }
}