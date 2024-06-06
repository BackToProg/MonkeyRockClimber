using UnityEngine;
using UnityEngine.UI;

namespace webgl
{
    [RequireComponent(typeof(Image))]
    public class ImageTranslator : MonoBehaviour
    {
        [SerializeField] private Sprite _en;
        [SerializeField] private Sprite _ru;

        private void Awake()
        {
            var image = GetComponent<Image>();
            image.sprite = Application.systemLanguage == SystemLanguage.English ? _en : _ru;
        }
    }
}
