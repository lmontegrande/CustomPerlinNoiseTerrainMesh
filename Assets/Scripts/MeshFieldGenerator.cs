using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshFieldGenerator : MonoBehaviour {
    public Button animateButton;
    public Button stopButton;
    public Slider animationSpeedSlider;
    public Slider xSlider;
    public Slider zSlider;
    public Slider sizeOffsetSlider;
    public Slider noiseScaleSlider;
    public Slider noiseSizeSlider;
    public Slider deltaTSlider;
    public GameObject cubeMakerGameObject;
    public bool isNoisy = true;
    
    private int xDimension = 1;
    private int zDimension = 1;
    private float cubeStartingSize = 1;

    private List<GameObject> cubeField = new List<GameObject>();

    public void Start()
    {
        animateButton.onClick.AddListener(Animate);
        stopButton.onClick.AddListener(StopAnimate);
        xSlider.onValueChanged.AddListener(GenerateField);
        zSlider.onValueChanged.AddListener(GenerateField);
        sizeOffsetSlider.onValueChanged.AddListener(GenerateField);
        noiseScaleSlider.onValueChanged.AddListener(GenerateField);
        noiseSizeSlider.onValueChanged.AddListener(GenerateField);
        deltaTSlider.onValueChanged.AddListener(GenerateField);

        GenerateField();
    }

    private void GenerateField(float val)
    {
        GenerateField();
    }

    private void GenerateField() {

        SetValues();
        ClearField();

        for(int x=0; x<xDimension; x++)
        {
            for(int z=0; z<zDimension; z++)
            {
                Vector3 startingLoc = (new Vector3((float)x-(xDimension/2), 0, (float)z-(zDimension/2))) * cubeStartingSize;
                GameObject cubeInstance = GameObject.Instantiate(cubeMakerGameObject, startingLoc, Quaternion.identity);
                cubeInstance.GetComponent<CubeMaker>().size = Vector3.one * (cubeStartingSize * sizeOffsetSlider.value);
                
                if (isNoisy)
                {
                    Vector2 perlinPosition = new Vector2((x * noiseScaleSlider.value) + deltaTSlider.value, (z * noiseScaleSlider.value));
                    float perlinHeight = Mathf.PerlinNoise(perlinPosition.x, perlinPosition.y);
                    cubeInstance.GetComponent<CubeMaker>().size.y = perlinHeight * noiseSizeSlider.value;
                }

                cubeField.Add(cubeInstance);
            }
        }
    }

    private void ClearField()
    {
        while (cubeField.Count > 0)
        {
            GameObject cube = cubeField[0];
            cubeField.RemoveAt(0);
            Destroy(cube);
        }
    }

    private void SetValues()
    {
        xDimension = (int) xSlider.value;
        zDimension = (int) zSlider.value;
    }

    private Coroutine animateCoroutine;
    private void Animate()
    {
        StopAnimate();
        animateCoroutine = StartCoroutine(AnimateCoroutine());
    }

    private void StopAnimate()
    {
        if (animateCoroutine != null)
            StopCoroutine(animateCoroutine);
    }

    private IEnumerator AnimateCoroutine()
    {
        GenerateField();
        deltaTSlider.value = 0f;
        while(true)
        {
            if (deltaTSlider.value >= deltaTSlider.maxValue)
                deltaTSlider.value = 0f;

            deltaTSlider.value += animationSpeedSlider.value * Time.deltaTime;
            yield return null;
        }
    }
}
