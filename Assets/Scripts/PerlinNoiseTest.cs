
using UnityEngine;

public class PerlinNoiseTest : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private float time = 0f;
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = GenerateTexture();
            time = 0;
        }



    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        //Generate perlin noise.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        //actually apply the texture data
        texture.Apply();

        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        print(transform.position);
        //PERLIN NOISE REPEATS AT WHOLE NUMBERS.
        float xPerlinCoord = (float)x / width * scale + offsetX + xPos;
        float yPerlinCoord = (float)y / height * scale + offsetY + yPos;
        float sample = Mathf.PerlinNoise(xPerlinCoord, yPerlinCoord);
        return new Color(sample, sample, sample);
    }
}
