using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Indicator : MonoBehaviour{

    public static float FADE_DISTANCE = 5f;
    public static float SCREEN_SPACE = 8f;

    [System.Serializable]
    public class Style {
        public Texture texture;
        public float space = 5;
        public Texture arrow;
        public Color color = Color.green;
        public float width = 32;
        public float height = 32;
    }

    [SerializeField] SpriteRenderer worldIndicator;
    [SerializeField] private Style style;
    [SerializeField] private int group = 0;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 1000f;


    private float initialWorldIndicatorAlpha;
    private float initialAlpha;

    private GameObject arrowGameObject;
    private RawImage arrowImage;
    private RectTransform arrowRect;

    private GameObject iconGameObject;
    private RawImage iconImage;
    private RectTransform iconRect;

    private GameObject meterGameObject;
    private RectTransform meterRect;
    private TextMeshProUGUI meterText;

    void Start(){
        initialAlpha = style.color.a;

        if(worldIndicator) {
            initialWorldIndicatorAlpha = worldIndicator.color.a;
		}

        // Creating icon game object
        iconGameObject = new GameObject(gameObject.name + ":Indicator");
        //iconGameObject.transform.SetParent(IndicatorManager.instance.groups[group]);
        iconGameObject.transform.localScale = Vector3.one;

        // Creating icon raw image
        iconImage = iconGameObject.AddComponent<RawImage>();
        iconImage.texture = style.texture;
        iconImage.color = style.color;

        // Setting up icon dimensions
        iconRect = iconGameObject.GetComponent<RectTransform>();
        //iconRect.sizeDelta = new Vector2(texture.width, texture.height);
        iconRect.sizeDelta = new Vector2(style.width, style.height);



        // Creating arrow game object
        arrowGameObject = new GameObject(gameObject.name + ":Indicator:Arrow");
        arrowGameObject.transform.SetParent(iconGameObject.transform);
        arrowGameObject.transform.localScale = Vector3.one;

        // Creating arrow raw image
        arrowImage = arrowGameObject.AddComponent<RawImage>();
        arrowImage.texture = style.arrow;
        arrowImage.color = style.color;

        // Setting up arrow dimensions
        arrowRect = arrowGameObject.GetComponent<RectTransform>();
        arrowRect.sizeDelta = new Vector2(style.arrow.width, style.arrow.height);
        arrowRect.pivot = new Vector2(0.5f, 1 + (style.height/2f + style.space)/ style.arrow.height);



        // Creating distance meter
        meterGameObject = Instantiate(Resources.Load<GameObject>("Distance Meter"), iconGameObject.transform);
        meterRect = meterGameObject.GetComponent<RectTransform>();
        meterText = meterGameObject.GetComponent<TextMeshProUGUI>();
        meterRect.anchoredPosition = new Vector2(0, -style.height);
        meterText.color = style.color;
    }

    private void OnDestroy() {
        Destroy(iconGameObject);
        Destroy(arrowGameObject);
    }

    private void OnEnable() {
        if (iconGameObject != null) iconGameObject.SetActive(true);
    }

    private void OnDisable() {
        if (iconGameObject != null) iconGameObject.SetActive(false);
    }

    void Update(){
        bool offscreen = false;

        //float scale = 1f/IndicatorManager.instance.canvas.GetComponent<RectTransform>().localScale.x;
        float scale = 1;

        float w = Screen.width * scale;
        float h = Screen.height * scale;

        Vector3 point = Camera.main.WorldToScreenPoint(transform.position) * scale;

        point.x -= w/2f;
        point.y -= h/2f;

        w -= 2f*SCREEN_SPACE + style.width + 2f* style.space + 2f*Mathf.Max(style.arrow.width, style.arrow.height);
        h -= 2f*SCREEN_SPACE + style.height + 2f* style.space + 2f*Mathf.Max(style.arrow.width, style.arrow.height);

        // On-Screen indicator
        if (point.z >= 0 && point.x >= -w/2f && point.x <= w/2f && point.y >= -h/2f && point.y <= h/2f){
            arrowGameObject.SetActive(false);

            iconRect.anchoredPosition = new Vector2(point.x, point.y);
            meterRect.anchoredPosition = new Vector2(0, -style.height);
        }
        // Off-Screen indicator
        else{
            offscreen = true;
            arrowGameObject.SetActive(true);

            if (point.z < 0) point *= -1;

            float a = 2f*point.x/w;
            float b = 2f*point.y/h;

            // The indicator lies on left or right corner
            if (Mathf.Abs(a) > Mathf.Abs(b)){

                // Right corner
                if (a > 0){
                    float y = w/2f * point.y/point.x;
                    iconRect.anchoredPosition = new Vector2(w/2f, y);
                    meterRect.anchoredPosition = new Vector2(0, -style.height);
                }
                // Left corner
                else{
                    float y = -w/2f * point.y/point.x;
                    iconRect.anchoredPosition = new Vector2(-w/2f, y);
                    meterRect.anchoredPosition = new Vector2(0, -style.height);
                }
            }
            // The indicator lies on top or bottom corner
            else{
                // Top corner
                if (b > 0){
                    float x = h/2f * point.x/point.y;
                    iconRect.anchoredPosition = new Vector2(x, h/2);
                    meterRect.anchoredPosition = new Vector2(0, -style.height);
                }
                // Bottom corner
                else{
                    float x = -h/2f * point.x/point.y;
                    iconRect.anchoredPosition = new Vector2(x, -h/2);
                    meterRect.anchoredPosition = new Vector2(0, style.height);
                }
            }

            // Update arrow rotation
            Vector2 dir = new Vector2(point.x, point.y);
            arrowRect.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, dir));
        }

        // Update distance meter
        //meterText.text = $"{Mathf.Round(Vector3.Distance(transform.position, Game.instance.cache.player.position))}m";


        // Update color
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if((distance < minDistance || distance > maxDistance) && !offscreen) {
            float diff = Mathf.Min(Mathf.Abs(distance - minDistance), Mathf.Abs(distance - maxDistance));


            float t = diff / FADE_DISTANCE;
            if(t > 1)
                t = 1;

            Color newColor = style.color;
            newColor.a = Mathf.Lerp(initialAlpha, 0, t);

            arrowImage.color = newColor;
            iconImage.color = newColor;
            meterText.color = newColor;

            // Update world indicator alpha (complementary)
            if(worldIndicator) {
                Color worldIndicatorColor = worldIndicator.color;
                worldIndicatorColor.a = Mathf.Lerp(0, initialWorldIndicatorAlpha, t);
                worldIndicator.color = worldIndicatorColor;
            }
        }
        else {
            arrowImage.color = style.color;
            iconImage.color = style.color;
            meterText.color = style.color;

            if(worldIndicator) {
                worldIndicator.color = new Color(
                    worldIndicator.color.r,
                    worldIndicator.color.g,
                    worldIndicator.color.b,
                    0
                );
            }
        }

    } 
}
