using UnityEngine;

public class SpritesFade : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private bool fade;

    private SpriteRenderer[] _renders;

    private void Awake()
    {
        _renders = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Fade(0f, _fadeSpeed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Fade(1f, 100f);
    }
    
    public void Fade(float fader, float speed)
    {
        foreach (var render in _renders)
        {
            var currentAlpha = render.color.a;
            var alpha = Mathf.Lerp(currentAlpha, fader, Time.deltaTime * speed);
            render.color = new Color(render.color.r, render.color.g, render.color.b, alpha);
        }
    }
}
