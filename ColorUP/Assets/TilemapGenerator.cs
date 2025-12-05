using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;       // رفرنس به Tilemap
    public TileBase baseTile;     // یک Tile ساده (مثلاً سفید)
    public Camera mainCamera;     // دوربین اصلی

    private float seed;

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        seed = Random.value * 1000f;
    }
    void Update()
    {
        GenerateTilesAroundCamera();
    }

    void GenerateTilesAroundCamera()
    {
        // محدوده‌ی دید دوربین
        float camHeight = mainCamera.orthographicSize * 2f;
        float camWidth = camHeight * mainCamera.aspect;

        // مرکز دوربین
        Vector3 camPos = mainCamera.transform.position;

        // محدوده‌ی تایل‌ها (به اضافه‌ی ۳ تایل اطراف)
        int minX = Mathf.FloorToInt(camPos.x - camWidth / 2) - 3;
        int maxX = Mathf.CeilToInt(camPos.x + camWidth / 2) + 3;
        int minY = Mathf.FloorToInt(camPos.y - camHeight / 2) - 3;
        int maxY = Mathf.CeilToInt(camPos.y + camHeight / 2) + 3;

        // پاک کردن تایل‌های خارج از محدوده
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
                {
                    tilemap.SetTile(pos, null); // حذف تایل
                }
            }
        }

        // تولید تایل‌ها در محدوده
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (!tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    int level = GetLevelByNoise(x, y);
                    Color32 color = GetColorByLevel(level);
                    tilemap.SetTile(pos, baseTile);
                    tilemap.SetTileFlags(pos, TileFlags.None); // اجازه تغییر رنگ
                    tilemap.SetColor(pos, color);
                }
            }
        }
    }

    int GetLevelByNoise(int x, int y)
    {
        float scale = 0.08f; // میزان کشیدگی نقشه
        float noiseValue = Mathf.PerlinNoise((x + seed) * scale, (y + seed) * scale);

        noiseValue *= 100; // تبدیل به بازه 0-100
        noiseValue = Mathf.Pow(noiseValue, 1.6f);// افزایش کنتراست
        int tempvalue = (int)noiseValue;
        int level = Mathf.FloorToInt(Mathf.Sqrt(tempvalue)); // کاهش دامنه مقادیر بالا
        level = (int)(level / 1.9); // تنظیم نهایی دامنه
        level -= 9; // جابجایی سطح به سمت سطح صفر
        return Mathf.Clamp(level, 0, 100); // محدود کردن سطح به بازه 0-100
    }



    Color32 GetColorByLevel(int level)
    {
        int r = 100 + level * 8;
        int g = 50 + level * 12;
        int b = 0 + level * 20;

        r = Mathf.Clamp(r, 0, 255);
        g = Mathf.Clamp(g, 0, 255);
        b = Mathf.Clamp(b, 0, 255);

        return new Color32((byte)r, (byte)g, (byte)b, 255);
    }
}
