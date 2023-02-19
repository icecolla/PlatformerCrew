using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private bool fade;

    private SpriteRenderer _render;

    private void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (fade)
        {
            Fade(0f);
        }
        else
        {
            Fade(1f);
        }
    }

    public void Fade(float fader)
    {
        var currentAlpha = _render.color.a;
        var alpha = Mathf.Lerp(currentAlpha, fader, Time.deltaTime * _fadeSpeed);
        _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, alpha);
    }
}
